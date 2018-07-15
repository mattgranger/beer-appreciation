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
    public class BeverageTypesController : ControllerBase
    {
        private readonly IBeverageTypeService beverageTypeService;

        public BeverageTypesController(IBeverageTypeService beverageTypeService)
        {
            this.beverageTypeService = beverageTypeService;
        }

        // GET api/v1/[controller]/?pageSize=3&pageIndex=10]
        [HttpGet]
        [ProducesResponseType(typeof(PagedResultModel<BeverageType>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<BeverageType>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get([FromQuery]int pageSize = 10, [FromQuery]int pageIndex = 0)
        {
            var pagedList = await this.beverageTypeService.GetPagedList(pageIndex, pageSize);

            return this.Ok(pagedList.ToPagedResultModel());
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

            var item = await this.beverageTypeService.GetById(id);

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