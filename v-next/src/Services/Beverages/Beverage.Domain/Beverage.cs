namespace BeerAppreciation.Beverage.Domain
{
    using global::Core.Shared.Domain;

    public class Beverage : BaseEntity<int>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal? AlcoholPercent { get; set; }

        public int? Volume { get; set; }

        public string Url { get; set; }

        public int BeverageStyleId { get; set; }

        public int BeverageTypeId { get; set; }

        public int ManufacturerId { get; set; }

        public BeverageStyle BeverageStyle { get; set; }

        public BeverageType BeverageType { get; set; }

        public Manufacturer Manufacturer { get; set; }
    }
}
