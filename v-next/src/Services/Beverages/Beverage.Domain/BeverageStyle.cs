namespace BeerAppreciation.Beverage.Domain
{
    using System.Collections.Generic;
    using global::Core.Shared.Domain;

    public class BeverageStyle : BaseEntity<int>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int? ParentId { get; set; }

        public BeverageStyle Parent { get; set; }

        public int BeverageTypeId { get; set; }

        public BeverageType BeverageType { get; set; }

        public ICollection<BeverageStyle> BeverageStyles { get; set; }
    }
}
