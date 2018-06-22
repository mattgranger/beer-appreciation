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

    public class DrinkingClubService : IDrinkingClubService
    {

        /// <summary>
        /// The unit of work factory
        /// </summary>
        private readonly IUnitOfWorkInterceptorFactory<DatabaseContext> unitOfWorkFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrinkingClubService" /> class.
        /// </summary>
        /// <param name="unitOfWorkFactory"></param>
        public DrinkingClubService(IUnitOfWorkInterceptorFactory<DatabaseContext> unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>
        /// Gets a list of all DrinkingClubs.
        /// </summary>
        /// <param name="queryOptions">The OData query options.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// A list of all DrinkingClubs
        /// </returns>
        public PageResult<DrinkingClub> GetDrinkingClubs(ODataQueryOptions queryOptions, string includes)
        {
            using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                IEntityRepository<DrinkingClub> entityRepository = unitOfWork.GetRepository<DrinkingClub, int>();

                // Query the generic repository using any odata query options and includes
                return entityRepository.GetList(queryOptions, includes);
            };
        }


        /// <summary>
        /// Adds the DrinkingClub to the database.
        /// </summary>
        /// <param name="drinkingClub">The DrinkingClub.</param>
        /// <param name="updateGraph">if set to <c>true</c> any child objects of the entity will also be persisted.</param>
        /// <returns>The identifier of the drinkingClub added to database</returns>
        public int InsertDrinkingClub(DrinkingClub drinkingClub, bool updateGraph)
        {
            using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                // Get the entity repository
                IEntityRepository<DrinkingClub> entityRepository = unitOfWork.GetRepository<DrinkingClub, int>();

                // Insert the entity
                if (updateGraph)
                {
                    // Update any entities in the graph
                    entityRepository.InsertOrUpdateGraph(drinkingClub);
                }
                else
                {
                    // Update just the root entity
                    entityRepository.Insert(drinkingClub);
                }

                // Persist the changes and return Id
                return unitOfWork.Save().GetInsertedEntityKey<int>("DrinkingClubs");
            }
        }

        /// <summary>
        /// Gets the DrinkingClub matching the specified Id
        /// </summary>
        /// <param name="drinkingClubId">The DrinkingClub unique identifier.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// The DrinkingClub matching the id
        /// </returns>
        public DrinkingClub GetDrinkingClub(int drinkingClubId, string includes)
        {
            using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                // Get the entity repository
                IEntityRepository<DrinkingClub> entityRepository = unitOfWork.GetRepository<DrinkingClub, int>();

                // GetChangedActionTypes the matching entity
                return entityRepository.GetSingle(t => t.Id == drinkingClubId, includes.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
            }
        }

        /// <summary>
        /// Updates the specified DrinkingClub.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="drinkingClub">The DrinkingClub.</param>
        /// <param name="updateGraph">if set to <c>true</c> any child objects of the entity will also be persisted.</param>
        public void UpdateDrinkingClub(int id, DrinkingClub drinkingClub, bool updateGraph)
        {
            using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                // Get the entity repository
                IEntityRepository<DrinkingClub> entityRepository = unitOfWork.GetRepository<DrinkingClub, int>();

                // Update the entity
                if (updateGraph)
                {
                    entityRepository.InsertOrUpdateGraph(drinkingClub);
                }
                else
                {
                    entityRepository.Update(drinkingClub);
                }

                // Persist the changes
                unitOfWork.Save();
            }
        }

        /// <summary>
        /// Deletes the specified DrinkingClub.
        /// </summary>
        /// <param name="id">The DrinkingClub identifier.</param>
        public void DeleteDrinkingClub(int id)
        {
            using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                // Get the entity repository
                IEntityRepository<DrinkingClub> entityRepository = unitOfWork.GetRepository<DrinkingClub, int>();

                // Insert the entity
                entityRepository.Delete(id);

                // Persist the changes
                unitOfWork.Save();
            }
        }
    }
}