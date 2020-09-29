using System.Web.Http;
using CarParts.Filters;
using CarParts.Models.TempModels;
using CarParts.Services.Services_Customer.Authentication;

namespace CarParts.Controllers.Customer.CarParts
{
    [AuthorizationRequired]
    [RoutePrefix("api/Customer")]
    public class CustomerAuthenticateController : ApiController
    {
        private readonly ICustomerAuthenticationServices _userAuthenticationServices;
        public CustomerAuthenticateController()
        {
            _userAuthenticationServices = new CustomerAuthenticationServices();
        }



        //POST METHODS        
        [Route("ChangePassword")]
        [HttpPost]
        public IHttpActionResult ChangePassword(TemporaryAuthentication obj)
        {
            return Ok(_userAuthenticationServices.ChangePassword(obj.customerId, obj.extPassword, obj.newPassword).Data);
        }

        [Route("ChangeEmail")]
        [HttpPost]
        public IHttpActionResult ChangeEmail(TemporaryAuthentication obj)
        {
            return Ok(_userAuthenticationServices.ChangeEmail(obj.customerId, obj.extEmail, obj.newEmail).Data);
        }

    }
}
