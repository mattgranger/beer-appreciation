namespace BeerAppreciation.Data.Services
{
    using System;
    using System.Web.Http.OData;
    using System.Web.Http.OData.Query;
    using Repositories.Context;
    using Domain;
    using EF.Extensions;
    using EF.Repository;
    using EF.UnitOfWork;

    public class ManufacturerService : IManufacturerService
    {
        /// <summary>
        /// The unit of work factory
        /// </summary>
        private readonly IUnitOfWorkInterceptorFactory<DatabaseContext> unitOfWorkFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManufacturerService" /> class.
        /// </summary>
        /// <param name="unitOfWorkFactory"></param>
        public ManufacturerService(IUnitOfWorkInterceptorFactory<DatabaseContext> unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>
        /// Gets a list of all Manufacturers.
        /// </summary>
        /// <param name="queryOptions">The OData query options.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// A list of all Manufacturers
        /// </returns>
        public PageResult<Manufacturer> GetManufacturers(ODataQueryOptions queryOptions, string includes)
        {
            using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                IEntityRepository<Manufacturer> entityRepository = unitOfWork.GetRepository<Manufacturer, int>();

                // Query the generic repository using any odata query options and includes
                return entityRepository.GetList(queryOptions, includes);
            };
        }


        /// <summary>
        /// Adds the Manufacturer to the database.
        /// </summary>
        /// <param name="manufacturer">The Manufacturer.</param>
        /// <param name="updateGraph">if set to <c>true</c> any child objects of the entity will also be persisted.</param>
        /// <returns>The identifier of the manufacturer added to database</returns>
        public int InsertManufacturer(Manufacturer manufacturer, bool updateGraph)
        {
            using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                // Get the entity repository
                IEntityRepository<Manufacturer> entityRepository = unitOfWork.GetRepository<Manufacturer, int>();

                // Insert the entity
                if (updateGraph)
                {
                    // Update any entities in the graph
                    entityRepository.InsertOrUpdateGraph(manufacturer);
                }
                else
                {
                    // Update just the root entity
                    entityRepository.Insert(manufacturer);
                }

                // Persist the changes and return Id
                return unitOfWork.Save().GetInsertedEntityKey<int>("Manufacturers");
            }
        }

        /// <summary>
        /// Gets the Manufacturer matching the specified Id
        /// </summary>
        /// <param name="manufacturerId">The Manufacturer unique identifier.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// The Manufacturer matching the id
        /// </returns>
        public Manufacturer GetManufacturer(int manufacturerId, string includes)
        {
            using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                // Get the entity repository
                IEntityRepository<Manufacturer> entityRepository = unitOfWork.GetRepository<Manufacturer, int>();

                // GetChangedActionTypes the matching entity
                return entityRepository.GetSingle(t => t.Id == manufacturerId, includes.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
            }
        }

        /// <summary>
        /// Updates the specified Manufacturer.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="manufacturer">The Manufacturer.</param>
        /// <param name="updateGraph">if set to <c>true</c> any child objects of the entity will also be persisted.</param>
        public void UpdateManufacturer(int id, Manufacturer manufacturer, bool updateGraph)
        {
            using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                // Get the entity repository
                IEntityRepository<Manufacturer> entityRepository = unitOfWork.GetRepository<Manufacturer, int>();

                // Update the entity
                if (updateGraph)
                {
                    entityRepository.InsertOrUpdateGraph(manufacturer);
                }
                else
                {
                    entityRepository.Update(manufacturer);
                }

                // Persist the changes
                unitOfWork.Save();
            }
        }

        /// <summary>
        /// Deletes the specified Manufacturer.
        /// </summary>
        /// <param name="id">The Manufacturer identifier.</param>
        public void DeleteManufacturer(int id)
        {
            using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                // Get the entity repository
                IEntityRepository<Manufacturer> entityRepository = unitOfWork.GetRepository<Manufacturer, int>();

                // Insert the entity
                entityRepository.Delete(id);

                // Persist the changes
                unitOfWork.Save();
            }
        }   
    }
}