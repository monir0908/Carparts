namespace CarParts.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class is_categorical_column_added : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MasterProductSpecificationLabels", "IsCategorical", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MasterProductSpecificationLabels", "IsCategorical");
        }
    }
}
