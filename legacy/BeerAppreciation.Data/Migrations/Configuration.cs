using System.Linq;
using BeerAppreciation.Data.Repositories.Context;

namespace BeerAppreciation.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using Core.Security;
    using Domain;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public sealed class Configuration : DbMigrationsConfiguration<DatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(DatabaseContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            if (!context.Manufacturers.Any())
            {
                RunDataPopulationScript(context);
            }
            if (!context.Roles.Any())
            {
                var userManager = new UserManager<Appreciator>(new UserStore<Appreciator>(context));
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

                CreateRoles(roleManager);
                CreateDefaultAdmin(userManager);
            }

            base.Seed(context);
        }

        private static void CreateRoles(RoleManager<IdentityRole, string> roleManager)
        {
            if (!roleManager.RoleExists(RoleNames.Admin))
            {
                roleManager.Create(new IdentityRole(RoleNames.Admin));
            }
            if (!roleManager.RoleExists(RoleNames.ClubAdmin))
            {
                roleManager.Create(new IdentityRole(RoleNames.ClubAdmin));
            }
            if (!roleManager.RoleExists(RoleNames.Member))
            {
                roleManager.Create(new IdentityRole(RoleNames.Member));
            }
        }

        private const string DefaultAdminEmail = "beer@codefusion.com.au";
        private const string DefaultAdminPassword = "B33r_1s_G00d!";

        private static void CreateDefaultAdmin(UserManager<Appreciator, string> userManager)
        {
            var admin = userManager.FindByNameAsync(DefaultAdminEmail).Result;
            if (admin != null) return;
            var adminUser = new Appreciator() { UserName = DefaultAdminEmail, DrinkingName = "UberDrunk", Email = DefaultAdminEmail };
            var userResult = userManager.Create(adminUser, DefaultAdminPassword);
            if (userResult.Succeeded)
            {
                userManager.AddToRole(adminUser.Id, RoleNames.Admin);
            }
        }

        private static void RunDataPopulationScript(DbContext context)
        {
            var sqlScript = Properties.Resources.DataPopulationScript;
            try
            {
                context.Database.ExecuteSqlCommand(sqlScript);
            }
            catch (Exception)
            {
                // need to hook up logging framework
                throw;
            }
        }
    }
}
