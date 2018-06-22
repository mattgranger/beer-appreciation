using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using BeerAppreciation.Domain;

namespace BeerAppreciation.Data.Services
{
    public interface IBeverageStyleService
    {
        /// <summary>
        /// Gets a list of all BeverageStyles.
        /// </summary>
        /// <param name="queryOptions">The OData query options.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// A list of all BeverageStyles
        /// </returns>
        PageResult<BeverageStyle> GetBeverageStyles(ODataQueryOptions queryOptions, string includes);

        /// <summary>
        /// Adds the BeverageStyle to the database.
        /// </summary>
        /// <param name="beverageStyle">The BeverageStyle.</param>
        /// <param name="updateGraph">if set to <c>true</c> any child objects of the entity will also be persisted.</param>
        /// <returns>The identifier of the beverageStyle added to database</returns>
        int InsertBeverageStyle(BeverageStyle beverageStyle, bool updateGraph);

        /// <summary>
        /// Gets the BeverageStyle matching the specified Id
        /// </summary>
        /// <param name="beverageStyleId">The BeverageStyle unique identifier.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// The BeverageStyle matching the id
        /// </returns>
        BeverageStyle GetBeverageStyle(int beverageStyleId, string includes);

        /// <summary>
        /// Updates the specified BeverageStyle.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="beverageStyle">The BeverageStyle.</param>
        /// <param name="updateGraph">if set to <c>true</c> any child objects of the entity will also be persisted.</param>
        void UpdateBeverageStyle(int id, BeverageStyle beverageStyle, bool updateGraph);

        /// <summary>
        /// Deletes the specified BeverageStyle.
        /// </summary>
        /// <param name="id">The BeverageStyle identifier.</param>
        void DeleteBeverageStyle(int id);   
    }
}
