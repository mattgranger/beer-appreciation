namespace Catalog.API.IntegrationEvents
{
    using System.Threading.Tasks;
    using BeerAppreciation.Core.EventBus.Events;

    public class CatalogIntegrationEventService : ICatalogIntegrationEventService
    {
        public Task SaveEventAndCatalogContextChangesAsync(IntegrationEvent evt)
        {
            throw new System.NotImplementedException();
        }

        public Task PublishThroughEventBusAsync(IntegrationEvent evt)
        {
            throw new System.NotImplementedException();
        }
    }
}