using CarParts.Models.Common;
using CarParts.Models.Models_Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarParts.Models.Models_Sahred
{
    public class Company:Entity<Guid>
    {
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string Phone_1 { get; set; }
        public string Phone_2 { get; set; }
        public string Mobile { get; set; }
        public string CompanyLogo { get; set; }
        public string CompanySignature { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPersonPhone { get; set; }
        public string ContactPersonDesignation { get; set; }
        public string ContactPersonEmail { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyWebsite { get; set; }
        public DateTime AddedOn { get; set; }
        public Admin Admin { get; set; }
        public Guid? AdminId { get; set; }
    }
}
