namespace BeerAppreciation.Beverage.Domain.Services
{
    using System.Threading.Tasks;
    using Core.Data.Repositories;

    public interface IBeverageTypeService  : IGenericRepository<Beverage, int>
    {
        Task<BeverageType> GetById(int id, string includes = "");
    }
}
