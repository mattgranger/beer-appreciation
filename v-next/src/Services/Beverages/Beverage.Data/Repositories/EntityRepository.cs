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
        private readonly IRepositoryFactory repositoryFactory;

        public EntityRepository(IUnitOfWork<BeverageContext> unitOfWork, IRepositoryFactory repositoryFactory)
        {
            this.unitOfWork = unitOfWork;
            this.repositoryFactory = repositoryFactory;
        }

        public DbSet<T> EntitySet => this.unitOfWork.DbContext.Set<T>();

        public async Task SaveChanges(bool ensureAutoHistory = false)
        {
            await this.unitOfWork.SaveChangesAsync(ensureAutoHistory);
        }

        public async Task<IList<T>> GetList()
        {
            var pagedResult = await this.repositoryFactory.GetRepository<T>().GetPagedListAsync().ConfigureAwait(false);
            return pagedResult.Items;
        }

        public async Task<global::Core.Shared.Paging.IPagedList<T>> GetPagedList(int pageIndex = 0, int pageSize = 100)
        {
            var pagedList = await this.repositoryFactory.GetRepository<T>().GetPagedListAsync(pageIndex:pageIndex, pageSize:pageSize).ConfigureAwait(false);
            return new global::Core.Shared.Paging.PagedList<T>(pageIndex, pageSize, pagedList.TotalCount, pagedList.Items);
        }

        public async Task<T> GetById(TKey id)
        {
            return await this.GetEntityById(id);
        }

        public async Task<TKey> Insert(T entity)
        {
            var repo = this.unitOfWork.GetRepository<T>();
            await repo.InsertAsync(entity);
            await this.unitOfWork.SaveChangesAsync();
            return entity.Id;
        }

        public async Task Update(T entity)
        {
            var repo = this.unitOfWork.GetRepository<T>();
            repo.Update(entity);
            await this.unitOfWork.SaveChangesAsync();
        }

        public async Task Delete(TKey id)
        {
            var repo = this.unitOfWork.GetRepository<T>();
            repo.Delete(id);
            await this.unitOfWork.SaveChangesAsync();
        }

        private async Task<T> GetEntityById(TKey id)
        {
            return await this.repositoryFactory.GetRepository<T>().GetFirstOrDefaultAsync(x => x, x => x.Id.Equals(id));
        }
    }
}
