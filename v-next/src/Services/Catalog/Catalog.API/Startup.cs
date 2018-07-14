namespace Catalog.API
{
    using System;
    using AppStart;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services
                .AddCustomOptions(this.Configuration)
                .AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            Dependencies.Register(services, this.Configuration);
            SwaggerConfig.ConfigureServices(services);

            var container = new ContainerBuilder();
            container.Populate(services);
            return new AutofacServiceProvider(container.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            SwaggerConfig.Configure(app, env);
        }
    }

    public static class StartupConfiguration
    {
        public static IServiceCollection AddCustomOptions(this IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }
        
        public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }
    }
}
