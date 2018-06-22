using System.Data.Entity;
using System.Web;
using System.Web.Http;
using System.Web.Optimization;
using System.Web.Routing;
using BeerAppreciation.Core.WebApi;
using BeerAppreciation.Data.Repositories.Context;
using BeerAppreciation.SignalR;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace BeerAppreciation.Web
{
    using System;

    public class MvcApplication : HttpApplication
    {
        private BackgroundHubTimer hubTimer;

        protected void Application_Start()
        {
            SetDatabaseInitialiser();

            var config = new HttpConfiguration();
            // Run installers and wire up the WebApi dependency injection
            IWindsorContainer container = RegisterWindsorContainer(config);

            hubTimer = new BackgroundHubTimer();

            FilterConfig.RegisterWebApiGlobalFilters(config.Filters, container);
            WebApiConfig.RegisterDelegatingHandlers(config, container);
            WebApiConfig.Register(config);
            GlobalConfiguration.Configure(WebApiConfig.Register);

            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private void SetDatabaseInitialiser()
        {
            // allow automatic database update
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DatabaseContext, Data.Migrations.Configuration>());
            using (var db = new DatabaseContext())
            {
                foreach (var type in db.BeverageTypes)
                {
                    Console.WriteLine(type.Name);
                }
            }
        }

        /// <summary>
        /// Runs all installers within this assembly and wires up WebApi to use the castle windsor dependency injection container
        /// </summary>
        /// <param name="config">The HTTP configuration.</param>
        /// <returns>
        /// The dependency injection container
        /// </returns>
        private static IWindsorContainer RegisterWindsorContainer(HttpConfiguration config)
        {
            // Create the windsor container and run all installers in the application 
            IWindsorContainer container = new WindsorContainer().Install(FromAssembly.InThisApplication());

            // Wire up webapi to use the windsor resolver for dependency injection
            var resolver = new WindsorDependencyResolver(container.Kernel);
            config.DependencyResolver = resolver;
            GlobalConfiguration.Configuration.DependencyResolver = resolver;

            return container;
        }
    }
}
