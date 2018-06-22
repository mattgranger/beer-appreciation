namespace BeerAppreciation.Data.Services
{
    using System;
    using System.Linq;
    using System.Web.Http.OData;
    using System.Web.Http.OData.Query;
    using EF.Extensions;
    using EF.UnitOfWork;
    using Domain;
    using EF.Repository;
    using Repositories.Context;

    public class RatingService : IRatingService
    {

        /// <summary>
        /// The unit of work factory
        /// </summary>
        private readonly IUnitOfWorkInterceptorFactory<DatabaseContext> unitOfWorkFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="RatingService" /> class.
        /// </summary>
        /// <param name="unitOfWorkFactory"></param>
        public RatingService(IUnitOfWorkInterceptorFactory<DatabaseContext> unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>
        /// Gets a list of all Ratings.
        /// </summary>
        /// <param name="queryOptions">The OData query options.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// A list of all Ratings
        /// </returns>
        public PageResult<Rating> GetRatings(ODataQueryOptions queryOptions, string includes)
        {
            using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                IEntityRepository<Rating> entityRepository = unitOfWork.GetRepository<Rating, int>();

                // Query the generic repository using any odata query options and includes
                return entityRepository.GetList(queryOptions, includes);
            };
        }

        /// <summary>
        /// Gets the Rating matching the specified Id
        /// </summary>
        /// <param name="ratingId">The Rating unique identifier.</param>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// The Rating matching the id
        /// </returns>
        public Rating GetRating(int ratingId, string includes)
        {
            using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                // Get the entity repository
                IEntityRepository<Rating> entityRepository = unitOfWork.GetRepository<Rating, int>();

                // GetChangedActionTypes the matching entity
                return entityRepository.GetSingle(t => t.Id == ratingId, includes.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
            }
        }

        /// <summary>
        /// Adds the Rating to the database.
        /// </summary>
        /// <param name="rating">The Rating.</param>
        /// <param name="updateGraph">if set to <c>true</c> any child objects of the entity will also be persisted.</param>
        /// <returns>The identifier of the rating added to database</returns>
        public int InsertRating(Rating rating, bool updateGraph)
        {
            using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                // Get the entity repository
                IEntityRepository<Rating> entityRepository = unitOfWork.GetRepository<Rating, int>();

                // Insert the entity
                if (updateGraph)
                {
                    // Update any entities in the graph
                    entityRepository.InsertOrUpdateGraph(rating);
                }
                else
                {
                    // Update just the root entity
                    entityRepository.Insert(rating);
                }

                // update score
                UpdateEventBeverageScore(rating);

                // Persist the changes and return Id
                return unitOfWork.Save().GetInsertedEntityKey<int>("Ratings");
            }
        }

        /// <summary>
        /// Updates the specified Rating.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="rating">The Rating.</param>
        /// <param name="updateGraph">if set to <c>true</c> any child objects of the entity will also be persisted.</param>
        public void UpdateRating(int id, Rating rating, bool updateGraph)
        {
            using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                // Get the entity repository
                IEntityRepository<Rating> entityRepository = unitOfWork.GetRepository<Rating, int>();

                // Update the entity
                if (updateGraph)
                {
                    entityRepository.InsertOrUpdateGraph(rating);
                }
                else
                {
                    entityRepository.Update(rating);
                }

                // Persist the changes
                unitOfWork.Save();

                // update score
                UpdateEventBeverageScore(rating);
            }
        }

        private void UpdateEventBeverageScore(Rating rating)
        {
            using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                var eventBeverageRepository = unitOfWork.GetRepository<EventBeverage, int>();
                var eventBeverage = eventBeverageRepository.GetSingle(eb => eb.Id == rating.EventBeverageId);
                var query = from r in unitOfWork.DbContext.Ratings
                    join er in unitOfWork.DbContext.EventRegistrations on r.EventRegistrationId equals er.Id
                    where r.EventBeverageId == rating.EventBeverageId
                    select r;


                eventBeverage.Score = query.Any() ? query.Sum(r => r.Score) : rating.Score;
                eventBeverageRepository.Update(eventBeverage);

                // Persist the changes
                unitOfWork.Save();
            }
        }

        /// <summary>
        /// Deletes the specified Rating.
        /// </summary>
        /// <param name="id">The Rating identifier.</param>
        public void DeleteRating(int id)
        {
            using (var unitOfWork = this.unitOfWorkFactory.Create())
            {
                // Get the entity repository
                IEntityRepository<Rating> entityRepository = unitOfWork.GetRepository<Rating, int>();

                // Insert the entity
                entityRepository.Delete(id);

                // Persist the changes
                unitOfWork.Save();
            }
        }
    }
}
