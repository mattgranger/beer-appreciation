namespace BeerAppreciation.Beverage.API.Controllers
{
    using Core.WebApi.Controllers;
    using Domain;
    using global::Core.Shared.Data.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [Route("api/v1/[controller]")]
    [ApiController]
    public class BeverageTypesController : EntityController<BeverageType, int>
    {
        public BeverageTypesController(IEntityService<BeverageType, int> entityService, ILogger<EntityController<BeverageType, int>> logger) : base(entityService, logger)
        {
        }
    }
}