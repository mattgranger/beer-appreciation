namespace BeerAppreciation.Data.Services
{
    using System.Web.Http.OData;
    using System.Web.Http.OData.Query;
    using Domain;

    public interface IRatingService
    {
        /// <summary>
        /// Gets a list of all Ratings.
        /// </summary>
        /// <param name="queryOptions">The OData query options.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// A list of all Ratings
        /// </returns>
        PageResult<Rating> GetRatings(ODataQueryOptions queryOptions, string includes);

        /// <summary>
        /// Adds the Rating to the database.
        /// </summary>
        /// <param name="rating">The Rating.</param>
        /// <param name="updateGraph">if set to <c>true</c> any child objects of the entity will also be persisted.</param>
        /// <returns>The identifier of the rating added to database</returns>
        int InsertRating(Rating rating, bool updateGraph);

        /// <summary>
        /// Gets the Rating matching the specified Id
        /// </summary>
        /// <param name="ratingId">The Rating unique identifier.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// The Rating matching the id
        /// </returns>
        Rating GetRating(int ratingId, string includes);

        /// <summary>
        /// Updates the specified Rating.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="rating">The Rating.</param>
        /// <param name="updateGraph">if set to <c>true</c> any child objects of the entity will also be persisted.</param>
        void UpdateRating(int id, Rating rating, bool updateGraph);

        /// <summary>
        /// Deletes the specified Rating.
        /// </summary>
        /// <param name="id">The Rating identifier.</param>
        void DeleteRating(int id);
    }
}