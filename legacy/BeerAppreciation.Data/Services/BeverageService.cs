using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using BeerAppreciation.Data.EF.Extensions;
using BeerAppreciation.Data.Repositories.Context;
using BeerAppreciation.Domain;

namespace BeerAppreciation.Data.Services
{
    using EF.Repository;
    using EF.UnitOfWork;

    public class BeverageService : IBeverageService
    {
        private readonly IUnitOfWorkInterceptorFactory<DatabaseContext> unitOfWorkFactory;

        public BeverageService(IUnitOfWorkInterceptorFactory<DatabaseContext> unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>
        /// Gets a list of all beverages.
        /// </summary>
        /// <param name="queryOptions">The OData query options.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// A paged list of all beverages
        /// </returns>
        public PageResult<Beverage> GetBeverages(ODataQueryOptions<Beverage> queryOptions, string includes)
        {
            using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                IEntityRepository<Beverage> travellerRepository = unitOfWork.GetRepository<Beverage, int>();

                // Query the generic repository using any odata query options and includes
                return travellerRepository.GetList(queryOptions, includes);
            }
        }

        /// <summary>
        /// Gets the beverage matching the specified Id
        /// </summary>
        /// <param name="beverageId">The beverage unique identifier.</param>
        /// <returns>The beverage matching the id</returns>
        public Beverage GetBeverage(int beverageId)
        {
            using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                // Get the beverage entity repository
                var entityRepository = unitOfWork.GetRepository<Beverage, int>();

                // Get the matching entity
                return entityRepository.GetSingle(
                        e => e.Id == beverageId,
                        new[] { "BeverageStyle", "BeverageType", "Manufacturer" }
                    );
            }
        }

        /// <summary>
        /// Adds the beverage to the database.
        /// </summary>
        /// <param name="beverage">The beverage.</param>
        /// <returns>
        /// THe id of the beverage added to database
        /// </returns>
        public int InsertBeverage(Beverage beverage)
        {
            using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                // Get the beverage entity repository
                IEntityRepository<Beverage> entityRepository = unitOfWork.GetRepository<Beverage, int>();

                // Insert the beverage
                entityRepository.InsertOrUpdateGraph(beverage);

                // Persist the changes
                IList<ObjectStateEntry> changes = unitOfWork.Save();

                // Return the Id of the added beverage
                return changes.GetInsertedEntityKey<int>("Beverages");
            }
        }

        /// <summary>
        /// Updates the specified beverage.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="beverage">The beverage.</param>
        public void UpdateBeverage(int id, Beverage beverage)
        {
            using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                // Get the beverage entity repository
                IEntityRepository<Beverage> entityRepository = unitOfWork.GetRepository<Beverage, int>();

                // Update the beverage
                entityRepository.Update(beverage);

                // Persist the changes
                unitOfWork.Save();
            }
        }

        /// <summary>
        /// Deletes the specified beverage.
        /// </summary>
        /// <param name="id">The beverage identifier.</param>
        public void DeleteBeverage(int id)
        {
            using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                // Get the beverage entity repository
                IEntityRepository<Beverage> entityRepository = unitOfWork.GetRepository<Beverage, int>();

                // Insert the beverage
                entityRepository.Delete(id);

                // Persist the changes
                unitOfWork.Save();
            }
        }

    }
}
