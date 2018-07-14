namespace BeerAppreciation.Core.EventBusRabbitMQ
{
    public static class ExchangeTypes
    {
        public const string Direct = "direct";

        public const string Topic = "topic";

        public const string Header = "header";

        public const string Fanout = "fanout";
    }
}
