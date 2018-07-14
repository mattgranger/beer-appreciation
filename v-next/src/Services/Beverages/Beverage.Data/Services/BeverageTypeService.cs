namespace BeerAppreciation.Beverage.Data.Services
{
    using System.Threading.Tasks;
    using Contexts;
    using Domain;
    using Domain.Services;
    using Microsoft.EntityFrameworkCore;

    public class BeverageTypeService : IBeverageTypeService
    {
        private readonly BeverageContext dbContext;

        public BeverageTypeService(BeverageContext beverageContext)
        {
            this.dbContext = beverageContext;
        }

        public async Task<BeverageType> GetById(int id, string includes = "")
        {
            var beverageType = await this.dbContext
                .BeverageTypes
                .Include(includes)
                .SingleOrDefaultAsync(ci => ci.Id == id);

            return beverageType;
        }
    }
}
