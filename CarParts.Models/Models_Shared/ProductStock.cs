using CarParts.Models.Common;
using CarParts.Models.Models_Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarParts.Models.Models_Shared
{
    public class ProductStock:Entity<Guid>
    {
        public Product Product { get; set; }
        public Guid? ProductId { get; set; }
        public double CurrentStock { get; set; }
        public Admin Admin { get; set; }
        public Guid? AdminId { get; set; }
        public DateTime AddedOn { get; set; }
    }
}
