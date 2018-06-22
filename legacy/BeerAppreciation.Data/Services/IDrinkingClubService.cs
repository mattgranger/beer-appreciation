namespace BeerAppreciation.Data.Services
{
    using System.Web.Http.OData;
    using System.Web.Http.OData.Query;
    using Domain;

    public interface IDrinkingClubService
    {
        /// <summary>
        /// Gets a list of all DrinkingClubs.
        /// </summary>
        /// <param name="queryOptions">The OData query options.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// A list of all DrinkingClubs
        /// </returns>
        PageResult<DrinkingClub> GetDrinkingClubs(ODataQueryOptions queryOptions, string includes);

        /// <summary>
        /// Adds the DrinkingClub to the database.
        /// </summary>
        /// <param name="drinkingClub">The DrinkingClub.</param>
        /// <param name="updateGraph">if set to <c>true</c> any child objects of the entity will also be persisted.</param>
        /// <returns>The identifier of the drinkingClub added to database</returns>
        int InsertDrinkingClub(DrinkingClub drinkingClub, bool updateGraph);

        /// <summary>
        /// Gets the DrinkingClub matching the specified Id
        /// </summary>
        /// <param name="drinkingClubId">The DrinkingClub unique identifier.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// The DrinkingClub matching the id
        /// </returns>
        DrinkingClub GetDrinkingClub(int drinkingClubId, string includes);

        /// <summary>
        /// Updates the specified DrinkingClub.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="drinkingClub">The DrinkingClub.</param>
        /// <param name="updateGraph">if set to <c>true</c> any child objects of the entity will also be persisted.</param>
        void UpdateDrinkingClub(int id, DrinkingClub drinkingClub, bool updateGraph);

        /// <summary>
        /// Deletes the specified DrinkingClub.
        /// </summary>
        /// <param name="id">The DrinkingClub identifier.</param>
        void DeleteDrinkingClub(int id);
    }
}