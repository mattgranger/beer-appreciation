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
    using IntegrationEvents.Events;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using Model;

    [Route("api/v1/[controller]")]
    public class BeveragesController : ControllerBase
    {
        private readonly CatalogContext catalogContext;
        private readonly IOptionsSnapshot<CatalogSettings> settings;
        private readonly ICatalogIntegrationEventService catalogIntegrationEventService;

        public BeveragesController(CatalogContext catalogContext, IOptionsSnapshot<CatalogSettings> settings, ICatalogIntegrationEventService catalogIntegrationEventService)
        {
            this.catalogContext = catalogContext ?? throw new ArgumentNullException(nameof(catalogContext)); ;
            this.settings = settings;
            this.catalogIntegrationEventService = catalogIntegrationEventService ?? throw new ArgumentNullException(nameof(catalogIntegrationEventService));
        }

        // GET api/v1/[controller]/?pageSize=3&pageIndex=10]
        [HttpGet]
        [ProducesResponseType(typeof(PagedResultModel<Beverage>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<Beverage>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get([FromQuery]int pageSize = 10, [FromQuery]int pageIndex = 0, [FromQuery] string ids = null)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                return await this.GetItemsByIds(ids);
            }

            var totalItems = await this.catalogContext.Beverages
                .LongCountAsync();

            var itemsOnPage = await this.catalogContext.Beverages
                .OrderBy(c => c.Name)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            var model = new PagedResultModel<Beverage>(
                pageIndex, pageSize, totalItems, itemsOnPage);

            return this.Ok(model);
        }

        private async Task<IActionResult> GetItemsByIds(string ids)
        {
            var numIds = ids.Split(',').Select(id => (Ok: int.TryParse(id, out var x), Value: x)).ToList();
            if (!numIds.All(nid => nid.Ok))
            {
                return this.BadRequest("ids value invalid. Must be comma-separated list of numbers");
            }

            var idsToSelect = numIds.Select(id => id.Value);
            var items = await this.catalogContext.Beverages.Where(ci => idsToSelect.Contains(ci.Id)).ToListAsync();

            return this.Ok(items);
        }

        [HttpGet]
        [Route("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Beverage), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
            {
                return this.BadRequest();
            }

            var item = await this.catalogContext.Beverages.SingleOrDefaultAsync(ci => ci.Id == id);

            if (item != null)
            {
                return this.Ok(item);
            }

            return this.NotFound();
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> UpdateProduct([FromBody]Beverage beverage)
        {
            var beverageItem = await this.catalogContext.Beverages
                .SingleOrDefaultAsync(i => i.Id == beverage.Id);

            if (beverageItem == null)
            {
                return this.NotFound(new { Message = $"Beverage with id {beverage.Id} not found." });
            }

            var oldName = beverageItem.Name;
            var raiseBeverageNameChangedEvent = oldName != beverage.Name;

            // Update current beverage
            beverageItem = beverage;
            this.catalogContext.Beverages.Update(beverageItem);

            if (raiseBeverageNameChangedEvent) // Save product's data and publish integration event through the Event Bus if price has changed
            {
                //Create Integration Event to be published through the Event Bus
                var priceChangedEvent = new BeverageNameChangedIntegrationEvent(beverageItem.Id, beverage.Name, oldName);

                // Achieving atomicity between original Catalog database operation and the IntegrationEventLog thanks to a local transaction
                await this.catalogIntegrationEventService.SaveEventAndCatalogContextChangesAsync(priceChangedEvent);

                // Publish through the Event Bus and mark the saved event as published
                await this.catalogIntegrationEventService.PublishThroughEventBusAsync(priceChangedEvent);
            }
            else // Just save the updated product because the Product's Price hasn't changed.
            {
                await this.catalogContext.SaveChangesAsync();
            }

            return this.CreatedAtAction(nameof(GetById), new { id = beverage.Id }, null);
        }

        //POST api/v1/[controller]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> Post([FromBody]Beverage beverage)
        {
            var item = new Beverage
            {
                BeverageTypeId = beverage.BeverageTypeId,
                BeverageStyleId = beverage.BeverageStyleId,
                Description = beverage.Description,
                Name = beverage.Name,
                AlcoholPercent = beverage.AlcoholPercent,
                ManufacturerId = beverage.ManufacturerId,
                Volume = beverage.Volume,
                Url = beverage.Url
            };

            this.catalogContext.Beverages.Add(item);

            await this.catalogContext.SaveChangesAsync();

            return this.CreatedAtAction(nameof(GetById), new { id = item.Id }, null);
        }

        //DELETE api/v1/[controller]/id
        [Route("{id}")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Delete(int id)
        {
            var product = this.catalogContext.Beverages.SingleOrDefault(x => x.Id == id);

            if (product == null)
            {
                return this.NotFound();
            }

            this.catalogContext.Beverages.Remove(product);

            await this.catalogContext.SaveChangesAsync();

            return this.NoContent();
        }
    }
}