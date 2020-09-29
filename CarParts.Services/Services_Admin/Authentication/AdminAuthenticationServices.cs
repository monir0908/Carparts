using System;
using System.Linq;
using System.Web.Mvc;
using CarParts.Common;
using CarParts.Models;
using Microsoft.AspNet.SignalR;

namespace CarParts.Services.Services_Admin.Authentication
{
    public class AdminAuthenticationServices : IAdminAuthenticationServices
    {
        private readonly CarPartsDbContext _context;
        private readonly IHubContext _hubContext;


        public AdminAuthenticationServices()
        {
            _context = new CarPartsDbContext();
            _hubContext = GlobalHost.ConnectionManager.GetHubContext("adminHub");
        }


        //PRIVATE METHODS
        private bool ExistingPasswordMatches(Guid adminId, string extPassword)
        {
            var result = _context.Admin.FirstOrDefault(x => x.Password == extPassword && x.Id == adminId);

            if (result == null)
            {
                return false;
            }
            return true;
        }
        private bool IsExistingEmailValid(Guid adminId, string extEmail)
        {
            var result = _context.Admin.FirstOrDefault(x => x.Email == extEmail && x.Id == adminId);

            if (result == null)
            {
                return false;
            }
            return true;
        }
        private bool EmailMatchesWithOthers(Guid adminId, string newEmail)
        {
            var result = _context.Admin.FirstOrDefault(x => x.Email == newEmail && x.Id != adminId);

            if (result == null)
            {
                return false;
            }
            return true;
        }

        private bool ExistingEmailAndPasswordMatches(Guid adminId, string extEmail, string extPassword)
        {
            var result = _context.Admin.FirstOrDefault(x => x.Email == extEmail && x.Password == extPassword && x.Id == adminId);

            if (result == null)
            {
                return false;
            }
            return true;
        }


        //GET METHODS

        public JsonResult GetAdminDetailsForPayload(Guid adminId)
        {
            return new JsonResult
            {
                Data = _context.Admin.Where(x => x.Id == adminId)
                .Select(x => new
                {
                    x.Id,
                    x.Email,
                    x.Role,
                }).FirstOrDefault(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }
        public JsonResult GetAdminDetailsForCookies(Guid adminId)
        {
            return new JsonResult
            {
                Data = _context.Admin.Where(x => x.Id == adminId)
                .Select(x => new
                {
                    x.Id,
                    x.Email,
                    x.Role,

                }).FirstOrDefault(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }


        //For login purpose :
        public Guid Admin_Authenticate(string adminEmail, string password)
        {
            var admin = _context.Admin.FirstOrDefault(u => u.Email == adminEmail && u.Password == password);
            if (admin != null && admin.Id != Guid.Empty)
            {
                return admin.Id;
            }
            return Guid.Empty;
        }

        //POST METHODS
        public JsonResult ChangePassword(Guid adminId, string extPassword, string newPassword)
        {
            string message;
            try
            {
                if (extPassword == newPassword)
                {
                    Generator.IsReport = "Warning";
                    message = "Apparently, your existing password and new password are same, Password NOT updated !";
                }
                else if (ExistingPasswordMatches(adminId, extPassword))
                {
                    _context.Database.ExecuteSqlCommand($"UPDATE [Admins] SET Password ='{newPassword}' WHERE Id = '{adminId}'");
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

                Generator.IsReport = "NotOk";
                message = ex.Message;
            }
            string content = null;
            return new JsonResult
            {
                Data = new
                {
                    Generator.IsReport,
                    Message = message,
                    Content = content
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult ChangeEmail(Guid adminId, string extEmail, string newEmail)
        {
            string message;
            try
            {
                if (extEmail == newEmail)
                {
                    Generator.IsReport = "Warning";
                    message = "Apparently, your existing email and new email are same, Password NOT updated !";
                }
                else if (!IsExistingEmailValid(adminId, extEmail))
                {
                    Generator.IsReport = "Warning";
                    message = "Your existing email is incorrect, Email NOT updated !";
                }
                else if (EmailMatchesWithOthers(adminId, newEmail))
                {
                    Generator.IsReport = "Warning";
                    message = "The email you have chosen is already taken, Please try another !";
                }
                else
                {
                    _context.Database.ExecuteSqlCommand($"UPDATE [Admins] SET Email ='{newEmail}' WHERE Id = '{adminId}'");
                    _context.SaveChanges();
                    Generator.IsReport = "Success";
                    message = "Your email has been updated successfully !";
                    // SignalR to update cookie when admin change his/her email
                    _hubContext.Clients.All.clientRecievedByAdminsOnChangeEmailByAdmins(adminId);
                }
            }
            catch (Exception ex)
            {

                Generator.IsReport = "NotOk";
                message = ex.Message;
            }
            string content = null;
            return new JsonResult
            {
                Data = new
                {
                    Generator.IsReport,
                    Message = message,
                    Content = content
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }


    public interface IAdminAuthenticationServices
    {

        //GET METHODS
        JsonResult GetAdminDetailsForPayload(Guid adminId);
        JsonResult GetAdminDetailsForCookies(Guid adminId);

        //For login purpose :
        Guid Admin_Authenticate(string adminEmail, string password);

        //POST METHODS
        JsonResult ChangePassword(Guid adminId, string extPassword, string newPassword);
        JsonResult ChangeEmail(Guid adminId, string extEmail, string newEmail);

    }
}
