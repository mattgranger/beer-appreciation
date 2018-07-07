using Microsoft.AspNetCore.Mvc;

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
    using Infrastructure.Services;
    using IntegrationEvents;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using Model;

    [Route("api/v1/[controller]")]
    [ApiController]
    public class BeverageTypesController : ControllerBase
    {
        private readonly CatalogContext catalogContext;
        private readonly IOptionsSnapshot<CatalogSettings> settings;
        private readonly ICatalogIntegrationEventService catalogIntegrationEventService;
        private readonly IBeverageTypeService beverageTypeService;

        public BeverageTypesController(
            CatalogContext catalogContext, 
            IOptionsSnapshot<CatalogSettings> settings, 
            ICatalogIntegrationEventService catalogIntegrationEventService,
            IBeverageTypeService beverageTypeService)
        {
            this.catalogContext = catalogContext ?? throw new ArgumentNullException(nameof(catalogContext)); ;
            this.settings = settings;
            this.catalogIntegrationEventService = catalogIntegrationEventService ?? throw new ArgumentNullException(nameof(catalogIntegrationEventService));
            this.beverageTypeService = beverageTypeService;
        }

        // GET api/v1/[controller]/?pageSize=3&pageIndex=10]
        [HttpGet]
        [ProducesResponseType(typeof(PagedResultModel<BeverageType>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<BeverageType>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get([FromQuery]int pageSize = 10, [FromQuery]int pageIndex = 0)
        {
            var totalItems = await this.catalogContext.BeverageTypes
                .LongCountAsync();

            var itemsOnPage = await this.catalogContext.BeverageTypes
                .OrderBy(c => c.Name)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            var model = new PagedResultModel<BeverageType>(
                pageIndex, pageSize, totalItems, itemsOnPage);

            return this.Ok(model);
        }

        [HttpGet]
        [Route("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BeverageType), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById(int id, string includes = "")
        {
            if (id <= 0)
            {
                return this.BadRequest();
            }

            var item = await this.catalogContext.BeverageTypes.Include(includes).SingleOrDefaultAsync(ci => ci.Id == id);

            if (item != null)
            {
                return this.Ok(item);
            }

            return this.NotFound();
        }

        // GET api/v1/[controller]/{id}/styles
        [HttpGet]
        [Route("{id}/styles")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<BeverageStyle>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetBeverageStyles(int id)
        {
            var beverageType = await this.beverageTypeService.GetById(id, "BeverageStyles");
            if (beverageType == null)
            {
                return this.NotFound();
            }

            return this.Ok(beverageType.BeverageStyles);
        }
    }
}