using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarParts.Models.TempModels
{
    public class TempVehicleFitment
    {
        public Guid? VehicleId { get; set; }
        public string VehicleName { get; set; }
        public string SubModel { get; set; }
        public string Engine { get; set; }
        public string FitmentInfo { get; set; }
        public string LabelName { get; set; }
    }
}
