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
                "dbo.Demand",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClientId = c.Int(nullable: false),
                        AgentId = c.Int(nullable: false),
                        Id_type = c.Int(nullable: false),
                        Address_City = c.String(),
                        Address_Street = c.String(),
                        Address_House = c.String(),
                        Address_Number = c.String(),
                        MinPrice = c.Long(),
                        MaxPrice = c.Long(),
                        Id_HouseDemand = c.Int(),
                        Id_LandDemand = c.Int(),
                        Id_ApartmentDemand = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Agent", t => t.AgentId, cascadeDelete: true)
                .ForeignKey("dbo.ApartmentDemand", t => t.Id_ApartmentDemand)
                .ForeignKey("dbo.Client", t => t.ClientId, cascadeDelete: true)
                .ForeignKey("dbo.HouseDemand", t => t.Id_HouseDemand)
                .ForeignKey("dbo.LandDemand", t => t.Id_LandDemand)
                .ForeignKey("dbo.TypeRealEstate", t => t.Id_type, cascadeDelete: true)
                .Index(t => t.ClientId)
                .Index(t => t.AgentId)
                .Index(t => t.Id_type)
                .Index(t => t.Id_HouseDemand)
                .Index(t => t.Id_LandDemand)
                .Index(t => t.Id_ApartmentDemand);
            
            CreateTable(
                "dbo.ApartmentDemand",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MinArea = c.Single(),
                        MaxArea = c.Single(),
                        MinRooms = c.Int(),
                        MaxRooms = c.Int(),
                        MinFloor = c.Int(),
                        MaxFloor = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
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
            
            CreateTable(
                "dbo.Supply",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClientId = c.Int(nullable: false),
                        AgentId = c.Int(nullable: false),
                        RealEstateId = c.Int(nullable: false),
                        Price = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Agent", t => t.AgentId, cascadeDelete: true)
                .ForeignKey("dbo.Client", t => t.ClientId, cascadeDelete: true)
                .ForeignKey("dbo.RealEstate", t => t.RealEstateId, cascadeDelete: true)
                .Index(t => t.ClientId)
                .Index(t => t.AgentId)
                .Index(t => t.RealEstateId);
            
            CreateTable(
                "dbo.Deal",
                c => new
                    {
                        Demand_Id = c.Int(nullable: false),
                        Supply_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Demand_Id, t.Supply_Id })
                .ForeignKey("dbo.Demand", t => t.Demand_Id, cascadeDelete: true)
                .ForeignKey("dbo.Supply", t => t.Supply_Id, cascadeDelete: true)
                .Index(t => t.Demand_Id)
                .Index(t => t.Supply_Id);
            
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
                "dbo.Apartment",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Floor = c.Int(),
                        Rooms = c.Int(),
                        TotalArea = c.Single(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RealEstate", t => t.Id)
                .Index(t => t.Id);
            
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
                        TotalFloors = c.Int(),
                        Rooms = c.Int(),
                        TotalArea = c.Single(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RealEstate", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Land",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        TotalArea = c.Single(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RealEstate", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.TypeRealEstate",
                c => new
                    {
                        Id_type = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id_type);
            
            CreateTable(
                "dbo.HouseDemand",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MinArea = c.Single(),
                        MaxArea = c.Single(),
                        MinRooms = c.Int(),
                        MaxRooms = c.Int(),
                        MinFloors = c.Int(),
                        MaxFloors = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LandDemand",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MinArea = c.Single(),
                        MaxArea = c.Single(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Demand", "Id_type", "dbo.TypeRealEstate");
            DropForeignKey("dbo.Demand", "Id_LandDemand", "dbo.LandDemand");
            DropForeignKey("dbo.Demand", "Id_HouseDemand", "dbo.HouseDemand");
            DropForeignKey("dbo.Demand", "ClientId", "dbo.Client");
            DropForeignKey("dbo.Supply", "RealEstateId", "dbo.RealEstate");
            DropForeignKey("dbo.RealEstate", "Id_type", "dbo.TypeRealEstate");
            DropForeignKey("dbo.Land", "Id", "dbo.RealEstate");
            DropForeignKey("dbo.House", "Id", "dbo.RealEstate");
            DropForeignKey("dbo.RealEstate", "District_Id", "dbo.District");
            DropForeignKey("dbo.Apartment", "Id", "dbo.RealEstate");
            DropForeignKey("dbo.Deal", "Supply_Id", "dbo.Supply");
            DropForeignKey("dbo.Deal", "Demand_Id", "dbo.Demand");
            DropForeignKey("dbo.Supply", "ClientId", "dbo.Client");
            DropForeignKey("dbo.Supply", "AgentId", "dbo.Agent");
            DropForeignKey("dbo.Demand", "Id_ApartmentDemand", "dbo.ApartmentDemand");
            DropForeignKey("dbo.Demand", "AgentId", "dbo.Agent");
            DropIndex("dbo.Land", new[] { "Id" });
            DropIndex("dbo.House", new[] { "Id" });
            DropIndex("dbo.Apartment", new[] { "Id" });
            DropIndex("dbo.RealEstate", new[] { "Id_type" });
            DropIndex("dbo.RealEstate", new[] { "District_Id" });
            DropIndex("dbo.Deal", new[] { "Supply_Id" });
            DropIndex("dbo.Deal", new[] { "Demand_Id" });
            DropIndex("dbo.Supply", new[] { "RealEstateId" });
            DropIndex("dbo.Supply", new[] { "AgentId" });
            DropIndex("dbo.Supply", new[] { "ClientId" });
            DropIndex("dbo.Demand", new[] { "Id_ApartmentDemand" });
            DropIndex("dbo.Demand", new[] { "Id_LandDemand" });
            DropIndex("dbo.Demand", new[] { "Id_HouseDemand" });
            DropIndex("dbo.Demand", new[] { "Id_type" });
            DropIndex("dbo.Demand", new[] { "AgentId" });
            DropIndex("dbo.Demand", new[] { "ClientId" });
            DropTable("dbo.LandDemand");
            DropTable("dbo.HouseDemand");
            DropTable("dbo.TypeRealEstate");
            DropTable("dbo.Land");
            DropTable("dbo.House");
            DropTable("dbo.District");
            DropTable("dbo.Apartment");
            DropTable("dbo.RealEstate");
            DropTable("dbo.Deal");
            DropTable("dbo.Supply");
            DropTable("dbo.Client");
            DropTable("dbo.ApartmentDemand");
            DropTable("dbo.Demand");
            DropTable("dbo.Agent");
        }
    }
}
