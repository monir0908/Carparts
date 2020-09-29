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
    public class MasterVehicleEngineServices : IMasterVehicleEngineServices
    {
        private readonly CarPartsDbContext _context;
        public MasterVehicleEngineServices()
        {
            _context = new CarPartsDbContext();
        }

        public JsonResult CreateMasterVehicleEngine(MasterVehicleEngine masterVehicleEngine)
        {
            if (masterVehicleEngine.EngineName != null && !_context.MasterVehicleEngine.ToList().Any(x => x.EngineName == masterVehicleEngine.EngineName))
            {
                masterVehicleEngine.Id = Guid.NewGuid();
                masterVehicleEngine.AddedOn = DateTime.UtcNow;
                _context.MasterVehicleEngine.Add(masterVehicleEngine);
                _context.SaveChanges();
                Generator.IsReport = "Success";
                Generator.Message = "Record added successfully";
            }
            else
            {
                if (masterVehicleEngine.EngineName == null)
                {
                    Generator.IsReport = "Error";
                    Generator.Message = "Failed to add record";
                }
                else if (_context.MasterVehicleEngine.ToList().Any(x => x.EngineName == masterVehicleEngine.EngineName))
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
                    MasterVehicleEngineId = masterVehicleEngine
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult UpdateMasterVehicleEngine(Guid? masterVehicleEngineId, string value)
        {
            var masterVehicleEngine = _context.MasterVehicleEngine.Where(x => x.Id == masterVehicleEngineId).Select(x => x).FirstOrDefault();
            if (masterVehicleEngine != null)
            {
                masterVehicleEngine.EngineName = value;
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
        public JsonResult GetMasterVehicleEngineFilteredList(Guid? yearId, Guid? makerId, Guid? modelId, Guid? engineId)
        {
            var engineListToExclude = _context.Vehicle
                            .Where(x => x.MasterVehicleYearId == yearId && x.MasterVehicleMakerId == makerId && x.MasterVehicleModelId == modelId && x.MasterVehicleEngineId == engineId)
                            .Select(x => x.MasterVehicleEngineId).ToList();
            return new JsonResult
            {
                Data = _context.MasterVehicleEngine.ToList().Where(x => engineListToExclude.Count() > 0 ? !engineListToExclude.Contains(x.Id) : true).Select(x => x).OrderBy(x => x.EngineName).ToList(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult GetMasterVehicleEngineList()
        {
            return new JsonResult
            {
                Data = _context.MasterVehicleEngine.Select(x => x).OrderBy(x => x.EngineName).ToList(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }

    public interface IMasterVehicleEngineServices
    {
        JsonResult CreateMasterVehicleEngine(MasterVehicleEngine masterVehicleEngine);
        JsonResult UpdateMasterVehicleEngine(Guid? masterVehicleEngineId, string value);
        JsonResult GetMasterVehicleEngineFilteredList(Guid? yearId, Guid? makerId, Guid? modelId, Guid? engineId);
        JsonResult GetMasterVehicleEngineList();
    }
}
