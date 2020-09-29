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
    [RoutePrefix("Api/MasterVehicleSubModel")]
    public class MasterVehicleSubModelController : ApiController
    {
        private readonly IMasterVehicleSubModelServices _services;
        public MasterVehicleSubModelController()
        {
            _services = new MasterVehicleSubModelServices();
        }

        [Route("CreateMasterVehicleSubModel")]
        [HttpPost]
        public IHttpActionResult CreateMasterVehicleSubModel(MasterVehicleSubModel masterVehicleSubModel)
        {
            return Ok(_services.CreateMasterVehicleSubModel(masterVehicleSubModel).Data);
        }

        [Route("UpdateMasterVehicleSubModel")]
        [HttpPost]
        public IHttpActionResult UpdateMasterVehicleSubModel(JObject jObject)
        {
            dynamic jsonData = jObject;
            var JsonMasterVehicleSubModelId = jsonData.MasterVehicleSubModelId;
            var masterVehicleSubModelId = JsonMasterVehicleSubModelId.ToObject<Guid?>();

            var JsonValue = jsonData.Value;
            var value = JsonValue.ToObject<string>();


            return Ok(_services.UpdateMasterVehicleSubModel(masterVehicleSubModelId, value).Data);
        }

        [Route("GetMasterVehicleSubModelFilteredList/{yearId}/{makerId}/{modelId}")]
        [HttpGet]
        public IHttpActionResult GetMasterVehicleSubModelFilteredList(Guid? yearId, Guid? makerId, Guid? modelId)
        {
            return Ok(_services.GetMasterVehicleSubModelFilteredList(yearId, makerId, modelId).Data);
        }

        [Route("GetMasterVehicleSubModelList")]
        [HttpGet]
        public IHttpActionResult GetMasterVehicleSubModelList()
        {
            return Ok(_services.GetMasterVehicleSubModelList().Data);
        }
    }
}
