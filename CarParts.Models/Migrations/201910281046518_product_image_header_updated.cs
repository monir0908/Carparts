namespace CarParts.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class product_image_header_updated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductImages", "IsHeader", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductImages", "IsHeader");
        }
    }
}
