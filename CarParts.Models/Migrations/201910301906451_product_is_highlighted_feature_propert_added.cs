namespace CarParts.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class product_is_highlighted_feature_propert_added : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductSpecificationLabelDetails", "IsHighlightedFeature", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductSpecificationLabelDetails", "IsHighlightedFeature");
        }
    }
}
