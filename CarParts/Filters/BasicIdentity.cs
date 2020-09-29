using System;
using System.Security.Principal;

namespace CarParts.Filters
{
    public class BasicIdentity : GenericIdentity
    {
        
        public string Password { get; set; }
        public string Customername { get; set; }
        public Guid AdminId { get; set; }
        public Guid CustomerId { get; set; }

        
        public BasicIdentity(string customername, string password): base(customername, "Basic")
        {
            Password = password;
            Customername = customername;
        }
    }
}