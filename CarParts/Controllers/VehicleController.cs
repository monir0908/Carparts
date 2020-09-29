using CarParts.Models.Models_Shared;
using CarParts.Services.Services_Shared;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CarParts.Controllers
{
    [RoutePrefix("Api/Vehicle")]
    public class VehicleController : ApiController
    {
        private readonly IVehicleServices _services;
        public VehicleController()
        {
            _services = new VehicleServices();
        }

        [Route("CreateVehicle")]
        [HttpPost]
        public IHttpActionResult CreateVehicle(Vehicle vehicle)
        {
            return Ok(_services.CreateVehicle(vehicle).Data);
        }

        [Route("DeletevehicleByVehicleId/{vehicleId}")]
        [HttpPost]
        public IHttpActionResult DeletevehicleByVehicleId(Guid? vehicleId)
        {
            return Ok(_services.DeletevehicleByVehicleId(vehicleId).Data);
        }

        [Route("GetVehicleList")]
        [HttpGet]
        public IHttpActionResult GetVehicleList()
        {
            return Ok(_services.GetVehicleList().Data);
        }

        [Route("GetMakerListByYearId/{yearId}")]
        [HttpGet]
        public IHttpActionResult GetMakerListByYearId(Guid? yearId)
        {
            return Ok(_services.GetMakerListByYearId(yearId).Data);
        }

        [Route("GetModelListByMakerId/{makerId}")]
        [HttpGet]
        public IHttpActionResult GetModelListByMakerId(Guid? makerId)
        {
            return Ok(_services.GetModelListByMakerId(makerId).Data);
        }

        [Route("GetSubModelListByModelId/{modelId}")]
        [HttpGet]
        public IHttpActionResult GetSubModelListByModelId(Guid? modelId)
        {
            return Ok(_services.GetSubModelListByModelId(modelId).Data);
        }

        [Route("GetEngineListBySubModelId/{subModelId}")]
        [HttpGet]
        public IHttpActionResult GetEngineListBySubModelId(Guid? subModelId)
        {
            return Ok(_services.GetEngineListBySubModelId(subModelId).Data);
        }

        [Route("SearchByVehicleWithOutSubModelAndEngine/{yearId}/{makerId}/{modelId}")]
        [HttpGet]
        public IHttpActionResult SearchByVehicleWithOutSubModelAndEngine(Guid? yearId, Guid? makerId, Guid? modelId)
        {
            return Ok(_services.SearchByVehicleWithOutSubModelAndEngine(yearId, makerId, modelId).Data);
        }

        [Route("SearchByVehicleWithSubModelAndEngine/{yearId}/{makerId}/{modelId}/{subModelId}/{engineId}")]
        [HttpGet]
        public IHttpActionResult SearchByVehicleWithSubModelAndEngine(Guid? yearId, Guid? makerId, Guid? modelId, Guid? subModelId, Guid? engineId)
        {
            return Ok(_services.SearchByVehicleWithSubModelAndEngine(yearId, makerId, modelId, subModelId, engineId).Data);
        }
    }
}
