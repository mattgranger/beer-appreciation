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
    public class EventsController : ApiController
    {
        private readonly IEventService eventService;

        public EventsController(IEventService eventService)
        {
            this.eventService = eventService;
        }

        /// <summary>
        /// Gets a list of events
        /// </summary>
        /// <param name="queryOptions">The OData query options.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// A list of events
        /// </returns>
        /// <remarks>
        /// GET: api/events
        /// </remarks>
        public PageResult<Event> Get(ODataQueryOptions<Event> queryOptions, string includes = "")
        {
            return this.eventService.GetEvents(queryOptions, includes);
        }

        /// <summary>
        /// Gets an event matching the identifier
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// The matching event
        /// </returns>
        /// <remarks>
        /// GET: api/events/{id}
        /// </remarks>
        public Event Get(int id)
        {
            return this.eventService.GetEvent(id);
        }

        /// <summary>
        /// Gets an event matching the identifier
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// The matching event
        /// </returns>
        /// <remarks>
        /// GET: api/events/{id}
        /// </remarks>
        [HttpGet]
        [Route("api/events/{id}/updatescores")]
        public Event UpdateEventRatingScores(int id)
        {
            return this.eventService.UpdateEventRatingScores(id);
        }

        /// <summary>
        /// Adds a new event
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>
        /// A HttpResponseMessage containing the result of the POST
        /// </returns>
        /// <remarks>
        /// POST: api/events
        /// </remarks>
        public HttpResponseMessage Post(Event @event)
        {
            var eventId = this.eventService.InsertEvent(@event);

            // Success, return a uri to the created resource in the response header
            var response = new HttpResponseMessage(HttpStatusCode.Created);
            response.Headers.Location = new Uri(this.Request.RequestUri.AbsoluteUri + "/" + eventId);

            return response;
        }

        /// <summary>
        /// Updates the specified event
        /// </summary>
        /// <param name="id">The event identifier.</param>
        /// <param name="event">The event.</param>
        /// <returns>
        /// A HttpResponseMessage containing the result of the POST
        /// </returns>
        /// <remarks>
        /// PUT: api/events/{id}
        /// </remarks>
        public HttpResponseMessage Put(int id, Event @event)
        {
            var utcEventDate = @event.Date.ToUniversalTime();

            this.eventService.UpdateEvent(id, @event);

            // Success
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        /// <summary>
        /// Deletes the specified event.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>A HttpResponseMessage containing the result of the DELETE</returns>
        /// <remarks>
        /// DELETE: api/events/{id}
        /// </remarks>
        public HttpResponseMessage Delete(int id)
        {
            this.eventService.DeleteEvent(id);

            // Success
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
