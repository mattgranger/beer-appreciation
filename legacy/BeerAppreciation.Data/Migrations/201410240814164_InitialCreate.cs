namespace BeerAppreciation.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "BA.Beverages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 800),
                        AlcoholPercent = c.Decimal(precision: 18, scale: 2),
                        Volume = c.Int(),
                        Url = c.String(),
                        StyleId = c.Int(nullable: false),
                        TypeId = c.Int(nullable: false),
                        ManufacturerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("BA.Manufacturers", t => t.ManufacturerId, cascadeDelete: true)
                .ForeignKey("BA.BeverageStyles", t => t.StyleId, cascadeDelete: true)
                .ForeignKey("BA.BeverageTypes", t => t.TypeId, cascadeDelete: true)
                .Index(t => t.StyleId)
                .Index(t => t.TypeId)
                .Index(t => t.ManufacturerId);
            
            CreateTable(
                "BA.Manufacturers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 500),
                        Country = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "IX_ManufacturerName");
            
            CreateTable(
                "BA.BeverageStyles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 500),
                        ParentId = c.Int(),
                        TypeId = c.Int(nullable: false),
                        BeverageType_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("BA.BeverageStyles", t => t.ParentId)
                .ForeignKey("BA.BeverageTypes", t => t.BeverageType_Id)
                .ForeignKey("BA.BeverageTypes", t => t.TypeId)
                .Index(t => t.Name, unique: true, name: "IX_BeverageStyleName")
                .Index(t => t.ParentId)
                .Index(t => t.TypeId)
                .Index(t => t.BeverageType_Id);
            
            CreateTable(
                "BA.BeverageTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "IX_BeverageTypeName");
            
            CreateTable(
                "BA.EventBeverages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EventId = c.Int(nullable: false),
                        EventRegistrationId = c.Int(nullable: false),
                        BeverageId = c.Int(nullable: false),
                        DrinkingOrder = c.Int(nullable: false),
                        Score = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("BA.Beverages", t => t.BeverageId, cascadeDelete: true)
                .ForeignKey("BA.Events", t => t.EventId, cascadeDelete: true)
                .ForeignKey("BA.EventRegistrations", t => t.EventRegistrationId)
                .Index(t => t.EventId)
                .Index(t => t.EventRegistrationId)
                .Index(t => t.BeverageId);
            
            CreateTable(
                "BA.Events",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(),
                        DrinkingClubId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("BA.DrinkingClubs", t => t.DrinkingClubId, cascadeDelete: true)
                .Index(t => t.DrinkingClubId);
            
            CreateTable(
                "BA.DrinkingClubs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 500),
                        PasswordHash = c.String(),
                        IsPrivate = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "IX_DrinkingClubName");
            
            CreateTable(
                "BA.Appreciators",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        DrinkingName = c.String(maxLength: 100),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "BA.AppreciatorClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("BA.Appreciators", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "BA.AppreciatorLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("BA.Appreciators", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "BA.EventRegistrations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AppreciatorId = c.String(nullable: false, maxLength: 128),
                        EventId = c.Int(nullable: false),
                        RegistrationDate = c.DateTime(nullable: false),
                        Comments = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("BA.Appreciators", t => t.AppreciatorId, cascadeDelete: true)
                .ForeignKey("BA.Events", t => t.EventId, cascadeDelete: true)
                .Index(t => t.AppreciatorId)
                .Index(t => t.EventId);
            
            CreateTable(
                "BA.Ratings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EventRegistrationId = c.Int(nullable: false),
                        Score = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SubmittedDate = c.DateTime(nullable: false),
                        Comments = c.String(),
                        EventBeverageId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("BA.EventBeverages", t => t.EventBeverageId, cascadeDelete: true)
                .ForeignKey("BA.EventRegistrations", t => t.EventRegistrationId)
                .Index(t => t.EventRegistrationId)
                .Index(t => t.EventBeverageId);
            
            CreateTable(
                "BA.AppreciatorRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("BA.Appreciators", t => t.UserId, cascadeDelete: true)
                .ForeignKey("BA.SecurityRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "BA.SecurityRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "BA.DrinkingClubMembers",
                c => new
                    {
                        DrinkingClubId = c.Int(nullable: false),
                        AppreciatorId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.DrinkingClubId, t.AppreciatorId })
                .ForeignKey("BA.DrinkingClubs", t => t.DrinkingClubId, cascadeDelete: true)
                .ForeignKey("BA.Appreciators", t => t.AppreciatorId, cascadeDelete: true)
                .Index(t => t.DrinkingClubId)
                .Index(t => t.AppreciatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("BA.AppreciatorRoles", "RoleId", "BA.SecurityRoles");
            DropForeignKey("BA.EventBeverages", "EventRegistrationId", "BA.EventRegistrations");
            DropForeignKey("BA.DrinkingClubMembers", "AppreciatorId", "BA.Appreciators");
            DropForeignKey("BA.DrinkingClubMembers", "DrinkingClubId", "BA.DrinkingClubs");
            DropForeignKey("BA.AppreciatorRoles", "UserId", "BA.Appreciators");
            DropForeignKey("BA.Ratings", "EventRegistrationId", "BA.EventRegistrations");
            DropForeignKey("BA.Ratings", "EventBeverageId", "BA.EventBeverages");
            DropForeignKey("BA.EventRegistrations", "EventId", "BA.Events");
            DropForeignKey("BA.EventRegistrations", "AppreciatorId", "BA.Appreciators");
            DropForeignKey("BA.AppreciatorLogins", "UserId", "BA.Appreciators");
            DropForeignKey("BA.AppreciatorClaims", "UserId", "BA.Appreciators");
            DropForeignKey("BA.Events", "DrinkingClubId", "BA.DrinkingClubs");
            DropForeignKey("BA.EventBeverages", "EventId", "BA.Events");
            DropForeignKey("BA.EventBeverages", "BeverageId", "BA.Beverages");
            DropForeignKey("BA.Beverages", "TypeId", "BA.BeverageTypes");
            DropForeignKey("BA.Beverages", "StyleId", "BA.BeverageStyles");
            DropForeignKey("BA.BeverageStyles", "TypeId", "BA.BeverageTypes");
            DropForeignKey("BA.BeverageStyles", "BeverageType_Id", "BA.BeverageTypes");
            DropForeignKey("BA.BeverageStyles", "ParentId", "BA.BeverageStyles");
            DropForeignKey("BA.Beverages", "ManufacturerId", "BA.Manufacturers");
            DropIndex("BA.DrinkingClubMembers", new[] { "AppreciatorId" });
            DropIndex("BA.DrinkingClubMembers", new[] { "DrinkingClubId" });
            DropIndex("BA.SecurityRoles", "RoleNameIndex");
            DropIndex("BA.AppreciatorRoles", new[] { "RoleId" });
            DropIndex("BA.AppreciatorRoles", new[] { "UserId" });
            DropIndex("BA.Ratings", new[] { "EventBeverageId" });
            DropIndex("BA.Ratings", new[] { "EventRegistrationId" });
            DropIndex("BA.EventRegistrations", new[] { "EventId" });
            DropIndex("BA.EventRegistrations", new[] { "AppreciatorId" });
            DropIndex("BA.AppreciatorLogins", new[] { "UserId" });
            DropIndex("BA.AppreciatorClaims", new[] { "UserId" });
            DropIndex("BA.Appreciators", "UserNameIndex");
            DropIndex("BA.DrinkingClubs", "IX_DrinkingClubName");
            DropIndex("BA.Events", new[] { "DrinkingClubId" });
            DropIndex("BA.EventBeverages", new[] { "BeverageId" });
            DropIndex("BA.EventBeverages", new[] { "EventRegistrationId" });
            DropIndex("BA.EventBeverages", new[] { "EventId" });
            DropIndex("BA.BeverageTypes", "IX_BeverageTypeName");
            DropIndex("BA.BeverageStyles", new[] { "BeverageType_Id" });
            DropIndex("BA.BeverageStyles", new[] { "TypeId" });
            DropIndex("BA.BeverageStyles", new[] { "ParentId" });
            DropIndex("BA.BeverageStyles", "IX_BeverageStyleName");
            DropIndex("BA.Manufacturers", "IX_ManufacturerName");
            DropIndex("BA.Beverages", new[] { "ManufacturerId" });
            DropIndex("BA.Beverages", new[] { "TypeId" });
            DropIndex("BA.Beverages", new[] { "StyleId" });
            DropTable("BA.DrinkingClubMembers");
            DropTable("BA.SecurityRoles");
            DropTable("BA.AppreciatorRoles");
            DropTable("BA.Ratings");
            DropTable("BA.EventRegistrations");
            DropTable("BA.AppreciatorLogins");
            DropTable("BA.AppreciatorClaims");
            DropTable("BA.Appreciators");
            DropTable("BA.DrinkingClubs");
            DropTable("BA.Events");
            DropTable("BA.EventBeverages");
            DropTable("BA.BeverageTypes");
            DropTable("BA.BeverageStyles");
            DropTable("BA.Manufacturers");
            DropTable("BA.Beverages");
        }
    }
}
