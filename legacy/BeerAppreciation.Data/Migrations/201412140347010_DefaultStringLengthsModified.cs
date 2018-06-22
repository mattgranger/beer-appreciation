namespace BeerAppreciation.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DefaultStringLengthsModified : DbMigration
    {
        public override void Up()
        {
            AlterColumn("BA.Beverages", "Description", c => c.String());
            AlterColumn("BA.Beverages", "Url", c => c.String(maxLength: 200));
            AlterColumn("BA.BeverageStyles", "Description", c => c.String());
            AlterColumn("BA.BeverageTypes", "Description", c => c.String());
            AlterColumn("BA.Manufacturers", "Description", c => c.String());
            AlterColumn("BA.Events", "Location", c => c.String(maxLength: 300));
            AlterColumn("BA.DrinkingClubs", "Description", c => c.String());
            AlterColumn("BA.DrinkingClubs", "PasswordHash", c => c.String(maxLength: 200));
            AlterColumn("BA.EventRegistrations", "Comments", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("BA.EventRegistrations", "Comments", c => c.String(maxLength: 500));
            AlterColumn("BA.DrinkingClubs", "PasswordHash", c => c.String());
            AlterColumn("BA.DrinkingClubs", "Description", c => c.String(maxLength: 500));
            AlterColumn("BA.Events", "Location", c => c.String());
            AlterColumn("BA.Manufacturers", "Description", c => c.String(maxLength: 500));
            AlterColumn("BA.BeverageTypes", "Description", c => c.String(maxLength: 500));
            AlterColumn("BA.BeverageStyles", "Description", c => c.String(maxLength: 500));
            AlterColumn("BA.Beverages", "Url", c => c.String());
            AlterColumn("BA.Beverages", "Description", c => c.String(maxLength: 800));
        }
    }
}
