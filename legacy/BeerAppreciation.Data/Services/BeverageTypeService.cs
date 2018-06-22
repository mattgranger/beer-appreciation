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

    public class BeverageTypeService : IBeverageTypeService
    {

        /// <summary>
        /// The unit of work factory
        /// </summary>
        private readonly IUnitOfWorkInterceptorFactory<DatabaseContext> unitOfWorkFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="BeverageTypeService" /> class.
        /// </summary>
        /// <param name="unitOfWorkFactory"></param>
        public BeverageTypeService(IUnitOfWorkInterceptorFactory<DatabaseContext> unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>
        /// Gets a list of all BeverageTypes.
        /// </summary>
        /// <param name="queryOptions">The OData query options.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// A list of all BeverageTypes
        /// </returns>
        public PageResult<BeverageType> GetBeverageTypes(ODataQueryOptions queryOptions, string includes)
        {
            using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                IEntityRepository<BeverageType> entityRepository = unitOfWork.GetRepository<BeverageType, int>();

                // Query the generic repository using any odata query options and includes
                return entityRepository.GetList(queryOptions, includes);
            };
        }


        /// <summary>
        /// Adds the BeverageType to the database.
        /// </summary>
        /// <param name="beverageType">The BeverageType.</param>
        /// <param name="updateGraph">if set to <c>true</c> any child objects of the entity will also be persisted.</param>
        /// <returns>The identifier of the beverageType added to database</returns>
        public int InsertBeverageType(BeverageType beverageType, bool updateGraph)
        {
            using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                // Get the entity repository
                IEntityRepository<BeverageType> entityRepository = unitOfWork.GetRepository<BeverageType, int>();

                // Insert the entity
                if (updateGraph)
                {
                    // Update any entities in the graph
                    entityRepository.InsertOrUpdateGraph(beverageType);
                }
                else
                {
                    // Update just the root entity
                    entityRepository.Insert(beverageType);
                }

                // Persist the changes and return Id
                return unitOfWork.Save().GetInsertedEntityKey<int>("BeverageTypes");
            }
        }

        /// <summary>
        /// Gets the BeverageType matching the specified Id
        /// </summary>
        /// <param name="beverageTypeId">The BeverageType unique identifier.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// The BeverageType matching the id
        /// </returns>
        public BeverageType GetBeverageType(int beverageTypeId, string includes)
        {
            using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                // Get the entity repository
                IEntityRepository<BeverageType> entityRepository = unitOfWork.GetRepository<BeverageType, int>();

                // GetChangedActionTypes the matching entity
                return entityRepository.GetSingle(t => t.Id == beverageTypeId, includes.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
            }
        }

        /// <summary>
        /// Updates the specified BeverageType.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="beverageType">The BeverageType.</param>
        /// <param name="updateGraph">if set to <c>true</c> any child objects of the entity will also be persisted.</param>
        public void UpdateBeverageType(int id, BeverageType beverageType, bool updateGraph)
        {
            using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                // Get the entity repository
                IEntityRepository<BeverageType> entityRepository = unitOfWork.GetRepository<BeverageType, int>();

                // Update the entity
                if (updateGraph)
                {
                    entityRepository.InsertOrUpdateGraph(beverageType);
                }
                else
                {
                    entityRepository.Update(beverageType);
                }

                // Persist the changes
                unitOfWork.Save();
            }
        }

        /// <summary>
        /// Deletes the specified BeverageType.
        /// </summary>
        /// <param name="id">The BeverageType identifier.</param>
        public void DeleteBeverageType(int id)
        {
            using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                // Get the entity repository
                IEntityRepository<BeverageType> entityRepository = unitOfWork.GetRepository<BeverageType, int>();

                // Insert the entity
                entityRepository.Delete(id);

                // Persist the changes
                unitOfWork.Save();
            }
        }
    }
}