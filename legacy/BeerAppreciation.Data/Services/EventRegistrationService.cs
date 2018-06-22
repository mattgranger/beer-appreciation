namespace BeerAppreciation.Data.Services
{
    using System.Collections.Generic;
    using System.Web.Http.OData;
    using System.Web.Http.OData.Query;
    using Domain;
    using EF.Extensions;
    using EF.Repository;
    using EF.UnitOfWork;
    using Repositories;
    using Repositories.Context;

    /// <summary>
    /// Service for Event Registration related functionality
    /// </summary>
    public class EventRegistrationService : IEventRegistrationService
    {
        private readonly IUnitOfWorkInterceptorFactory<DatabaseContext> unitOfWorkFactory;

        public EventRegistrationService(IUnitOfWorkInterceptorFactory<DatabaseContext> unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>
        /// Gets a list of all EventRegistrations.
        /// </summary>
        /// <param name="queryOptions">The OData query options.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// A paged list of all EventRegistrations
        /// </returns>
        public PageResult<EventRegistration> GetEventRegistrations(ODataQueryOptions<EventRegistration> queryOptions, string includes)
        {
            using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                IEntityRepository<EventRegistration> EventRegistrationRepository = unitOfWork.GetRepository<EventRegistration, int>();

                // Query the generic repository using any odata query options and includes
                return EventRegistrationRepository.GetList(queryOptions, includes);
            }
        }

        /// <summary>
        /// Gets the EventRegistration matching the specified Id
        /// </summary>
        /// <param name="eventRegistrationId">The EventRegistration unique identifier.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// The EventRegistration matching the id
        /// </returns>
        public EventRegistration GetEventRegistration(int eventRegistrationId, string includes)
        {
            using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                // Get the EventRegistration entity repository
                var entityRepository = (IEventRegistrationRepository)unitOfWork.GetRepository<EventRegistration, int>();

                // GetChangedEventRegistrations the matching entity
                var registration = entityRepository.GetSingle(t => t.Id == eventRegistrationId, !string.IsNullOrWhiteSpace(includes) ? includes.Split(',') : null);
                
                // populate ratings to ensure that each event beverage has a record
                registration.Ratings = entityRepository.GetEventBeverageRatings(registration);

                // return
                return registration;
            }
        }

        /// <summary>
        /// Adds the EventRegistration to the database.
        /// </summary>
        /// <param name="eventRegistration">The EventRegistration.</param>
        /// <param name="updateGraph">if set to <c>true</c> any child objects of the entity will also be persisted.</param>
        /// <returns>
        /// THe id of the EventRegistration added to database
        /// </returns>
        public int InsertEventRegistration(EventRegistration eventRegistration, bool updateGraph)
        {
            using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                // Get the EventRegistration entity repository
                IEntityRepository<EventRegistration> entityRepository = unitOfWork.GetRepository<EventRegistration, int>();

                // Insert the EventRegistration
                if (updateGraph)
                {
                    // Adds/Updates any entities in the graph
                    entityRepository.InsertOrUpdateGraph(eventRegistration);
                }
                else
                {
                    // Update just the root entity
                    entityRepository.Insert(eventRegistration);
                }

                // Persist the changes and return Id
                return unitOfWork.Save().GetInsertedEntityKey<int>("EventRegistrations");
            }
        }

        /// <summary>
        /// Updates the specified EventRegistration.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="eventRegistration">The EventRegistration.</param>
        /// <param name="updateGraph">if set to <c>true</c> any child objects of the entity will also be persisted.</param>
        public void UpdateEventRegistration(int id, EventRegistration eventRegistration, bool updateGraph)
        {
            using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                // Get the EventRegistration entity repository
                IEntityRepository<EventRegistration> entityRepository = unitOfWork.GetRepository<EventRegistration, int>();

                // Update the EventRegistration
                if (updateGraph)
                {
                    entityRepository.InsertOrUpdateGraph(eventRegistration);
                }
                else
                {
                    entityRepository.Update(eventRegistration);
                }

                // Persist the changes
                unitOfWork.Save();
            }
        }

        /// <summary>
        /// Deletes the specified EventRegistration.
        /// </summary>
        /// <param name="id">The EventRegistration identifier.</param>
        public void DeleteEventRegistration(int id)
        {
            using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                // Get the EventRegistration entity repository
                IEntityRepository<EventRegistration> entityRepository = unitOfWork.GetRepository<EventRegistration, int>();

                // Insert the EventRegistration
                entityRepository.Delete(id);

                // Persist the changes
                unitOfWork.Save();
            }
        }

        /// <summary>
        /// Returns a list of ratings (left joined) for a given event registration
        /// </summary>
        /// <param name="eventRegistrationId"></param>
        /// <returns>A list of ratings</returns>
        public List<Rating> GetEventBeverageRatings(int eventRegistrationId)
        {
            using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                // Get the EventRegistration entity repository
                var entityRepository = (IEventRegistrationRepository)unitOfWork.GetRepository<EventRegistration, int>();

                var eventRegistration = GetEventRegistration(eventRegistrationId, string.Empty);
                // GetChangedEventRegistrations the matching entity
                return entityRepository.GetEventBeverageRatings(eventRegistration);
            }
        }
    }
}