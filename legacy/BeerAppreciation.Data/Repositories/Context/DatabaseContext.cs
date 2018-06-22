using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeerAppreciation.Domain;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BeerAppreciation.Data.Repositories.Context
{
    public class DatabaseContext : IdentityDbContext<Appreciator>
    {
        public DatabaseContext()
            : base("DatabaseContext")
        {
            // lazy loading can also be turned off
            // by removing the virtual keyword on properties
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<BeverageStyle> BeverageStyles { get; set; }
        public DbSet<BeverageType> BeverageTypes { get; set; }
        public DbSet<Beverage> Beverages { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventRegistration> EventRegistrations { get; set; }
        public DbSet<EventBeverage> EventBeverages { get; set; }

        public static DatabaseContext Create()
        {
            return new DatabaseContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Appreciator>().ToTable("Appreciators", "BA");
            modelBuilder.Entity<IdentityUserRole>().ToTable("AppreciatorRoles", "BA");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("AppreciatorClaims", "BA");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("AppreciatorLogins", "BA");
            modelBuilder.Entity<IdentityRole>().ToTable("SecurityRoles", "BA");
                
            modelBuilder.Entity<BeverageStyle>()
                .HasRequired(t => t.BeverageType)
                .WithMany(bt => bt.Styles)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DrinkingClub>()
                .HasMany(d => d.Members)
                .WithMany(a => a.DrinkingClubs)
                .Map(cm => cm.ToTable("DrinkingClubMembers", "BA").MapLeftKey(new [] { "DrinkingClubId"}).MapRightKey(new []{ "AppreciatorId"}));

            modelBuilder.Entity<EventBeverage>()
                .HasRequired(c => c.EventRegistration)
                .WithMany(er => er.Beverages)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Rating>()
                .HasRequired(c => c.EventRegistration)
                .WithMany(er => er.Ratings)
                .WillCascadeOnDelete(false);
        }
    }
}
