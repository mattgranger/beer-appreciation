namespace BeerAppreciation.Data.EF.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web.Http.OData;
    using System.Web.Http.OData.Query;

    /// <summary>
    /// Defines the repository pattern interface for use to manage CRUD operations
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IEntityRepository<TEntity> where TEntity : class
    {
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
        IList<TEntity> GetList(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includeExpressions = null,
            int? page = null,
            int? pageSize = null);

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
        IList<TEntity> GetList(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int? page = null,
            int? pageSize = null,
            params string[] includes);

        /// <summary>
        /// Gets a list of entities by applying the OData query options
        /// </summary>
        /// <param name="queryOptions">The oData query options.</param>
        /// <param name="includes">The includes as a comma separated string.</param>
        /// <returns>
        /// A paged list of entities
        /// </returns>
        PageResult<TEntity> GetList(ODataQueryOptions queryOptions, string includes);

        /// <summary>
        /// Gets all the entities from the provider and applies optional filters and clauses
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="orderBy">The order by clause.</param>
        /// <param name="includeExpressions">The include properties as expressions.</param>
        /// <param name="page">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>
        /// A paged list of domain entities
        /// </returns>
        PageResult<TEntity> GetPagedList(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includeExpressions = null,
            int page = 0,
            int pageSize = 0);

        /// <summary>
        /// Gets a the first entity from the repository matching the specified query.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="includes">The include properties as a list of expressions.</param>
        /// <returns>
        /// A list of domain entities
        /// </returns>
        TEntity GetSingle(
            Expression<Func<TEntity, bool>> filter = null,
            List<Expression<Func<TEntity, object>>> includes = null);

        /// <summary>
        /// Gets a the first entity from the repository matching the specified query.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="includes">The include properties as a list of expressions.</param>
        /// <returns>
        /// A list of domain entities
        /// </returns>
        TEntity GetSingle(
            Expression<Func<TEntity, bool>> filter = null,
            params string[] includes);

        /// <summary>
        /// Updates the specified domain entity only, any objects in the graph will not be updated
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Update(TEntity entity);

        /// <summary>
        /// Inserts the specified domain entity only, any objects in the graph will not be updated
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Insert(TEntity entity);

        /// <summary>
        /// Inserts or updates the specified domain entity and any entities within its graph. The calling client will need
        /// to set the appropriate state on the domain entities within the graph.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void InsertOrUpdateGraph(TEntity entity);

        /// <summary>
        /// Deletes the specified entity from the repository
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="id">The Id of the entity to delete.</param>
        void Delete<TKey>(TKey id) where TKey : struct;

        /// <summary>
        /// Clears the cache.
        /// </summary>
        void ClearCache();
    }
}
