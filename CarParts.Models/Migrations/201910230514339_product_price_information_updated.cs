namespace CarParts.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class product_price_information_updated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductPrices", "CurrentUnitPrice", c => c.Double(nullable: false));
            AddColumn("dbo.ProductPrices", "CurrentSalesPrice", c => c.Double(nullable: false));
            AddColumn("dbo.ProductPriceLogs", "UnitValue", c => c.Double(nullable: false));
            AddColumn("dbo.ProductPriceLogs", "SalesValue", c => c.Double(nullable: false));
            AddColumn("dbo.ProductPriceLogs", "SalesPriceUpdateStatus", c => c.String());
            AddColumn("dbo.ProductPriceLogs", "UnitPriceUpdateStatus", c => c.String());
            DropColumn("dbo.ProductPrices", "CurrentPrice");
            DropColumn("dbo.ProductPriceLogs", "Value");
            DropColumn("dbo.ProductPriceLogs", "Status");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProductPriceLogs", "Status", c => c.String());
            AddColumn("dbo.ProductPriceLogs", "Value", c => c.Double(nullable: false));
            AddColumn("dbo.ProductPrices", "CurrentPrice", c => c.Double(nullable: false));
            DropColumn("dbo.ProductPriceLogs", "UnitPriceUpdateStatus");
            DropColumn("dbo.ProductPriceLogs", "SalesPriceUpdateStatus");
            DropColumn("dbo.ProductPriceLogs", "SalesValue");
            DropColumn("dbo.ProductPriceLogs", "UnitValue");
            DropColumn("dbo.ProductPrices", "CurrentSalesPrice");
            DropColumn("dbo.ProductPrices", "CurrentUnitPrice");
        }
    }
}
