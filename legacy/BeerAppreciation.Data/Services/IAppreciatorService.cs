namespace BeerAppreciation.Data.Services
{
    using System.Web.Http.OData;
    using System.Web.Http.OData.Query;
    using BeerAppreciation.Domain;

    public interface IAppreciatorService
    {
        /// <summary>
        /// Gets a list of all Appreciators.
        /// </summary>
        /// <param name="queryOptions">The OData query options.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// A list of all Appreciators
        /// </returns>
        PageResult<Appreciator> GetAppreciators(ODataQueryOptions queryOptions, string includes);

        /// <summary>
        /// Adds the Appreciator to the database.
        /// </summary>
        /// <param name="appreciator">The Appreciator.</param>
        /// <param name="updateGraph">if set to <c>true</c> any child objects of the entity will also be persisted.</param>
        /// <returns>The identifier of the appreciator added to database</returns>
        string InsertAppreciator(Appreciator appreciator, bool updateGraph);

        /// <summary>
        /// Gets the Appreciator matching the specified Id
        /// </summary>
        /// <param name="appreciatorId">The Appreciator unique identifier.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// The Appreciator matching the id
        /// </returns>
        Appreciator GetAppreciator(string appreciatorId, string includes);

        /// <summary>
        /// Updates the specified Appreciator.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="appreciator">The Appreciator.</param>
        /// <param name="updateGraph">if set to <c>true</c> any child objects of the entity will also be persisted.</param>
        void UpdateAppreciator(string id, Appreciator appreciator, bool updateGraph);

        ///// <summary>
        ///// Deletes the specified Appreciator.
        ///// </summary>
        ///// <param name="id">The Appreciator identifier.</param>
        //void DeleteAppreciator(string id);
    }
}