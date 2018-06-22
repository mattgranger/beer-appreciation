using System.Collections.Generic;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using BeerAppreciation.Domain;

namespace BeerAppreciation.Data.Services
{
    /// <summary>
    ///  Interface of service for Beverage related functionality
    /// </summary>
    public interface IBeverageService
    {
        /// <summary>
        /// Gets a list of all beverages.
        /// </summary>
        /// <param name="queryOptions">The OData query options.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// A paged list of all beverages
        /// </returns>
        PageResult<Beverage> GetBeverages(ODataQueryOptions<Beverage> queryOptions, string includes);

        /// <summary>
        /// Adds the beverage to the database.
        /// </summary>
        /// <param name="beverage">The beverage.</param>
        /// <returns>The identifier of the beverage added to database</returns>
        int InsertBeverage(Beverage beverage);

        /// <summary>
        /// Gets the beverage matching the specified Id
        /// </summary>
        /// <param name="beverageId">The beverage unique identifier.</param>
        /// <returns>The beverage matching the id</returns>
        Beverage GetBeverage(int beverageId);

        /// <summary>
        /// Updates the specified beverage.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="beverage">The beverage.</param>
        void UpdateBeverage(int id, Beverage beverage);

        /// <summary>
        /// Deletes the specified beverage.
        /// </summary>
        /// <param name="id">The beverage identifier.</param>
        void DeleteBeverage(int id);
    }
}
