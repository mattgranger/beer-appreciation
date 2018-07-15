namespace BeerAppreciation.Beverage.Data.Repositories
{
    using Core.Shared.Data.Repositories;
    using Domain;

    public interface IBeverageRepository : IEntityRepository<Beverage, int>
    {
    }
}
