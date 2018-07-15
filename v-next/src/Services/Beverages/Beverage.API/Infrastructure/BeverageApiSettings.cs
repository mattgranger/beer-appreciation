namespace BeerAppreciation.Beverage.API.Infrastructure
{
    using global::Core.Shared.Settings;

    public class BeverageApiSettings : IDatabaseSettings, IEventIntegrationSettings
    {
        public string ConnectionString { get; set; }

        public string SubscriptionClientName { get; set; }

        public string EventBusConnection { get; set; }

        public bool AzureStorageEnabled { get; set; }
    }
}
