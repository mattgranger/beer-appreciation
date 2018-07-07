namespace Catalog.API.Domain
{
    using System.Collections.Generic;

    public class BeverageType
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<BeverageStyle> BeverageStyles { get; set; }
    }
}
