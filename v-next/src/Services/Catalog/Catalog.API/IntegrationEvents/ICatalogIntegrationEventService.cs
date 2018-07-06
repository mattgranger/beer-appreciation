namespace Catalog.API.IntegrationEvents
{
    using System.Threading.Tasks;
    using BeerAppreciation.Core.EventBus.Events;

    public interface ICatalogIntegrationEventService
    {
        Task SaveEventAndCatalogContextChangesAsync(IntegrationEvent evt);

        Task PublishThroughEventBusAsync(IntegrationEvent evt);
    }
}
