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
    public class MasterVehicleModelServices : IMasterVehicleModelServices
    {
        private readonly CarPartsDbContext _context;
        public MasterVehicleModelServices()
        {
            _context = new CarPartsDbContext();
        }

        public JsonResult CreateMasterVehicleModel(MasterVehicleModel masterVehicleModel)
        {
            if (masterVehicleModel.ModelName != null && !_context.MasterVehicleModel.ToList().Any(x => x.ModelName == masterVehicleModel.ModelName))
            {
                masterVehicleModel.Id = Guid.NewGuid();
                masterVehicleModel.AddedOn = DateTime.UtcNow;
                _context.MasterVehicleModel.Add(masterVehicleModel);
                _context.SaveChanges();
                Generator.IsReport = "Success";
                Generator.Message = "Record added successfully";
            }
            else
            {
                if (masterVehicleModel.ModelName == null)
                {
                    Generator.IsReport = "Error";
                    Generator.Message = "Failed to add record";
                }
                else if (_context.MasterVehicleModel.ToList().Any(x => x.ModelName == masterVehicleModel.ModelName))
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
                    MasterVehicleModelId = masterVehicleModel
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult UpdateMasterVehicleModel(Guid? masterVehicleModelId, string value)
        {
            var masterVehicleModel = _context.MasterVehicleModel.Where(x => x.Id == masterVehicleModelId).Select(x => x).FirstOrDefault();
            if (masterVehicleModel != null)
            {
                masterVehicleModel.ModelName = value;
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
        public JsonResult GetMasterVehicleModelList()
        {
            return new JsonResult
            {
                Data = _context.MasterVehicleModel.Select(x => x).OrderBy(x => x.ModelName).ToList(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }

    public interface IMasterVehicleModelServices
    {
        JsonResult CreateMasterVehicleModel(MasterVehicleModel masterVehicleModel);
        JsonResult UpdateMasterVehicleModel(Guid? masterVehicleModelId, string value);
        JsonResult GetMasterVehicleModelList();
    }
}
