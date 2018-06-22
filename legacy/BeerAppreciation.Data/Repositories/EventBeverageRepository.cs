namespace BeerAppreciation.Data.Repositories
{
    using Domain;
    using Context;
    using EF.Repository;
    using EF.UnitOfWork;

    public class EventBeverageRepository : BaseEntityRepository<EventBeverage, int, DatabaseContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventBeverageRepository" /> class.
        /// This overload allows the context to be passed in for use within a Unit of Work
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public EventBeverageRepository(DatabaseContext dbContext, IUnitOfWork<DatabaseContext> unitOfWork)
            : base(dbContext, unitOfWork, false)
        {
        }
    }
}

