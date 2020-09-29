using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CarParts.Common;
using CarParts.Filters;
using CarParts.Models;
using CarParts.Models.Models_Customer;
using CarParts.Models.TempModels;
using CarParts.Services.Services_Customer.Profile;

namespace CarParts.Controllers.Customer.Profile
{
    [AuthorizationRequired]
    [RoutePrefix("api/CustomerProfile")]
    public class CustomerProfileController : ApiController
    {
        private readonly ICustomerProfileServices _services;
        public CustomerProfileController()
        {
            _services = new CustomerProfileServices();
        }
        
        [Route("GetCustomerDetailsByCustomerId/{customerId}")]
        [HttpGet]
        public IHttpActionResult GetCustomerDetailsByCustomerId(Guid customerId)
        {
            return Ok(_services.GetCustomerDetailsByCustomerId(customerId).Data);
        }

        [Route("UploadProfilePictureByCustomerId/{customerId}")]
        [HttpPost]        
        public IHttpActionResult UploadProfilePictureByCustomerId(Guid customerId)
        {
            return Ok(_services.UploadProfilePictureByCustomerId(customerId).Data);
        }

        [Route("UpdateCustomerProfile")]
        [HttpPost]
        public IHttpActionResult UpdateCustomerProfile(Models.Models_Customer.Customer obj )
        {
            return Ok(_services.UpdateCustomerProfile(obj).Data);
        }
    }
}
