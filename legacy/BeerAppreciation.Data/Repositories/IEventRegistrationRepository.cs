namespace BeerAppreciation.Data.Repositories
{
    using System.Collections.Generic;
    using Domain;
    using EF.Repository;

    public interface IEventRegistrationRepository : IEntityRepository<EventRegistration>
    {
        /// <summary>
        /// Returns a list of ratings (left joined) for a given event registration
        /// </summary>
        /// <param name="eventRegistration"></param>
        /// <returns>A list of ratings</returns>
        List<Rating> GetEventBeverageRatings(EventRegistration eventRegistration);
    }
}
