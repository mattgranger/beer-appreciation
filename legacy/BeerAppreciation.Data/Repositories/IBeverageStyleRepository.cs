namespace BeerAppreciation.Data.Repositories
{
    using Domain;
    using EF.Repository;

    public interface IBeverageStyleRepository : IEntityRepository<BeverageStyle>
    {
    }
}
