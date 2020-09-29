using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CarParts.Common;
using CarParts.Models;
using CarParts.Models.TempModels;
using CarParts.Services.Services_Customer.Profile;
using CarParts.Filters;
using CarParts.Models.Models_Customer;

namespace CarParts.Controllers.Common
{
    [RoutePrefix("api/Register")]
    public class RegistrationController : ApiController
    {
        private readonly ICustomerProfileServices _services;
        public RegistrationController()
        {
            _services = new CustomerProfileServices();
        }

        [Route("RegisterCustomer")]
        [HttpPost]
        public IHttpActionResult RegisterCustomer(Models.Models_Customer.Customer applicant)
        {
            return Ok(_services.RegisterCustomer(applicant).Data);
        }
    }
}
