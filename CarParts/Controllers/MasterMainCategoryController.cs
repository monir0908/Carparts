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
    [RoutePrefix("Api/MasterMainCategory")]
    public class MasterMainCategoryController : ApiController
    {
        private readonly IMasterMainCategoryServices _services;
        public MasterMainCategoryController()
        {
            _services = new MasterMainCategoryServices();
        }

        [Route("CreateMasterMainCategory")]
        [HttpPost]
        public IHttpActionResult CreateMasterMainCategory(MasterMainCategory masterMainCategory)
        {
            return Ok(_services.CreateMasterMainCategory(masterMainCategory).Data);
        }

        [Route("UpdateMasterMainCategory")]
        [HttpPost]
        public IHttpActionResult UpdateMasterMainCategory(JObject jObject)
        {
            dynamic jsonData = jObject;
            var JsonMasterMainCategoryId = jsonData.MasterMainCategoryId;
            var masterMainCategoryId = JsonMasterMainCategoryId.ToObject<Guid?>();

            var JsonValue = jsonData.Value;
            var value = JsonValue.ToObject<string>();


            return Ok(_services.UpdateMasterMainCategory(masterMainCategoryId, value).Data);
        }

        [Route("GetMasterMainCategoryList")]
        [HttpGet]
        public IHttpActionResult GetMasterMainCategoryList()
        {
            return Ok(_services.GetMasterMainCategoryList().Data);
        }

        [Route("UploadMasterMainCategoryLogo/{masterMainCategoryId}")]
        [HttpPost]
        public IHttpActionResult UploadMasterMainCategoryLogo(Guid? masterMainCategoryId)
        {
            return Ok(_services.UploadMasterMainCategoryLogo(masterMainCategoryId).Data);
        }

        [Route("GetMasterMainCategoryDetails/{masterMainCategoryId}")]
        [HttpGet]
        public IHttpActionResult GetMasterMainCategoryDetails(Guid? masterMainCategoryId)
        {
            return Ok(_services.GetMasterMainCategoryDetails(masterMainCategoryId).Data);
        }
    }
}
