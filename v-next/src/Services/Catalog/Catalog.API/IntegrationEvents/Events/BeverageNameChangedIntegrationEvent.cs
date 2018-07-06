namespace Catalog.API.IntegrationEvents.Events
{
    using BeerAppreciation.Core.EventBus.Events;

    public class BeverageNameChangedIntegrationEvent : IntegrationEvent
    {
        public BeverageNameChangedIntegrationEvent(int beverageId, string newName, string oldName)
        {
            this.BeverageId = beverageId;
            this.NewName = newName;
            this.OldName = oldName;
        }

        public int BeverageId { get; private set; }

        public string NewName { get; private set; }

        public string OldName { get; private set; }
    }
}
