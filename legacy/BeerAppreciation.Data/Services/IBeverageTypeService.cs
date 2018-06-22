namespace BeerAppreciation.Data.Services
{
    using System.Web.Http.OData;
    using System.Web.Http.OData.Query;
    using Domain;

    public interface IBeverageTypeService
    {
        /// <summary>
        /// Gets a list of all BeverageTypes.
        /// </summary>
        /// <param name="queryOptions">The OData query options.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// A list of all BeverageTypes
        /// </returns>
        PageResult<BeverageType> GetBeverageTypes(ODataQueryOptions queryOptions, string includes);

        /// <summary>
        /// Adds the BeverageType to the database.
        /// </summary>
        /// <param name="beverageType">The BeverageType.</param>
        /// <param name="updateGraph">if set to <c>true</c> any child objects of the entity will also be persisted.</param>
        /// <returns>The identifier of the beverageType added to database</returns>
        int InsertBeverageType(BeverageType beverageType, bool updateGraph);

        /// <summary>
        /// Gets the BeverageType matching the specified Id
        /// </summary>
        /// <param name="beverageTypeId">The BeverageType unique identifier.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// The BeverageType matching the id
        /// </returns>
        BeverageType GetBeverageType(int beverageTypeId, string includes);

        /// <summary>
        /// Updates the specified BeverageType.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="beverageType">The BeverageType.</param>
        /// <param name="updateGraph">if set to <c>true</c> any child objects of the entity will also be persisted.</param>
        void UpdateBeverageType(int id, BeverageType beverageType, bool updateGraph);

        /// <summary>
        /// Deletes the specified BeverageType.
        /// </summary>
        /// <param name="id">The BeverageType identifier.</param>
        void DeleteBeverageType(int id);
    }
}