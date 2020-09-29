using CarParts.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarParts.Models.Models_Sahred
{
    public class MailSettings:Entity<Guid>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public bool Ssl { get; set; }
        public string HostAddress { get; set; }
        public string DisplayName { get; set; }
    }
}
