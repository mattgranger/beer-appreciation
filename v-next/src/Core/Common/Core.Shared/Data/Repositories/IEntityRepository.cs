﻿namespace Core.Shared.Data.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Paging;

    public interface IEntityRepository<T, TKey> where T : class
    {
        DbSet<T> EntitySet { get; }
        Task<IList<T>> GetList();
        Task<IPagedList<T>> GetPagedList(int pageIndex = 0, int pageSize = 100);
        Task<T> GetById(TKey id);
        Task<TKey> Insert(T entity);
        Task Update(T entity);
        Task Delete(object id);
    }
}
