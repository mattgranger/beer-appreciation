﻿namespace BeerAppreciation.Beverage.API
{
    using System;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Core.EventBus.Abstractions;
    using Core.WebApi.Startup;
    using Data.Contexts;
    using Infrastructure;
    using Infrastructure.AutofacModules;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

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
                .AddUnitOfWork<BeverageContext>();

            var container = new ContainerBuilder();
            container.Populate(services);

            container.RegisterModule(new ApplicationModule());

            return new AutofacServiceProvider(container.Build());
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var pathBase = this.Configuration["PATH_BASE"];

            if (!string.IsNullOrEmpty(pathBase))
            {
                loggerFactory.CreateLogger("init").LogDebug($"Using PATH BASE '{pathBase}'");
                app.UsePathBase(pathBase);
            }

            app.UseCors("CorsPolicy");

            app.UseMvcWithDefaultRoute();

            app.UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint($"{ (!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty) }/swagger/v1/swagger.json", "Catalog.API V1");
                });

            this.ConfigureEventBus(app);
        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
        }
    }

    public static class StartupConfiguration
    {
        public static IServiceCollection AddCustomOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<BeverageApiSettings>(configuration);
            return services;
        }
    }
}
