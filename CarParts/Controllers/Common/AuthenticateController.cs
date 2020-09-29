using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CarParts.Filters;
using CarParts.Models;
using CarParts.Services.Services_Admin.Authentication;
using CarParts.Services.Services_Customer.Authentication;
using CarParts.Services.Services_Common;
using AttributeRouting.Web.Http;
using CarParts.Models.Models_Admin;
using CarParts.Models.Models_Customer;

namespace CarParts.Controllers.Common
{

    [CarPartsFilter]

    public class AuthenticateController : ApiController
    {
        private readonly ITokenServices _tokenServices;
        private readonly IAdminAuthenticationServices _adminSevices;
        private readonly ICustomerAuthenticationServices _customerSevices;

        public AuthenticateController()
        {
            _tokenServices = new TokenServices();
            _adminSevices = new AdminAuthenticationServices();
            _customerSevices = new CustomerAuthenticationServices();
        }

        [POST("LogIn")]
        public HttpResponseMessage LogIn()
        {
            if (System.Threading.Thread.CurrentPrincipal != null && System.Threading.Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                var basicAuthenticationIdentity = System.Threading.Thread.CurrentPrincipal.Identity as BasicIdentity;
                if (basicAuthenticationIdentity != null)
                {
                    var adminId = basicAuthenticationIdentity.AdminId;
                    var customerId = basicAuthenticationIdentity.CustomerId;

                    return GetAuthToken(adminId, customerId);
                }
            }
            return null;
        }


        private HttpResponseMessage GetAuthToken(Guid adminId, Guid customerId)
        {
            Admin_Token tokenCP = new Admin_Token();
            Customer_Token tokenST = new Customer_Token();

            var response = Request.CreateResponse();
            if (adminId != Guid.Empty)
            {
                tokenCP = _tokenServices.GenerateAdminToken(adminId);
                response = Request.CreateResponse(HttpStatusCode.OK, new
                {
                    Success = true,
                    Token = tokenCP.AuthToken,
                    AdminInfo = _adminSevices.GetAdminDetailsForCookies(adminId).Data,
                });
                return response;
            }
            else if (customerId != Guid.Empty)
            {
                tokenST = _tokenServices.GenerateCustomerToken(customerId);
                response = Request.CreateResponse(HttpStatusCode.OK, new
                {
                    Success = true,
                    Token = tokenST.AuthToken,
                    CustomerInfo = _customerSevices.GetCustomerDetailsForCookies(customerId).Data,
                });
            }
            else
            {
                response = Request.CreateResponse(HttpStatusCode.OK, new
                {
                    Success = false,
                    Token = ""
                });
            }


            return response;

        }


    }
}