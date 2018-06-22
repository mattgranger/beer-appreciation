namespace BeerAppreciation.Data.Services
{
    using System;
    using System.Web.Http.OData;
    using System.Web.Http.OData.Query;
    using EF.Extensions;
    using Repositories.Context;
    using Domain;
    using EF.Repository;
    using EF.UnitOfWork;

    public class BeverageStyleService : IBeverageStyleService
    {
        /// <summary>
        /// The unit of work factory
        /// </summary>
        private readonly IUnitOfWorkInterceptorFactory<DatabaseContext> unitOfWorkFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="BeverageStyleService" /> class.
        /// </summary>
        /// <param name="unitOfWorkFactory"></param>
        public BeverageStyleService(IUnitOfWorkInterceptorFactory<DatabaseContext> unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>
        /// Gets a list of all BeverageStyles.
        /// </summary>
        /// <param name="queryOptions">The OData query options.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// A list of all BeverageStyles
        /// </returns>
        public PageResult<BeverageStyle> GetBeverageStyles(ODataQueryOptions queryOptions, string includes)
        {
            using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                IEntityRepository<BeverageStyle> entityRepository = unitOfWork.GetRepository<BeverageStyle, int>();

                // Query the generic repository using any odata query options and includes
                return entityRepository.GetList(queryOptions, includes);
            };
        }


        /// <summary>
        /// Adds the BeverageStyle to the database.
        /// </summary>
        /// <param name="beverageStyle">The BeverageStyle.</param>
        /// <param name="updateGraph">if set to <c>true</c> any child objects of the entity will also be persisted.</param>
        /// <returns>The identifier of the beverageStyle added to database</returns>
        public int InsertBeverageStyle(BeverageStyle beverageStyle, bool updateGraph)
        {
            using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                // Get the entity repository
                IEntityRepository<BeverageStyle> entityRepository = unitOfWork.GetRepository<BeverageStyle, int>();

                // Insert the entity
                if (updateGraph)
                {
                    // Update any entities in the graph
                    entityRepository.InsertOrUpdateGraph(beverageStyle);
                }
                else
                {
                    // Update just the root entity
                    entityRepository.Insert(beverageStyle);
                }

                // Persist the changes and return Id
                return unitOfWork.Save().GetInsertedEntityKey<int>("BeverageStyles");
            }
        }

        /// <summary>
        /// Gets the BeverageStyle matching the specified Id
        /// </summary>
        /// <param name="beverageStyleId">The BeverageStyle unique identifier.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// The BeverageStyle matching the id
        /// </returns>
        public BeverageStyle GetBeverageStyle(int beverageStyleId, string includes)
        {
            using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                // Get the entity repository
                IEntityRepository<BeverageStyle> entityRepository = unitOfWork.GetRepository<BeverageStyle, int>();

                // GetChangedActionTypes the matching entity
                return entityRepository.GetSingle(t => t.Id == beverageStyleId, includes.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
            }
        }

        /// <summary>
        /// Updates the specified BeverageStyle.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="beverageStyle">The BeverageStyle.</param>
        /// <param name="updateGraph">if set to <c>true</c> any child objects of the entity will also be persisted.</param>
        public void UpdateBeverageStyle(int id, BeverageStyle beverageStyle, bool updateGraph)
        {
            using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                // Get the entity repository
                IEntityRepository<BeverageStyle> entityRepository = unitOfWork.GetRepository<BeverageStyle, int>();

                // Update the entity
                if (updateGraph)
                {
                    entityRepository.InsertOrUpdateGraph(beverageStyle);
                }
                else
                {
                    entityRepository.Update(beverageStyle);
                }

                // Persist the changes
                unitOfWork.Save();
            }
        }

        /// <summary>
        /// Deletes the specified BeverageStyle.
        /// </summary>
        /// <param name="id">The BeverageStyle identifier.</param>
        public void DeleteBeverageStyle(int id)
        {
            using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                // Get the entity repository
                IEntityRepository<BeverageStyle> entityRepository = unitOfWork.GetRepository<BeverageStyle, int>();

                // Insert the entity
                entityRepository.Delete(id);

                // Persist the changes
                unitOfWork.Save();
            }
        }   
    }
}