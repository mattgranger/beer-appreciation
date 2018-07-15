namespace BeerAppreciation.Beverage.Data.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Contexts;
    using Domain;
    using Domain.Repositories;
    using Microsoft.EntityFrameworkCore;

    public class BeverageTypeRepository : IBeverageTypeRepository
    {
        private readonly BeverageContext dbContext;

        public BeverageTypeRepository(BeverageContext beverageContext)
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

        public Task<IList<BeverageType>> Get()
        {
            throw new System.NotImplementedException();
        }

        public Task<Core.Data.Paging.IPagedList<BeverageType>> GetPagedList(int pageIndex = 0, int pageSize = 100)
        {
            throw new System.NotImplementedException();
        }

        public Task<BeverageType> GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task Insert(BeverageType entity)
        {
            throw new System.NotImplementedException();
        }

        public Task Update(BeverageType entity)
        {
            throw new System.NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task SaveChanges(bool ensureAutoHistory = false)
        {
            throw new System.NotImplementedException();
        }
    }
}
