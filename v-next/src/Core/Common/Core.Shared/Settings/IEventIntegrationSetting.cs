namespace Core.Shared.Settings
{
    public interface IEventIntegrationSetting
    {
        string SubscriptionClientName { get; set; }

        string EventBusConnection { get; set; }

        bool AzureStorageEnabled { get; set; }
    }
}
