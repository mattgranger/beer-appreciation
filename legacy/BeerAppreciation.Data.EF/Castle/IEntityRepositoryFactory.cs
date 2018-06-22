namespace BeerAppreciation.Data.EF.Castle
{
    using Domain;
    using Repository;
    using UnitOfWork;

    /// <summary>
    /// A factory for creating generic repositorys of type TEntity
    /// </summary>
    public interface IEntityRepositoryFactory
    {
        /// <summary>
        /// Creates an entity provider instance of the specified TEntity
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TEntityKey">The type of the entity key.</typeparam>
        /// <typeparam name="TDbContext">The type of the database context.</typeparam>
        /// <param name="dbContext">The database context.</param>
        /// <param name="unitOfWork">The unit of work managing the repository.</param>
        /// <returns>
        /// An instance of an entity provider
        /// </returns>
        IEntityRepository<TEntity> Create<TEntity, TEntityKey, TDbContext>(TDbContext dbContext, IUnitOfWork<TDbContext> unitOfWork) where TEntity : class, IEntityWithState<TEntityKey>;

        /// <summary>
        /// Releases the specified entity provider.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TEntityKey">The type of the entity key.</typeparam>
        /// <param name="entityProvider">The entity provider.</param>
        void Release<TEntity, TEntityKey>(IEntityRepository<TEntity> entityProvider) where TEntity : class, IEntityWithState<TEntityKey>;
    }
}
