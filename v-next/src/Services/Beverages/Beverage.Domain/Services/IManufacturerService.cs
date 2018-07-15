namespace BeerAppreciation.Beverage.Domain.Services
{
    using Core.Data.Repositories;

    public interface IManufacturerService : IGenericRepository<Beverage, int>
    {
    }
}
