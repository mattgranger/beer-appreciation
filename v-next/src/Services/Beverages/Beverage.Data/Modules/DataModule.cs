namespace BeerAppreciation.Beverage.Data.Modules
{
    using System.Reflection;
    using Autofac;
    using Core.Shared.Data.Repositories;
    using Core.Shared.Data.Services;
    using Core.Shared.Domain;
    using Domain.Services;
    using Repositories;

    public class DataModule
        : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            this.RegisterRepositories(builder);
            this.RegisterServices(builder);
        }

        private void RegisterRepositories(ContainerBuilder builder)
        {
            var dataAssembly = typeof(EntityRepository<,>).GetTypeInfo().Assembly;

            builder.RegisterAssemblyTypes(dataAssembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(EntityRepository<,>))
                .As(typeof(IEntityRepository<,>))
                .InstancePerLifetimeScope();
        }
        
        private void RegisterServices(ContainerBuilder builder)
        {
            var serviceAssembly = typeof(EntityService<,>).GetTypeInfo().Assembly;

            builder.RegisterAssemblyTypes(serviceAssembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(EntityService<,>))
                .As(typeof(IEntityService<,>))
                .InstancePerLifetimeScope();
        }
    }
}
