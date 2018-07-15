namespace BeerAppreciation.Beverage.Data.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Contexts;
    using Core.Data.Repositories;
    using Domain;
    using Microsoft.EntityFrameworkCore;

    public class BeverageRepository : IGenericRepository<Beverage, int>
    {
        private readonly IUnitOfWork<BeverageContext> unitOfWork;
        private readonly IRepository<Beverage> beverageRepository;

        public BeverageRepository(IUnitOfWork<BeverageContext> unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.beverageRepository = this.unitOfWork.GetRepository<Beverage>();
        }

        public async Task<IList<Beverage>> Get()
        {
            var pagedResult = await this.beverageRepository.GetPagedListAsync().ConfigureAwait(false);
            return pagedResult.Items;
        }

        public async Task<Core.Data.Paging.IPagedList<Beverage>> GetPagedList(int pageIndex = 0, int pageSize = 100)
        {
            var pagedList = await this.beverageRepository.GetPagedListAsync(pageIndex:pageIndex, pageSize:pageSize).ConfigureAwait(false);
            return new Core.Data.Paging.PagedList<Beverage>(pageIndex, pageSize, pagedList.TotalCount, pagedList.Items);
        }

        public async Task<Beverage> GetById(int id)
        {
            return await this.GetBeverageById(id);
        }

        public async Task Insert(Beverage entity)
        {
            await this.beverageRepository.InsertAsync(entity);
        }

        public async Task Update(Beverage entity)
        {
            this.beverageRepository.Update(entity);
            await Task.CompletedTask;
        }

        public async Task Delete(int id)
        {
            var entity = await this.GetBeverageById(id);
            this.beverageRepository.Delete(entity);
        }

        private async Task<Beverage> GetBeverageById(int id)
        {
            return await this.beverageRepository.FindAsync(new[] {id});
        }

        public async Task SaveChanges(bool ensureAutoHistory = false)
        {
            await this.unitOfWork.SaveChangesAsync(ensureAutoHistory);
        }
    }
}
