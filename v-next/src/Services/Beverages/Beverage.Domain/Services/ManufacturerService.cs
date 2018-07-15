namespace BeerAppreciation.Beverage.Domain.Services
{
    using Core.Shared.Data.Repositories;

    public class ManufacturerService : EntityService<Manufacturer, int>, IManufacturerService
    {
        public ManufacturerService(IEntityRepository<Manufacturer, int> repository) : base(repository)
        {
        }
    }
}