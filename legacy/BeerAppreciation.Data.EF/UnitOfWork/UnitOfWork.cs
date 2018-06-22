namespace BeerAppreciation.Data.EF.UnitOfWork
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Castle;
    using Domain;
    using Repository;

    /// <summary>
    /// An implementation of the UnitOfWork pattern allowing us to make transactional operations against many repositories.
    /// </summary>
    /// <typeparam name="TDbContext">The type of the database context.</typeparam>
    public class UnitOfWork<TDbContext> : IUnitOfWork<TDbContext> where TDbContext : DbContext
    {
        /// <summary>
        /// The entity framework object context
        /// </summary>
        private readonly ObjectContext objectContext;

        /// <summary>
        /// The entity repository factory
        /// </summary>
        private readonly IEntityRepositoryFactory entityRepositoryFactory;

        /// <summary>
        /// A hashtable to store instantiated repositories.
        /// </summary>
        private readonly Hashtable repositories = new Hashtable();

        /// <summary>
        /// The transaction
        /// </summary>
        private DbContextTransaction transaction;

        /// <summary>
        /// A count of how deep in a nested transaction the unit of work currently is
        /// </summary>
        private int transactionDepth;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork{TDbContext}" /> class.
        /// </summary>
        /// <param name="entityRepositoryFactory">The entity repository factory.</param>
        public UnitOfWork(IEntityRepositoryFactory entityRepositoryFactory)
        {
            // The unit of work is responsible for creating/disposing the DbContext
            this.InstantiateContext();

            this.entityRepositoryFactory = entityRepositoryFactory;
            this.objectContext = ((IObjectContextAdapter)this.DbContext).ObjectContext;
        }

        /// <summary>
        /// Gets or sets the instantiation count.
        /// </summary>
        public int InstantiationCount { get; set; }

        /// <summary>
        /// Gets the database context.
        /// </summary>
        public TDbContext DbContext { get; private set; }

        /// <summary>
        /// Gets the explicit database transaction.
        /// </summary>
        public DbContextTransaction Transaction
        {
            get
            {
                return this.transaction;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the context is disposed
        /// </summary>
        public bool IsContextDisposed { get; set; }

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        /// <returns>The transaction context</returns>
        public DbContextTransaction BeginTransaction()
        {
            if (this.transaction == null)
            {
                // Start a transaction if one is not currently started
                this.transaction = this.DbContext.Database.BeginTransaction();
                this.transactionDepth = 1;
            }
            else
            {
                // Already in a transaction, so increase the transaction depth
                this.transactionDepth++;
            }

            return this.transaction;
        }

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        public void CommitTransaction()
        {
            // Only commit if the unit of work is executing the last commit of a nested transaction
            if (this.transaction != null)
            {
                if (this.transactionDepth == 1)
                {
                    // Commit the transaction and clean up
                    this.transaction.Commit();

                    this.transaction.Dispose();
                    this.transaction = null;
                }
                else
                {
                    this.transactionDepth--;
                }
            }
        }

        /// <summary>
        /// Rolls back the transaction.
        /// </summary>
        public void RollbackTransaction()
        {
            if (this.transaction != null)
            {
                // Even if we're in a nested transaction (i.e. transactionDepth > 1) rollback the transaction and clean up
                this.transaction.Rollback();

                this.transaction.Dispose();
                this.transaction = null;
                this.transactionDepth = 0;
            }
        }

        /// <summary>
        /// Instantiates the db context.
        /// </summary>
        public void InstantiateContext()
        {
            // The unit of work is responsible for creating/disposing the DbContext
            this.DbContext = Activator.CreateInstance<TDbContext>();
            this.IsContextDisposed = false;
        }

        /// <summary>
        /// Gets an entity repository of type TEntity
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TEntityKey">The type of entity's primary key.</typeparam>
        /// <returns>
        /// The repository
        /// </returns>
        public IEntityRepository<TEntity> GetRepository<TEntity, TEntityKey>() where TEntity : class, IEntityWithState<TEntityKey>
        {
            IEntityRepository<TEntity> repository;

            if (!this.repositories.ContainsKey(typeof(TEntity)))
            {
                // Use the windsor castle factory to create an instance of a generic repository of type <TEntity>
                repository = this.entityRepositoryFactory.Create<TEntity, TEntityKey, TDbContext>(this.DbContext, this);

                this.repositories.Add(typeof(TEntity), repository);
            }
            else
            {
                repository = (IEntityRepository<TEntity>)this.repositories[typeof(TEntity)];
            }

            return repository;
        }

        /// <summary>
        /// Saves any modified entities being tracked in the DbContext
        /// </summary>
        /// <returns>A list of the persisted entities</returns>
        [SuppressMessage("Microsoft.Usage", "CA2200:RethrowToPreserveStackDetails", Justification = "Editing the exception before throwing")]
        public IList<ObjectStateEntry> Save()
        {
            try
            {
                // Only save changes if instantiation count is 1, or we're in an explict transaction
                if (this.InstantiationCount == 1)
                {
                    // Get the entities that have changed and return, these will be updated with any database generated primary keys.
                    IList<ObjectStateEntry> changedEntities = this.objectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Added | EntityState.Modified | EntityState.Deleted).ToList();

                    // Persist the changes to database
                    this.DbContext.SaveChanges();

                    return changedEntities;
                }

                return new List<ObjectStateEntry>();
            }
            catch (Exception exception)
            {
                // Localte the actual database exception
                Exception innerException = exception;

                while (innerException != null)
                {
                    if (innerException.Source == ".Net SqlClient Data Provider")
                    {
                        break;
                    }

                    innerException = innerException.InnerException;
                }

                throw;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Only dispose of the context if this is that last unit of work in the transaction
                if (this.InstantiationCount == 1)
                {
                    // If we're in a transaction, then roll it back as calling service should have called Commit
                    if (this.transaction != null)
                    {
                        this.RollbackTransaction();
                    }

                    // Make sure the context is disposed
                    this.DbContext.Dispose();
                    this.DbContext = null;
                    this.IsContextDisposed = true;
                }

                this.InstantiationCount--;
            }
        }
    }
}
