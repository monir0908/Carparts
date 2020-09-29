using CarParts.Common;
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
    [RoutePrefix("Api/MasterVehicleYear")]
    public class MasterVehicleYearController : ApiController
    {
        private readonly IMasterVehicleYearServices _services;
        public MasterVehicleYearController()
        {
            _services = new MasterVehicleYearServices();
        }

        [Route("CreateMasterVehicleYear")]
        [HttpPost]
        public IHttpActionResult CreateMasterVehicleYear(MasterVehicleYear masterVehicleYear)
        {
            return Ok(_services.CreateMasterVehicleYear(masterVehicleYear).Data);
        }

        [Route("UpdateMasterVehicleYear")]
        [HttpPost]
        public IHttpActionResult UpdateMasterVehicleYear(JObject jObject)
        {
            try
            {
                dynamic jsonData = jObject;
                var JsonMasterVehicleYearId = jsonData.MasterVehicleYearId;
                var masterVehicleYearId = JsonMasterVehicleYearId.ToObject<Guid?>();

                var JsonValue = jsonData.Value;
                var value = JsonValue.ToObject<int>();


                return Ok(_services.UpdateMasterVehicleYear(masterVehicleYearId, value).Data);
            }
            catch (Exception ex)
            {
                return Ok();
            }
        }

        [Route("GetMasterVehicleYearList")]
        [HttpGet]
        public IHttpActionResult GetMasterVehicleYearList()
        {
            return Ok(_services.GetMasterVehicleYearList().Data);
        }

        [Route("GetMaxYear")]
        [HttpGet]
        public IHttpActionResult GetMaxYear()
        {
            return Ok(_services.GetMaxYear().Data);
        }
    }
}
