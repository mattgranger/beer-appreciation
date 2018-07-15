namespace BeerAppreciation.Beverage.Domain.Repositories
{
    using Core.Shared.Data.Repositories;

    public interface IBeverageRepository : IEntityRepository<Beverage, int>
    {
    }
}
