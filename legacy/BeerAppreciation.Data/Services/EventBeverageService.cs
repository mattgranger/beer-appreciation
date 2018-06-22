namespace BeerAppreciation.Data.Services
{
    using System;
    using System.Web.Http.OData;
    using System.Web.Http.OData.Query;
    using EF.Extensions;
    using EF.UnitOfWork;
    using Domain;
    using EF.Repository;
    using Repositories.Context;

    public class EventBeverageService : IEventBeverageService
    {

        /// <summary>
        /// The unit of work factory
        /// </summary>
        private readonly IUnitOfWorkInterceptorFactory<DatabaseContext> unitOfWorkFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventBeverageService" /> class.
        /// </summary>
        /// <param name="unitOfWorkFactory"></param>
        public EventBeverageService(IUnitOfWorkInterceptorFactory<DatabaseContext> unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>
        /// Gets a list of all EventBeverages.
        /// </summary>
        /// <param name="queryOptions">The OData query options.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// A list of all EventBeverages
        /// </returns>
        public PageResult<EventBeverage> GetEventBeverages(ODataQueryOptions queryOptions, string includes)
        {
            using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                IEntityRepository<EventBeverage> entityRepository = unitOfWork.GetRepository<EventBeverage, int>();

                // Query the generic repository using any odata query options and includes
                return entityRepository.GetList(queryOptions, includes);
            };
        }


        /// <summary>
        /// Adds the EventBeverage to the database.
        /// </summary>
        /// <param name="eventBeverage">The EventBeverage.</param>
        /// <param name="updateGraph">if set to <c>true</c> any child objects of the entity will also be persisted.</param>
        /// <returns>The identifier of the eventBeverage added to database</returns>
        public int InsertEventBeverage(EventBeverage eventBeverage, bool updateGraph)
        {
            using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                // Get the entity repository
                IEntityRepository<EventBeverage> entityRepository = unitOfWork.GetRepository<EventBeverage, int>();

                // Insert the entity
                if (updateGraph)
                {
                    // Update any entities in the graph
                    entityRepository.InsertOrUpdateGraph(eventBeverage);
                }
                else
                {
                    // Update just the root entity
                    entityRepository.Insert(eventBeverage);
                }

                // Persist the changes and return Id
                return unitOfWork.Save().GetInsertedEntityKey<int>("EventBeverages");
            }
        }

        /// <summary>
        /// Gets the EventBeverage matching the specified Id
        /// </summary>
        /// <param name="eventBeverageId">The EventBeverage unique identifier.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// The EventBeverage matching the id
        /// </returns>
        public EventBeverage GetEventBeverage(int eventBeverageId, string includes)
        {
            using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                // Get the entity repository
                IEntityRepository<EventBeverage> entityRepository = unitOfWork.GetRepository<EventBeverage, int>();

                // GetChangedActionTypes the matching entity
                return entityRepository.GetSingle(t => t.Id == eventBeverageId, includes.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
            }
        }

        /// <summary>
        /// Updates the specified EventBeverage.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="eventBeverage">The EventBeverage.</param>
        /// <param name="updateGraph">if set to <c>true</c> any child objects of the entity will also be persisted.</param>
        public void UpdateEventBeverage(int id, EventBeverage eventBeverage, bool updateGraph)
        {
            using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                // Get the entity repository
                IEntityRepository<EventBeverage> entityRepository = unitOfWork.GetRepository<EventBeverage, int>();

                // Update the entity
                if (updateGraph)
                {
                    entityRepository.InsertOrUpdateGraph(eventBeverage);
                }
                else
                {
                    entityRepository.Update(eventBeverage);
                }

                // Persist the changes
                unitOfWork.Save();
            }
        }

        /// <summary>
        /// Deletes the specified EventBeverage.
        /// </summary>
        /// <param name="id">The EventBeverage identifier.</param>
        public void DeleteEventBeverage(int id)
        {
            using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                // Get the entity repository
                IEntityRepository<EventBeverage> entityRepository = unitOfWork.GetRepository<EventBeverage, int>();

                // Insert the entity
                entityRepository.Delete(id);

                // Persist the changes
                unitOfWork.Save();
            }
        }
    }
}