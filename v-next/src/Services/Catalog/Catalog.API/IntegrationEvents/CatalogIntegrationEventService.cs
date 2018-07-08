namespace Catalog.API.IntegrationEvents
{
    using System;
    using System.Data.Common;
    using System.Threading.Tasks;
    using BeerAppreciation.Core.EventBus.Abstractions;
    using BeerAppreciation.Core.EventBus.Events;
    using BeerAppreciation.Core.IntegrationEventLogEF.Services;
    using BeerAppreciation.Core.IntegrationEventLogEF.Utilities;
    using Infrastructure.Contexts;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage;

    public class CatalogIntegrationEventService : ICatalogIntegrationEventService
    {
        private readonly IEventBus eventBus;
        private readonly CatalogContext catalogContext;
        private readonly Func<DbConnection, IIntegrationEventLogService> integrationEventLogServiceFactory;
        private readonly IIntegrationEventLogService eventLogService;

        public CatalogIntegrationEventService(IEventBus eventBus, CatalogContext catalogContext,
            Func<DbConnection, IIntegrationEventLogService> integrationEventLogServiceFactory)
        {
            this.eventBus = eventBus;
            this.catalogContext = catalogContext;
            this.integrationEventLogServiceFactory = integrationEventLogServiceFactory;
            this.eventLogService = this.integrationEventLogServiceFactory(this.catalogContext.Database.GetDbConnection());
        }

        public async Task SaveEventAndCatalogContextChangesAsync(IntegrationEvent evt)
        {
            //Use of an EF Core resiliency strategy when using multiple DbContexts within an explicit BeginTransaction():
            //See: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency            
            await ResilientTransaction.New(this.catalogContext)
                .ExecuteAsync(async () => {
                    // Achieving atomicity between original catalog database operation and the IntegrationEventLog thanks to a local transaction
                    await this.catalogContext.SaveChangesAsync();
                    await this.eventLogService.SaveEventAsync(evt, this.catalogContext.Database.CurrentTransaction.GetDbTransaction());
                });

        }

        public async Task PublishThroughEventBusAsync(IntegrationEvent evt)
        {
            this.eventBus.Publish(evt);

            await this.eventLogService.MarkEventAsPublishedAsync(evt);
        }
    }
}