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
    [RoutePrefix("Api/MasterProductSpecificationLabel")]
    public class MasterProductSpecificationLabelController : ApiController
    {
        private readonly IMasterProductSpecificationLabelServices _services;
        public MasterProductSpecificationLabelController()
        {
            _services = new MasterProductSpecificationLabelServices();
        }

        [Route("CreateMasterProductSpecificationLabel")]
        [HttpPost]
        public IHttpActionResult CreateMasterProductSpecificationLabel(MasterProductSpecificationLabel masterProductSpecificationLabel)
        {
            return Ok(_services.CreateMasterProductSpecificationLabel(masterProductSpecificationLabel).Data);
        }

        [Route("UpdateMasterProductSpecificationLabel")]
        [HttpPost]
        public IHttpActionResult UpdateMasterProductSpecificationLabel(JObject jObject)
        {
            dynamic jsonData = jObject;
            var JsonMasterProductSpecificationLabelId = jsonData.MasterProductSpecificationLabelId;
            var masterProductSpecificationLabelId = JsonMasterProductSpecificationLabelId.ToObject<Guid?>();

            var JsonValue = jsonData.Value;
            var value = JsonValue.ToObject<string>();


            return Ok(_services.UpdateMasterProductSpecificationLabel(masterProductSpecificationLabelId, value).Data);
        }

        [Route("GetMasterProductSpecificationLabelList")]
        [HttpGet]
        public IHttpActionResult GetMasterProductSpecificationLabelList()
        {
            return Ok(_services.GetMasterProductSpecificationLabelList().Data);
        }

        [Route("ToggleCategory/{masterProductSpecificationLabelId}")]
        [HttpPost]
        public IHttpActionResult ToggleCategory(Guid? masterProductSpecificationLabelId)
        {
            return Ok(_services.ToggleCategory(masterProductSpecificationLabelId).Data);
        }
    }
}
