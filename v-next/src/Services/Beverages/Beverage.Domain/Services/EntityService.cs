namespace BeerAppreciation.Beverage.Domain.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Shared.Data.Repositories;
    using Core.Shared.Data.Services;
    using global::Core.Shared.Paging;
    using Microsoft.EntityFrameworkCore;

    public class EntityService<T, TKey> : IEntityService<T, TKey> where T : class
    {
        private readonly IEntityRepository<T, TKey> repository;

        public EntityService(IEntityRepository<T, TKey> repository)
        {
            this.repository = repository;
        }

        public DbSet<T> EntitySet => this.repository.EntitySet;

        public async Task<IList<T>> GetList()
        {
            return await this.repository.GetList();
        }

        public async Task<IPagedList<T>> GetPagedList(int pageIndex, int pageSize)
        {
            return await this.repository.GetPagedList(pageIndex, pageSize);
        }

        public async Task<T> GetById(TKey id)
        {
            return await this.repository.GetById(id);
        }

        public async Task<TKey> Insert(T entity)
        {
            return await this.repository.Insert(entity);
        }

        public async Task<T> Update(T entity)
        {
            await this.repository.Update(entity);
            return entity;
        }

        public async Task Delete(TKey id)
        {
            await this.repository.Delete(id);
        }

        public async Task SaveChanges(bool ensureAutoHistory = false)
        {
            await this.repository.SaveChanges(ensureAutoHistory);
        }
    }
}

