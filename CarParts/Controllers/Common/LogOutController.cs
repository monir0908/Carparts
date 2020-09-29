using System;
using System.Web.Http;
using CarParts.Services.Services_Common;

namespace CarParts.Controllers.Common
{


    [RoutePrefix("api/Common")]
    public class LogOutController : ApiController
    {
        private readonly ITokenServices _tokenServices;

        public LogOutController()
        {
            _tokenServices = new TokenServices();
        }


        [Route("LogOutByAdminId/{adminId:guid}")]
        [HttpPost]
        public IHttpActionResult LogOutByAdminId(Guid adminId)
        {
            return Ok(_tokenServices.LogOutByAdminId(adminId));
        }

        [Route("LogOutByCustomerId/{userId:guid}")]
        [HttpPost]
        public IHttpActionResult LogOutByCustomerId(Guid userId)
        {
            return Ok(_tokenServices.LogOutByCustomerId(userId));
        }


    }
}