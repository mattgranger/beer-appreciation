
namespace EventSauce.Core.Repositories
{
    using EventSauce.Domain.Common;
    using INX.Core.DataAccess;
    using INX.Core.DataAccess.UnitOfWork;

    public class AddressTypeEntityRepository : BaseEntityRepository<AddressType, int, EventSauceContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddressTypeEntityRepository" /> class.
        /// This overload allows the context to be passed in for use within a Unit of Work
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public AddressTypeEntityRepository(EventSauceContext dbContext, IUnitOfWork<EventSauceContext> unitOfWork)
            : base(dbContext, unitOfWork, false)
        {
        }
    }
}

