namespace BeerAppreciation.Data.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web.Http.OData;
    using System.Web.Http.OData.Query;
    using EF.Extensions;
    using Repositories;
    using Repositories.Context;
    using Domain;
    using EF.Repository;
    using EF.UnitOfWork;

    public class EventService : IEventService
    {
        private readonly IUnitOfWorkInterceptorFactory<DatabaseContext> unitOfWorkFactory;

        public EventService(IUnitOfWorkInterceptorFactory<DatabaseContext> unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>
        /// Gets a list of events.
        /// </summary>
        /// <param name="queryOptions">The OData query options.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// A paged list of events
        /// </returns>
        public PageResult<Event> GetEvents(ODataQueryOptions<Event> queryOptions, string includes)
        {
            using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                IEntityRepository<Event> entityRepository = unitOfWork.GetRepository<Event, int>();

                // Query the generic repository using any odata query options and includes
                return entityRepository.GetList(queryOptions, includes);
            }
        }

        public PageResult<Event> GetAllEvents(int? clubId, string searchKey, int pageIndex, int pageSize)
        {
            using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                // Get the event entity repository
                var eventRepository = unitOfWork.GetRepository<Event, int>();

                ////return eventRepository.GetList(
                ////    @event => event.Name.Contains("Aa"), 
                ////    queryable => queryable.OrderBy(@event => @event.Date),
                ////    new List<Expression<Func<Event, object>>> { @event => @event.Appreciators },
                ////    null,
                ////    null);

                var eventList = eventRepository.GetPagedList(
                    @event => (String.IsNullOrEmpty(searchKey) || (@event.Description.Contains(searchKey) || @event.Name.Contains(searchKey))) && (!clubId.HasValue || @event.DrinkingClub.Id == clubId),
                    queryable => queryable.OrderBy(@event => @event.Date),
                    new List<Expression<Func<Event, object>>> { @event => @event.DrinkingClub },
                    pageIndex,
                    pageSize);

                return eventList;
            }
        }

        public PageResult<Event> GetUpcomingEvents(int? clubId, string searchKey, int pageIndex, int pageSize)
        {
            using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                // Get the event entity repository
                var eventRepository = (IEventRepository)unitOfWork.GetRepository<Event, int>();

                var eventList = eventRepository.GetPagedList(
                    @event => (@event.Date > DateTime.Now) && (String.IsNullOrEmpty(searchKey) || (@event.Description.Contains(searchKey) || @event.Name.Contains(searchKey))) && (!clubId.HasValue || @event.DrinkingClub.Id == clubId),
                    queryable => queryable.OrderBy(@event => @event.Date),
                    new List<Expression<Func<Event, object>>> { @event => @event.DrinkingClub },
                    pageIndex,
                    pageSize);

                return eventList;
            }
        }

        /// <summary>
        /// Adds the event to the database.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>
        /// THe id of the event added to database
        /// </returns>
        public int InsertEvent(Domain.Event @event)
        {
            using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                // Get the event entity repository
                IEntityRepository<Event> entityRepository = unitOfWork.GetRepository<Event, int>();

                // Insert the event
                entityRepository.Insert(@event);

                // Persist the changes
                IList<ObjectStateEntry> changes = unitOfWork.Save();

                // Return the Id of the added beverage
                return changes.GetInsertedEntityKey<int>("Events");
            }
        }

        /// <summary>
        /// Gets the event matching the specified Id
        /// </summary>
        /// <param name="eventId">The event unique identifier.</param>
        /// <returns>The event matching the id</returns>
        public Domain.Event GetEvent(int eventId)
        {
            using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                // Get the event entity repository
                var entityRepository = unitOfWork.GetRepository<Event, int>();

                // this is the fluent include option but
                // couldnt see how to include properties of collections
                //new List<Expression<Func<Event, object>>>
                //{
                //    @event => @event.DrinkingClub, 
                //    @event => @event.Beverages
                //},

                // Get the matching entity
                var selectedEvent = entityRepository.GetSingle(
                        e => e.Id == eventId,
                        new[]
                        {
                            "DrinkingClub", 
                            "Registrations", 
                            "Beverages", 
                            "Beverages.Beverage",
                            "Beverages.Beverage.Manufacturer",
                            "Beverages.Beverage.BeverageType",
                            "Beverages.Beverage.BeverageStyle"
                        }
                    );

                //selectedEvent.Beverages = unitOfWork.DbContext.EventBeverages
                //    .Where(e => e.EventId == eventId).Include("Beverage").Include("Beverage.Manufacturer")
                //    .Select(e => e.Beverage).ToList();

                selectedEvent.Appreciators = unitOfWork.DbContext.Events
                    .Where(e => e.Id == eventId).Include("Registrations").Include("Registrations.Appreciator")
                    .SelectMany(e => e.Registrations.Select(r => r.Appreciator)).ToList();

                return selectedEvent;
            }
        }

        /// <summary>
        /// Updates the specified event.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="event">The event.</param>
        public void UpdateEvent(int id, Event @event)
        {
            using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                // Get the event entity repository
                IEntityRepository<Event> entityRepository = unitOfWork.GetRepository<Event, int>();

                // Update the event
                entityRepository.Update(@event);

                // Persist the changes
                unitOfWork.Save();
            }
        }

        /// <summary>
        /// Deletes the specified event.
        /// </summary>
        /// <param name="id">The event identifier.</param>
        public void DeleteEvent(int id)
        {
            using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                // Get the event entity repository
                IEntityRepository<Event> entityRepository = unitOfWork.GetRepository<Event, int>();

                // Insert the event
                entityRepository.Delete(id);

                // Persist the changes
                unitOfWork.Save();
            }
        }

        /// <summary>
        /// Updates the scores for the event beverages
        /// and returns a fully populated event instance.
        /// </summary>
        /// <param name="eventId">The event unique identifier.</param>
        /// <returns>The event matching the id</returns>
        public Event UpdateEventRatingScores(int eventId)
        {
            using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                var eventBeverageRepository = unitOfWork.GetRepository<EventBeverage, int>();
                var eventBeverages = eventBeverageRepository.GetList(eb => eb.EventId == eventId);
                foreach (var beverage in eventBeverages)
                {
                    var eventBeverageId = beverage.Id;
                    var beverageRatings = from r in unitOfWork.DbContext.Ratings
                                join er in unitOfWork.DbContext.EventRegistrations on r.EventRegistrationId equals er.Id
                                where r.EventBeverageId == eventBeverageId
                                select r;

                    beverage.Score = beverageRatings.Any() ? beverageRatings.Sum(r => r.Score) : 0;
                    eventBeverageRepository.Update(beverage);
                }

                // Persist the changes
                unitOfWork.Save();
            }

            return GetEvent(eventId);
        }
    }
}
