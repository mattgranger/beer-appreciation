﻿namespace BeerAppreciation.Beverage.API
{
    using System;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Core.EventBus.Abstractions;
    using Core.WebApi.Startup;
    using Data.Contexts;
    using Data.Extensions;
    using Infrastructure.AutofacModules;
    using Installers;
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
                .AddBeverageDependencies(this.Configuration);

            var container = new ContainerBuilder();
            container.Populate(services);

            container
                .RegisterBeverageDataModule()
                .RegisterModule(new ApplicationModule());

            return new AutofacServiceProvider(container.Build());
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.ConfigureSwaggerWithUi();

            this.ConfigureEventBus(app);
        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
        }
    }
}
