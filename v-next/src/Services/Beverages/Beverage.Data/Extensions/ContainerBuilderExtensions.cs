namespace BeerAppreciation.Beverage.Data.Extensions
{
    using Autofac;
    using Modules;

    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterBeverageDataModule(this ContainerBuilder container)
        {
            container.RegisterModule(new DataModule());
            return container;
        }
    }
}
