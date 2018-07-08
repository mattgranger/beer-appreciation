namespace BeerAppreciation.Core.IntegrationEventLogEF.Services
{
    using System;
    using System.Data.Common;
    using System.Linq;
    using System.Threading.Tasks;
    using EventBus.Events;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Diagnostics;

    public class IntegrationEventLogService : IIntegrationEventLogService
    {
        private readonly IntegrationEventLogContext integrationEventLogContext;
        private readonly DbConnection dbConnection;

        public IntegrationEventLogService(DbConnection dbConnection)
        {
            this.dbConnection = dbConnection?? throw new ArgumentNullException(nameof(dbConnection));
            this.integrationEventLogContext = new IntegrationEventLogContext(
                new DbContextOptionsBuilder<IntegrationEventLogContext>()
                    .UseSqlServer(this.dbConnection)
                    .ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning))
                    .Options);
        }

        public Task SaveEventAsync(IntegrationEvent @event, DbTransaction transaction)
        {
            if(transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction), $"A {typeof(DbTransaction).FullName} is required as a pre-requisite to save the event.");
            }
            
            var eventLogEntry = new IntegrationEventLogEntry(@event);
            
            this.integrationEventLogContext.Database.UseTransaction(transaction);
            this.integrationEventLogContext.IntegrationEventLogs.Add(eventLogEntry);

            return this.integrationEventLogContext.SaveChangesAsync();
        }

        public Task MarkEventAsPublishedAsync(IntegrationEvent @event)
        {
            var eventLogEntry = this.integrationEventLogContext.IntegrationEventLogs.Single(ie => ie.EventId == @event.Id);
            eventLogEntry.TimesSent++;
            eventLogEntry.State = EventStateEnum.Published;

            this.integrationEventLogContext.IntegrationEventLogs.Update(eventLogEntry);

            return this.integrationEventLogContext.SaveChangesAsync();
        }
    }
}
