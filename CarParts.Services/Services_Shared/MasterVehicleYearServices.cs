using CarParts.Common;
using CarParts.Models;
using CarParts.Models.Models_Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CarParts.Services.Services_Shared
{
    public class MasterVehicleYearServices : IMasterVehicleYearServices
    {
        private readonly CarPartsDbContext _context;
        public MasterVehicleYearServices()
        {
            _context = new CarPartsDbContext();
        }

        public JsonResult CreateMasterVehicleYear(MasterVehicleYear masterVehicleYear)
        {
            if (!_context.MasterVehicleYear.ToList().Any(x => x.Year == masterVehicleYear.Year))
            {
                masterVehicleYear.Id = Guid.NewGuid();
                masterVehicleYear.AddedOn = DateTime.UtcNow;
                _context.MasterVehicleYear.Add(masterVehicleYear);
                _context.SaveChanges();
                Generator.IsReport = "Success";
                Generator.Message = "Record added successfully";
            }
            else
            {
                if (_context.MasterVehicleYear.ToList().Any(x => x.Year == masterVehicleYear.Year))
                {
                    Generator.IsReport = "Warning";
                    Generator.Message = "There is another record having the same name";
                }
                else
                {
                    Generator.IsReport = "Error";
                    Generator.Message = "Failed to add record";
                }
            }

            return new JsonResult
            {
                Data = new
                {
                    Generator.IsReport,
                    Generator.Message,
                    MasterVehicleYearId = masterVehicleYear
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult UpdateMasterVehicleYear(Guid? masterVehicleYearId, int value)
        {
            var masterVehicleYear = _context.MasterVehicleYear.Where(x => x.Id == masterVehicleYearId).Select(x => x).FirstOrDefault();
            if (masterVehicleYear != null)
            {
                masterVehicleYear.Year = value;
                _context.SaveChanges();
                Generator.IsReport = "Success";
                Generator.Message = "Record updated successfully";
            }
            else
            {
                Generator.IsReport = "Error";
                Generator.Message = "404 Not Found!";
            }

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
        public JsonResult GetMasterVehicleYearList()
        {
            return new JsonResult
            {
                Data = _context.MasterVehicleYear.ToList().Select(x => new
                {
                    x.AddedOn,
                    x.AdminId,
                    Id = x.Id.ToString(),
                    Year = x.Year.ToString()
                }).OrderBy(x => x.Year).ToList(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult GetMaxYear()
        {
            return new JsonResult
            {
                Data = DateTime.UtcNow.Year,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }

    public interface IMasterVehicleYearServices
    {
        JsonResult CreateMasterVehicleYear(MasterVehicleYear masterVehicleYear);
        JsonResult UpdateMasterVehicleYear(Guid? masterVehicleYearId, int value);
        JsonResult GetMasterVehicleYearList();
        JsonResult GetMaxYear();
    }
}
