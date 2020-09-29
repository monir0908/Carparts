using CarParts.Models.Common;
using CarParts.Models.Models_Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarParts.Models.Models_Shared
{
    public class MasterProductCategory:Entity<Guid>
    {
        public MasterMainCategory MasterMainCategory { get; set; }
        public Guid? MasterMainCategoryId { get; set; }
        public MasterSubCategory MasterSubCategory { get; set; }
        public Guid? MasterSubCategoryId { get; set; }
        /// <summary>
        /// Also known as Part Name
        /// </summary>
        public string MasterProductCategoryName { get; set; }
        public Admin Admin { get; set; }
        public Guid? AdminId { get; set; }
        public DateTime AddedOn { get; set; }
        public string LogoFileName { get; set; }
    }
}
