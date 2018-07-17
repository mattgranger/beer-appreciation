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
        private readonly BeverageContext beverageContext;

        public EntityRepository(BeverageContext beverageContext)
        {
            this.beverageContext = beverageContext;
        }

        public DbSet<T> EntitySet => this.beverageContext.Set<T>();

        public async Task<IList<T>> GetList()
        {
            return await this.EntitySet.AsNoTracking().ToListAsync();
        }

        public async Task<Core.Shared.Paging.IPagedList<T>> GetPagedList(int pageIndex = 0, int pageSize = 100)
        {
            var pagedList = await this.EntitySet.ToPagedListAsync(pageIndex:pageIndex, pageSize:pageSize).ConfigureAwait(false);
            return new global::Core.Shared.Paging.PagedList<T>(pageIndex, pageSize, pagedList.TotalCount, pagedList.Items);
        }

        public async Task<T> GetById(TKey id)
        {
            return await this.GetEntityById(id);
        }

        public async Task<TKey> Insert(T entity)
        {
            await this.beverageContext.AddAsync(entity);
            await this.beverageContext.SaveChangesAsync();
            return entity.Id;
        }

        public async Task Update(T entity)
        {
            this.beverageContext.Update(entity);
            await this.beverageContext.SaveChangesAsync();
        }

        public async Task Delete(object id)
        {
            this.beverageContext.Remove(id);
            await this.beverageContext.SaveChangesAsync();
        }

        private async Task<T> GetEntityById(TKey id)
        {
            return await this.EntitySet.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }
    }
}
