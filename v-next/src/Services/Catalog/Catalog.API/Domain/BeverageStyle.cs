namespace Catalog.API.Domain
{
    public class BeverageStyle
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int? ParentId { get; set; }

        public BeverageStyle Parent { get; set; }

        public int BeverageTypeId { get; set; }

        public BeverageType BeverageType { get; set; }
    }
}
