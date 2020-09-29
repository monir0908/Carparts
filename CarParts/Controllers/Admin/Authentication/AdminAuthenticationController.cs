using System.Web.Http;
using CarParts.Filters;
using CarParts.Models.TempModels;
using CarParts.Services.Services_Admin.Authentication;

namespace CarParts.Controllers.Admin.Authentication
{
    [AuthorizationRequired]

    [RoutePrefix("api/Admin")]
    public class AdminAuthenticationController : ApiController
    {
        private readonly IAdminAuthenticationServices _adminAuthenticationServices;
        public AdminAuthenticationController()
        {
            _adminAuthenticationServices = new AdminAuthenticationServices();
        }


        //POST METHODS        
        [Route("ChangePassword")]
        [HttpPost]
        public IHttpActionResult ChangePassword(TemporaryAuthentication obj)
        {
            return Ok(_adminAuthenticationServices.ChangePassword(obj.adminId, obj.extPassword, obj.newPassword).Data);
        }

        [Route("ChangeEmail")]
        [HttpPost]
        public IHttpActionResult ChangeEmail(TemporaryAuthentication obj)
        {
            return Ok(_adminAuthenticationServices.ChangeEmail(obj.adminId, obj.extEmail, obj.newEmail).Data);
        }
    }
}
