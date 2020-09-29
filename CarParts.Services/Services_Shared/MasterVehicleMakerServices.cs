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
    public class MasterVehicleMakerServices : IMasterVehicleMakerServices
    {
        private readonly CarPartsDbContext _context;
        public MasterVehicleMakerServices()
        {
            _context = new CarPartsDbContext();
        }

        public JsonResult CreateMasterVehicleMaker(MasterVehicleMaker masterVehicleMaker)
        {
            if (masterVehicleMaker.MakerName != null && !_context.MasterVehicleMaker.ToList().Any(x => x.MakerName == masterVehicleMaker.MakerName))
            {
                masterVehicleMaker.Id = Guid.NewGuid();
                masterVehicleMaker.AddedOn = DateTime.UtcNow;
                _context.MasterVehicleMaker.Add(masterVehicleMaker);
                _context.SaveChanges();
                Generator.IsReport = "Success";
                Generator.Message = "Record added successfully";
            }
            else
            {
                if (masterVehicleMaker.MakerName == null)
                {
                    Generator.IsReport = "Error";
                    Generator.Message = "Failed to add record";
                }
                else if (_context.MasterVehicleMaker.ToList().Any(x => x.MakerName == masterVehicleMaker.MakerName))
                {
                    Generator.IsReport = "Warning";
                    Generator.Message = "There is another record having the same value";
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
                    MasterVehicleMakerId = masterVehicleMaker
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult UpdateMasterVehicleMaker(Guid? masterVehicleMakerId, string value)
        {
            var masterVehicleMaker = _context.MasterVehicleMaker.Where(x => x.Id == masterVehicleMakerId).Select(x => x).FirstOrDefault();
            if (masterVehicleMaker != null)
            {
                masterVehicleMaker.MakerName = value;
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
        public JsonResult GetMasterVehicleMakerList()
        {
            return new JsonResult
            {
                Data = _context.MasterVehicleMaker.Select(x => x).OrderBy(x => x.MakerName).ToList(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }

    public interface IMasterVehicleMakerServices
    {
        JsonResult CreateMasterVehicleMaker(MasterVehicleMaker masterVehicleMaker);
        JsonResult UpdateMasterVehicleMaker(Guid? masterVehicleMakerId, string value);
        JsonResult GetMasterVehicleMakerList();
    }
}
