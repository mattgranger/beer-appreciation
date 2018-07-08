namespace BeerAppreciation.Core.EventBusRabbitMQ
{
    using System;
    using RabbitMQ.Client;

    public interface IRabbitMqPersistentConnection : IDisposable
    {
        bool IsConnected { get; }

        bool TryConnect();

        IModel CreateModel();
    }
}
