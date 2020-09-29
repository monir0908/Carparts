using System;
using System.IO;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using CarParts.Services.Services_Admin.Authentication;
using CarParts.Services.Services_Customer.Authentication;
using Newtonsoft.Json;

namespace CarParts.Filters
{

    public class CarPartsFilter : GenericAuthenticationFilter
    {
        private readonly IAdminAuthenticationServices _adminAuthenticationServices;
        private readonly ICustomerAuthenticationServices _customerAuthenticationServices;
        public CarPartsFilter()
        {
            _adminAuthenticationServices = new AdminAuthenticationServices();
            _customerAuthenticationServices = new CustomerAuthenticationServices();
        }

        public CarPartsFilter(bool isActive) : base(isActive)
        {
        }

        public class LoginType
        {
            public string Type { get; set; }
        }

        protected override bool OnAuthorizeUser(string customername, string password, HttpActionContext actionContext)
        {
            //Finding whether the request is from Admin or Applicant
            var bodyStream = new StreamReader(HttpContext.Current.Request.InputStream);
            bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
            var bodyText = bodyStream.ReadToEnd();

            var obj = JsonConvert.DeserializeObject<LoginType>(bodyText);

            var loginType = obj.Type;


            if (loginType == "Admin")
            {
                Guid adminId = _adminAuthenticationServices.Admin_Authenticate(customername, password);
                if (adminId != Guid.Empty)
                {
                    var basicCarPartsIdentity = Thread.CurrentPrincipal.Identity as BasicIdentity;
                    if (basicCarPartsIdentity != null)
                        basicCarPartsIdentity.AdminId = adminId;
                    return true;
                }
            }
            else if (loginType == "Customer")
            {
                Guid customerId = _customerAuthenticationServices.Customer_Authenticate(customername, password);
                if (customerId != Guid.Empty)
                {
                    var basicAuthenticationIdentity = Thread.CurrentPrincipal.Identity as BasicIdentity;
                    if (basicAuthenticationIdentity != null)
                        basicAuthenticationIdentity.CustomerId = customerId;
                    return true;
                }
            }
            else
            {
                return false;
            }

            return false;
        }
    }
}