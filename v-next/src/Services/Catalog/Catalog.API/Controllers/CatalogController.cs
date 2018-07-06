namespace Catalog.API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Domain;
    using Infrastructure;
    using Infrastructure.Contexts;
    using IntegrationEvents;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using Model;

    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly CatalogContext catalogContext;
        private readonly IOptionsSnapshot<CatalogSettings> settings;
        private readonly ICatalogIntegrationEventService catalogIntegrationEventService;

        public CatalogController(CatalogContext catalogContext, IOptionsSnapshot<CatalogSettings> settings, ICatalogIntegrationEventService catalogIntegrationEventService)
        {
            this.catalogContext = catalogContext ?? throw new ArgumentNullException(nameof(catalogContext)); ;
            this.settings = settings;
            this.catalogIntegrationEventService = catalogIntegrationEventService ?? throw new ArgumentNullException(nameof(catalogIntegrationEventService));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(List<Manufacturer>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Manufacturers()
        {
            var items = await this.catalogContext.Manufacturers
                .ToListAsync();

            return this.Ok(items);
        }
    }
}