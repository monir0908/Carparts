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
    public class MasterVehicleSubModelServices : IMasterVehicleSubModelServices
    {
        private readonly CarPartsDbContext _context;
        public MasterVehicleSubModelServices()
        {
            _context = new CarPartsDbContext();
        }

        public JsonResult CreateMasterVehicleSubModel(MasterVehicleSubModel masterVehicleSubModel)
        {
            if (masterVehicleSubModel.SubModelName != null && !_context.MasterVehicleSubModel.ToList().Any(x => x.SubModelName == masterVehicleSubModel.SubModelName))
            {
                masterVehicleSubModel.Id = Guid.NewGuid();
                masterVehicleSubModel.AddedOn = DateTime.UtcNow;
                _context.MasterVehicleSubModel.Add(masterVehicleSubModel);
                _context.SaveChanges();
                Generator.IsReport = "Success";
                Generator.Message = "Record added successfully";
            }
            else
            {
                if (masterVehicleSubModel.SubModelName == null)
                {
                    Generator.IsReport = "Error";
                    Generator.Message = "Failed to add record";
                }
                else if (_context.MasterVehicleSubModel.ToList().Any(x => x.SubModelName == masterVehicleSubModel.SubModelName))
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
                    MasterVehicleSubModelId = masterVehicleSubModel
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult UpdateMasterVehicleSubModel(Guid? masterVehicleSubModelId, string value)
        {
            var masterVehicleSubModel = _context.MasterVehicleSubModel.Where(x => x.Id == masterVehicleSubModelId).Select(x => x).FirstOrDefault();
            if (masterVehicleSubModel != null)
            {
                masterVehicleSubModel.SubModelName = value;
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
        public JsonResult GetMasterVehicleSubModelFilteredList(Guid? yearId, Guid? makerId, Guid? modelId)
        {
            var subModelListToExclude = _context.Vehicle
                                        .Where(x => x.MasterVehicleYearId == yearId && x.MasterVehicleMakerId == makerId && x.MasterVehicleModelId == modelId)
                                        .Select(x => x.MasterVehicleSubModelId).ToList();
            return new JsonResult
            {
                Data = _context.MasterVehicleSubModel.ToList().Where(x => subModelListToExclude.Count() > 0 ? !subModelListToExclude.Contains(x.Id) : true).Select(x => x).OrderBy(x => x.SubModelName).ToList(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult GetMasterVehicleSubModelList()
        {
            return new JsonResult
            {
                Data = _context.MasterVehicleSubModel.Select(x => x).OrderBy(x => x.SubModelName).ToList(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }

    public interface IMasterVehicleSubModelServices
    {
        JsonResult CreateMasterVehicleSubModel(MasterVehicleSubModel masterVehicleSubModel);
        JsonResult UpdateMasterVehicleSubModel(Guid? masterVehicleSubModelId, string value);
        JsonResult GetMasterVehicleSubModelFilteredList(Guid? yearId, Guid? makerId, Guid? modelId);
        JsonResult GetMasterVehicleSubModelList();
    }
}
