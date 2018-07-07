namespace Catalog.API.AppStart
{
    using Infrastructure.Services;
    using IntegrationEvents;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class Dependencies
    {
        public static void Register(IServiceCollection services, IConfiguration configuration)
        {
            DbContextConfig.ConfigureServices(services, configuration);
            RegisterServices(services);
        }

        public static void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<ICatalogIntegrationEventService, CatalogIntegrationEventService>();

            services.AddTransient<IBeverageTypeService, BeverageTypeService>();
        }
    }
}
