namespace BeerAppreciation.Beverage.Data.Repositories
{
    using Contexts;
    using Domain;
    using Microsoft.EntityFrameworkCore;

    public class BeverageRepository : EntityRepository<Beverage, int>, IBeverageRepository
    {
        public BeverageRepository(BeverageContext beverageContext) : base(beverageContext)
        {
        }
    }
}
