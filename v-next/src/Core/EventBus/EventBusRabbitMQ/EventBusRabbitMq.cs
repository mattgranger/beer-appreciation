namespace BeerAppreciation.Core.EventBusRabbitMQ
{
    using System;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;
    using Autofac;
    using EventBus;
    using EventBus.Abstractions;
    using EventBus.Events;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Polly;
    using Polly.Retry;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;
    using RabbitMQ.Client.Exceptions;

    public class EventBusRabbitMq: IEventBus, IDisposable
    {
        public const string BROKER_NAME = "beerappreciation_event_bus";

        private readonly IRabbitMqPersistentConnection persistentConnection;
        private readonly ILogger<EventBusRabbitMq> logger;
        private readonly ILifetimeScope autofac;
        private readonly IEventBusSubscriptionsManager subsManager;
        private readonly int retryCount;
        private readonly string AUTOFAC_SCOPE_NAME = "beerappreciation_event_bus";
        
        private IModel consumerChannel;
        private string queueName;
        private string exchangeType;

        public EventBusRabbitMq(IRabbitMqPersistentConnection persistentConnection, ILogger<EventBusRabbitMq> logger,
            ILifetimeScope autofac, IEventBusSubscriptionsManager subsManager, string exchangeType = ExchangeTypes.Direct, string queueName = null, int retryCount = 5)
        {
            this.persistentConnection = persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));;
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));;
            this.subsManager = subsManager ?? new InMemoryEventBusSubscriptionsManager();;
            this.autofac = autofac;
            this.exchangeType = exchangeType;
            this.queueName = queueName;
            this.retryCount = retryCount;

            this.consumerChannel = this.CreateConsumerChannel();
            this.subsManager.OnEventRemoved += this.SubsManager_OnEventRemoved;
        }

        private void SubsManager_OnEventRemoved(object sender, string eventName)
        {
            this.EnsurePersistedConnection();

            using (var channel = this.persistentConnection.CreateModel())
            {
                channel.QueueUnbind(queue: this.queueName,
                    exchange: BROKER_NAME,
                    routingKey: eventName);

                if (this.subsManager.IsEmpty)
                {
                    this.queueName = string.Empty;
                    this.consumerChannel.Close();
                }
            }
        }

        private IModel CreateConsumerChannel()
        {
            this.EnsurePersistedConnection();

            var channel = this.persistentConnection.CreateModel();

            channel.ExchangeDeclare(
                exchange: BROKER_NAME,
                type: this.exchangeType);

            channel.QueueDeclare(queue: this.queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);


            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var eventName = ea.RoutingKey;
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());

                await this.ProcessEvent(eventName, message);

                channel.BasicAck(ea.DeliveryTag,multiple:false);
            };

            channel.BasicConsume(queue: this.queueName,
                autoAck: false,
                consumer: consumer);

            channel.CallbackException += (sender, ea) =>
            {
                this.consumerChannel.Dispose();
                this.consumerChannel = this.CreateConsumerChannel();
            };

            return channel;
        }

        private async Task ProcessEvent(string eventName, string message)
        {
            if (this.subsManager.HasSubscriptionsForEvent(eventName))
            {
                using (var scope = this.autofac.BeginLifetimeScope(this.AUTOFAC_SCOPE_NAME))
                {
                    var subscriptions = this.subsManager.GetHandlersForEvent(eventName);
                    foreach (var subscription in subscriptions)
                    {
                        if (subscription.IsDynamic)
                        { 
                            var handler = scope.ResolveOptional(subscription.HandlerType) as IDynamicIntegrationEventHandler;
                            dynamic eventData = JObject.Parse(message);
                            await handler?.Handle(eventData);
                        }
                        else
                        {
                            var eventType = this.subsManager.GetEventTypeByName(eventName);
                            var integrationEvent = JsonConvert.DeserializeObject(message, eventType);
                            var handler = scope.ResolveOptional(subscription.HandlerType);
                            var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                            await (Task)concreteType.GetMethod("Handle")?.Invoke(handler, new object[] { integrationEvent });
                        }
                    }
                }
            }
        }


        public void Publish(IntegrationEvent @event)
        {
            this.EnsurePersistedConnection();

            var policy = RetryPolicy.Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(this.retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                    this.logger.LogWarning(ex.ToString());
                });

            using (var channel = this.persistentConnection.CreateModel())
            {
                var eventName = @event.GetType().Name;

                channel.ExchangeDeclare(exchange: BROKER_NAME, type: this.exchangeType);

                var message = JsonConvert.SerializeObject(@event);
                var body = Encoding.UTF8.GetBytes(message);

                policy.Execute(() =>
                {
                    var properties = channel.CreateBasicProperties();
                    properties.DeliveryMode = 2; // persistent

                    channel.BasicPublish(
                        exchange: BROKER_NAME,
                        routingKey: eventName,
                        mandatory:true,
                        basicProperties: properties,
                        body: body);
                });
            }
        }

        public void Subscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>
        {
            var eventName = this.subsManager.GetEventKey<T>();
            this.Subscribe(eventName);
            this.subsManager.AddSubscription<T, TH>();
        }

        private void Subscribe(string eventName)
        {
            var containsKey = this.subsManager.HasSubscriptionsForEvent(eventName);
            if (!containsKey)
            {
                this.EnsurePersistedConnection();

                using (var channel = this.persistentConnection.CreateModel())
                {
                    channel.QueueBind(
                        queue: this.queueName,
                        exchange: BROKER_NAME,
                        routingKey: eventName);
                }
            }
        }

        private void EnsurePersistedConnection()
        {
            if (!this.persistentConnection.IsConnected)
            {
                this.persistentConnection.TryConnect();
            }
        }

        public void SubscribeDynamic<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
        {
            this.Subscribe(eventName);
            this.subsManager.AddDynamicSubscription<TH>(eventName);
        }

        public void UnsubscribeDynamic<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
        {
            this.subsManager.RemoveDynamicSubscription<TH>(eventName);
        }

        public void Unsubscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>
        {
            this.subsManager.RemoveSubscription<T, TH>();
        }

        public void Dispose()
        {
            this.consumerChannel?.Dispose();

            this.subsManager.Clear();
        }
    }
}
