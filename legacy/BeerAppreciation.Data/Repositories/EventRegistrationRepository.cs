namespace BeerAppreciation.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using Context;
    using Domain;
    using EF.Repository;
    using EF.UnitOfWork;

    public class EventRegistrationRepository : BaseEntityRepository<EventRegistration, int, DatabaseContext>, IEventRegistrationRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventRegistrationRepository" /> class.
        /// This overload allows the context to be passed in for use within a Unit of Work
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public EventRegistrationRepository(DatabaseContext dbContext, IUnitOfWork<DatabaseContext> unitOfWork)
            : base(dbContext, unitOfWork, false)
        {
        }

        public List<Rating> GetEventBeverageRatings(EventRegistration eventRegistration)
        {
            var eventBeverages =
                    (
                        from eb in this.DbContext.EventBeverages
                        join er in this.DbContext.Events on eb.EventId equals er.Id
                        where er.Id == eventRegistration.EventId
                        select eb
                    )
                    .Include("Beverage").Include("Beverage.Manufacturer").Include("Beverage.BeverageType").Include("Beverage.BeverageStyle")
                    .AsNoTracking().ToList();

            var beverageRatings = this.DbContext.Ratings
                .Include("EventBeverage")
                .Include("EventBeverage.Beverage")
                .Where(r => r.EventRegistrationId == eventRegistration.Id).ToList();

            var missingRatings = (from beverage in eventBeverages
                where beverageRatings.FirstOrDefault(br => br.EventBeverageId == beverage.Id) == null
                select new Rating
                {
                    Id = 0,
                    EventRegistrationId = eventRegistration.Id,
                    EventBeverageId = beverage.Id,
                    EventBeverage = beverage
                }).ToList();

            beverageRatings.AddRange(missingRatings);
            return beverageRatings;
        }
    }
}