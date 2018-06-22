namespace BeerAppreciation.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FreeloaderAndConsistentNavigationRenames : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "BA.Beverages", name: "StyleId", newName: "BeverageStyleId");
            RenameColumn(table: "BA.Beverages", name: "TypeId", newName: "BeverageTypeId");
            RenameIndex(table: "BA.Beverages", name: "IX_StyleId", newName: "IX_BeverageStyleId");
            RenameIndex(table: "BA.Beverages", name: "IX_TypeId", newName: "IX_BeverageTypeId");
            AddColumn("BA.EventRegistrations", "Freeloader", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("BA.EventRegistrations", "Freeloader");
            RenameIndex(table: "BA.Beverages", name: "IX_BeverageTypeId", newName: "IX_TypeId");
            RenameIndex(table: "BA.Beverages", name: "IX_BeverageStyleId", newName: "IX_StyleId");
            RenameColumn(table: "BA.Beverages", name: "BeverageTypeId", newName: "TypeId");
            RenameColumn(table: "BA.Beverages", name: "BeverageStyleId", newName: "StyleId");
        }
    }
}
