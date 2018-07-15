namespace Core.Shared.Data.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Paging;

    public interface IEntityRepository<T, TKey> : IUnitOfWork
    {
        Task<IList<T>> GetList();
        Task<IPagedList<T>> GetPagedList(int pageIndex = 0, int pageSize = 100);
        Task<T> GetById(TKey id);
        Task<TKey> Insert(T entity);
        Task Update(T entity);
        Task Delete(TKey id);
    }
}
