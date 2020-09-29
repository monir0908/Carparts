using CarParts.Models.Common;
using CarParts.Models.Models_Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarParts.Models.Models_Sahred
{
    public class MasterCompany:Entity<Guid>
    {
        public string MasterCompanyName { get; set; }
        public string MasterCompanyLogo { get; set; }
        public string ShowOnReport { get; set; }
        public DateTime AddedOn { get; set; }
        public Admin Admin { get; set; }
        public Guid? AdminId { get; set; }
    }
}
