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

    /// <summary>
    /// WebApi controller exposing functionality relating to BeverageTypes
    /// </summary>
    public class BeverageTypesController : ApiController
    {

        /// <summary>
        /// The BeverageType service
        /// </summary>
        private readonly IBeverageTypeService beverageTypeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BeverageTypesController" /> class.
        /// </summary>
        /// <param name="beverageTypeService"></param>
        public BeverageTypesController(IBeverageTypeService beverageTypeService)
        {
            this.beverageTypeService = beverageTypeService;
        }

        /// <summary>
        /// Gets a list of BeverageTypes
        /// </summary>
        /// <param name="queryOptions">The OData query options.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// A list of BeverageTypes
        /// </returns>
        /// <remarks>
        /// GET: api/beveragetypes
        /// </remarks>
        public PageResult<BeverageType> Get(ODataQueryOptions<BeverageType> queryOptions, string includes = "")
        {
            return beverageTypeService.GetBeverageTypes(queryOptions, includes);
        }

        /// <summary>
        /// Gets an BeverageType matching the identifier
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// The matching BeverageType
        /// </returns>
        /// <remarks>
        /// GET: api/beveragetypes/{id}
        /// </remarks>
        public BeverageType Get(int id, string includes = "")
        {
            return beverageTypeService.GetBeverageType(id, includes);
        }

        /// <summary>
        /// Adds a new BeverageType
        /// </summary>
        /// <param name="beverageType">The BeverageType.</param>
        /// <param name="updateGraph">if set to <c>true</c> any child objects of the entity will also be persisted.</param>
        /// <returns>
        /// A HttpResponseMessage containing the result of the POST
        /// </returns>
        /// <remarks>
        /// POST: api/BeverageTypes
        /// </remarks>
        public HttpResponseMessage Post(BeverageType beverageType, bool updateGraph = false)
        {
            int beverageTypeId = this.beverageTypeService.InsertBeverageType(beverageType, updateGraph);

            // Success, return a uri to the created resource in the response header
            var response = new HttpResponseMessage(HttpStatusCode.Created);
            response.Headers.Location = new Uri(this.Request.RequestUri.AbsoluteUri + "/" + beverageTypeId.ToString());

            return response;
        }

        /// <summary>
        /// Updates the specified BeverageType
        /// </summary>
        /// <param name="id">The BeverageType identifier.</param>
        /// <param name="beverageType">The beverageType.</param>
        /// <param name="updateGraph">if set to <c>true</c> any child objects of the entity will also be persisted.</param>
        /// <returns>
        /// A HttpResponseMessage containing the result of the PUT
        /// </returns>
        /// <remarks>
        /// PUT: api/beveragetypes/{id}
        /// </remarks>
        public HttpResponseMessage Put(int id, BeverageType beverageType, bool updateGraph = false)
        {
            this.beverageTypeService.UpdateBeverageType(id, beverageType, updateGraph);

            // Success
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>A HttpResponseMessage containing the result of the DELETE</returns>
        /// <remarks>
        /// DELETE: api/beveragetypes/{id}
        /// </remarks>
        public HttpResponseMessage Delete(int id)
        {
            this.beverageTypeService.DeleteBeverageType(id);

            // Success
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

    }
}
