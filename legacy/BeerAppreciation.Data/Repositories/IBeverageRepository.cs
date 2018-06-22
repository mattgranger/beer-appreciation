namespace BeerAppreciation.Data.Repositories
{
    using Domain;
    using EF.Repository;

    public interface IBeverageRepository : IEntityRepository<Beverage>
    {
    }
}
