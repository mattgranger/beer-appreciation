namespace BeerAppreciation.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.OData;
    using System.Web.Http.OData.Query;
    using Data.Services;
    using Domain;

    public class EventRegistrationsController : ApiController
    {
        private readonly IEventRegistrationService registrationService;

        public EventRegistrationsController(IEventRegistrationService registrationService)
        {
            this.registrationService = registrationService;
        }

        /// <summary>
        /// Gets a list of event registrations
        /// </summary>
        /// <param name="queryOptions">The OData query options.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// A list of event registrations
        /// </returns>
        /// <remarks>
        /// GET: api/eventregistrations
        /// </remarks>
        public PageResult<EventRegistration> Get(ODataQueryOptions<EventRegistration> queryOptions, string includes = "")
        {
            return this.registrationService.GetEventRegistrations(queryOptions, includes);
        }

        /// <summary>
        /// Gets an event registration matching the identifier
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="includes"></param>
        /// <returns>
        /// The matching registration
        /// </returns>
        /// <remarks>
        /// GET: api/eventregistrations/{id}
        /// </remarks>
        public EventRegistration Get(int id, string includes = "")
        {
            return this.registrationService.GetEventRegistration(id, includes);
        }

        /// <summary>
        /// Adds a new event registration
        /// </summary>
        /// <param name="eventRegistration">The event registration.</param>
        /// <param name="updateGraph"></param>
        /// <returns>
        /// A HttpResponseMessage containing the result of the POST
        /// </returns>
        /// <remarks>
        /// POST: api/eventregistrations
        /// </remarks>
        public HttpResponseMessage Post(EventRegistration eventRegistration, bool updateGraph = false)
        {
            var eventRegistrationId = this.registrationService.InsertEventRegistration(eventRegistration, updateGraph);

            // Success, return a uri to the created resource in the response header
            var response = new HttpResponseMessage(HttpStatusCode.Created);
            response.Headers.Add("EntityId", new[] { eventRegistrationId.ToString() });
            response.Headers.Location = new Uri(this.Request.RequestUri.AbsoluteUri + "/" + eventRegistrationId);

            return response;
        }

        /// <summary>
        /// Updates the specified event registration
        /// </summary>
        /// <param name="id">The event identifier.</param>
        /// <param name="eventRegistration"></param>
        /// <param name="updateGraph"></param>
        /// <returns>
        /// A HttpResponseMessage containing the result of the POST
        /// </returns>
        /// <remarks>
        /// PUT: api/eventregistrations/{id}
        /// </remarks>
        public HttpResponseMessage Put(int id, EventRegistration eventRegistration, bool updateGraph = true)
        {
            var utcEventDate = eventRegistration.RegistrationDate.ToUniversalTime();

            this.registrationService.UpdateEventRegistration(id, eventRegistration, updateGraph);

            // Success
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        /// <summary>
        /// Deletes the specified event registration.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>A HttpResponseMessage containing the result of the DELETE</returns>
        /// <remarks>
        /// DELETE: api/eventregistrations/{id}
        /// </remarks>
        public HttpResponseMessage Delete(int id)
        {
            this.registrationService.DeleteEventRegistration(id);

            // Success
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        /// <summary>
        /// Gets a list of ratings for a given event registratino
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// The list of ratings
        /// </returns>
        /// <remarks>
        /// GET: api/eventregistrations/getratings/{id}
        /// </remarks>
        [Route("api/eventregistrations/{id}/ratings")]
        public List<Rating> GetEventRegistrationRatings(int id)
        {
            return this.registrationService.GetEventBeverageRatings(id);
        }
    }
}
