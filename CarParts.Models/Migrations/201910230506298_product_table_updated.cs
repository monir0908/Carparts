namespace CarParts.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class product_table_updated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "ModifiedOn", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "ModifiedOn");
        }
    }
}
