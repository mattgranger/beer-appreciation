namespace BeerAppreciation.Web.Installers
{
    using Castle.Facilities.Logging;
    using Castle.Facilities.TypedFactory;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using System.Web.Http.Controllers;
    using Core.WebApi.ActionFilters;
    using Core.WebApi.DelegatingHandlers;
    using Data.EF.Castle;
    using Data.EF.Repository;
    using Data.EF.UnitOfWork;

    /// <summary>
    /// Windsor installer class that specifies the concrete implementations of interfaces that are required to be under
    /// the control of the dependency injection framework
    /// </summary>
    public class WindsorInstaller : IWindsorInstaller
    {
        /// <summary>
        /// Register any components that will be under dependency injection control
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="store">The configuration store.</param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Kernel.AddFacility<TypedFactoryFacility>();
            container.AddFacility<LoggingFacility>(f => f.LogUsing(LoggerImplementation.Log4net).WithAppConfig());

            // Register the IHttpControllers (for web api controllers)
            container.Register(
                Classes.FromThisAssembly()
                .BasedOn<IHttpController>()
                .LifestylePerWebRequest());

            // Register the windsor container so we can inject it into classes via constructor where required.
            container.Register(
                Component.For<IWindsorContainer>().Instance(container)
                .LifestyleTransient());

            // Register any services by convention (E.g. IBeverageService will map to BeverageService)
            container.Register(
                Classes.FromAssemblyInThisApplication()
                .InNamespace("BeerAppreciation.Data.Services", true)
                .WithService.DefaultInterfaces()
                .LifestylePerWebRequest());

            // Register any repositories by convention (E.g. IRepository<Beverage> will map to BeverageRepository)
            container.Register(
                Classes.FromAssemblyInThisApplication()
                .BasedOn(typeof(IEntityRepository<>)).WithService.Base()
                .LifestylePerWebRequest());

            RegisterFactories(container);

            RegisterFilters(container);
        }

        /// <summary>
        /// Registers any windsor typed factories.
        /// </summary>
        /// <param name="container">The container.</param>
        private static void RegisterFactories(IWindsorContainer container)
        {
            // Register the unit of work factory
            container.Register(Component.For(typeof(IUnitOfWorkFactory<>)).AsFactory());
            container.Register(Component.For(typeof(IEntityRepositoryFactory)).AsFactory());

            container.Register(
                Component.For(typeof(IUnitOfWorkInterceptorFactory<>)).ImplementedBy(typeof(UnitOfWorkInterceptorFactory<>))
                .LifestylePerWebRequest());

            container.Register(
                Component.For(typeof(IUnitOfWork<>)).ImplementedBy(typeof(UnitOfWork<>))
                .LifestylePerWebRequest());
        }

        /// <summary>
        /// Registers the action filters with the windsor container.
        /// </summary>
        /// <param name="container">The container.</param>
        private static void RegisterFilters(IWindsorContainer container)
        {
            container.Register(
                Component.For<WebApiExceptionFilterAttribute>().ImplementedBy<WebApiExceptionFilterAttribute>()
                .LifeStyle.Transient);

            container.Register(
                Component.For<ValidationFilterAttribute>().ImplementedBy<ValidationFilterAttribute>()
                .LifeStyle.Transient);

            container.Register(
                Component.For<RequestResponseLoggingDelegatingHandler>().ImplementedBy<RequestResponseLoggingDelegatingHandler>()
                .LifeStyle.Transient);
        }
    }
}