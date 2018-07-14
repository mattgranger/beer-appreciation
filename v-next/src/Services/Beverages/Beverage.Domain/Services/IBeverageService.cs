namespace BeerAppreciation.Beverage.Domain.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IBeverageService
    {
        Task<IList<Beverage>> GetBeverages();
    }
}
