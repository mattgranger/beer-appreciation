namespace BeerAppreciation.Beverage.Domain
{
    using System.Collections.Generic;
    using global::Core.Shared.Domain;

    public class Manufacturer : BaseEntity<int>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Country { get; set; }

        public ICollection<Beverage> Beverages { get; set; }
    }
}
