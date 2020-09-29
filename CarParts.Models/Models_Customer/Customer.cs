using CarParts.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarParts.Models.Models_Customer
{
    public class Customer : Entity<Guid>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ProfilePicture { get; set; }
        public Nullable<DateTime> CreatedOn { get; set; }
        public string UserCode { get; set; }
    }
}
