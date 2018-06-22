namespace BeerAppreciation.Data.Tests.Mocks
{
    using System.Data.Entity;

    /// <summary>
    /// A DbContext used for unit testing
    /// </summary>
    public class FakeDbContext : DbContext
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeDbContext"/> class.
        /// </summary>
        public FakeDbContext()
        {
            // As we'll be using EF primarily in a disconnected state (i.e. through the WebApi services), turn off
            // lazy loading and proxy creation for optimisation.
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        #endregion

        #region Fake DbSets

        /// <summary>
        /// Gets or sets the simple entities.
        /// </summary>
        public virtual IDbSet<SimpleEntity> SimpleEntities { get; set; }

        /// <summary>
        /// Gets or sets the another simple entities.
        /// </summary>
        public virtual IDbSet<AnotherSimpleEntity> AnotherSimpleEntities { get; set; }

        /// <summary>
        /// Gets or sets the aggregate root entities.
        /// </summary>
        public IDbSet<AggregateRootEntity> AggregateRootEntities { get; set; }

        #endregion
    }
}
