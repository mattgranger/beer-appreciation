namespace BeerAppreciation.Core.IntegrationEventLogEF.Services
{
    using System.Data.Common;
    using System.Threading.Tasks;
    using EventBus.Events;

    public interface IIntegrationEventLogService
    {
        Task SaveEventAsync(IntegrationEvent @event, DbTransaction transaction);
        Task MarkEventAsPublishedAsync(IntegrationEvent @event);
    }
}
