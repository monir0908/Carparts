using System;
using System.Linq;
using CarParts.Models;
using CarParts.Models.Models_Admin;
using CarParts.Models.Models_Customer;
using CarParts.Models.TempModels;
using CarParts.Services.Services_Admin.Authentication;
using CarParts.Services.Services_Customer.Authentication;
using JWT;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Configuration;

namespace CarParts.Services.Services_Common
{
    public class TokenServices : ITokenServices
    {
        #region Private member variables.
        private readonly IEntityService<Admin_Token> _adminTokenRepository;
        private readonly IEntityService<Customer_Token> _customerTokenRepository;
        private readonly CarPartsDbContext _context;
        private readonly IAdminAuthenticationServices _adminServices;
        private readonly ICustomerAuthenticationServices _applicantSevices;
        #endregion


        public TokenServices()
        {
            _context = new CarPartsDbContext();
            _adminTokenRepository = new EntityService<Admin_Token>(_context);
            _customerTokenRepository = new EntityService<Customer_Token>(_context);
            _adminServices = new AdminAuthenticationServices();
            _applicantSevices = new CustomerAuthenticationServices();
        }

        public Admin_Token GenerateAdminToken(Guid adminId)
        {
            try
            {
                ////Add system.web.extensions
                ////Add system.configuration for ConfigurationManager
                var admin = JsonConvert.DeserializeObject<Admin>(new JavaScriptSerializer().Serialize(_adminServices.GetAdminDetailsForPayload(adminId).Data));
                var issuedOn = DateTime.Now;
                var expiredOn = DateTime.Now.AddHours(Convert.ToDouble(ConfigurationManager.AppSettings["AuthTokenExpiry"]));

                var payload = new Admin_PayLoad
                {
                    Username = admin.Email

                };
                //Install-Package JWT -Version 1.3.0   through nuget package in the corresponded services (i.e. CarParts.Services)
                var token = "Admin-CarParts#" + JsonWebToken.Encode(payload, EncryptionHelper.GetPrivateKey(),
                JwtHashAlgorithm.HS256);

                var tokenEntity = new Admin_Token
                {
                    Id = Guid.NewGuid(),
                    AdminID = adminId,
                    AuthToken = token,
                    IssuedOn = issuedOn,
                    ExpiresOn = expiredOn
                };

                _adminTokenRepository.Save(tokenEntity);
                _adminTokenRepository.SaveChanges();
                return tokenEntity;
            }
            catch (Exception)
            {
                return new Admin_Token();
            }
        }
        public Customer_Token GenerateCustomerToken(Guid customerId)
        {
            try
            {
                var applicant = JsonConvert.DeserializeObject<Customer>(new JavaScriptSerializer().Serialize(_applicantSevices.GetCustomerDetailsForPayload(customerId).Data));
                var issuedOn = DateTime.Now;
                var expiredOn = DateTime.Now.AddHours(Convert.ToDouble(ConfigurationManager.AppSettings["AuthTokenExpiry"]));

                var payload = new Customer_PayLoad
                {
                    Username = applicant.Email

                };
                //Install-Package JWT -Version 1.3.0   through nuget package in the corresponded services (i.e. CarParts.Services)
                var token = "Customer-CarParts#" + JsonWebToken.Encode(payload, EncryptionHelper.GetPrivateKey(),
                JwtHashAlgorithm.HS256);

                var tokenEntity = new Customer_Token
                {
                    Id = Guid.NewGuid(),
                    CustomerID = customerId,
                    AuthToken = token,
                    IssuedOn = issuedOn,
                    ExpiresOn = expiredOn
                };

                _customerTokenRepository.Save(tokenEntity);
                _customerTokenRepository.SaveChanges();
                return tokenEntity;
            }
            catch (Exception)
            {
                return new Customer_Token();
            }
        }
        public bool ValidateToken(string longToken)
        {
            if (longToken.Contains("Admin-CarParts#"))
            {
                using (CarPartsDbContext a = new CarPartsDbContext())
                {
                    Admin_Token token = a.Admin_Token.FirstOrDefault(x => x.AuthToken == longToken);


                    if (token != null)
                    {
                        return true;

                    }
                    else
                    {
                        var adminId = a.Admin_Token.Where(x => x.AuthToken == longToken).Select(x => x.AdminID).FirstOrDefault();
                        //DeleteCPTokenByAdminId((int) AdminId);
                        return false;
                    }
                }
            }
            else if (longToken.Contains("Customer-CarParts#"))
            {
                using (CarPartsDbContext a = new CarPartsDbContext())
                {
                    var token = a.Customer_Token.FirstOrDefault(x => x.AuthToken == longToken);


                    if (token != null)
                    {
                        return true;

                    }
                    else
                    {
                        var customerId = a.Customer_Token.Where(x => x.AuthToken == longToken).Select(x => x.CustomerID).FirstOrDefault();
                        //DeleteTPTokenByUserId((int) userId);
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }





        }
        public bool Kill(string tokenId)
        {
            var token = _context.Admin_Token.FirstOrDefault(x => x.AuthToken == tokenId);
            _adminTokenRepository.Delete(token);
            _adminTokenRepository.SaveChanges();
            var isNotDeleted = _context.Admin_Token.Any(x => x.AuthToken == tokenId);
            if (isNotDeleted)
            {
                return false;
            }
            return true;
        }
        public bool LogOutByAdminId(Guid adminId)
        {
            _context.Admin_Token.RemoveRange(_context.Admin_Token.Where(x => x.AdminID == adminId));
            _adminTokenRepository.SaveChanges();
            var isNotDeleted = _context.Admin_Token.Any(x => x.AdminID == adminId);
            return !isNotDeleted;
        }
        public bool LogOutByCustomerId(Guid customerId)
        {
            _context.Customer_Token.RemoveRange(_context.Customer_Token.Where(x => x.CustomerID == customerId));
            _adminTokenRepository.SaveChanges();
            var isNotDeleted = _context.Customer_Token.Any(x => x.CustomerID == customerId);
            return !isNotDeleted;
        }

    }

    public interface ITokenServices
    {
        Admin_Token GenerateAdminToken(Guid adminId);
        Customer_Token GenerateCustomerToken(Guid customerId);
        bool ValidateToken(string longToken);
        bool Kill(string tokenId);
        bool LogOutByAdminId(Guid adminId);
        bool LogOutByCustomerId(Guid customerId);
    }


}
