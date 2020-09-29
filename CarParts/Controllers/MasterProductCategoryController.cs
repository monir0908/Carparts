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
    [RoutePrefix("Api/MasterProductCategory")]
    public class MasterProductCategoryController : ApiController
    {
        private readonly IMasterProductCategoryServices _services;
        public MasterProductCategoryController()
        {
            _services = new MasterProductCategoryServices();
        }

        [Route("CreateMasterProductCategory")]
        [HttpPost]
        public IHttpActionResult CreateMasterProductCategory(MasterProductCategory masterProductCategory)
        {
            return Ok(_services.CreateMasterProductCategory(masterProductCategory).Data);
        }

        [Route("UpdateMasterProductCategory")]
        [HttpPost]
        public IHttpActionResult UpdateMasterProductCategory(JObject jObject)
        {
            dynamic jsonData = jObject;
            var JsonMasterProductCategoryId = jsonData.MasterProductCategoryId;
            var masterProductCategoryId = JsonMasterProductCategoryId.ToObject<Guid?>();

            var JsonValue = jsonData.Value;
            var value = JsonValue.ToObject<string>();


            return Ok(_services.UpdateMasterProductCategory(masterProductCategoryId, value).Data);
        }

        [Route("GetMasterProductCategoryList")]
        [HttpGet]
        public IHttpActionResult GetMasterProductCategoryList()
        {
            return Ok(_services.GetMasterProductCategoryList().Data);
        }

        [Route("UploadMasterProductCategoryLogo/{masterProductCategoryId}")]
        [HttpPost]
        public IHttpActionResult UploadMasterProductCategoryLogo(Guid? masterProductCategoryId)
        {
            return Ok(_services.UploadMasterProductCategoryLogo(masterProductCategoryId).Data);
        }

        [Route("GetMasterProductCategoryDetails/{masterProductCategoryId}")]
        [HttpGet]
        public IHttpActionResult GetMasterProductCategoryDetails(Guid? masterProductCategoryId)
        {
            return Ok(_services.GetMasterProductCategoryDetails(masterProductCategoryId).Data);
        }
    }
}
