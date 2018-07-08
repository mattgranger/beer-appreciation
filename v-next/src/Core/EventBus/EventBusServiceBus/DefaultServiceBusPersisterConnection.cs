namespace BeerAppreciation.Core.EventBusServiceBus
{
    using System;
    using Microsoft.Azure.ServiceBus;
    using Microsoft.Extensions.Logging;

    public class DefaultServiceBusPersisterConnection :IServiceBusPersisterConnection
    {
        private readonly ILogger<DefaultServiceBusPersisterConnection> logger;
        private readonly ServiceBusConnectionStringBuilder serviceBusConnectionStringBuilder;
        private ITopicClient topicClient;

        bool disposed;

        public DefaultServiceBusPersisterConnection(ServiceBusConnectionStringBuilder serviceBusConnectionStringBuilder,
            ILogger<DefaultServiceBusPersisterConnection> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            
            this.serviceBusConnectionStringBuilder = serviceBusConnectionStringBuilder ?? 
                throw new ArgumentNullException(nameof(serviceBusConnectionStringBuilder));
            this.topicClient = new TopicClient(this.serviceBusConnectionStringBuilder, RetryPolicy.Default);
        }

        public ServiceBusConnectionStringBuilder ServiceBusConnectionStringBuilder => this.serviceBusConnectionStringBuilder;

        public ITopicClient CreateModel()
        {
            if(this.topicClient.IsClosedOrClosing)
            {
                this.topicClient = new TopicClient(this.serviceBusConnectionStringBuilder, RetryPolicy.Default);
            }

            return this.topicClient;
        }

        public void Dispose()
        {
            if (this.disposed) return;

            this.disposed = true;
        }
    }
}
