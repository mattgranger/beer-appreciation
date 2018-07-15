namespace BeerAppreciation.Beverage.Data.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Contexts;
    using Domain;
    using Domain.Repositories;
    using Microsoft.EntityFrameworkCore;

    public class BeverageRepository : IBeverageRepository
    {
        private readonly IUnitOfWork<BeverageContext> unitOfWork;
        private readonly IRepository<Beverage> beverageRepository;

        public BeverageRepository(IUnitOfWork<BeverageContext> unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.beverageRepository = this.unitOfWork.GetRepository<Beverage>();
        }

        public async Task<IList<Beverage>> GetList()
        {
            var pagedResult = await this.beverageRepository.GetPagedListAsync().ConfigureAwait(false);
            return pagedResult.Items;
        }

        public async Task<global::Core.Shared.Paging.IPagedList<Beverage>> GetPagedList(int pageIndex = 0, int pageSize = 100)
        {
            var pagedList = await this.beverageRepository.GetPagedListAsync(pageIndex:pageIndex, pageSize:pageSize).ConfigureAwait(false);
            return new global::Core.Shared.Paging.PagedList<Beverage>(pageIndex, pageSize, pagedList.TotalCount, pagedList.Items);
        }

        public async Task<Beverage> GetById(int id)
        {
            return await this.GetBeverageById(id);
        }

        public async Task<int> Insert(Beverage entity)
        {
            await this.beverageRepository.InsertAsync(entity);
            return entity.Id;
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

        public async Task SaveChanges(bool ensureAutoHistory = false)
        {
            await this.unitOfWork.SaveChangesAsync(ensureAutoHistory);
        }

        private async Task<Beverage> GetBeverageById(int id)
        {
            return await this.beverageRepository.FindAsync(new[] {id});
        }
    }
}
