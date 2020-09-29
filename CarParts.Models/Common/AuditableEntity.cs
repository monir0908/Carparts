using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarParts.Models.Common
{
    public class AuditableEntity<T> : Entity<T>
    {
        [ScaffoldColumn(false)]
        public DateTime? CreatedDate { get; set; }
        [ScaffoldColumn(false)]
        public int? CreatedBy { get; set; }
        [ScaffoldColumn(false)]
        public DateTime? UpdatedDate { get; set; }
        [ScaffoldColumn(false)]
        public int? UpdatedBy { get; set; }
    }
}
