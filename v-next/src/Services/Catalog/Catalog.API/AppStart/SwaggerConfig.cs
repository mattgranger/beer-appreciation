namespace Catalog.API.AppStart
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;

    public static class SwaggerConfig
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "BeerAppreciation - Catalog HTTP API",
                    Version = "v1",
                    Description = "The Catalog Microservice HTTP API. All beverages and associated domain can be found here",
                    //TermsOfService = "Terms Of Service - Be Cool"
                });
            });
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "BeerAppreciation API V1");
                });
        }
    }
}
