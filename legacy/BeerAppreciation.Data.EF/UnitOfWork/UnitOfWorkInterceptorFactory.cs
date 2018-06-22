namespace BeerAppreciation.Data.EF.UnitOfWork
{
    using System.Data.Entity;
    using Castle;

    /// <summary>
    /// A factory for creating unit of work, that allows us to intercept the creation process
    /// </summary>
    /// <typeparam name="TDbContext">The type of the database context.</typeparam>
    public class UnitOfWorkInterceptorFactory<TDbContext> : IUnitOfWorkInterceptorFactory<TDbContext>
        where TDbContext : DbContext
    {
        #region Fields and Constants

        /// <summary>
        /// The unit of work castle windsor factory
        /// </summary>
        private readonly IUnitOfWorkFactory<TDbContext> unitOfWorkFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWorkInterceptorFactory{TDbContext}"/> class.
        /// </summary>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        public UnitOfWorkInterceptorFactory(IUnitOfWorkFactory<TDbContext> unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        #endregion

        #region IUnitOfWorkInterceptorFactory Implementation

        /// <summary>
        /// Creates an instance of a unit of work using the castle windsor factory
        /// </summary>
        /// <returns>A unit of work</returns>
        public IUnitOfWork<TDbContext> Create()
        {
            // Use the windsor factory to create the unit of work for the given context
            // This will return the same instance of the UnitOfWork per web request, so multiple calls to this method from multiple service methods will return
            // the same unit of work instance.
            IUnitOfWork<TDbContext> unitOfWork = this.unitOfWorkFactory.Create();

            if (unitOfWork.IsContextDisposed)
            {
                // As the unitOfWork has a PerRequest LifeStyle, the context could already have been disposed if we're using multiple
                // unit of works for a particular request. So need to re-instantiate the context
                unitOfWork.InstantiateContext();
            }

            // Increment the instantiation count, this is to allow us to make calls to other service methods in a transactional way
            unitOfWork.InstantiationCount++;

            return unitOfWork;
        }

        #endregion
    }
}