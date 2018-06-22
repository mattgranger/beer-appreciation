using System.Data.Entity;

namespace BeerAppreciation.Data.EF.Castle
{
    using UnitOfWork;

    /// <summary>
    /// The windsor typed factory interface for creating UnitOfWorks
    /// </summary>
    /// <typeparam name="TDbContext">The type of the database context.</typeparam>
    public interface IUnitOfWorkFactory<TDbContext> where TDbContext : DbContext
    {
        /// <summary>
        /// Creates a UnitOfWork instance with the specified DbContext
        /// </summary>
        /// <returns>An instance of a UnitofWork for a </returns>
        IUnitOfWork<TDbContext> Create();

        /// <summary>
        /// Releases the specified unit of work.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        void Release(IUnitOfWork<TDbContext> unitOfWork);
    }
}
