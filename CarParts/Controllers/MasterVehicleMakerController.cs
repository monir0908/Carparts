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
    [RoutePrefix("Api/MasterVehicleMaker")]
    public class MasterVehicleMakerController : ApiController
    {
        private readonly IMasterVehicleMakerServices _services;
        public MasterVehicleMakerController()
        {
            _services = new MasterVehicleMakerServices();
        }

        [Route("CreateMasterVehicleMaker")]
        [HttpPost]
        public IHttpActionResult CreateMasterVehicleMaker(MasterVehicleMaker masterVehicleMaker)
        {
            return Ok(_services.CreateMasterVehicleMaker(masterVehicleMaker).Data);
        }

        [Route("UpdateMasterVehicleMaker")]
        [HttpPost]
        public IHttpActionResult UpdateMasterVehicleMaker(JObject jObject)
        {
            dynamic jsonData = jObject;
            var JsonMasterVehicleMakerId = jsonData.MasterVehicleMakerId;
            var masterVehicleMakerId = JsonMasterVehicleMakerId.ToObject<Guid?>();

            var JsonValue = jsonData.Value;
            var value = JsonValue.ToObject<string>();


            return Ok(_services.UpdateMasterVehicleMaker(masterVehicleMakerId, value).Data);
        }

        [Route("GetMasterVehicleMakerList")]
        [HttpGet]
        public IHttpActionResult GetMasterVehicleMakerList()
        {
            return Ok(_services.GetMasterVehicleMakerList().Data);
        }
    }
}
