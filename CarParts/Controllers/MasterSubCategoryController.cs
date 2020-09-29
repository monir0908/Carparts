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
    [RoutePrefix("Api/MasterSubCategory")]
    public class MasterSubCategoryController : ApiController
    {
        private readonly IMasterSubCategoryServices _services;
        public MasterSubCategoryController()
        {
            _services = new MasterSubCategoryServices();
        }

        [Route("CreateMasterSubCategory")]
        [HttpPost]
        public IHttpActionResult CreateMasterSubCategory(MasterSubCategory masterSubCategory)
        {
            return Ok(_services.CreateMasterSubCategory(masterSubCategory).Data);
        }

        [Route("UpdateMasterSubCategory")]
        [HttpPost]
        public IHttpActionResult UpdateMasterSubCategory(JObject jObject)
        {
            dynamic jsonData = jObject;
            var JsonMasterSubCategoryId = jsonData.MasterSubCategoryId;
            var masterSubCategoryId = JsonMasterSubCategoryId.ToObject<Guid?>();

            var JsonValue = jsonData.Value;
            var value = JsonValue.ToObject<string>();


            return Ok(_services.UpdateMasterSubCategory(masterSubCategoryId, value).Data);
        }

        [Route("GetMasterSubCategoryList")]
        [HttpGet]
        public IHttpActionResult GetMasterSubCategoryList()
        {
            return Ok(_services.GetMasterSubCategoryList().Data);
        }

        [Route("UploadMasterSubCategoryLogo/{masterSubCategoryId}")]
        [HttpPost]
        public IHttpActionResult UploadMasterSubCategoryLogo(Guid? masterSubCategoryId)
        {
            return Ok(_services.UploadMasterSubCategoryLogo(masterSubCategoryId).Data);
        }

        [Route("GetMasterSubCategoryDetails/{masterSubCategoryId}")]
        [HttpGet]
        public IHttpActionResult GetMasterSubCategoryDetails(Guid? masterSubCategoryId)
        {
            return Ok(_services.GetMasterSubCategoryDetails(masterSubCategoryId).Data);
        }
    }
}
