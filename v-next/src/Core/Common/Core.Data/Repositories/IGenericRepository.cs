namespace BeerAppreciation.Core.Data.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Paging;

    public interface IGenericRepository<T, in TKey> : IUnitOfWork
    {
        Task<IList<T>> Get();
        Task<IPagedList<T>> GetPagedList(int pageIndex = 0, int pageSize = 100);
        Task<T> GetById(TKey id);
        Task Insert(T entity);
        Task Update(T entity);
        Task Delete(TKey id);
    }
}
