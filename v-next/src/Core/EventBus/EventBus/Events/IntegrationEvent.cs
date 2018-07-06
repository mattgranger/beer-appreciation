namespace BeerAppreciation.Core.EventBus.Events
{
    using System;

    public class IntegrationEvent
    {
        public IntegrationEvent()
        {
            this.Id = Guid.NewGuid();
            this.CreationDate = DateTime.UtcNow;
        }

        public Guid Id { get; }

        public DateTime CreationDate { get; }
    }
}
