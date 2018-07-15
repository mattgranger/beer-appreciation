namespace BeerAppreciation.Core.WebApi.Startup
{
    using Microsoft.AspNetCore.Builder;
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

        public static void ConfigureSwagger(
            this IApplicationBuilder app)
        {
            app.UseSwagger();
        }

        public static void ConfigureSwaggerWithUi(this IApplicationBuilder app, 
            string endpointName = "API V1",
            string url = "/swagger/v1/swagger.json")
        {
            app.UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint(url, endpointName);
                });
        }
    }
}
