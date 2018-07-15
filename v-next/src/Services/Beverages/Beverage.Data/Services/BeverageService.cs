namespace BeerAppreciation.Beverage.Data.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain;
    using Domain.Services;
    using Microsoft.EntityFrameworkCore;

    public class BeverageService : IBeverageService
    {
        private readonly IUnitOfWork unitOfWork;

        public BeverageService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<IList<Beverage>> GetBeverages()
        {
            var beverageRepository = this.unitOfWork.GetRepository<Beverage>();
            var pagedList = await beverageRepository.GetPagedListAsync();

            return pagedList.Items;
        }
    }
}