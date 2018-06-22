namespace BeerAppreciation.Data.EF.Repository
{
    using Domain;
    using global::Castle.Core.Internal;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Threading;
    using System.Web.Http.OData;
    using System.Web.Http.OData.Query;
    using UnitOfWork;

    /// <summary>
    /// A generic implementation of an entity repository that supports CRUD operations of any repository of type TEntity
    /// </summary>
    /// <typeparam name="TEntity">The type of the database entity.</typeparam>
    /// <typeparam name="TEntityKey">The type of the entity key.</typeparam>
    /// <typeparam name="TDbContext">The type of the database context.</typeparam>
    [SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Justification = "Framework Code")]
    public class BaseEntityRepository<TEntity, TEntityKey, TDbContext> : IEntityRepository<TEntity>
        where TEntity : class, IEntityWithState<TEntityKey>
        where TDbContext : DbContext
    {
        /// <summary>
        /// The cache lock to provide thread safety for modifying cache
        /// </summary>
        private static readonly ReaderWriterLockSlim CacheLock = new ReaderWriterLockSlim();

        /// <summary>
        /// A list of cached entities
        /// </summary>
        /// <returns></returns>
        private static IList<TEntity> entityCache;

        /// <summary>
        /// The EF data set for the TEntity
        /// </summary>
        private readonly IDbSet<TEntity> dbSet;

        /// <summary>
        /// The unit of work managing this repository
        /// </summary>
        private readonly IUnitOfWork<TDbContext> unitOfWork;

        /// <summary>
        /// If set to true indicates that if a call is made to GetList() with no filter then the results will be cached.
        /// Any subsequent calls to GetList/GetSingle will then work off the cached entities
        /// </summary>
        private readonly bool isCacheable;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseEntityRepository{TEntity, TEntityKey, TDbContext}" /> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="isCacheable">If set to true indicates that if a call is made to GetList() with no filter then the results will be cached.
        /// Any subsequent calls to GetList/GetSingle will then work off the cached entities</param>
        protected BaseEntityRepository(TDbContext dbContext, IUnitOfWork<TDbContext> unitOfWork, bool isCacheable)
            : this(dbContext, dbContext.Set<TEntity>(), unitOfWork, isCacheable)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseEntityRepository{TEntity, TEntityKey, TDbContext}" /> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="dbSet">The dbSet for the particular entity.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="isCacheable">If set to true indicates that if a call is made to GetList() with no filter then the results will be cached.
        /// Any subsequent calls to GetList/GetSingle will then work off the cached entities</param>
        /// <remarks>
        /// This overload is for unit tests to be able to pass in mock implementations of the DbSet to unit test the queryable methods
        /// </remarks>
        protected BaseEntityRepository(TDbContext dbContext, IDbSet<TEntity> dbSet, IUnitOfWork<TDbContext> unitOfWork, bool isCacheable)
        {
            this.DbContext = dbContext;
            this.dbSet = dbSet;
            this.isCacheable = isCacheable;
            this.unitOfWork = unitOfWork;

            // If this is a cacheable service, make a call to Getlist() without any criteria to initially populate the cache
            if (this.isCacheable && EntityCache == null)
            {
                this.GetList();
            }
        }

        /// <summary>
        /// Gets the database context.
        /// </summary>
        public TDbContext DbContext { get; private set; }

        /// <summary>
        /// Gets or sets the entity cache.
        /// </summary>
        private static IList<TEntity> EntityCache
        {
            get
            {
                // Place a read lock while getting the cache and free on exit
                CacheLock.EnterReadLock();

                try
                {
                    return entityCache;
                }
                finally
                {
                    CacheLock.ExitReadLock();
                }
            }

            set
            {
                // Place a write lock while changing the cache and free on exit
                CacheLock.EnterWriteLock();

                try
                {
                    entityCache = value;
                }
                finally
                {
                    CacheLock.ExitWriteLock();
                }
            }
        }

        /// <summary>
        /// Gets all the entities from the repository and applies optional filters and clauses
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="orderBy">The order by clause.</param>
        /// <param name="includeExpressions">The include properties as expressions.</param>
        /// <param name="page">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>
        /// A list of domain entities
        /// </returns>
        public IList<TEntity> GetList(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includeExpressions = null,
            int? page = null,
            int? pageSize = null)
        {
            return this.GetList(filter, orderBy, includeExpressions, null, page, pageSize);
        }

        /// <summary>
        /// Gets all the entities from the repository and applies optional filters and clauses
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="orderBy">The order by clause.</param>
        /// <param name="page">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="includes">The include properties as strings.</param>
        /// <returns>
        /// A list of domain entities
        /// </returns>
        public IList<TEntity> GetList(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int? page = null,
            int? pageSize = null,
            params string[] includes)
        {
            return this.GetList(filter, orderBy, null, includes, page, pageSize);
        }

        /// <summary>
        /// Gets a list of entities by applying the OData query options
        /// </summary>
        /// <param name="queryOptions">The oData query options.</param>
        /// <param name="includes">The includes as a comma separated string.</param>
        /// <returns>
        /// A paged list of entities
        /// </returns>
        public PageResult<TEntity> GetList(ODataQueryOptions queryOptions, string includes)
        {
            int totalCount = default(int);
            IEnumerable<TEntity> entities;

            string[] includesList = !string.IsNullOrWhiteSpace(includes) ? includes.Split(',') : null;
            bool hasIncludes = includesList != null && includesList.Length > 0;

            // Get the IQueryable, will either be against DbSet or the cached list of entities
            IQueryable<TEntity> query = this.GetQueryable(hasIncludes);

            if (includesList != null && includesList.Length > 0)
            {
                includesList.ToList().ForEach(includedProperty => query = query.Include(includedProperty));
            }

            if (queryOptions.Filter != null)
            {
                // Apply just the filter (not any paging) so we can get the total count of records
                IQueryable filteredQueryable = queryOptions.Filter.ApplyTo(query, new ODataQuerySettings());
                totalCount = filteredQueryable.Cast<TEntity>().Count();

                // Apply all the odata query options to get the paged list of entities
                entities = queryOptions.ApplyTo(filteredQueryable, new ODataQuerySettings()).Cast<TEntity>().AsNoTracking().ToList();
            }
            else
            {
                // No filter, so just apply the full odata query
                IQueryable unfilteredQueryable = queryOptions.ApplyTo(query, new ODataQuerySettings());

                // Get the actual entities
                entities = unfilteredQueryable.Cast<TEntity>().AsNoTracking().ToList();

                if (queryOptions.Skip != null || queryOptions.Top != null)
                {
                    // Just get the count
                    totalCount = query.Count();
                }
                else
                {
                    totalCount = entities.Count();
                }
            }

            // Cache the results if the provider is cacheable, and there is no filter or paging restrictions
            if (this.isCacheable && (queryOptions.Filter == null || queryOptions.Filter.FilterClause == null) &&
                queryOptions.Skip == null && queryOptions.Top == null)
            {
                EntityCache = entities.ToList();
            }

            return new PageResult<TEntity>(entities.ToList(), null, totalCount);
        }

        /// <summary>
        /// Gets all the entities from the provider and applies optional filters and clauses
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="orderBy">The order by clause.</param>
        /// <param name="includeExpressions">The include properties as expressions.</param>
        /// <param name="page">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>
        /// A list of domain entities
        /// </returns>
        public PageResult<TEntity> GetPagedList(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includeExpressions = null,
            int page = 0,
            int pageSize = 0)
        {
            return this.GetPagedList(filter, orderBy, includeExpressions, null, page, pageSize);
        }

        /// <summary>
        /// Gets a the first entity from the repository matching the specified query.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="includes">The include properties as a list of expressions.</param>
        /// <returns>
        /// A list of domain entities
        /// </returns>
        public TEntity GetSingle(
            Expression<Func<TEntity, bool>> filter = null,
            params string[] includes)
        {
            return this.GetSingle(filter, null, includes);
        }

        /// <summary>
        /// Gets a the first entity from the repository matching the specified query.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="includes">The include properties as a list of expressions.</param>
        /// <returns>
        /// A list of domain entities
        /// </returns>
        public TEntity GetSingle(
            Expression<Func<TEntity, bool>> filter = null,
            List<Expression<Func<TEntity, object>>> includes = null)
        {
            return this.GetSingle(filter, includes, null);
        }

        /// <summary>
        /// Updates the specified domain entity only, any objects in the graph will not be updated
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Update(TEntity entity)
        {
            // Check whether the entity is already being tracked by the context
            TEntity foundEntity = this.dbSet.Local.FirstOrDefault(e => e.Id.Equals(entity.Id));

            if (foundEntity != null)
            {
                // In context, so get the entry and update values
                this.DbContext.Entry(foundEntity).CurrentValues.SetValues(entity);
            }
            else
            {
                // Entity not in context, so attach and set state to modified
                this.DbContext.Entry(entity).State = EntityState.Modified;
            }

            if (this.unitOfWork != null && this.unitOfWork.Transaction != null)
            {
                this.DbContext.SaveChanges();
            }

            this.ClearCache();
        }

        /// <summary>
        /// Inserts the specified domain entity only, any objects in the graph will not be updated
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Insert(TEntity entity)
        {
            DbEntityEntry entityEntry = this.DbContext.Entry(entity);

            // Can't set the entity straight to added, as this will set ALL child entities in the entity graph to 'Added'. To get around
            // this set to Modified first and then to added.
            entityEntry.State = EntityState.Modified;
            entityEntry.State = EntityState.Added;

            if (this.unitOfWork != null && this.unitOfWork.Transaction != null)
            {
                this.DbContext.SaveChanges();
            }

            this.ClearCache();
        }

        /// <summary>
        /// Inserts or updates the specified domain entity and any entities within its graph. The calling client will need
        /// to set the appropriate state on the domain entities within the graph.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void InsertOrUpdateGraph(TEntity entity)
        {
            // Get a list of entities currently being tracked, as don't want to reprocess them in the code below
            IList<DbEntityEntry> trackedEntrys = this.DbContext.ChangeTracker.Entries().ToList();

            // Adding the entity will mark attach all entities in the graph to the dbContext. It will also ensure all FK relationships are set correctly.
            this.dbSet.Add(entity);

            var changedEntities = this.DbContext.ChangeTracker.Entries();

            // For any entities being tracked, tell the context about the entities state.
            foreach (DbEntityEntry entry in changedEntities)
            {
                if (trackedEntrys.Contains(entry))
                {
                    // This entry was already in the tracker before this method call, so ignore
                    continue;
                }

                if (entry.Entity is IEntityWithState)
                {
                    IEntityWithState entityWithState = (IEntityWithState)entry.Entity;

                    if (entityWithState.State == State.NotSet)
                    {
                        // If the entity states have not been set, then use a convention to determine then entity state based on whether 
                        // the entity has an Id
                        entityWithState.State = CalculateEntityStateBasedOnConvention(entry.Entity);
                    }

                    // Convert the domain representation of state to the entity framework enumeration
                    entry.State = GetEntityState(entityWithState.State);
                }
            }

            if (this.unitOfWork != null && this.unitOfWork.Transaction != null)
            {
                this.DbContext.SaveChanges();
            }

            this.ClearCache();
        }

        /// <summary>
        /// Deletes the specified domain entity from the repository
        /// </summary>
        /// <typeparam name="TKey">The type of the primary key.</typeparam>
        /// <param name="id">The Id of the entity to delete.</param>
        public void Delete<TKey>(TKey id) where TKey : struct
        {
            // Entity Framework only needs a new entity with its primary key set in order to delete a recorde
            TEntity entity = Activator.CreateInstance<TEntity>();
            entity.Id = (TEntityKey)Convert.ChangeType(id, typeof(TKey));

            this.DbContext.Entry(entity).State = EntityState.Deleted;

            if (this.unitOfWork != null && this.unitOfWork.Transaction != null)
            {
                this.DbContext.SaveChanges();
            }

            this.ClearCache();
        }

        /// <summary>
        /// Clears the cache.
        /// </summary>
        public void ClearCache()
        {
            EntityCache = null;
        }

        /// <summary>
        /// Maps the domain State to the entity framework State
        /// </summary>
        /// <param name="state">State of the entity.</param>
        /// <returns>The mapped entity framework State</returns>
        protected static EntityState GetEntityState(State state)
        {
            switch (state)
            {
                case State.Unchanged:
                    return EntityState.Unchanged;
                case State.Added:
                    return EntityState.Added;
                case State.Modified:
                    return EntityState.Modified;
                case State.Deleted:
                    return EntityState.Deleted;
                default:
                    return EntityState.Detached;
            }
        }

        /// <summary>
        /// Calculates the entity state based on convention.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The calculated entity state</returns>
        private static State CalculateEntityStateBasedOnConvention(object entity)
        {
            State entityState = State.Unchanged;

            // As we have no way of knowing the generic type of the entities IEntityWithState<T> interface, we'll need to use reflection to get it
            PropertyInfo idProperty = entity.GetType().GetProperty("Id");

            if (idProperty != null)
            {
                // If the Id property value equals the default of its property type, then its an add
                entityState = idProperty.GetValue(entity, null).ToString() == GetDefaultValue(idProperty.PropertyType).ToString() ? State.Added : State.Modified;
            }

            return entityState;
        }

        /// <summary>
        /// Gets the default value of the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The default value for the type</returns>
        private static object GetDefaultValue(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        /// <summary>
        /// Determines the queryable root on which queries will be performed. This can either be the dbSet (which means query will hit DB) or
        /// the cached list of entities
        /// </summary>
        /// <param name="hasIncludes">Specifies whether includes have been requested on the incoming query.</param>
        /// <returns>
        /// The IQueryable
        /// </returns>
        private IQueryable<TEntity> GetQueryable(bool hasIncludes)
        {
            // Create a read lock
            CacheLock.EnterReadLock();

            try
            {
                // Create the queryable, will either be against the DbSet for this entity (i.e. we'll query the database) or against the list of cached entities.
                // If we have includes, then can't get from cache and will have to hit the database
                return entityCache == null || hasIncludes ? this.dbSet : entityCache.AsQueryable<TEntity>();
            }
            finally
            {
                CacheLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Gets all the entities from the provider and applies optional filters and clauses. Returns results as a paged results set.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="orderBy">The order by clause.</param>
        /// <param name="includeExpressions">The include expressions.</param>
        /// <param name="includes">The include properties.</param>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>
        /// A list of paged domain entities
        /// </returns>
        private PageResult<TEntity> GetPagedList(
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            List<Expression<Func<TEntity, object>>> includeExpressions,
            string[] includes,
            int page,
            int pageSize)
        {
            int totalCount = 0;
            bool hasIncludes = includeExpressions != null || includes != null;

            // Get the IQueryable, will either be against DbSet or the cached list of entities
            IQueryable<TEntity> query = this.GetQueryable(hasIncludes);

            // Add any includes to the query
            if (includeExpressions != null)
            {
                includeExpressions.ForEach(includedProperty => query = query.Include(includedProperty));
            }

            if (includes != null && includes.Length > 0)
            {
                includes.ToList().ForEach(includedProperty => query = query.Include(includedProperty));
            }

            // Apply any filters
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Apply the order by clause
            if (orderBy != null)
            {
                query = orderBy(query);
            }

            // Determine the total results count, this will query the database
            totalCount = query.Count();

            // Apply any paging constraints
            if (page != 0 && pageSize != 0)
            {
                query = query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize);
            }

            // Execute the query with no tracking to improve performance
            IList<TEntity> results = query.AsNoTracking().ToList();

            // Return the paged results
            return new PageResult<TEntity>(results, null, totalCount);
        }

        /// <summary>
        /// Gets all the entities from the repository and applies optional filters and clauses
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="orderBy">The order by clause.</param>
        /// <param name="includeExpressions">The include expressions.</param>
        /// <param name="includes">The include properties.</param>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>
        /// A list of domain entities
        /// </returns>
        private IList<TEntity> GetList(
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            List<Expression<Func<TEntity, object>>> includeExpressions,
            string[] includes,
            int? page,
            int? pageSize)
        {
            bool hasIncludes = includeExpressions != null || includes != null;

            // Get the IQueryable, will either be against DbSet or the cached list of entities
            IQueryable<TEntity> query = this.GetQueryable(hasIncludes);

            // Add any includes to the query
            if (includeExpressions != null)
            {
                includeExpressions.ForEach(includedProperty => query = query.Include(includedProperty));
            }

            if (includes != null && includes.Length > 0)
            {
                includes.ForEach(includedProperty => query = query.Include(includedProperty));
            }

            // Apply any filters
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Apply the order by clause
            if (orderBy != null)
            {
                query = orderBy(query);
            }

            // Apply any paging constraints
            if (page != null && pageSize != null)
            {
                query = query
                    .Skip((page.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);
            }

            // Execute the queryable with no tracking to improve performance
            IList<TEntity> results = query.AsNoTracking().ToList();

            // If repository is cacheable and no filter or paging parameters, then cache
            if (this.isCacheable && filter == null && !page.HasValue && !pageSize.HasValue)
            {
                EntityCache = results;
            }

            return results;
        }

        /// <summary>
        /// Gets a the first entity from the repository matching the specified query.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="includeExpressions">The include expressions.</param>
        /// <param name="includes">The include properties.</param>
        /// <returns>
        /// The first entity matching the where clause
        /// </returns>
        private TEntity GetSingle(
            Expression<Func<TEntity, bool>> filter,
            List<Expression<Func<TEntity, object>>> includeExpressions,
            string[] includes)
        {
            bool hasIncludes = includeExpressions != null || includes != null;

            // Get the IQueryable, will either be against DbSet or the cached list of entities
            IQueryable<TEntity> query = this.GetQueryable(hasIncludes);

            // Add any includes to the query
            if (includeExpressions != null)
            {
                includeExpressions.ForEach(includedProperty => query = query.Include(includedProperty));
            }

            if (includes != null && includes.Length > 0)
            {
                includes.ForEach(includedProperty => query = query.Include(includedProperty));
            }

            // Apply any filters
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Execute the query (turn off tracking for optimisation of query)
            TEntity entity = query.AsNoTracking().FirstOrDefault();

            return entity;
        }
    }
}
