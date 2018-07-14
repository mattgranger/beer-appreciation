namespace BeerAppreciation.Beverage.Domain.Services
{
    using System.Threading.Tasks;

    public interface IBeverageTypeService
    {
        Task<BeverageType> GetById(int id, string includes = "");
    }
}
