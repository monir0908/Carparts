using CarParts.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarParts.Models.Models_Admin
{
    public class Admin_Token : Entity<Guid>
    {
        public Nullable<Guid> AdminID { get; set; }
        public string AuthToken { get; set; }
        public Nullable<DateTime> IssuedOn { get; set; }
        public Nullable<DateTime> ExpiresOn { get; set; }
    }
}
