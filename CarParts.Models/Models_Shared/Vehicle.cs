using CarParts.Models.Common;
using CarParts.Models.Models_Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarParts.Models.Models_Shared
{
    public class Vehicle : Entity<Guid>
    {
        public MasterVehicleYear MasterVehicleYear { get; set; }
        public Guid? MasterVehicleYearId { get; set; }
        public MasterVehicleMaker MasterVehicleMaker { get; set; }
        public Guid? MasterVehicleMakerId { get; set; }
        public MasterVehicleModel MasterVehicleModel { get; set; }
        public Guid? MasterVehicleModelId { get; set; }
        public MasterVehicleSubModel MasterVehicleSubModel { get; set; }
        public Guid? MasterVehicleSubModelId { get; set; }
        public MasterVehicleEngine MasterVehicleEngine { get; set; }
        public Guid? MasterVehicleEngineId { get; set; }
        public Admin Admin { get; set; }
        public Guid? AdminId { get; set; }
        public DateTime AddedOn { get; set; }
    }
}
