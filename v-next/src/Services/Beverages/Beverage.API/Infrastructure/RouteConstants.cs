namespace BeerAppreciation.Beverage.API.Infrastructure
{
    public static class RouteConstants
    {
        public const string RouteVersion = "v1";

        public static string ODataRoute = $"odata/{RouteVersion}";

        public static string BeveragesRoute = $"api/{RouteVersion}/beverages";

        public static string BeverageTypesRoute = $"api/{RouteVersion}/beveragetypes";

        public static string BeverageStylesRoute = $"api/{RouteVersion}/beveragestyles";

        public static string ManufacturersRoute = $"api/{RouteVersion}/manufacturers";

    }
}
