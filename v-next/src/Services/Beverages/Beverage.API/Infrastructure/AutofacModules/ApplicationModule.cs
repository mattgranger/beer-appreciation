namespace BeerAppreciation.Beverage.API.Infrastructure.AutofacModules
{
    using Autofac;
    using Domain.Repositories;

    public class ApplicationModule
        : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<IBeverageRepository>()
                .As<IBeverageRepository>()
                .InstancePerLifetimeScope();

            //builder.RegisterAssemblyTypes(typeof(BasicIntegrationEventHandler).GetTypeInfo().Assembly)
            //    .AsClosedTypesOf(typeof(IIntegrationEventHandler<>));
        }
    }
}
