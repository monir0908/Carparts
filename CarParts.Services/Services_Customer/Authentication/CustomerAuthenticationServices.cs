using System;
using System.Linq;
using System.Web.Mvc;
using CarParts.Common;
using CarParts.Models;
using Microsoft.AspNet.SignalR;

namespace CarParts.Services.Services_Customer.Authentication
{
    public class CustomerAuthenticationServices : ICustomerAuthenticationServices
    {
        private readonly CarPartsDbContext _context;
        private readonly IHubContext _adminHubContext;

        public CustomerAuthenticationServices()
        {
            _context = new CarPartsDbContext();
            _adminHubContext = GlobalHost.ConnectionManager.GetHubContext("adminHub");
        }

        //PRIVATE METHODS
        private bool ExistingPasswordMatches(Guid userId, string extPassword)
        {
            var result = _context.Customer.FirstOrDefault(x => x.Password == extPassword && x.Id == userId);

            if (result == null)
            {
                return false;
            }
            return true;
        }
        private bool IsExistingEmailValid(Guid userId, string extEmail)
        {
            var result = _context.Customer.FirstOrDefault(x => x.Email == extEmail && x.Id == userId);

            if (result == null)
            {
                return false;
            }
            return true;
        }
        private bool EmailMatchesWithOthers(Guid userId, string newEmail)
        {
            var result = _context.Customer.FirstOrDefault(x => x.Email == newEmail && x.Id != userId);

            if (result == null)
            {
                return false;
            }
            return true;
        }
        private bool ExistingEmailAndPasswordMatches(Guid userId, string extEmail, string extPassword)
        {
            var result = _context.Customer.FirstOrDefault(x => x.Email == extEmail && x.Password == extPassword && x.Id == userId);

            if (result == null)
            {
                return false;
            }
            return true;
        }


        //GET METHODS
        public JsonResult GetCustomerDetailsForPayload(Guid userId)
        {
            return new JsonResult
            {
                Data = _context.Customer.Where(x => x.Id == userId)
                .Select(x => new
                {
                    x.Id,
                    x.Email,

                }).FirstOrDefault(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }
        public JsonResult GetCustomerDetailsForCookies(Guid userId)
        {
            return new JsonResult
            {
                Data = _context.Customer.Where(x => x.Id == userId)
                .Select(x => new
                {
                    x.Id,
                    x.Email,
                    x.Name

                }).FirstOrDefault(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        //For login purpose :
        public Guid Customer_Authenticate(string userEmail, string password)
        {
            var applicant = _context.Customer.FirstOrDefault(u => u.Email == userEmail && u.Password == password);
            if (applicant != null && applicant.Id != Guid.Empty)
            {
                return applicant.Id;
            }
            return Guid.Empty;
        }

        //POST METHODS
        public JsonResult ChangePassword(Guid userId, string extPassword, string newPassword)
        {
            string message;
            try
            {
                if (extPassword == newPassword)
                {
                    Generator.IsReport = "Warning";
                    message = "Apparently, your existing password and new password are same, Password NOT updated !";
                }
                else if (ExistingPasswordMatches(userId, extPassword))
                {
                    _context.Database.ExecuteSqlCommand($"UPDATE [Customers] SET Password ='{newPassword}' WHERE Id = '{userId}'");
                    _context.SaveChanges();
                    Generator.IsReport = "Success";
                    message = "Your password has been updated successfully !";
                }
                else
                {
                    Generator.IsReport = "Error";
                    message = "Your existing password is incorrect, Password NOT updated !";
                }
            }
            catch (Exception ex)
            {

                Generator.IsReport = "Error";
                message = ex.Message;
            }
            return new JsonResult
            {
                Data = new
                {
                    Generator.IsReport,
                    Message = message
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult ChangeEmail(Guid userId, string extEmail, string newEmail)
        {
            string message;
            try
            {
                if (extEmail == newEmail)
                {
                    Generator.IsReport = "Warning";
                    message = "Apparently, your existing email and new email are same, Password NOT updated !";
                }
                else if (!IsExistingEmailValid(userId, extEmail))
                {
                    Generator.IsReport = "Warning";
                    message = "Your existing email is incorrect, Email NOT updated !";
                }
                else if (EmailMatchesWithOthers(userId, newEmail))
                {
                    Generator.IsReport = "Warning";
                    message = "The email you have chosen is already taken, Please try another !";
                }
                else
                {
                    _context.Database.ExecuteSqlCommand($"UPDATE [Customers] SET Email ='{newEmail}' WHERE Id = '{userId}'");
                    _context.SaveChanges();
                    _adminHubContext.Clients.All.clientRecievedByAdminOnChangeEmail(userId, _context.Customer.Where(x => x.Id == userId).Select(x => x.Name).FirstOrDefault());
                    Generator.IsReport = "Success";
                    message = "Your email has been updated successfully !";
                }
            }
            catch (Exception ex)
            {
                Generator.IsReport = "Error";
                message = ex.Message;
            }
            return new JsonResult
            {
                Data = new
                {
                    Generator.IsReport,
                    Message = message
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

    }


    public interface ICustomerAuthenticationServices
    {

        //For login purpose :
        Guid Customer_Authenticate(string username, string password);

        // GET METHODS
        JsonResult GetCustomerDetailsForPayload(Guid userId);
        JsonResult GetCustomerDetailsForCookies(Guid userId);

        //POST METHODS
        JsonResult ChangePassword(Guid userId, string extPassword, string newPassword);
        JsonResult ChangeEmail(Guid userId, string extEmail, string newEmail);

    }
}
