using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using BeerAppreciation.Core.Collections;
using BeerAppreciation.Data.Services;
using BeerAppreciation.Domain;

namespace BeerAppreciation.Web.Controllers
{
    using Core.Security;

    [RoutePrefix("api/beverages")]
    //[Authorize]
    public class BeveragesController : ApiController
    {
        private readonly IBeverageService beverageService;

        public BeveragesController(IBeverageService beverageService)
        {
            this.beverageService = beverageService;
        }

        /// <summary>
        /// Gets a list of beverages
        /// </summary>
        /// <param name="queryOptions">The OData query options.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// A list of beverages
        /// </returns>
        /// <remarks>
        /// GET: api/beverages
        /// </remarks>
        public PageResult<Beverage> Get(ODataQueryOptions<Beverage> queryOptions, string includes = "")
        {
            return this.beverageService.GetBeverages(queryOptions, includes);
        }

        /// <summary>
        /// Gets a beverage matching the identifier
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// The matching beverage
        /// </returns>
        /// <remarks>
        /// GET: api/beverages/{id}
        /// </remarks>
        [Route("{id:int}")]
        public Beverage Get(int id)
        {
            return this.beverageService.GetBeverage(id);
        }

        /// <summary>
        /// Adds a new beverage
        /// </summary>
        /// <param name="beverage">The beverage.</param>
        /// <returns>
        /// A HttpResponseMessage containing the result of the POST
        /// </returns>
        /// <remarks>
        /// POST: api/beverages
        /// </remarks>
        //[Authorize(Roles = RoleNames.Member)]
        public HttpResponseMessage Post(Beverage beverage)
        {
            var beverageId = this.beverageService.InsertBeverage(beverage);

            // Success, return a uri to the created resource in the response header
            var response = new HttpResponseMessage(HttpStatusCode.Created);
            response.Headers.Add("EntityId", new[] { beverageId.ToString() });
            response.Headers.Location = new Uri(this.Request.RequestUri.AbsoluteUri + "/" + beverageId);

            return response;
        }

        /// <summary>
        /// Updates the specified beverage
        /// </summary>
        /// <param name="id">The event identifier.</param>
        /// <param name="beverage">The beverage.</param>
        /// <returns>
        /// A HttpResponseMessage containing the result of the POST
        /// </returns>
        /// <remarks>
        /// PUT: api/beverages/{id}
        /// </remarks>
        [Route("{id:int}")]
        //[Authorize(Roles = RoleNames.Member)]
        public HttpResponseMessage Put(int id, [FromBody]Beverage beverage)
        {
            this.beverageService.UpdateBeverage(id, beverage);

            // Success
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        /// <summary>
        /// Deletes the specified beverage.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>A HttpResponseMessage containing the result of the DELETE</returns>
        /// <remarks>
        /// DELETE: api/beverages/{id}
        /// </remarks>
        [Route("{id:int}")]
        [HttpDelete]
        [Authorize(Roles = RoleNames.Admin)]
        public HttpResponseMessage Delete(int id)
        {
            this.beverageService.DeleteBeverage(id);

            // Success
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
