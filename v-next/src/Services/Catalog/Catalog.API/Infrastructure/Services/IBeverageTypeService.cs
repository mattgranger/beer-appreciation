namespace Catalog.API.Infrastructure.Services
{
    using System.Threading.Tasks;
    using Contexts;
    using Domain;
    using Microsoft.EntityFrameworkCore;

    public interface IBeverageTypeService
    {
        Task<BeverageType> GetById(int id, string includes = "");
    }

    public class BeverageTypeService : IBeverageTypeService
    {
        private readonly CatalogContext catalogContext;

        public BeverageTypeService(CatalogContext catalogContext)
        {
            this.catalogContext = catalogContext;
        }

        public async Task<BeverageType> GetById(int id, string includes = "")
        {
            var beverageType = await this.catalogContext
                .BeverageTypes
                .Include(includes)
                .SingleOrDefaultAsync(ci => ci.Id == id);

            return beverageType;
        }
    }
}
