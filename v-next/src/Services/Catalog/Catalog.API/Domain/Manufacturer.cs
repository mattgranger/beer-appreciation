namespace Catalog.API.Domain
{
    using System.Collections.Generic;

    public class Manufacturer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Country { get; set; }

        public ICollection<Beverage> Beverages { get; set; }
    }
}
