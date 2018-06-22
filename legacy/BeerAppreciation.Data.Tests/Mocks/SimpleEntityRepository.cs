namespace BeerAppreciation.Data.Tests.Mocks
{
    using System.Data.Entity;
    using EF;
    using EF.Repository;

    /// <summary>
    /// Entity repository used just for unit tests
    /// </summary>
    internal class SimpleEntityRepository : BaseEntityRepository<SimpleEntity, int, FakeDbContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleEntityRepository" /> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public SimpleEntityRepository(FakeDbContext dbContext)
            : base(dbContext, null, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleEntityRepository" /> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="dbSet">The database set.</param>
        public SimpleEntityRepository(FakeDbContext dbContext, IDbSet<SimpleEntity> dbSet)
            : base(dbContext, dbSet, null, true)
        {
        }
    }
}
