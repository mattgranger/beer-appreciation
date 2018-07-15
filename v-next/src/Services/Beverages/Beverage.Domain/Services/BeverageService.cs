namespace BeerAppreciation.Beverage.Domain.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Repositories;

    public class BeverageService : IBeverageService
    {
        public BeverageService(IBeverageRepository beverageRepository)
        {
            
        }

        public Task<IList<Beverage>> GetBeverages()
        {
            throw new System.NotImplementedException();
        }
    }
}