namespace BeerAppreciation.Beverage.API.Controllers
{
    using Core.WebApi.Controllers;
    using Domain;
    using global::Core.Shared.Data.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [Route("api/v1/[controller]")]
    [ApiController]
    public class BeveragesController : EntityController<Beverage, int>
    {
        public BeveragesController(IEntityService<Beverage, int> entityService, ILogger<EntityController<Beverage, int>> logger) : base(entityService, logger)
        {
        }
    }
}