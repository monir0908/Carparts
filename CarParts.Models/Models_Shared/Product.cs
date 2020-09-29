using CarParts.Models.Common;
using CarParts.Models.Models_Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarParts.Models.Models_Shared
{
    public class Product : Entity<Guid>
    {
        public MasterSubCategory MasterSubCategory { get; set; }
        public Guid? MasterSubCategoryId { get; set; }
        public MasterMainCategory MasterMainCategory { get; set; }
        public Guid? MasterMainCategoryId { get; set; }
        public MasterProductCategory MasterProductCategory { get; set; }
        public Guid? MasterProductCategoryId { get; set; }
        public MasterProductBrand MasterProductBrand { get; set; }
        public Guid? MasterProductBrandId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProductSKU { get; set; }
        public double DiscountPercentage { get; set; }
        public bool IsDiscountActive { get; set; }
        public Admin Admin { get; set; }
        public Guid? AdminId { get; set; }
        public DateTime AddedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
