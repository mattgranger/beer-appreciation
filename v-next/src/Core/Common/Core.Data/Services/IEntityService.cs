﻿namespace Core.Shared.Domain
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Data;
    using Paging;

    public interface IEntityService<T, TKey> : IUnitOfWork
    {
        Task<IList<T>> GetList();
        Task<IPagedList<T>> GetPagedList(int pageIndex, int pageSize);
        Task<T> GetById(TKey id);
        Task<TKey> Insert(T entity);
        Task<T> Update(T entity);
        Task Delete(TKey id);
    }
}
