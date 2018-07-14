namespace BeerAppreciation.Beverage.Data.Contexts
{
    using Domain;
    using EntityConfigurations;
    using Microsoft.EntityFrameworkCore;

    public class BeverageContext : DbContext
    {
        public BeverageContext(DbContextOptions<BeverageContext> options) : base(options)
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
