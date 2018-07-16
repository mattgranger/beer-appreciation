namespace BeerAppreciation.Beverage.API
{
    using System;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Core.EventBus.Abstractions;
    using Core.WebApi.Startup;
    using Data.Extensions;
    using Domain;
    using Infrastructure.AutofacModules;
    using Installers;
    using Microsoft.AspNet.OData.Builder;
    using Microsoft.AspNet.OData.Extensions;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.OData.Edm;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services
                .AddCustomMvc(this.Configuration)
                .AddBehaviorOptions(this.Configuration)
                .AddCustomOptions(this.Configuration)
                .AddIntegrationServices(this.Configuration)
                .AddEventBus(this.Configuration, this.Configuration["SubscriptionClientName"])
                .AddSwagger("Beverage API", "v1", "The Beer Appreciation Beverage Microservice API.")
                .AddBeverageDependencies(this.Configuration);

            services
                .AddOData();

            var container = new ContainerBuilder();
            container.Populate(services);

            container
                .RegisterBeverageDataModule()
                .RegisterModule(new ApplicationModule());

            return new AutofacServiceProvider(container.Build());
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //Enabling OData routing.
            app.UseMvc(routeBuilder =>
            {
                routeBuilder.MapODataServiceRoute(
                    "ODataRoute", 
                    "odata",
                    GetBeverageModel());
                routeBuilder.MapRoute("default", "api/v1/{controller}");
                routeBuilder.EnableDependencyInjection();
            });

            app.ConfigureSwaggerWithUi();

            this.ConfigureEventBus(app);
        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
        }

        private static IEdmModel GetBeverageModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Beverage>("Beverages");
            builder.EntitySet<BeverageType>("BeverageTypes");
            builder.EntitySet<BeverageStyle>("BeverageStyles");
            builder.EntitySet<Manufacturer>("Manufacturers");
            return builder.GetEdmModel();
        }
    }
}
