using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CarParts.Models.TempModels;
using CarParts.Services.Services_Admin.Authentication;
using CarParts.Services.Services_Admin.Profile;
using CarParts.Filters;

namespace CarParts.Controllers.Admin.Profile
{    
    [RoutePrefix("api/AdminProfile")]
    public class AdminProfileController : ApiController
    {
        private readonly IAdminProfileServices _adminProfileServices;
        private readonly IAdminAuthenticationServices _adminAuthenticationServices;
        public AdminProfileController()
        {
            _adminProfileServices = new AdminProfileServices();
            _adminAuthenticationServices = new AdminAuthenticationServices();
        }

        //POST METHODS        
        [Route("RegisterAdmin")]
        [HttpPost]
        public IHttpActionResult RegisterAdmin(Models.Models_Admin.Admin admin)
        {
            return Ok(_adminProfileServices.RegisterAdmin(admin).Data);
        }

        //GET METHODS        
        [AuthorizationRequired]
        [Route("GetAdminProfileDetailsByAdminId/{adminId}")]
        [HttpGet]
        public IHttpActionResult GetAdminProfileDetailsByAdminId(Guid adminId)
        {
            return Ok(_adminProfileServices.GetAdminProfileDetailsByAdminId(adminId).Data);
        }

        [AuthorizationRequired]
        [Route("GetAdminDetailsForCookies/{adminId}")]
        [HttpGet]
        public IHttpActionResult GetAdminDetailsForCookies(Guid adminId)
        {
            return Ok(_adminAuthenticationServices.GetAdminDetailsForCookies(adminId).Data);
        }
    }
}
