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
    [RoutePrefix("api/Company")]
    public class CompanyController : ApiController
    {
        private readonly ICompanyServices _services;
        public CompanyController()
        {
            _services = new CompanyServices();
        }
        [AuthorizationRequired]
        [Route("CreateCompany")]
        [HttpPost]
        public IHttpActionResult CreateCompany(Company company)
        {
            return Ok(_services.CreateCompany(company).Data);
        }


        [AuthorizationRequired]
        [Route("UploadCompanyLogo/{adminId}")]
        [HttpPost]
        public IHttpActionResult UploadCompanyLogo(Guid? adminId)
        {
            return Ok(_services.UploadCompanyLogo(adminId).Data);
        }

        [AuthorizationRequired]
        [Route("UploadCompanySignature/{companyId}")]
        [HttpPost]
        public IHttpActionResult UploadCompanySignature(Guid? companyId)
        {
            return Ok(_services.UploadCompanySignature(companyId).Data);
        }


        [Route("GetCompanyList")]
        [HttpGet]
        public IHttpActionResult GetCompanyList()
        {
            return Ok(_services.GetCompanyList().Data);
        }


        [AuthorizationRequired]
        [Route("UpdateCompany")]
        [HttpPost]
        public IHttpActionResult UpdateCompany(Company company)
        {
            return Ok(_services.UpdateCompany(company).Data);
        }


        [Route("GetCompanyDetails")]
        [HttpGet]
        public IHttpActionResult GetCompanyDetails()
        {
            return Ok(_services.GetCompanyDetails().Data);
        }


        [Route("GetCompanyDetailsForWebSite")]
        [HttpGet]
        public IHttpActionResult GetCompanyDetailsForWebSite()
        {
            return Ok(_services.GetCompanyDetailsForWebSite().Data);
        }
    }
}
