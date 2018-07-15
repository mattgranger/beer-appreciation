namespace BeerAppreciation.Beverage.Data.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Contexts;
    using Core.Shared.Data.Repositories;
    using global::Core.Shared.Domain;
    using Microsoft.EntityFrameworkCore;

    public class EntityRepository<T, TKey> : IEntityRepository<T, TKey> where T : BaseEntity<TKey>
    {
        private readonly IUnitOfWork<BeverageContext> unitOfWork;
        private readonly IRepository<T> repository;

        public EntityRepository(IUnitOfWork<BeverageContext> unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.repository = this.unitOfWork.GetRepository<T>();
        }

        public async Task SaveChanges(bool ensureAutoHistory = false)
        {
            await this.unitOfWork.SaveChangesAsync(ensureAutoHistory);
        }

        public async Task<IList<T>> GetList()
        {
            var pagedResult = await this.repository.GetPagedListAsync().ConfigureAwait(false);
            return pagedResult.Items;
        }

        public async Task<global::Core.Shared.Paging.IPagedList<T>> GetPagedList(int pageIndex = 0, int pageSize = 100)
        {
            var pagedList = await this.repository.GetPagedListAsync(pageIndex:pageIndex, pageSize:pageSize).ConfigureAwait(false);
            return new global::Core.Shared.Paging.PagedList<T>(pageIndex, pageSize, pagedList.TotalCount, pagedList.Items);
        }

        public async Task<T> GetById(TKey id)
        {
            return await this.GetEntityById(id);
        }

        public async Task<TKey> Insert(T entity)
        {
            await this.repository.InsertAsync(entity);
            return entity.Id;
        }

        public async Task Update(T entity)
        {
            this.repository.Update(entity);
            await Task.CompletedTask;
        }

        public async Task Delete(TKey id)
        {
            var entity = await this.GetEntityById(id);
            this.repository.Delete(entity);
        }

        private async Task<T> GetEntityById(TKey id)
        {
            return await this.repository.FindAsync(new[] {id});
        }
    }
}
