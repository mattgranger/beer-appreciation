namespace BeerAppreciation.Data.EF.UnitOfWork
{
    using System.Data.Entity;

    /// <summary>
    /// A factory for creating unit of work, that allows us to intercept the creation process
    /// </summary>
    /// <typeparam name="TDbContext">The type of the database context.</typeparam>
    public interface IUnitOfWorkInterceptorFactory<TDbContext> where TDbContext : DbContext
    {
        /// <summary>
        /// Creates the unit of work
        /// </summary>
        /// <returns>A unit of work</returns>
        IUnitOfWork<TDbContext> Create();
    }
}
