namespace BeerAppreciation.Core.WebApi.Startup
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;

    public static class SwaggerServices
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services, string title, string version, string description, string termsOfService = "Terms of Service")
        {
            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc(version, new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Title = title,
                    Version = version,
                    Description = description,
                    TermsOfService = termsOfService
                });
            });

            return services;
        }

        public static void Configure(
            IApplicationBuilder app, 
            IHostingEnvironment env, 
            string url = "/swagger/v1/swagger.json", 
            string endpointName = "API V1")
        {
            app.UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint(url, endpointName);
                });
        }
    }
}
