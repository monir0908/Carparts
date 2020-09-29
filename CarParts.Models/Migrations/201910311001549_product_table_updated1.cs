namespace CarParts.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class product_table_updated1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "DiscountPercentage", c => c.Double(nullable: false));
            AddColumn("dbo.Products", "IsDiscountActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "IsDiscountActive");
            DropColumn("dbo.Products", "DiscountPercentage");
        }
    }
}
