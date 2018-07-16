namespace BeerAppreciation.Beverage.API.Controllers.OData
{
    using System.Linq;
    using Domain;
    using Domain.Services;
    using Microsoft.AspNet.OData;
    using Microsoft.AspNet.OData.Routing;
    using Microsoft.AspNetCore.Mvc;

    [ODataRoutePrefix("beverages")]
    public class BeveragesController : ODataController
    {
        private readonly IBeverageService beverageService;

        public BeveragesController(IBeverageService beverageService)
        {
            this.beverageService = beverageService;
        }

        // GET odata/v1/[controller]/?[odata]
        [HttpGet]
        [EnableQuery]
        public IQueryable<Beverage> Get()
        {
            return this.beverageService.EntitySet;
        }

        // GET odata/v1/Beverages(1)
        [EnableQuery]
        [HttpGet]
        public SingleResult<Beverage> Get([FromODataUri] int key)
        {
            IQueryable<Beverage> result = this.beverageService.EntitySet.Where(p => p.Id == key);
            return SingleResult.Create(result);
        }

        // GET odata/v1/Beverages(1)/BeverageType
        [EnableQuery]
        [HttpGet]
        public SingleResult<BeverageType> GetBeverageType([FromODataUri] int key)
        {
            var result = this.beverageService.EntitySet.Where(b => b.Id == key).Select(bt => bt.BeverageType);
            return SingleResult.Create(result);
        }

        // GET odata/v1/Beverages(1)/BeverageStyle
        [EnableQuery]
        public SingleResult<BeverageStyle> GetBeverageStyle([FromODataUri] int key)
        {
            var result = this.beverageService.EntitySet.Where(b => b.Id == key).Select(bt => bt.BeverageStyle);
            return SingleResult.Create(result);
        }

        // GET odata/v1/Beverages(1)/Manufacturer
        [EnableQuery]
        public SingleResult<Manufacturer> GetManufacturer([FromODataUri] int key)
        {
            var result = this.beverageService.EntitySet.Where(b => b.Id == key).Select(bt => bt.Manufacturer);
            return SingleResult.Create(result);
        }
    }
}