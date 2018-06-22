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

    public class AppreciatorService : IAppreciatorService
    {

        /// <summary>
        /// The unit of work factory
        /// </summary>
        private readonly IUnitOfWorkInterceptorFactory<DatabaseContext> unitOfWorkFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppreciatorService" /> class.
        /// </summary>
        /// <param name="unitOfWorkFactory"></param>
        public AppreciatorService(IUnitOfWorkInterceptorFactory<DatabaseContext> unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>
        /// Gets a list of all Appreciators.
        /// </summary>
        /// <param name="queryOptions">The OData query options.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// A list of all Appreciators
        /// </returns>
        public PageResult<Appreciator> GetAppreciators(ODataQueryOptions queryOptions, string includes)
        {
            using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                IEntityRepository<Appreciator> entityRepository = unitOfWork.GetRepository<Appreciator, string>();

                // Query the generic repository using any odata query options and includes
                return entityRepository.GetList(queryOptions, includes);
            };
        }


        /// <summary>
        /// Adds the Appreciator to the database.
        /// </summary>
        /// <param name="appreciator">The Appreciator.</param>
        /// <param name="updateGraph">if set to <c>true</c> any child objects of the entity will also be persisted.</param>
        /// <returns>The identifier of the appreciator added to database</returns>
        public string InsertAppreciator(Appreciator appreciator, bool updateGraph)
        {
            using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                // Get the entity repository
                IEntityRepository<Appreciator> entityRepository = unitOfWork.GetRepository<Appreciator, string>();

                // Insert the entity
                if (updateGraph)
                {
                    // Update any entities in the graph
                    entityRepository.InsertOrUpdateGraph(appreciator);
                }
                else
                {
                    // Update just the root entity
                    entityRepository.Insert(appreciator);
                }

                // Persist the changes and return Id
                return unitOfWork.Save().GetInsertedEntityKey<string>("Appreciators");
            }
        }

        /// <summary>
        /// Gets the Appreciator matching the specified Id
        /// </summary>
        /// <param name="appreciatorId">The Appreciator unique identifier.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// The Appreciator matching the id
        /// </returns>
        public Appreciator GetAppreciator(string appreciatorId, string includes)
        {
            using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                // Get the entity repository
                IEntityRepository<Appreciator> entityRepository = unitOfWork.GetRepository<Appreciator, string>();

                // GetChangedActionTypes the matching entity
                return entityRepository.GetSingle(t => t.Id == appreciatorId, includes.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
            }
        }

        /// <summary>
        /// Updates the specified Appreciator.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="appreciator">The Appreciator.</param>
        /// <param name="updateGraph">if set to <c>true</c> any child objects of the entity will also be persisted.</param>
        public void UpdateAppreciator(string id, Appreciator appreciator, bool updateGraph)
        {
            using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                // Get the entity repository
                IEntityRepository<Appreciator> entityRepository = unitOfWork.GetRepository<Appreciator, string>();

                // Update the entity
                if (updateGraph)
                {
                    entityRepository.InsertOrUpdateGraph(appreciator);
                }
                else
                {
                    entityRepository.Update(appreciator);
                }

                // Persist the changes
                unitOfWork.Save();
            }
        }

        ///// <summary>
        ///// Deletes the specified Appreciator.
        ///// </summary>
        ///// <param name="id">The Appreciator identifier.</param>
        //public void DeleteAppreciator(string id)
        //{
        //    using (var unitOfWork = this.unitOfWorkFactory.Create())
        //    {
        //        // Get the entity repository
        //        IEntityRepository<Appreciator> entityRepository = unitOfWork.GetRepository<Appreciator, string>();

        //        // Insert the entity
        //        entityRepository.Delete(id);

        //        // Persist the changes
        //        unitOfWork.Save();
        //    }
        //}
    }
}