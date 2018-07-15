namespace BeerAppreciation.Beverage.Data.Repositories
{
    using Contexts;
    using Domain;
    using Microsoft.EntityFrameworkCore;

    public class BeverageRepository : EntityRepository<Beverage, int>, IBeverageRepository
    {
        public BeverageRepository(IUnitOfWork<BeverageContext> unitOfWork) : base(unitOfWork)
        {
        }
    }
}
