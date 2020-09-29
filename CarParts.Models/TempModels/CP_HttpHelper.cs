using System;
using System.Linq;
using System.Net.Http;
using CarParts.Models.TempModels;
using JWT;
using Newtonsoft.Json;

namespace CarParts.Models.TempModels
{
    public class CP_HttpHelper
    {
        //This class is created to decode the token (brought from Client side).
        public Admin_PayLoad GetCustomToken(HttpRequestMessage httpRequest)
        {
            Admin_PayLoad token = null;
            try
            {
                if (httpRequest.Headers.Contains("Token"))
                {
                    //Decode token
                    string tokenKey = JsonWebToken.Decode(httpRequest.Headers.GetValues("Token").First(), EncryptionHelper.GetPrivateKey(), true);
                    //Users
                    token = JsonConvert.DeserializeObject<Admin_PayLoad>(tokenKey);
                }
            }
            catch (Exception )
            {
                return null;
            }
            return token;
        }
    }
}