using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarParts.Models.TempModels
{
    public class TempVehicleFilter
    {
        public Guid? Id { get; set; }
        public bool Model { get; set; }
        public string Year { get; set; }
        public string MakerName { get; set; }
        public string ModelName { get; set; }
        public string SubModelName { get; set; }
        public string EngineName { get; set; }
        public List<Guid?> VehicleIdList { get; set; }
    }
}
