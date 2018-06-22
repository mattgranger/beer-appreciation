using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using BeerAppreciation.Core.Collections;
using BeerAppreciation.Domain;

namespace BeerAppreciation.Data.Services
{
    public interface IManufacturerService
    {
        /// <summary>
        /// Gets a list of all Manufacturers.
        /// </summary>
        /// <param name="queryOptions">The OData query options.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// A list of all Manufacturers
        /// </returns>
        PageResult<Manufacturer> GetManufacturers(ODataQueryOptions queryOptions, string includes);

        /// <summary>
        /// Adds the Manufacturer to the database.
        /// </summary>
        /// <param name="manufacturer">The Manufacturer.</param>
        /// <param name="updateGraph">if set to <c>true</c> any child objects of the entity will also be persisted.</param>
        /// <returns>The identifier of the manufacturer added to database</returns>
        int InsertManufacturer(Manufacturer manufacturer, bool updateGraph);

        /// <summary>
        /// Gets the Manufacturer matching the specified Id
        /// </summary>
        /// <param name="manufacturerId">The Manufacturer unique identifier.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// The Manufacturer matching the id
        /// </returns>
        Manufacturer GetManufacturer(int manufacturerId, string includes);

        /// <summary>
        /// Updates the specified Manufacturer.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="manufacturer">The Manufacturer.</param>
        /// <param name="updateGraph">if set to <c>true</c> any child objects of the entity will also be persisted.</param>
        void UpdateManufacturer(int id, Manufacturer manufacturer, bool updateGraph);

        /// <summary>
        /// Deletes the specified Manufacturer.
        /// </summary>
        /// <param name="id">The Manufacturer identifier.</param>
        void DeleteManufacturer(int id);
    }
}
