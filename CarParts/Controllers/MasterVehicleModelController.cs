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
    [RoutePrefix("Api/MasterVehicleModel")]
    public class MasterVehicleModelController : ApiController
    {
        private readonly IMasterVehicleModelServices _services;
        public MasterVehicleModelController()
        {
            _services = new MasterVehicleModelServices();
        }

        [Route("CreateMasterVehicleModel")]
        [HttpPost]
        public IHttpActionResult CreateMasterVehicleModel(MasterVehicleModel masterVehicleModel)
        {
            return Ok(_services.CreateMasterVehicleModel(masterVehicleModel).Data);
        }

        [Route("UpdateMasterVehicleModel")]
        [HttpPost]
        public IHttpActionResult UpdateMasterVehicleModel(JObject jObject)
        {
            dynamic jsonData = jObject;
            var JsonMasterVehicleModelId = jsonData.MasterVehicleModelId;
            var masterVehicleModelId = JsonMasterVehicleModelId.ToObject<Guid?>();

            var JsonValue = jsonData.Value;
            var value = JsonValue.ToObject<string>();


            return Ok(_services.UpdateMasterVehicleModel(masterVehicleModelId, value).Data);
        }

        [Route("GetMasterVehicleModelList")]
        [HttpGet]
        public IHttpActionResult GetMasterVehicleModelList()
        {
            return Ok(_services.GetMasterVehicleModelList().Data);
        }
    }
}
