namespace BeerAppreciation.Beverage.Domain
{
    using System.Collections.Generic;
    using global::Core.Shared.Domain;

    public class BeverageType : BaseEntity<int>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<BeverageStyle> BeverageStyles { get; set; }
    }
}
