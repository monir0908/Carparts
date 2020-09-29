namespace CarParts.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DB_naming_convention_fixed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MasterProductCategories", "MasterProductCategoryName", c => c.String());
            DropColumn("dbo.MasterProductCategories", "ProductCategoryName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MasterProductCategories", "ProductCategoryName", c => c.String());
            DropColumn("dbo.MasterProductCategories", "MasterProductCategoryName");
        }
    }
}
