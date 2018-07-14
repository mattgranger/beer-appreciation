namespace BeerAppreciation.Beverage.Data.Contexts
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;

    public class BeverageContextDesignFactory : IDesignTimeDbContextFactory<BeverageContext>
    {
        public BeverageContext CreateDbContext(string[] args)
        {
            var optionsBuilder =  new DbContextOptionsBuilder<BeverageContext>()
                .UseSqlServer("Server=.;Initial Catalog=BeerAppreciation.BeverageDb;Integrated Security=true");

            return new BeverageContext(optionsBuilder.Options);
        }
    }
}