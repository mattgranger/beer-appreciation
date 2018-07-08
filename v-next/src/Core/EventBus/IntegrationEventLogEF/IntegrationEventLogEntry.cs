namespace BeerAppreciation.Core.IntegrationEventLogEF
{
    using System;
    using EventBus.Events;
    using Newtonsoft.Json;

    public class IntegrationEventLogEntry
    {
        private IntegrationEventLogEntry()
        {
        }

        public IntegrationEventLogEntry(IntegrationEvent @event)
        {
            this.EventId = @event.Id;
            this.CreationTime = @event.CreationDate;
            this.EventTypeName = @event.GetType().FullName;
            this.Content = JsonConvert.SerializeObject(@event);
            this.State = EventStateEnum.NotPublished;
            this.TimesSent = 0;
        }

        public Guid EventId { get; private set; }

        public string EventTypeName { get; private set; }

        public EventStateEnum State { get; set; }

        public int TimesSent { get; set; }

        public DateTime CreationTime { get; private set; }

        public string Content { get; private set; }
    }
}
