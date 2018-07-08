namespace BeerAppreciation.Core.EventBusServiceBus
{
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using Autofac;
    using EventBus;
    using EventBus.Abstractions;
    using EventBus.Events;
    using Microsoft.Azure.ServiceBus;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class EventBusServiceBus : IEventBus
    {
        private readonly IServiceBusPersisterConnection serviceBusPersisterConnection;
        private readonly ILogger<EventBusServiceBus> logger;
        private readonly IEventBusSubscriptionsManager subsManager;
        private readonly SubscriptionClient subscriptionClient;
        private readonly ILifetimeScope autofac;
        private readonly string AUTOFAC_SCOPE_NAME = "beerappreciation_event_bus";
        private const string INTEGRATION_EVENT_SUFIX = "IntegrationEvent";

        public EventBusServiceBus(IServiceBusPersisterConnection serviceBusPersisterConnection, 
            ILogger<EventBusServiceBus> logger, IEventBusSubscriptionsManager subsManager, string subscriptionClientName,
            ILifetimeScope autofac)
        {
            this.serviceBusPersisterConnection = serviceBusPersisterConnection;
            this.logger = logger;
            this.subsManager = subsManager ?? new InMemoryEventBusSubscriptionsManager();

            this.subscriptionClient = new SubscriptionClient(serviceBusPersisterConnection.ServiceBusConnectionStringBuilder, 
                subscriptionClientName);
            this.autofac = autofac;

            this.RemoveDefaultRule();
            this.RegisterSubscriptionClientMessageHandler();
        }

        public void Publish(IntegrationEvent @event)
        {
            var eventName = @event.GetType().Name.Replace(INTEGRATION_EVENT_SUFIX, "");
            var jsonMessage = JsonConvert.SerializeObject(@event);
            var body = Encoding.UTF8.GetBytes(jsonMessage);

            var message = new Message
            {
                MessageId = Guid.NewGuid().ToString(),
                Body = body,
                Label = eventName,
            };

            var topicClient = this.serviceBusPersisterConnection.CreateModel();

            topicClient.SendAsync(message)
                .GetAwaiter()
                .GetResult();
        }

        public void SubscribeDynamic<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler
        {
            this.subsManager.AddDynamicSubscription<TH>(eventName);
        }

        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName =  typeof(T).Name.Replace(INTEGRATION_EVENT_SUFIX, "");

            var containsKey = this.subsManager.HasSubscriptionsForEvent<T>();
            if (!containsKey)
            {
                try
                {
                    this.subscriptionClient.AddRuleAsync(new RuleDescription
                    {
                        Filter = new CorrelationFilter { Label = eventName },
                        Name = eventName
                    }).GetAwaiter().GetResult();
                }
                catch(ServiceBusException)
                {
                    this.logger.LogInformation($"The messaging entity {eventName} already exists.");
                }
            }

            this.subsManager.AddSubscription<T, TH>();
        }

        public void Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = typeof(T).Name.Replace(INTEGRATION_EVENT_SUFIX, "");

            try
            {
                this.subscriptionClient
                 .RemoveRuleAsync(eventName)
                 .GetAwaiter()
                 .GetResult();
            }
            catch (MessagingEntityNotFoundException)
            {
                this.logger.LogInformation($"The messaging entity {eventName} Could not be found.");
            }

            this.subsManager.RemoveSubscription<T, TH>();
        }

        public void UnsubscribeDynamic<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler
        {
            this.subsManager.RemoveDynamicSubscription<TH>(eventName);
        }

        public void Dispose()
        {
            this.subsManager.Clear();
        }

        private void RegisterSubscriptionClientMessageHandler()
        {
            this.subscriptionClient.RegisterMessageHandler(
                async (message, token) =>
                {
                    var eventName = $"{message.Label}{INTEGRATION_EVENT_SUFIX}";
                    var messageData = Encoding.UTF8.GetString(message.Body);
                    await this.ProcessEvent(eventName, messageData);
                    
                    // Complete the message so that it is not received again.
                    await this.subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
                },
               new MessageHandlerOptions(ExceptionReceivedHandler) { MaxConcurrentCalls = 10, AutoComplete = false });
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
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
                            await handler.Handle(eventData);
                        }
                        else
                        {
                            var eventType = this.subsManager.GetEventTypeByName(eventName);
                            var integrationEvent = JsonConvert.DeserializeObject(message, eventType);
                            var handler = scope.ResolveOptional(subscription.HandlerType);
                            var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                            await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
                        }
                    }
                }
            }
        }

        private void RemoveDefaultRule()
        {
            try
            {
                this.subscriptionClient
                 .RemoveRuleAsync(RuleDescription.DefaultRuleName)
                 .GetAwaiter()
                 .GetResult();
            }
            catch (MessagingEntityNotFoundException)
            {
                this.logger.LogInformation($"The messaging entity { RuleDescription.DefaultRuleName } Could not be found.");
            }
        }
    }
}
