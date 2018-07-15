namespace BeerAppreciation.Beverage.Domain.Services
{
    using Core.Shared.Data.Repositories;

    public class BeverageService : EntityService<Beverage, int>, IBeverageService
    {
        public BeverageService(IEntityRepository<Beverage, int> repository) : base(repository)
        {
        }
    }
}