namespace BeerAppreciation.Beverage.API.Controllers.OData
{
    using System.Linq;
    using Domain;
    using Domain.Services;
    using Microsoft.AspNet.OData;
    using Microsoft.AspNet.OData.Routing;
    using Microsoft.AspNetCore.Mvc;

    [ODataRoutePrefix("beveragetypes")]
    public class BeverageTypesController : ODataController
    {
        private readonly IBeverageTypeService beverageTypeService;

        public BeverageTypesController(IBeverageTypeService beverageTypeService)
        {
            this.beverageTypeService = beverageTypeService;
        }

        // GET odata/v1/[controller]/?[odata]
        [HttpGet]
        [EnableQuery]
        public IQueryable<BeverageType> Get()
        {
            return this.beverageTypeService.EntitySet;
        }

        // GET odata/v1/Beverages(1)
        [HttpGet]
        [EnableQuery]
        public SingleResult<BeverageType> Get([FromODataUri] int key)
        {
            IQueryable<BeverageType> result = this.beverageTypeService.EntitySet.Where(p => p.Id == key);
            return SingleResult.Create(result);
        }
    }
}