namespace CarParts.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DB_Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admins",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Email = c.String(),
                        Password = c.String(),
                        Role = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Admin_Token",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AdminID = c.Guid(),
                        AuthToken = c.String(),
                        IssuedOn = c.DateTime(),
                        ExpiresOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Email = c.String(),
                        Password = c.String(),
                        Phone = c.String(),
                        Name = c.String(),
                        Address = c.String(),
                        ProfilePicture = c.String(),
                        CreatedOn = c.DateTime(),
                        UserCode = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Customer_Token",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CustomerID = c.Guid(),
                        AuthToken = c.String(),
                        IssuedOn = c.DateTime(),
                        ExpiresOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MasterMainCategories",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        MasterMainCategoryName = c.String(),
                        AdminId = c.Guid(),
                        AddedOn = c.DateTime(nullable: false),
                        LogoFileName = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Admins", t => t.AdminId)
                .Index(t => t.AdminId);
            
            CreateTable(
                "dbo.MasterProductBrands",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        MasterProductBrandName = c.String(),
                        AdminId = c.Guid(),
                        AddedOn = c.DateTime(nullable: false),
                        LogoFileName = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Admins", t => t.AdminId)
                .Index(t => t.AdminId);
            
            CreateTable(
                "dbo.MasterProductCategories",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        MasterMainCategoryId = c.Guid(),
                        MasterSubCategoryId = c.Guid(),
                        ProductCategoryName = c.String(),
                        AdminId = c.Guid(),
                        AddedOn = c.DateTime(nullable: false),
                        LogoFileName = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Admins", t => t.AdminId)
                .ForeignKey("dbo.MasterMainCategories", t => t.MasterMainCategoryId)
                .ForeignKey("dbo.MasterSubCategories", t => t.MasterSubCategoryId)
                .Index(t => t.MasterMainCategoryId)
                .Index(t => t.MasterSubCategoryId)
                .Index(t => t.AdminId);
            
            CreateTable(
                "dbo.MasterSubCategories",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        MasterMainCategoryId = c.Guid(),
                        MasterSubCategoryName = c.String(),
                        AdminId = c.Guid(),
                        AddedOn = c.DateTime(nullable: false),
                        LogoFileName = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Admins", t => t.AdminId)
                .ForeignKey("dbo.MasterMainCategories", t => t.MasterMainCategoryId)
                .Index(t => t.MasterMainCategoryId)
                .Index(t => t.AdminId);
            
            CreateTable(
                "dbo.MasterProductSpecificationLabels",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Label = c.String(),
                        AdminId = c.Guid(),
                        AddedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Admins", t => t.AdminId)
                .Index(t => t.AdminId);
            
            CreateTable(
                "dbo.MasterVehicleEngines",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        EngineName = c.String(),
                        AdminId = c.Guid(),
                        AddedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Admins", t => t.AdminId)
                .Index(t => t.AdminId);
            
            CreateTable(
                "dbo.MasterVehicleMakers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        MakerName = c.String(),
                        AdminId = c.Guid(),
                        AddedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Admins", t => t.AdminId)
                .Index(t => t.AdminId);
            
            CreateTable(
                "dbo.MasterVehicleModels",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ModelName = c.String(),
                        AdminId = c.Guid(),
                        AddedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Admins", t => t.AdminId)
                .Index(t => t.AdminId);
            
            CreateTable(
                "dbo.MasterVehicleSubModels",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        SubModelName = c.String(),
                        AdminId = c.Guid(),
                        AddedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Admins", t => t.AdminId)
                .Index(t => t.AdminId);
            
            CreateTable(
                "dbo.MasterVehicleYears",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Year = c.Int(nullable: false),
                        AdminId = c.Guid(),
                        AddedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Admins", t => t.AdminId)
                .Index(t => t.AdminId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        MasterSubCategoryId = c.Guid(),
                        MasterMainCategoryId = c.Guid(),
                        MasterProductCategoryId = c.Guid(),
                        MasterProductBrandId = c.Guid(),
                        Name = c.String(),
                        ProductSKU = c.String(),
                        AdminId = c.Guid(),
                        AddedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Admins", t => t.AdminId)
                .ForeignKey("dbo.MasterMainCategories", t => t.MasterMainCategoryId)
                .ForeignKey("dbo.MasterProductBrands", t => t.MasterProductBrandId)
                .ForeignKey("dbo.MasterProductCategories", t => t.MasterProductCategoryId)
                .ForeignKey("dbo.MasterSubCategories", t => t.MasterSubCategoryId)
                .Index(t => t.MasterSubCategoryId)
                .Index(t => t.MasterMainCategoryId)
                .Index(t => t.MasterProductCategoryId)
                .Index(t => t.MasterProductBrandId)
                .Index(t => t.AdminId);
            
            CreateTable(
                "dbo.ProductImages",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ProductId = c.Guid(),
                        FileName = c.String(),
                        AdminId = c.Guid(),
                        AddedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Admins", t => t.AdminId)
                .ForeignKey("dbo.Products", t => t.ProductId)
                .Index(t => t.ProductId)
                .Index(t => t.AdminId);
            
            CreateTable(
                "dbo.ProductPrices",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ProductId = c.Guid(),
                        CurrentPrice = c.Double(nullable: false),
                        AdminId = c.Guid(),
                        AddedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Admins", t => t.AdminId)
                .ForeignKey("dbo.Products", t => t.ProductId)
                .Index(t => t.ProductId)
                .Index(t => t.AdminId);
            
            CreateTable(
                "dbo.ProductPriceLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Guid(),
                        Value = c.Double(nullable: false),
                        Status = c.String(),
                        AdminId = c.Guid(),
                        AddedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Admins", t => t.AdminId)
                .ForeignKey("dbo.Products", t => t.ProductId)
                .Index(t => t.ProductId)
                .Index(t => t.AdminId);
            
            CreateTable(
                "dbo.ProductSpecificationLabelDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Guid(),
                        MasterProductSpecificationLabelId = c.Guid(),
                        Value = c.String(),
                        AdminId = c.Guid(),
                        AddedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Admins", t => t.AdminId)
                .ForeignKey("dbo.MasterProductSpecificationLabels", t => t.MasterProductSpecificationLabelId)
                .ForeignKey("dbo.Products", t => t.ProductId)
                .Index(t => t.ProductId)
                .Index(t => t.MasterProductSpecificationLabelId)
                .Index(t => t.AdminId);
            
            CreateTable(
                "dbo.ProductStocks",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ProductId = c.Guid(),
                        CurrentStock = c.Double(nullable: false),
                        AdminId = c.Guid(),
                        AddedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Admins", t => t.AdminId)
                .ForeignKey("dbo.Products", t => t.ProductId)
                .Index(t => t.ProductId)
                .Index(t => t.AdminId);
            
            CreateTable(
                "dbo.ProductStockLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Guid(),
                        Value = c.Double(nullable: false),
                        Status = c.String(),
                        AdminId = c.Guid(),
                        AddedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Admins", t => t.AdminId)
                .ForeignKey("dbo.Products", t => t.ProductId)
                .Index(t => t.ProductId)
                .Index(t => t.AdminId);
            
            CreateTable(
                "dbo.Vehicles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        MasterVehicleYearId = c.Guid(),
                        MasterVehicleMakerId = c.Guid(),
                        MasterVehicleModelId = c.Guid(),
                        MasterVehicleSubModelId = c.Guid(),
                        MasterVehicleEngineId = c.Guid(),
                        AdminId = c.Guid(),
                        AddedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Admins", t => t.AdminId)
                .ForeignKey("dbo.MasterVehicleEngines", t => t.MasterVehicleEngineId)
                .ForeignKey("dbo.MasterVehicleMakers", t => t.MasterVehicleMakerId)
                .ForeignKey("dbo.MasterVehicleModels", t => t.MasterVehicleModelId)
                .ForeignKey("dbo.MasterVehicleSubModels", t => t.MasterVehicleSubModelId)
                .ForeignKey("dbo.MasterVehicleYears", t => t.MasterVehicleYearId)
                .Index(t => t.MasterVehicleYearId)
                .Index(t => t.MasterVehicleMakerId)
                .Index(t => t.MasterVehicleModelId)
                .Index(t => t.MasterVehicleSubModelId)
                .Index(t => t.MasterVehicleEngineId)
                .Index(t => t.AdminId);
            
            CreateTable(
                "dbo.VehicleFitments",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ProductId = c.Guid(),
                        VehicleId = c.Guid(),
                        FitmentInfo = c.String(),
                        AdminId = c.Guid(),
                        AddedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Admins", t => t.AdminId)
                .ForeignKey("dbo.Products", t => t.ProductId)
                .ForeignKey("dbo.Vehicles", t => t.VehicleId)
                .Index(t => t.ProductId)
                .Index(t => t.VehicleId)
                .Index(t => t.AdminId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VehicleFitments", "VehicleId", "dbo.Vehicles");
            DropForeignKey("dbo.VehicleFitments", "ProductId", "dbo.Products");
            DropForeignKey("dbo.VehicleFitments", "AdminId", "dbo.Admins");
            DropForeignKey("dbo.Vehicles", "MasterVehicleYearId", "dbo.MasterVehicleYears");
            DropForeignKey("dbo.Vehicles", "MasterVehicleSubModelId", "dbo.MasterVehicleSubModels");
            DropForeignKey("dbo.Vehicles", "MasterVehicleModelId", "dbo.MasterVehicleModels");
            DropForeignKey("dbo.Vehicles", "MasterVehicleMakerId", "dbo.MasterVehicleMakers");
            DropForeignKey("dbo.Vehicles", "MasterVehicleEngineId", "dbo.MasterVehicleEngines");
            DropForeignKey("dbo.Vehicles", "AdminId", "dbo.Admins");
            DropForeignKey("dbo.ProductStockLogs", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ProductStockLogs", "AdminId", "dbo.Admins");
            DropForeignKey("dbo.ProductStocks", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ProductStocks", "AdminId", "dbo.Admins");
            DropForeignKey("dbo.ProductSpecificationLabelDetails", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ProductSpecificationLabelDetails", "MasterProductSpecificationLabelId", "dbo.MasterProductSpecificationLabels");
            DropForeignKey("dbo.ProductSpecificationLabelDetails", "AdminId", "dbo.Admins");
            DropForeignKey("dbo.ProductPriceLogs", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ProductPriceLogs", "AdminId", "dbo.Admins");
            DropForeignKey("dbo.ProductPrices", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ProductPrices", "AdminId", "dbo.Admins");
            DropForeignKey("dbo.ProductImages", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ProductImages", "AdminId", "dbo.Admins");
            DropForeignKey("dbo.Products", "MasterSubCategoryId", "dbo.MasterSubCategories");
            DropForeignKey("dbo.Products", "MasterProductCategoryId", "dbo.MasterProductCategories");
            DropForeignKey("dbo.Products", "MasterProductBrandId", "dbo.MasterProductBrands");
            DropForeignKey("dbo.Products", "MasterMainCategoryId", "dbo.MasterMainCategories");
            DropForeignKey("dbo.Products", "AdminId", "dbo.Admins");
            DropForeignKey("dbo.MasterVehicleYears", "AdminId", "dbo.Admins");
            DropForeignKey("dbo.MasterVehicleSubModels", "AdminId", "dbo.Admins");
            DropForeignKey("dbo.MasterVehicleModels", "AdminId", "dbo.Admins");
            DropForeignKey("dbo.MasterVehicleMakers", "AdminId", "dbo.Admins");
            DropForeignKey("dbo.MasterVehicleEngines", "AdminId", "dbo.Admins");
            DropForeignKey("dbo.MasterProductSpecificationLabels", "AdminId", "dbo.Admins");
            DropForeignKey("dbo.MasterProductCategories", "MasterSubCategoryId", "dbo.MasterSubCategories");
            DropForeignKey("dbo.MasterSubCategories", "MasterMainCategoryId", "dbo.MasterMainCategories");
            DropForeignKey("dbo.MasterSubCategories", "AdminId", "dbo.Admins");
            DropForeignKey("dbo.MasterProductCategories", "MasterMainCategoryId", "dbo.MasterMainCategories");
            DropForeignKey("dbo.MasterProductCategories", "AdminId", "dbo.Admins");
            DropForeignKey("dbo.MasterProductBrands", "AdminId", "dbo.Admins");
            DropForeignKey("dbo.MasterMainCategories", "AdminId", "dbo.Admins");
            DropIndex("dbo.VehicleFitments", new[] { "AdminId" });
            DropIndex("dbo.VehicleFitments", new[] { "VehicleId" });
            DropIndex("dbo.VehicleFitments", new[] { "ProductId" });
            DropIndex("dbo.Vehicles", new[] { "AdminId" });
            DropIndex("dbo.Vehicles", new[] { "MasterVehicleEngineId" });
            DropIndex("dbo.Vehicles", new[] { "MasterVehicleSubModelId" });
            DropIndex("dbo.Vehicles", new[] { "MasterVehicleModelId" });
            DropIndex("dbo.Vehicles", new[] { "MasterVehicleMakerId" });
            DropIndex("dbo.Vehicles", new[] { "MasterVehicleYearId" });
            DropIndex("dbo.ProductStockLogs", new[] { "AdminId" });
            DropIndex("dbo.ProductStockLogs", new[] { "ProductId" });
            DropIndex("dbo.ProductStocks", new[] { "AdminId" });
            DropIndex("dbo.ProductStocks", new[] { "ProductId" });
            DropIndex("dbo.ProductSpecificationLabelDetails", new[] { "AdminId" });
            DropIndex("dbo.ProductSpecificationLabelDetails", new[] { "MasterProductSpecificationLabelId" });
            DropIndex("dbo.ProductSpecificationLabelDetails", new[] { "ProductId" });
            DropIndex("dbo.ProductPriceLogs", new[] { "AdminId" });
            DropIndex("dbo.ProductPriceLogs", new[] { "ProductId" });
            DropIndex("dbo.ProductPrices", new[] { "AdminId" });
            DropIndex("dbo.ProductPrices", new[] { "ProductId" });
            DropIndex("dbo.ProductImages", new[] { "AdminId" });
            DropIndex("dbo.ProductImages", new[] { "ProductId" });
            DropIndex("dbo.Products", new[] { "AdminId" });
            DropIndex("dbo.Products", new[] { "MasterProductBrandId" });
            DropIndex("dbo.Products", new[] { "MasterProductCategoryId" });
            DropIndex("dbo.Products", new[] { "MasterMainCategoryId" });
            DropIndex("dbo.Products", new[] { "MasterSubCategoryId" });
            DropIndex("dbo.MasterVehicleYears", new[] { "AdminId" });
            DropIndex("dbo.MasterVehicleSubModels", new[] { "AdminId" });
            DropIndex("dbo.MasterVehicleModels", new[] { "AdminId" });
            DropIndex("dbo.MasterVehicleMakers", new[] { "AdminId" });
            DropIndex("dbo.MasterVehicleEngines", new[] { "AdminId" });
            DropIndex("dbo.MasterProductSpecificationLabels", new[] { "AdminId" });
            DropIndex("dbo.MasterSubCategories", new[] { "AdminId" });
            DropIndex("dbo.MasterSubCategories", new[] { "MasterMainCategoryId" });
            DropIndex("dbo.MasterProductCategories", new[] { "AdminId" });
            DropIndex("dbo.MasterProductCategories", new[] { "MasterSubCategoryId" });
            DropIndex("dbo.MasterProductCategories", new[] { "MasterMainCategoryId" });
            DropIndex("dbo.MasterProductBrands", new[] { "AdminId" });
            DropIndex("dbo.MasterMainCategories", new[] { "AdminId" });
            DropTable("dbo.VehicleFitments");
            DropTable("dbo.Vehicles");
            DropTable("dbo.ProductStockLogs");
            DropTable("dbo.ProductStocks");
            DropTable("dbo.ProductSpecificationLabelDetails");
            DropTable("dbo.ProductPriceLogs");
            DropTable("dbo.ProductPrices");
            DropTable("dbo.ProductImages");
            DropTable("dbo.Products");
            DropTable("dbo.MasterVehicleYears");
            DropTable("dbo.MasterVehicleSubModels");
            DropTable("dbo.MasterVehicleModels");
            DropTable("dbo.MasterVehicleMakers");
            DropTable("dbo.MasterVehicleEngines");
            DropTable("dbo.MasterProductSpecificationLabels");
            DropTable("dbo.MasterSubCategories");
            DropTable("dbo.MasterProductCategories");
            DropTable("dbo.MasterProductBrands");
            DropTable("dbo.MasterMainCategories");
            DropTable("dbo.Customer_Token");
            DropTable("dbo.Customers");
            DropTable("dbo.Admin_Token");
            DropTable("dbo.Admins");
        }
    }
}
