namespace BeerAppreciation.Beverage.API.Controllers
{
    using Core.WebApi.Controllers;
    using Domain;
    using global::Core.Shared.Data.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [Route("api/v1/[controller]")]
    public class BeverageStylesController : EntityController<BeverageStyle, int>
    {
        public BeverageStylesController(IEntityService<BeverageStyle, int> entityService, ILogger<EntityController<BeverageStyle, int>> logger) : base(entityService, logger)
        {
        }
    }
}