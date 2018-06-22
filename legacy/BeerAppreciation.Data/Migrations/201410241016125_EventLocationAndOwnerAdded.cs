namespace BeerAppreciation.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventLocationAndOwnerAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("BA.Events", "Location", c => c.String());
            AddColumn("BA.Events", "OwnerId", c => c.String(maxLength: 128));

            Sql("Update [BA].[Events] Set OwnerId = (SELECT Id FROM [BA].[Appreciators] Where Email = 'beer@codefusion.com.au')");

            CreateIndex("BA.Events", "OwnerId");
            AddForeignKey("BA.Events", "OwnerId", "BA.Appreciators", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("BA.Events", "OwnerId", "BA.Appreciators");
            DropIndex("BA.Events", new[] { "OwnerId" });
            DropColumn("BA.Events", "OwnerId");
            DropColumn("BA.Events", "Location");
        }
    }
}
