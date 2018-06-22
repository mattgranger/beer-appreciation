namespace BeerAppreciation.Data.Services
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http.OData;
    using System.Web.Http.OData.Query;
    using Domain;

    /// <summary>
    ///  Interface of service for Event Registration related functionality
    /// </summary>
    public interface IEventRegistrationService
    {
        /// <summary>
        /// Gets a list of all EventRegistrations.
        /// </summary>
        /// <param name="queryOptions">The OData query options.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// A paged list of all EventRegistrations
        /// </returns>
        PageResult<EventRegistration> GetEventRegistrations(ODataQueryOptions<EventRegistration> queryOptions, string includes);

        /// <summary>
        /// Adds the EventRegistration to the database.
        /// </summary>
        /// <param name="eventRegistration">The EventRegistration.</param>
        /// <param name="updateGraph">if set to <c>true</c> any child objects of the entity will also be persisted.</param>
        /// <returns>The identifier of the EventRegistration added to database</returns>
        int InsertEventRegistration(EventRegistration eventRegistration, bool updateGraph);

        /// <summary>
        /// Gets the EventRegistration matching the specified Id
        /// </summary>
        /// <param name="eventRegistrationId">The EventRegistration unique identifier.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// The EventRegistration matching the id
        /// </returns>
        EventRegistration GetEventRegistration(int eventRegistrationId, string includes);

        /// <summary>
        /// Updates the specified EventRegistration.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="eventRegistration">The EventRegistration.</param>
        /// <param name="updateGraph">if set to <c>true</c> any child objects of the entity will also be persisted.</param>
        void UpdateEventRegistration(int id, EventRegistration eventRegistration, bool updateGraph);

        /// <summary>
        /// Deletes the specified EventRegistration.
        /// </summary>
        /// <param name="id">The EventRegistration identifier.</param>
        void DeleteEventRegistration(int id);

        /// <summary>
        /// Returns a list of ratings (left joined) for a given event registration
        /// </summary>
        /// <param name="eventRegistrationId"></param>
        /// <returns>A list of ratings</returns>
        List<Rating> GetEventBeverageRatings(int eventRegistrationId);
    }
}
