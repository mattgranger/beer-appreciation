namespace BeerAppreciation.Beverage.API.Installers
{
    using System;
    using System.Reflection;
    using Data.Contexts;
    using Data.Repositories;
    using Domain.Services;
    using global::Core.Shared.Data.Repositories;
    using global::Core.Shared.Data.Services;
    using Infrastructure;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Diagnostics;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    public static class Dependencies
    {
        public static IServiceCollection AddCustomOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<BeverageApiSettings>(configuration);
            return services;
        }

        public static IServiceCollection AddBeverageDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .ConfigureDbContexts(configuration)
                .ConfigureDomainRepositories(configuration)
                .ConfigureDomainRepositories(configuration);

            return services;
        }

        public static IServiceCollection ConfigureDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddUnitOfWork<BeverageContext>()
                .AddDbContext<BeverageContext>(options =>
            {
                options.UseSqlServer(configuration["ConnectionString"],
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(typeof(BeverageContext).GetTypeInfo().Assembly.GetName().Name);
                        //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    });

                // Changing default behavior when client evaluation occurs to throw. 
                // Default in EF Core would be to log a warning when client evaluation is performed.
                options.ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));
                //Check Client vs. Server evaluation: https://docs.microsoft.com/en-us/ef/core/querying/client-eval
            });

            return services;
        }

        public static IServiceCollection ConfigureDomainRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.TryAddTransient(typeof(IEntityRepository<,>), typeof(EntityRepository<,>));

            return services;
        }

        public static IServiceCollection ConfigureDomainServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.TryAddTransient(typeof(IEntityService<,>), typeof(EntityService<,>));

            return services;
        }
    }
}
