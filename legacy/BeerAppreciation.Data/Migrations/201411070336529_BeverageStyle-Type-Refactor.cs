namespace BeerAppreciation.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BeverageStyleTypeRefactor : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("BA.BeverageStyles", "TypeId", "BA.BeverageTypes");
            DropIndex("BA.BeverageStyles", new[] { "TypeId" });
            DropForeignKey("BA.BeverageStyles", "BeverageType_Id", "BA.BeverageTypes");
            DropIndex("BA.BeverageStyles", new[] { "BeverageType_Id" });
            DropColumn("BA.BeverageStyles", "BeverageType_Id");

            AddColumn("BA.BeverageStyles", "BeverageTypeId", c => c.Int(true));
            Sql("Update BA.BeverageStyles SET BeverageTypeId = TypeId");
            AlterColumn("BA.BeverageStyles", "BeverageTypeId", c => c.Int(nullable: false));

            CreateIndex("BA.BeverageStyles", "BeverageTypeId");
            AddForeignKey("BA.BeverageStyles", "BeverageTypeId", "BA.BeverageTypes", "Id");
            DropColumn("BA.BeverageStyles", "TypeId");
        }
        
        public override void Down()
        {
            AddColumn("BA.BeverageStyles", "TypeId", c => c.Int(nullable: false));
            DropIndex("BA.BeverageStyles", new[] { "BeverageTypeId" });
            AlterColumn("BA.BeverageStyles", "BeverageTypeId", c => c.Int());
            RenameColumn(table: "BA.BeverageStyles", name: "BeverageTypeId", newName: "BeverageType_Id");
            CreateIndex("BA.BeverageStyles", "BeverageType_Id");
            CreateIndex("BA.BeverageStyles", "TypeId");
            AddForeignKey("BA.BeverageStyles", "TypeId", "BA.BeverageTypes", "Id");
        }
    }
}
