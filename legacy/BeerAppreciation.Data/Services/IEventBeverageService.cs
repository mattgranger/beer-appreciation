namespace BeerAppreciation.Data.Services
{
    using System.Web.Http.OData;
    using System.Web.Http.OData.Query;
    using BeerAppreciation.Domain;

    public interface IEventBeverageService
    {
        /// <summary>
        /// Gets a list of all EventBeverages.
        /// </summary>
        /// <param name="queryOptions">The OData query options.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// A list of all EventBeverages
        /// </returns>
        PageResult<EventBeverage> GetEventBeverages(ODataQueryOptions queryOptions, string includes);

        /// <summary>
        /// Adds the EventBeverage to the database.
        /// </summary>
        /// <param name="eventBeverage">The EventBeverage.</param>
        /// <param name="updateGraph">if set to <c>true</c> any child objects of the entity will also be persisted.</param>
        /// <returns>The identifier of the eventBeverage added to database</returns>
        int InsertEventBeverage(EventBeverage eventBeverage, bool updateGraph);

        /// <summary>
        /// Gets the EventBeverage matching the specified Id
        /// </summary>
        /// <param name="eventBeverageId">The EventBeverage unique identifier.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// The EventBeverage matching the id
        /// </returns>
        EventBeverage GetEventBeverage(int eventBeverageId, string includes);

        /// <summary>
        /// Updates the specified EventBeverage.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="eventBeverage">The EventBeverage.</param>
        /// <param name="updateGraph">if set to <c>true</c> any child objects of the entity will also be persisted.</param>
        void UpdateEventBeverage(int id, EventBeverage eventBeverage, bool updateGraph);

        /// <summary>
        /// Deletes the specified EventBeverage.
        /// </summary>
        /// <param name="id">The EventBeverage identifier.</param>
        void DeleteEventBeverage(int id);
    }
}
