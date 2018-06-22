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
    /// WebApi controller exposing functionality relating to Appreciators
    /// </summary>
    public class AppreciatorsController : ApiController
    {

        /// <summary>
        /// The Appreciator service
        /// </summary>
        private readonly IAppreciatorService appreciatorService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppreciatorsController" /> class.
        /// </summary>
        /// <param name="appreciatorService"></param>
        public AppreciatorsController(IAppreciatorService appreciatorService)
        {
            this.appreciatorService = appreciatorService;
        }

        /// <summary>
        /// Gets a list of Appreciators
        /// </summary>
        /// <param name="queryOptions">The OData query options.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// A list of Appreciators
        /// </returns>
        /// <remarks>
        /// GET: api/appreciators
        /// </remarks>
        public PageResult<Appreciator> Get(ODataQueryOptions<Appreciator> queryOptions, string includes = "")
        {
            return appreciatorService.GetAppreciators(queryOptions, includes);
        }

        /// <summary>
        /// Gets an Appreciator matching the identifier
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// The matching Appreciator
        /// </returns>
        /// <remarks>
        /// GET: api/appreciators/{id}
        /// </remarks>
        public Appreciator Get(string id, string includes = "")
        {
            return appreciatorService.GetAppreciator(id, includes);
        }

        /// <summary>
        /// Adds a new Appreciator
        /// </summary>
        /// <param name="appreciator">The Appreciator.</param>
        /// <param name="updateGraph">if set to <c>true</c> any child objects of the entity will also be persisted.</param>
        /// <returns>
        /// A HttpResponseMessage containing the result of the POST
        /// </returns>
        /// <remarks>
        /// POST: api/Appreciators
        /// </remarks>
        public HttpResponseMessage Post(Appreciator appreciator, bool updateGraph = false)
        {
            string appreciatorId = this.appreciatorService.InsertAppreciator(appreciator, updateGraph);

            // Success, return a uri to the created resource in the response header
            var response = new HttpResponseMessage(HttpStatusCode.Created);
            response.Headers.Location = new Uri(this.Request.RequestUri.AbsoluteUri + "/" + appreciatorId.ToString());

            return response;
        }

        /// <summary>
        /// Updates the specified Appreciator
        /// </summary>
        /// <param name="id">The Appreciator identifier.</param>
        /// <param name="appreciator">The appreciator.</param>
        /// <param name="updateGraph">if set to <c>true</c> any child objects of the entity will also be persisted.</param>
        /// <returns>
        /// A HttpResponseMessage containing the result of the PUT
        /// </returns>
        /// <remarks>
        /// PUT: api/appreciators/{id}
        /// </remarks>
        public HttpResponseMessage Put(string id, Appreciator appreciator, bool updateGraph = false)
        {
            this.appreciatorService.UpdateAppreciator(id, appreciator, updateGraph);

            // Success
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        ///// <summary>
        ///// Deletes the specified identifier.
        ///// </summary>
        ///// <param name="id">The identifier.</param>
        ///// <returns>A HttpResponseMessage containing the result of the DELETE</returns>
        ///// <remarks>
        ///// DELETE: api/appreciators/{id}
        ///// </remarks>
        //public HttpResponseMessage Delete(string id)
        //{
        //    this.appreciatorService.DeleteAppreciator(id);

        //    // Success
        //    return new HttpResponseMessage(HttpStatusCode.OK);
        //}

    }
}