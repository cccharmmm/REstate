namespace REstate1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Agent",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LastName = c.String(),
                        FirstName = c.String(),
                        MiddleName = c.String(),
                        DealShare = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Apartment",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Floor = c.Int(nullable: false),
                        Rooms = c.Int(nullable: false),
                        TotalArea = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RealEstate", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.RealEstate",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Address_City = c.String(),
                        Address_Street = c.String(),
                        Address_House = c.String(),
                        Address_Number = c.String(),
                        District_Id = c.Int(),
                        Id_type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.District", t => t.District_Id)
                .ForeignKey("dbo.TypeRealEstate", t => t.Id_type, cascadeDelete: true)
                .Index(t => t.District_Id)
                .Index(t => t.Id_type);
            
            CreateTable(
                "dbo.District",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Area = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.House",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        TotalFloors = c.Int(nullable: false),
                        Rooms = c.Int(nullable: false),
                        TotalArea = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RealEstate", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Land",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        TotalArea = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RealEstate", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Supply",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClientId = c.Int(nullable: false),
                        AgentId = c.Int(nullable: false),
                        RealEstateId = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RealEstate", t => t.RealEstateId, cascadeDelete: true)
                .Index(t => t.RealEstateId);
            
            CreateTable(
                "dbo.TypeRealEstate",
                c => new
                    {
                        Id_type = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id_type);
            
            CreateTable(
                "dbo.Client",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LastName = c.String(),
                        FirstName = c.String(),
                        MiddleName = c.String(),
                        Phone = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Apartment", "Id", "dbo.RealEstate");
            DropForeignKey("dbo.RealEstate", "Id_type", "dbo.TypeRealEstate");
            DropForeignKey("dbo.Supply", "RealEstateId", "dbo.RealEstate");
            DropForeignKey("dbo.Land", "Id", "dbo.RealEstate");
            DropForeignKey("dbo.House", "Id", "dbo.RealEstate");
            DropForeignKey("dbo.RealEstate", "District_Id", "dbo.District");
            DropIndex("dbo.Supply", new[] { "RealEstateId" });
            DropIndex("dbo.Land", new[] { "Id" });
            DropIndex("dbo.House", new[] { "Id" });
            DropIndex("dbo.RealEstate", new[] { "Id_type" });
            DropIndex("dbo.RealEstate", new[] { "District_Id" });
            DropIndex("dbo.Apartment", new[] { "Id" });
            DropTable("dbo.Client");
            DropTable("dbo.TypeRealEstate");
            DropTable("dbo.Supply");
            DropTable("dbo.Land");
            DropTable("dbo.House");
            DropTable("dbo.District");
            DropTable("dbo.RealEstate");
            DropTable("dbo.Apartment");
            DropTable("dbo.Agent");
        }
    }
}
