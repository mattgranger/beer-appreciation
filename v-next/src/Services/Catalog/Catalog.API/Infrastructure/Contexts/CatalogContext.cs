namespace Catalog.API.Infrastructure.Contexts
{
    using Domain;
    using EntityConfigurations;
    using Microsoft.EntityFrameworkCore;

    public class CatalogContext : DbContext
    {
        public CatalogContext(DbContextOptions<CatalogContext> options) : base(options)
        {
        }

        public DbSet<Manufacturer> Manufacturers { get; set; }

        public DbSet<BeverageType> BeverageTypes { get; set; }

        public DbSet<BeverageStyle> BeverageStyles { get; set; }

        public DbSet<Beverage> Beverages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ManufacturerEntityConfiguration());
            modelBuilder.ApplyConfiguration(new BeverageTypeEntityConfiguration());
            modelBuilder.ApplyConfiguration(new BeverageStyleEntityConfiguration());
            modelBuilder.ApplyConfiguration(new BeverageEntityConfiguration());
        }
    }
}
