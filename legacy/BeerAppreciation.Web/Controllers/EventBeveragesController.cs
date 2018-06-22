namespace BeerAppreciation.Web.Controllers
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.OData;
    using System.Web.Http.OData.Query;
    using BeerAppreciation.Data.Services;
    using BeerAppreciation.Domain;

    /// <summary>
    /// WebApi controller exposing functionality relating to EventBeverages
    /// </summary>
    public class EventBeveragesController : ApiController
    {

        /// <summary>
        /// The EventBeverage service
        /// </summary>
        private readonly IEventBeverageService eventBeverageService;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventBeveragesController" /> class.
        /// </summary>
        /// <param name="eventBeverageService"></param>
        public EventBeveragesController(IEventBeverageService eventBeverageService)
        {
            this.eventBeverageService = eventBeverageService;
        }

        /// <summary>
        /// Gets a list of EventBeverages
        /// </summary>
        /// <param name="queryOptions">The OData query options.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// A list of EventBeverages
        /// </returns>
        /// <remarks>
        /// GET: api/eventbeverages/eventbeverages
        /// </remarks>
        public PageResult<EventBeverage> Get(ODataQueryOptions<EventBeverage> queryOptions, string includes = "")
        {
            return eventBeverageService.GetEventBeverages(queryOptions, includes);
        }

        /// <summary>
        /// Gets an EventBeverage matching the identifier
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// The matching EventBeverage
        /// </returns>
        /// <remarks>
        /// GET: api/eventbeverages/eventbeverages/{id}
        /// </remarks>
        public EventBeverage Get(int id, string includes = "")
        {
            return eventBeverageService.GetEventBeverage(id, includes);
        }

        /// <summary>
        /// Adds a new EventBeverage
        /// </summary>
        /// <param name="eventBeverage">The EventBeverage.</param>
        /// <param name="updateGraph">if set to <c>true</c> any child objects of the entity will also be persisted.</param>
        /// <returns>
        /// A HttpResponseMessage containing the result of the POST
        /// </returns>
        /// <remarks>
        /// POST: api/eventbeverages/EventBeverages
        /// </remarks>
        public HttpResponseMessage Post(EventBeverage eventBeverage, bool updateGraph = false)
        {
            int eventBeverageId = this.eventBeverageService.InsertEventBeverage(eventBeverage, updateGraph);

            // Success, return a uri to the created resource in the response header
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Created);
            response.Headers.Location = new Uri(this.Request.RequestUri.AbsoluteUri + "/" + eventBeverageId.ToString());

            return response;
        }

        /// <summary>
        /// Updates the specified EventBeverage
        /// </summary>
        /// <param name="id">The EventBeverage identifier.</param>
        /// <param name="eventBeverage">The eventBeverage.</param>
        /// <param name="updateGraph">if set to <c>true</c> any child objects of the entity will also be persisted.</param>
        /// <returns>
        /// A HttpResponseMessage containing the result of the PUT
        /// </returns>
        /// <remarks>
        /// PUT: api/eventbeverages/eventbeverages/{id}
        /// </remarks>
        public HttpResponseMessage Put(int id, EventBeverage eventBeverage, bool updateGraph = false)
        {
            this.eventBeverageService.UpdateEventBeverage(id, eventBeverage, updateGraph);

            // Success
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>A HttpResponseMessage containing the result of the DELETE</returns>
        /// <remarks>
        /// DELETE: api/eventbeverages/eventbeverages/{id}
        /// </remarks>
        public HttpResponseMessage Delete(int id)
        {
            this.eventBeverageService.DeleteEventBeverage(id);

            // Success
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

    }
}
