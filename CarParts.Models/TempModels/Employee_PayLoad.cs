using System;

namespace CarParts.Models.TempModels
{
    public class Customer_PayLoad
    {
        public int TokenId { get; set; }
        public string Username { get; set; }
        public string RandomValue { get; set; } = Guid.NewGuid().ToString();
    }
}
