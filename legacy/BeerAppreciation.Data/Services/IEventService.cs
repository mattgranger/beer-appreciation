using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using BeerAppreciation.Core.Collections;
using BeerAppreciation.Domain;

namespace BeerAppreciation.Data.Services
{
    /// <summary>
    ///  Interface of service for Event related functionality
    /// </summary>
    public interface IEventService
    {
        /// <summary>
        /// Gets a list of all events.
        /// </summary>
        /// <param name="queryOptions">The OData query options.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// A paged list of all events
        /// </returns>
        PageResult<Event> GetEvents(ODataQueryOptions<Event> queryOptions, string includes);

        /// <summary>
        /// Adds the event to the database.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>The identifier of the event added to database</returns>
        int InsertEvent(Event @event);

        /// <summary>
        /// Gets the event matching the specified Id
        /// </summary>
        /// <param name="eventId">The event unique identifier.</param>
        /// <returns>The event matching the id</returns>
        Event GetEvent(int eventId);

        /// <summary>
        /// Updates the specified event.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="event">The event.</param>
        void UpdateEvent(int id, Event @event);

        /// <summary>
        /// Deletes the specified event.
        /// </summary>
        /// <param name="id">The event identifier.</param>
        void DeleteEvent(int id);

        /// <summary>
        /// Updates the scores for the event beverages
        /// and returns a fully populated event instance.
        /// </summary>
        /// <param name="eventId">The event unique identifier.</param>
        /// <returns>The event matching the id</returns>
        Event UpdateEventRatingScores(int eventId);
    }
}
