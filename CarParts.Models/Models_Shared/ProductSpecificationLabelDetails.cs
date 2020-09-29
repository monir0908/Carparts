using CarParts.Models.Common;
using CarParts.Models.Models_Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarParts.Models.Models_Shared
{
    public class ProductSpecificationLabelDetails : Entity<int>
    {
        public Product Product { get; set; }
        public Guid? ProductId { get; set; }
        public MasterProductSpecificationLabel MasterProductSpecificationLabel { get; set; }
        public Guid? MasterProductSpecificationLabelId { get; set; }
        public string Value { get; set; }
        public bool IsHighlightedFeature { get; set; }
        public Admin Admin { get; set; }
        public Guid? AdminId { get; set; }
        public DateTime AddedOn { get; set; }
    }
}
