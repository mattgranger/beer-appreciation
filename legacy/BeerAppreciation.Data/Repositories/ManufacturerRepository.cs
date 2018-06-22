namespace BeerAppreciation.Data.Repositories
{
    using Context;
    using Domain;
    using EF.Repository;
    using EF.UnitOfWork;

    public class ManufacturerRepository : BaseEntityRepository<Manufacturer, int, DatabaseContext>, IManufacturerRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ManufacturerRepository" /> class.
        /// This overload allows the context to be passed in for use within a Unit of Work
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public ManufacturerRepository(DatabaseContext dbContext, IUnitOfWork<DatabaseContext> unitOfWork)
            : base(dbContext, unitOfWork, false)
        {
        }
    }
}