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
    /// WebApi controller exposing functionality relating to Ratings
    /// </summary>
    public class RatingsController : ApiController
    {

        /// <summary>
        /// The Rating service
        /// </summary>
        private readonly IRatingService ratingService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RatingsController" /> class.
        /// </summary>
        /// <param name="ratingService"></param>
        public RatingsController(IRatingService ratingService)
        {
            this.ratingService = ratingService;
        }

        /// <summary>
        /// Gets a list of Ratings
        /// </summary>
        /// <param name="queryOptions">The OData query options.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// A list of Ratings
        /// </returns>
        /// <remarks>
        /// GET: api/ratings
        /// </remarks>
        public PageResult<Rating> Get(ODataQueryOptions<Rating> queryOptions, string includes = "")
        {
            return ratingService.GetRatings(queryOptions, includes);
        }

        /// <summary>
        /// Gets an Rating matching the identifier
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// The matching Rating
        /// </returns>
        /// <remarks>
        /// GET: api/ratings/{id}
        /// </remarks>
        public Rating Get(int id, string includes = "")
        {
            return ratingService.GetRating(id, includes);
        }

        /// <summary>
        /// Adds a new Rating
        /// </summary>
        /// <param name="rating">The Rating.</param>
        /// <param name="updateGraph">if set to <c>true</c> any child objects of the entity will also be persisted.</param>
        /// <returns>
        /// A HttpResponseMessage containing the result of the POST
        /// </returns>
        /// <remarks>
        /// POST: api/ratings
        /// </remarks>
        public HttpResponseMessage Post(Rating rating, bool updateGraph = false)
        {
            int ratingId = this.ratingService.InsertRating(rating, updateGraph);

            // Success, return a uri to the created resource in the response header
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Created);
            response.Headers.Add("EntityId", new[] { ratingId.ToString() }); 
            response.Headers.Location = new Uri(this.Request.RequestUri.AbsoluteUri + "/" + ratingId.ToString());

            return response;
        }

        /// <summary>
        /// Updates the specified Rating
        /// </summary>
        /// <param name="id">The Rating identifier.</param>
        /// <param name="rating">The rating.</param>
        /// <param name="updateGraph">if set to <c>true</c> any child objects of the entity will also be persisted.</param>
        /// <returns>
        /// A HttpResponseMessage containing the result of the PUT
        /// </returns>
        /// <remarks>
        /// PUT: api/ratings/{id}
        /// </remarks>
        public HttpResponseMessage Put(int id, Rating rating, bool updateGraph = false)
        {
            this.ratingService.UpdateRating(id, rating, updateGraph);

            // Success
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>A HttpResponseMessage containing the result of the DELETE</returns>
        /// <remarks>
        /// DELETE: api/ratings/{id}
        /// </remarks>
        public HttpResponseMessage Delete(int id)
        {
            this.ratingService.DeleteRating(id);

            // Success
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

    }
}
