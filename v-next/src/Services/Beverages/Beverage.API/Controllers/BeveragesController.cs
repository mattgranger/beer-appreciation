namespace BeerAppreciation.Beverage.API.Controllers
{
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using Core.WebApi.Extensions;
    using Core.WebApi.Models;
    using Domain;
    using Domain.Services;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/v1/[controller]")]
    public class BeveragesController : ControllerBase
    {
        private readonly IBeverageService beverageService;

        public BeveragesController(IBeverageService beverageService)
        {
            this.beverageService = beverageService;
        }

        // GET api/v1/[controller]/?pageSize=3&pageIndex=10]
        [HttpGet]
        [ProducesResponseType(typeof(PagedResultModel<Beverage>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<Beverage>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get([FromQuery]int pageIndex = 0, [FromQuery]int pageSize = 10)
        {
            var pagedList = await this.beverageService.GetPagedList(pageIndex, pageSize);

            return this.Ok(pagedList.ToPagedResultModel());
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

            var beverage = await this.beverageService.GetById(id);

            if (beverage != null)
            {
                return this.Ok(beverage);
            }

            return this.NotFound();
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> Update([FromBody]Beverage beverage)
        {
            var beverageItem = await this.beverageService.GetById(beverage.Id);

            if (beverageItem == null)
            {
                return this.NotFound(new { Message = $"Beverage with id {beverage.Id} not found." });
            }

            var oldName = beverageItem.Name;
            var raiseBeverageNameChangedEvent = oldName != beverage.Name;

            // Update current beverage
            beverageItem = beverage;
            await this.beverageService.Update(beverageItem);

            if (raiseBeverageNameChangedEvent) // Save product's data and publish integration event through the Event Bus if price has changed
            {
                //Create Integration Event to be published through the Event Bus
                //var priceChangedEvent = new BeverageNameChangedIntegrationEvent(beverageItem.Id, beverage.Name, oldName);

                // Achieving atomicity between original Catalog database operation and the IntegrationEventLog thanks to a local transaction
                //await this.catalogIntegrationEventService.SaveEventAndBeverageContextChangesAsync(priceChangedEvent);

                // Publish through the Event Bus and mark the saved event as published
                //await this.catalogIntegrationEventService.PublishThroughEventBusAsync(priceChangedEvent);
            }
            else // Just save the updated product because the Product's Price hasn't changed.
            {
                await this.beverageService.SaveChanges();
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

            await this.beverageService.Insert(item);

            await this.beverageService.SaveChanges();

            return this.CreatedAtAction(nameof(GetById), new { id = item.Id }, null);
        }

        //DELETE api/v1/[controller]/id
        [Route("{id}")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Delete(int id)
        {
            var beverage = await this.beverageService.GetById(id);

            if (beverage == null)
            {
                return this.NotFound();
            }

            await this.beverageService.Delete(id);

            await this.beverageService.SaveChanges();

            return this.NoContent();
        }
    }
}