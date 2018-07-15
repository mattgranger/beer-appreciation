namespace Core.Shared.Settings
{
    public interface IEventIntegrationSettings
    {
        string SubscriptionClientName { get; set; }

        string EventBusConnection { get; set; }

        bool AzureStorageEnabled { get; set; }
    }
}
