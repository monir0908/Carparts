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
    [RoutePrefix("Api/MasterVehicleEngine")]
    public class MasterVehicleEngineController : ApiController
    {
        private readonly IMasterVehicleEngineServices _services;
        public MasterVehicleEngineController()
        {
            _services = new MasterVehicleEngineServices();
        }

        [Route("CreateMasterVehicleEngine")]
        [HttpPost]
        public IHttpActionResult CreateMasterVehicleEngine(MasterVehicleEngine masterVehicleEngine)
        {
            return Ok(_services.CreateMasterVehicleEngine(masterVehicleEngine).Data);
        }

        [Route("UpdateMasterVehicleEngine")]
        [HttpPost]
        public IHttpActionResult UpdateMasterVehicleEngine(JObject jObject)
        {
            dynamic jsonData = jObject;
            var JsonMasterVehicleEngineId = jsonData.MasterVehicleEngineId;
            var masterVehicleEngineId = JsonMasterVehicleEngineId.ToObject<Guid?>();

            var JsonValue = jsonData.Value;
            var value = JsonValue.ToObject<string>();


            return Ok(_services.UpdateMasterVehicleEngine(masterVehicleEngineId, value).Data);
        }

        [Route("GetMasterVehicleEngineFilteredList/{yearId}/{makerId}/{modelId}/{engineId}")]
        [HttpGet]
        public IHttpActionResult GetMasterVehicleEngineFilteredList(Guid? yearId, Guid? makerId, Guid? modelId, Guid? engineId)
        {
            return Ok(_services.GetMasterVehicleEngineFilteredList(yearId, makerId, modelId, engineId).Data);
        }

        [Route("GetMasterVehicleEngineList")]
        [HttpGet]
        public IHttpActionResult GetMasterVehicleEngineList()
        {
            return Ok(_services.GetMasterVehicleEngineList().Data);
        }
    }
}
