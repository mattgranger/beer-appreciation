namespace BeerAppreciation.Web.Controllers
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.OData;
    using System.Web.Http.OData.Query;
    using Data.Services;
    using Domain;
    
    public class ManufacturersController : ApiController
    {
        /// <summary>
        /// The Manufacturer service
        /// </summary>
        private readonly IManufacturerService manufacturerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManufacturersController" /> class.
        /// </summary>
        /// <param name="manufacturerService"></param>
        public ManufacturersController(IManufacturerService manufacturerService)
        {
            this.manufacturerService = manufacturerService;
        }

        /// <summary>
        /// Gets a list of Manufacturers
        /// </summary>
        /// <param name="queryOptions">The OData query options.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// A list of Manufacturers
        /// </returns>
        /// <remarks>
        /// GET: api/manufacturers
        /// </remarks>
        public PageResult<Manufacturer> Get(ODataQueryOptions<Manufacturer> queryOptions, string includes = "")
        {
            return manufacturerService.GetManufacturers(queryOptions, includes);
        }

        /// <summary>
        /// Gets an Manufacturer matching the identifier
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// The matching Manufacturer
        /// </returns>
        /// <remarks>
        /// GET: api/manufacturers/{id}
        /// </remarks>
        public Manufacturer Get(int id, string includes = "Beverages")
        {
            return manufacturerService.GetManufacturer(id, includes);
        }

        /// <summary>
        /// Adds a new Manufacturer
        /// </summary>
        /// <param name="manufacturer">The Manufacturer.</param>
        /// <param name="updateGraph">if set to <c>true</c> any child objects of the entity will also be persisted.</param>
        /// <returns>
        /// A HttpResponseMessage containing the result of the POST
        /// </returns>
        /// <remarks>
        /// POST: api/Manufacturers
        /// </remarks>
        public HttpResponseMessage Post(Manufacturer manufacturer, bool updateGraph = false)
        {
            int manufacturerId = this.manufacturerService.InsertManufacturer(manufacturer, updateGraph);

            // Success, return a uri to the created resource in the response header
            var response = new HttpResponseMessage(HttpStatusCode.Created);
            response.Headers.Location = new Uri(this.Request.RequestUri.AbsoluteUri + "/" + manufacturerId.ToString());

            return response;
        }

        /// <summary>
        /// Updates the specified Manufacturer
        /// </summary>
        /// <param name="id">The Manufacturer identifier.</param>
        /// <param name="manufacturer">The manufacturer.</param>
        /// <param name="updateGraph">if set to <c>true</c> any child objects of the entity will also be persisted.</param>
        /// <returns>
        /// A HttpResponseMessage containing the result of the PUT
        /// </returns>
        /// <remarks>
        /// PUT: api/manufacturers/{id}
        /// </remarks>
        public HttpResponseMessage Put(int id, Manufacturer manufacturer, bool updateGraph = false)
        {
            this.manufacturerService.UpdateManufacturer(id, manufacturer, updateGraph);

            // Success
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>A HttpResponseMessage containing the result of the DELETE</returns>
        /// <remarks>
        /// DELETE: api/manufacturers/{id}
        /// </remarks>
        public HttpResponseMessage Delete(int id)
        {
            this.manufacturerService.DeleteManufacturer(id);

            // Success
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
