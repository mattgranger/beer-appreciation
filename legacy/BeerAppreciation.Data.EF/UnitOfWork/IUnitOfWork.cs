namespace BeerAppreciation.Data.EF.UnitOfWork
{
    using Domain;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Core.Objects;
    using Repository;

    /// <summary>
    /// An interface defining a UnitOfWork, the UnitOfWork pattern allows us to make transactional operations against many repositories.
    /// </summary>
    /// <typeparam name="TDbContext">The type of the database context.</typeparam>
    public interface IUnitOfWork<TDbContext> : IDisposable
    {
        /// <summary>
        /// Gets or sets the instantiation count.
        /// </summary>
        int InstantiationCount { get; set; }

        /// <summary>
        /// Gets the database context.
        /// </summary>
        TDbContext DbContext { get; }

        /// <summary>
        /// Gets the explicit database transaction.
        /// </summary>
        DbContextTransaction Transaction { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the context is disposed
        /// </summary>
        bool IsContextDisposed { get; set; }

        /// <summary>
        /// Instantiates the db context.
        /// </summary>
        void InstantiateContext();

        /// <summary>
        /// Gets an entity repository of type TEntity
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TEntityKey">The type of entity's primary key.</typeparam>
        /// <returns>
        /// The repository
        /// </returns>
        IEntityRepository<TEntity> GetRepository<TEntity, TEntityKey>() where TEntity : class, IEntityWithState<TEntityKey>;

        /// <summary>
        /// Saves any modified entities being tracked in the DbContext
        /// </summary>
        /// <returns>A list of the savede entities</returns>
        IList<ObjectStateEntry> Save();

        /// <summary>
        /// Explicitly begins a transaction against the context
        /// </summary>
        /// <returns>The transaction context</returns>
        DbContextTransaction BeginTransaction();

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// Rolls back the transaction.
        /// </summary>
        void RollbackTransaction();
    }
}
