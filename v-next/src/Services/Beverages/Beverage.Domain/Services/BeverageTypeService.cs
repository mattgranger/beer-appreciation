namespace BeerAppreciation.Beverage.Domain.Services
{
    using Core.Shared.Data.Repositories;

    public class BeverageTypeService : EntityService<BeverageType, int>, IBeverageTypeService
    {
        public BeverageTypeService(IEntityRepository<BeverageType, int> repository) : base(repository)
        {
        }
    }
}