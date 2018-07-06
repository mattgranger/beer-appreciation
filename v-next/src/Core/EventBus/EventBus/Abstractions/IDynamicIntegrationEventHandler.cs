namespace BeerAppreciation.Core.EventBus.Abstractions
{
    using System.Threading.Tasks;

    public interface IDynamicIntegrationEventHandler
    {
        Task Handle(dynamic eventData);
    }
}
