using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using CarParts.Models.TempModels;
using CarParts.Models;
using CarParts.Common;
using CarParts.Models.Models_Admin;

namespace CarParts.Services.Services_Admin.Profile
{
    public class AdminProfileServices : IAdminProfileServices
    {
        private readonly CarPartsDbContext _context;
        public AdminProfileServices()
        {
            _context = new CarPartsDbContext();
        }

        public JsonResult RegisterAdmin(Admin admin)
        {
            admin.Id = Guid.NewGuid();
            _context.Admin.Add(admin);
            _context.SaveChanges();
            Generator.IsReport = "Success";
            Generator.Message = "Admin created successfully";
            return new JsonResult
            {
                Data = new
                {
                    Generator.IsReport,
                    Generator.Message
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }
        public JsonResult GetAdminProfileDetailsByAdminId(Guid adminId)
        {
            TemporaryAuthentication ta = new TemporaryAuthentication();
            ta.extEmail = _context.Admin.Where(x => x.Id == adminId).Select(x => x.Email).FirstOrDefault();
            return new JsonResult
            {
                Data = ta,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
    public interface IAdminProfileServices
    {
        JsonResult RegisterAdmin(Admin admin);
        JsonResult GetAdminProfileDetailsByAdminId(Guid adminId);
    }
}
