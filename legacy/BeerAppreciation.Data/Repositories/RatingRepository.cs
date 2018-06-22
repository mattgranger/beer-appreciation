
namespace BeerAppreciation.Data.Repositories
{
    using Domain;
    using Context;
    using EF.Repository;
    using EF.UnitOfWork;

    public class RatingRepository : BaseEntityRepository<Rating, int, DatabaseContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RatingRepository" /> class.
        /// This overload allows the context to be passed in for use within a Unit of Work
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public RatingRepository(DatabaseContext dbContext, IUnitOfWork<DatabaseContext> unitOfWork)
            : base(dbContext, unitOfWork, false)
        {
        }
    }
}

