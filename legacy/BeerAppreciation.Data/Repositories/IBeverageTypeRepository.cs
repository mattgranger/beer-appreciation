namespace BeerAppreciation.Data.Repositories
{
    using Domain;
    using EF.Repository;

    public interface IBeverageTypeRepository : IEntityRepository<BeverageType>
    {
    }
}
