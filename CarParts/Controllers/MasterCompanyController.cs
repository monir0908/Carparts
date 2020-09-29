using CarParts.Filters;
using CarParts.Models.Models_Sahred;
using CarParts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CarParts.Controllers
{
    [AuthorizationRequired]
    [RoutePrefix("api/MasterCompany")]
    public class MasterCompanyController : ApiController
    {
        private readonly IMasterCompanyServices _services;
        public MasterCompanyController()
        {
            _services = new MasterCompanyServices();
        }
        [Route("CreateMasterCompany")]
        [HttpPost]
        public IHttpActionResult CreateMasterCompany(MasterCompany masterCompany)
        {
            return Ok(_services.CreateMasterCompany(masterCompany).Data);
        }


        [Route("UploadMasterCompanyLogo/{adminId}")]
        [HttpPost]
        public IHttpActionResult UploadMasterCompanyLogo(Guid? adminId)
        {
            return Ok(_services.UploadMasterCompanyLogo(adminId).Data);
        }


        [Route("GetMasterCompanyList")]
        [HttpGet]
        public IHttpActionResult GetMasterCompanyList()
        {
            return Ok(_services.GetMasterCompanyList().Data);
        }


        [Route("UpdateMasterCompany")]
        [HttpPost]
        public IHttpActionResult UpdateMasterCompany(MasterCompany masterCompany)
        {
            return Ok(_services.UpdateMasterCompany(masterCompany).Data);
        }


        [Route("GetMasterCompanyDetails/")]
        [HttpGet]
        public IHttpActionResult GetMasterCompanyDetails()
        {
            return Ok(_services.GetMasterCompanyDetails().Data);
        }


        [Route("HasAnyMasterCompany")]
        [HttpGet]
        public IHttpActionResult HasAnyMasterCompany()
        {
            return Ok(_services.HasAnyMasterCompany().Data);
        }


        [Route("HasAccessToMasterCompany/{adminId}")]
        [HttpGet]
        public IHttpActionResult HasAccessToMasterCompany(Guid? adminId)
        {
            return Ok(_services.HasAccessToMasterCompany(adminId).Data);
        }


        [Route("ToggleMasterSettingsApperance/{masterCompanyid}")]
        [HttpPost]
        public IHttpActionResult ToggleMasterSettingsApperance(Guid? masterCompanyid)
        {
            return Ok(_services.ToggleMasterSettingsApperance(masterCompanyid).Data);
        }
    }
}
