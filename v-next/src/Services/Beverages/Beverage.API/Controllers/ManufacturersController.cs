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
    [ApiController]
    public class ManufacturersController : ControllerBase
    {
        private readonly IManufacturerService manufacturerService;

        public ManufacturersController(IManufacturerService manufacturerService)
        {
            this.manufacturerService = manufacturerService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResultModel<Manufacturer>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<Manufacturer>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get([FromQuery]int pageSize = 10, [FromQuery]int pageIndex = 0)
        {
            var pagedList = await this.manufacturerService.GetPagedList(pageIndex, pageSize);

            return this.Ok(pagedList.ToPagedResultModel());
        }
    }
}