namespace BeerAppreciation.Data.Tests.Mocks
{
    using EF.Repository;

    /// <summary>
    /// Entity repository for a SimpleEntity used just for unit tests
    /// </summary>
    internal class AggregateRootEntityRepository : BaseEntityRepository<AggregateRootEntity, int, FakeDbContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateRootEntityRepository" /> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public AggregateRootEntityRepository(FakeDbContext dbContext)
            : base(dbContext, null, false)
        {
        }
    }
}
