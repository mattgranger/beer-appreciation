namespace BeerAppreciation.Beverage.Data.Settings
{
    using Core.Shared.Settings;

    public class BeverageSettings : IDatabaseSetting
    {
        public string ConnectionString { get; set; }
    }
}
