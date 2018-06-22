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
    /// WebApi controller exposing functionality relating to DrinkingClubs
    /// </summary>
    public class DrinkingClubsController : ApiController
    {

        /// <summary>
        /// The DrinkingClub service
        /// </summary>
        private readonly IDrinkingClubService drinkingClubService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrinkingClubsController" /> class.
        /// </summary>
        /// <param name="drinkingClubService"></param>
        public DrinkingClubsController(IDrinkingClubService drinkingClubService)
        {
            this.drinkingClubService = drinkingClubService;
        }

        /// <summary>
        /// Gets a list of DrinkingClubs
        /// </summary>
        /// <param name="queryOptions">The OData query options.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// A list of DrinkingClubs
        /// </returns>
        /// <remarks>
        /// GET: api/drinkingclubs
        /// </remarks>
        public PageResult<DrinkingClub> Get(ODataQueryOptions<DrinkingClub> queryOptions, string includes = "")
        {
            return drinkingClubService.GetDrinkingClubs(queryOptions, includes);
        }

        /// <summary>
        /// Gets an DrinkingClub matching the identifier
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// The matching DrinkingClub
        /// </returns>
        /// <remarks>
        /// GET: api/drinkingclubs/{id}
        /// </remarks>
        public DrinkingClub Get(int id, string includes = "")
        {
            return drinkingClubService.GetDrinkingClub(id, includes);
        }

        /// <summary>
        /// Adds a new DrinkingClub
        /// </summary>
        /// <param name="drinkingClub">The DrinkingClub.</param>
        /// <param name="updateGraph">if set to <c>true</c> any child objects of the entity will also be persisted.</param>
        /// <returns>
        /// A HttpResponseMessage containing the result of the POST
        /// </returns>
        /// <remarks>
        /// POST: api/DrinkingClubs
        /// </remarks>
        public HttpResponseMessage Post(DrinkingClub drinkingClub, bool updateGraph = false)
        {
            int drinkingClubId = this.drinkingClubService.InsertDrinkingClub(drinkingClub, updateGraph);

            // Success, return a uri to the created resource in the response header
            var response = new HttpResponseMessage(HttpStatusCode.Created);
            response.Headers.Location = new Uri(this.Request.RequestUri.AbsoluteUri + "/" + drinkingClubId.ToString());

            return response;
        }

        /// <summary>
        /// Updates the specified DrinkingClub
        /// </summary>
        /// <param name="id">The DrinkingClub identifier.</param>
        /// <param name="drinkingClub">The drinkingClub.</param>
        /// <param name="updateGraph">if set to <c>true</c> any child objects of the entity will also be persisted.</param>
        /// <returns>
        /// A HttpResponseMessage containing the result of the PUT
        /// </returns>
        /// <remarks>
        /// PUT: api/drinkingclubs/{id}
        /// </remarks>
        public HttpResponseMessage Put(int id, DrinkingClub drinkingClub, bool updateGraph = false)
        {
            this.drinkingClubService.UpdateDrinkingClub(id, drinkingClub, updateGraph);

            // Success
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>A HttpResponseMessage containing the result of the DELETE</returns>
        /// <remarks>
        /// DELETE: api/drinkingclubs/{id}
        /// </remarks>
        public HttpResponseMessage Delete(int id)
        {
            this.drinkingClubService.DeleteDrinkingClub(id);

            // Success
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

    }
}