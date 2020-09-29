using CarParts.Models.Models_Admin;
using CarParts.Models.Models_Customer;
using CarParts.Models.Models_Sahred;
using CarParts.Models.Models_Shared;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarParts.Models
{
    public class CarPartsDbContext : DbContext
    {
        public CarPartsDbContext() : base("Cn")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<CarPartsDbContext, Migrations.Configuration>("Cn"));
        }

        #region Customer DbSet
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Customer_Token> Customer_Token { get; set; }
        #endregion

        #region Admin DbSet
        public DbSet<Admin> Admin { get; set; }
        public DbSet<Admin_Token> Admin_Token { get; set; }
        #endregion

        #region Shared DbSet
        public DbSet<MasterMainCategory> MasterMainCategory { get; set; }
        public DbSet<MasterProductBrand> MasterProductBrand { get; set; }
        public DbSet<MasterProductCategory> MasterProductCategory { get; set; }
        public DbSet<MasterProductSpecificationLabel> MasterProductSpecificationLabel { get; set; }
        public DbSet<MasterSubCategory> MasterSubCategory { get; set; }
        public DbSet<MasterVehicleEngine> MasterVehicleEngine { get; set; }
        public DbSet<MasterVehicleMaker> MasterVehicleMaker { get; set; }
        public DbSet<MasterVehicleModel> MasterVehicleModel { get; set; }
        public DbSet<MasterVehicleSubModel> MasterVehicleSubModel { get; set; }
        public DbSet<MasterVehicleYear> MasterVehicleYear { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductImage> ProductImage { get; set; }
        public DbSet<ProductPrice> ProductPrice { get; set; }
        public DbSet<ProductPriceLog> ProductPriceLog { get; set; }
        public DbSet<ProductSpecificationLabelDetails> ProductSpecificationLabelDetails { get; set; }
        public DbSet<ProductStock> ProductStock { get; set; }
        public DbSet<ProductStockLog> ProductStockLog { get; set; }
        public DbSet<Vehicle> Vehicle { get; set; }
        public DbSet<VehicleFitment> VehicleFitment { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<MasterCompany> MasterCompany { get; set; }
        #endregion
    }
}
