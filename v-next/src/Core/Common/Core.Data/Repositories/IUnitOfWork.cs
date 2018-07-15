namespace BeerAppreciation.Core.Data.Repositories
{
    using System.Threading.Tasks;

    public interface IUnitOfWork
    {
        Task SaveChanges(bool ensureAutoHistory = false);
    }
}
