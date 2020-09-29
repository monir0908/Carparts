using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using CarParts.Services.Services_Common;

namespace CarParts.Filters
{
    public class AuthorizationRequiredAttribute : ActionFilterAttribute
    {
        private readonly TokenServices _tokenServices;

        public AuthorizationRequiredAttribute()
        {
            _tokenServices = new TokenServices();
        }
        private const string Token = "Token";
        public override void OnActionExecuting(HttpActionContext filterContext)

        {
            if (filterContext.Request.Headers.Contains(Token))
            {
                var tokenValue = filterContext.Request.Headers.GetValues(Token).First();

                if (_tokenServices != null && !_tokenServices.ValidateToken(tokenValue))
                {
                    var responseMessage = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "Invalid Request" };
                    filterContext.Response = responseMessage;
                }

            }
            else
            {
                filterContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            base.OnActionExecuting(filterContext);

        }
    }
}