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
    [RoutePrefix("Api/MasterProductBrand")]
    public class MasterProductBrandController : ApiController
    {
        private readonly IMasterProductBrandServices _services;
        public MasterProductBrandController()
        {
            _services = new MasterProductBrandServices();
        }

        [Route("CreateMasterProductBrand")]
        [HttpPost]
        public IHttpActionResult CreateMasterProductBrand(MasterProductBrand masterProductBrand)
        {
            return Ok(_services.CreateMasterProductBrand(masterProductBrand).Data);
        }

        [Route("UpdateMasterProductBrand")]
        [HttpPost]
        public IHttpActionResult UpdateMasterProductBrand(JObject jObject)
        {
            dynamic jsonData = jObject;
            var JsonMasterProductBrandId = jsonData.MasterProductBrandId;
            var masterProductBrandId = JsonMasterProductBrandId.ToObject<Guid?>();

            var JsonValue = jsonData.Value;
            var value = JsonValue.ToObject<string>();


            return Ok(_services.UpdateMasterProductBrand(masterProductBrandId, value).Data);
        }

        [Route("GetMasterProductBrandList")]
        [HttpGet]
        public IHttpActionResult GetMasterProductBrandList()
        {
            return Ok(_services.GetMasterProductBrandList().Data);
        }

        [Route("UploadMasterProductBrandLogo/{masterProductBrandId}")]
        [HttpPost]
        public IHttpActionResult UploadMasterProductBrandLogo(Guid? masterProductBrandId)
        {
            return Ok(_services.UploadMasterProductBrandLogo(masterProductBrandId).Data);
        }

        [Route("GetMasterProductBrandDetails/{masterProductBrandId}")]
        [HttpGet]
        public IHttpActionResult GetMasterProductBrandDetails(Guid? masterProductBrandId)
        {
            return Ok(_services.GetMasterProductBrandDetails(masterProductBrandId).Data);
        }
    }
}
