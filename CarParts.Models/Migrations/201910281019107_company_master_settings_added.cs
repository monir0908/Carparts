namespace CarParts.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class company_master_settings_added : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CompanyName = c.String(),
                        Address = c.String(),
                        Phone_1 = c.String(),
                        Phone_2 = c.String(),
                        Mobile = c.String(),
                        CompanyLogo = c.String(),
                        CompanySignature = c.String(),
                        ContactPersonName = c.String(),
                        ContactPersonPhone = c.String(),
                        ContactPersonDesignation = c.String(),
                        ContactPersonEmail = c.String(),
                        CompanyEmail = c.String(),
                        CompanyWebsite = c.String(),
                        AddedOn = c.DateTime(nullable: false),
                        AdminId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Admins", t => t.AdminId)
                .Index(t => t.AdminId);
            
            CreateTable(
                "dbo.MasterCompanies",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        MasterCompanyName = c.String(),
                        MasterCompanyLogo = c.String(),
                        ShowOnReport = c.String(),
                        AddedOn = c.DateTime(nullable: false),
                        AdminId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Admins", t => t.AdminId)
                .Index(t => t.AdminId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MasterCompanies", "AdminId", "dbo.Admins");
            DropForeignKey("dbo.Companies", "AdminId", "dbo.Admins");
            DropIndex("dbo.MasterCompanies", new[] { "AdminId" });
            DropIndex("dbo.Companies", new[] { "AdminId" });
            DropTable("dbo.MasterCompanies");
            DropTable("dbo.Companies");
        }
    }
}
