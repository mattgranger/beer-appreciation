using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using BeerAppreciation.Data.Services;
using BeerAppreciation.Domain;

namespace BeerAppreciation.Web.Controllers
{
    public class BeverageStylesController : ApiController
    {
        /// <summary>
        /// The BeverageStyle service
        /// </summary>
        private readonly IBeverageStyleService beverageStyleService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BeverageStylesController" /> class.
        /// </summary>
        /// <param name="beverageStyleService"></param>
        public BeverageStylesController(IBeverageStyleService beverageStyleService)
        {
            this.beverageStyleService = beverageStyleService;
        }

        /// <summary>
        /// Gets a list of BeverageStyles
        /// </summary>
        /// <param name="queryOptions">The OData query options.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// A list of BeverageStyles
        /// </returns>
        /// <remarks>
        /// GET: api/beveragestyles
        /// </remarks>
        public PageResult<BeverageStyle> Get(ODataQueryOptions<BeverageStyle> queryOptions, string includes = "")
        {
            return beverageStyleService.GetBeverageStyles(queryOptions, includes);
        }

        /// <summary>
        /// Gets an BeverageStyle matching the identifier
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// The matching BeverageStyle
        /// </returns>
        /// <remarks>
        /// GET: api/beveragestyles/{id}
        /// </remarks>
        public BeverageStyle Get(int id, string includes = "")
        {
            return beverageStyleService.GetBeverageStyle(id, includes);
        }

        /// <summary>
        /// Adds a new BeverageStyle
        /// </summary>
        /// <param name="beverageStyle">The BeverageStyle.</param>
        /// <param name="updateGraph">if set to <c>true</c> any child objects of the entity will also be persisted.</param>
        /// <returns>
        /// A HttpResponseMessage containing the result of the POST
        /// </returns>
        /// <remarks>
        /// POST: api/BeverageStyles
        /// </remarks>
        public HttpResponseMessage Post(BeverageStyle beverageStyle, bool updateGraph = false)
        {
            int beverageStyleId = this.beverageStyleService.InsertBeverageStyle(beverageStyle, updateGraph);

            // Success, return a uri to the created resource in the response header
            var response = new HttpResponseMessage(HttpStatusCode.Created);
            response.Headers.Location = new Uri(this.Request.RequestUri.AbsoluteUri + "/" + beverageStyleId.ToString());

            return response;
        }

        /// <summary>
        /// Updates the specified BeverageStyle
        /// </summary>
        /// <param name="id">The BeverageStyle identifier.</param>
        /// <param name="beverageStyle">The beverageStyle.</param>
        /// <param name="updateGraph">if set to <c>true</c> any child objects of the entity will also be persisted.</param>
        /// <returns>
        /// A HttpResponseMessage containing the result of the PUT
        /// </returns>
        /// <remarks>
        /// PUT: api/beveragestyles/{id}
        /// </remarks>
        public HttpResponseMessage Put(int id, BeverageStyle beverageStyle, bool updateGraph = false)
        {
            this.beverageStyleService.UpdateBeverageStyle(id, beverageStyle, updateGraph);

            // Success
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>A HttpResponseMessage containing the result of the DELETE</returns>
        /// <remarks>
        /// DELETE: api/beveragestyles/{id}
        /// </remarks>
        public HttpResponseMessage Delete(int id)
        {
            this.beverageStyleService.DeleteBeverageStyle(id);

            // Success
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
