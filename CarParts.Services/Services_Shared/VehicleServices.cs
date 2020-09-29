using CarParts.Common;
using CarParts.Models;
using CarParts.Models.Models_Shared;
using CarParts.Models.TempModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CarParts.Services.Services_Shared
{
    public class VehicleServices : IVehicleServices
    {
        private readonly CarPartsDbContext _context;
        public VehicleServices()
        {
            _context = new CarPartsDbContext();
        }

        public JsonResult CreateVehicle(Vehicle vehicle)
        {
            if (vehicle.MasterVehicleYearId != null && vehicle.MasterVehicleYearId != Guid.Empty && vehicle.MasterVehicleMakerId != null && vehicle.MasterVehicleMakerId != Guid.Empty && vehicle.MasterVehicleModelId != null && vehicle.MasterVehicleModelId != Guid.Empty && vehicle.MasterVehicleSubModelId != null && vehicle.MasterVehicleSubModelId != Guid.Empty && vehicle.MasterVehicleEngineId != null && vehicle.MasterVehicleEngineId != Guid.Empty)
            {
                vehicle.Id = Guid.NewGuid();
                vehicle.AddedOn = DateTime.UtcNow;
                _context.Vehicle.Add(vehicle);
                _context.SaveChanges();
                Generator.IsReport = "Success";
                Generator.Message = "Vehicle added successfully";
            }
            else
            {
                Generator.IsReport = "Error";
                Generator.Message = "Vehicle record addition failed as some information is not provided";
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
        public JsonResult DeletevehicleByVehicleId(Guid? vehicleId)
        {
            var associatedVehicleFitmentInfo = _context.VehicleFitment.Where(x => x.VehicleId == vehicleId).Select(x => x).ToList();
            if (associatedVehicleFitmentInfo.Count() == 0)
            {
                var vehicleToRemove = _context.Vehicle.Where(x => x.Id == vehicleId).Select(x => x).FirstOrDefault();
                _context.Vehicle.Remove(vehicleToRemove);
                _context.SaveChanges();
                Generator.IsReport = "Success";
                Generator.Message = "Vehicle deleted successfully";
            }
            else if (associatedVehicleFitmentInfo.Count() > 0)
            {
                Generator.IsReport = "Warning";
                Generator.Message = "There is total <b>" + associatedVehicleFitmentInfo.Count() + "</b> fitment information associated with this vehicle. Please remove them before you remove this vehicle.";
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
        public JsonResult GetVehicleList()
        {
            return new JsonResult
            {
                Data = _context.Vehicle.Join(_context.MasterVehicleYear,
                                        x => x.MasterVehicleYearId,
                                        y => y.Id,
                                        (x, y) => new
                                        {
                                            Vehicle = x,
                                            MasterVehicleYear = y
                                        })
                                        .Join(_context.MasterVehicleMaker,
                                        x => x.Vehicle.MasterVehicleMakerId,
                                        y => y.Id,
                                        (x, y) => new
                                        {
                                            x.Vehicle,
                                            x.MasterVehicleYear,
                                            MasterVehicleMaker = y
                                        })
                                        .Join(_context.MasterVehicleModel,
                                        x => x.Vehicle.MasterVehicleModelId,
                                        y => y.Id,
                                        (x, y) => new
                                        {
                                            x.Vehicle,
                                            x.MasterVehicleYear,
                                            x.MasterVehicleMaker,
                                            MasterVehicleModel = y
                                        })
                                        .Join(_context.MasterVehicleSubModel,
                                        x => x.Vehicle.MasterVehicleSubModelId,
                                        y => y.Id,
                                        (x, y) => new
                                        {
                                            x.Vehicle,
                                            x.MasterVehicleYear,
                                            x.MasterVehicleMaker,
                                            x.MasterVehicleModel,
                                            MasterVehicleSubModel = y
                                        })
                                        .Join(_context.MasterVehicleEngine,
                                        x => x.Vehicle.MasterVehicleEngineId,
                                        y => y.Id,
                                        (x, y) => new
                                        {
                                            x.Vehicle,
                                            x.MasterVehicleYear,
                                            x.MasterVehicleMaker,
                                            x.MasterVehicleModel,
                                            x.MasterVehicleSubModel,
                                            MasterVehicleEngine = y
                                        })
                                        .ToList()
                                        .Select(x => new
                                        {
                                            Year = x.MasterVehicleYear.Year.ToString(),
                                            x.MasterVehicleMaker.MakerName,
                                            x.MasterVehicleModel.ModelName,
                                            x.MasterVehicleSubModel.SubModelName,
                                            x.MasterVehicleEngine.EngineName,
                                            x.Vehicle.Id,
                                            x.Vehicle.MasterVehicleEngineId,
                                            x.Vehicle.MasterVehicleMakerId,
                                            x.Vehicle.MasterVehicleModelId,
                                            x.Vehicle.MasterVehicleSubModelId,
                                            x.Vehicle.MasterVehicleYearId,
                                            x.Vehicle.AddedOn
                                        }).ToList(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult GetMakerListByYearId(Guid? yearId)
        {
            return new JsonResult
            {
                Data = _context.Vehicle.Where(x => x.MasterVehicleYearId == yearId)
                                        .Join(_context.MasterVehicleMaker,
                                        x => x.MasterVehicleMakerId,
                                        y => y.Id,
                                        (x, y) => new
                                        {
                                            Vehicle = x,
                                            MasterVehicleMaker = y
                                        })
                                        .Select(x => new
                                        {
                                            x.MasterVehicleMaker.Id,
                                            x.MasterVehicleMaker.MakerName,
                                            x.MasterVehicleMaker.AddedOn,
                                            x.MasterVehicleMaker.AdminId,
                                        }).Distinct()
                                        .ToList(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult GetModelListByMakerId(Guid? makerId)
        {
            return new JsonResult
            {
                Data = _context.Vehicle.Where(x => x.MasterVehicleMakerId == makerId)
                                        .Join(_context.MasterVehicleModel,
                                        x => x.MasterVehicleModelId,
                                        y => y.Id,
                                        (x, y) => new
                                        {
                                            Vehicle = x,
                                            MasterVehicleModel = y
                                        })
                                        .Select(x => new
                                        {
                                            x.MasterVehicleModel.Id,
                                            x.MasterVehicleModel.ModelName,
                                            x.MasterVehicleModel.AddedOn,
                                            x.MasterVehicleModel.AdminId,
                                        }).Distinct()
                                        .ToList(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult GetSubModelListByModelId(Guid? modelId)
        {
            return new JsonResult
            {
                Data = _context.Vehicle.Where(x => x.MasterVehicleModelId == modelId)
                                        .Join(_context.MasterVehicleSubModel,
                                        x => x.MasterVehicleSubModelId,
                                        y => y.Id,
                                        (x, y) => new
                                        {
                                            Vehicle = x,
                                            MasterVehicleSubModel = y
                                        })
                                        .Select(x => new
                                        {
                                            x.MasterVehicleSubModel.Id,
                                            x.MasterVehicleSubModel.SubModelName,
                                            x.MasterVehicleSubModel.AddedOn,
                                            x.MasterVehicleSubModel.AdminId,
                                        }).Distinct()
                                        .ToList(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult GetEngineListBySubModelId(Guid? subModelId)
        {
            return new JsonResult
            {
                Data = _context.Vehicle.Where(x => x.MasterVehicleSubModelId == subModelId)
                                        .Join(_context.MasterVehicleEngine,
                                        x => x.MasterVehicleEngineId,
                                        y => y.Id,
                                        (x, y) => new
                                        {
                                            Vehicle = x,
                                            MasterVehicleEngine = y
                                        })
                                        .Select(x => new
                                        {
                                            x.MasterVehicleEngine.Id,
                                            x.MasterVehicleEngine.EngineName,
                                            x.MasterVehicleEngine.AddedOn,
                                            x.MasterVehicleEngine.AdminId,
                                        }).Distinct()
                                        .ToList(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult SearchByVehicleWithOutSubModelAndEngine(Guid? yearId, Guid? makerId, Guid? modelId)
        {
            TempVehicleOnlyCookieModel tempVehicleOnlyCookieModel = new TempVehicleOnlyCookieModel();
            var vehicleDetails = _context.Vehicle.Where(x => x.MasterVehicleYearId == yearId && x.MasterVehicleMakerId == makerId && x.MasterVehicleModelId == modelId)
                                        .Join(_context.MasterVehicleYear.Where(x => x.Id == yearId),
                                        x => x.MasterVehicleYearId,
                                        y => y.Id,
                                        (x, y) => new
                                        {
                                            Vehicle = x,
                                            MasterVehicleYear = y
                                        })
                                        .Join(_context.MasterVehicleMaker.Where(x => x.Id == makerId),
                                        x => x.Vehicle.MasterVehicleMakerId,
                                        y => y.Id,
                                        (x, y) => new
                                        {
                                            x.Vehicle,
                                            x.MasterVehicleYear,
                                            MasterVehicleMaker = y
                                        })
                                        .Join(_context.MasterVehicleModel.Where(x => x.Id == modelId),
                                        x => x.Vehicle.MasterVehicleModelId,
                                        y => y.Id,
                                        (x, y) => new
                                        {
                                            x.Vehicle,
                                            x.MasterVehicleYear,
                                            x.MasterVehicleMaker,
                                            MasterVehicleModel = y
                                        })
                                        .ToList()
                                        .Select(x => new
                                        {
                                            Year = x.MasterVehicleYear.Year.ToString(),
                                            x.MasterVehicleMaker.MakerName,
                                            x.MasterVehicleModel.ModelName,
                                            x.Vehicle.Id,
                                            x.Vehicle.AddedOn
                                        }).FirstOrDefault();
            var vehicleIdList = _context.Vehicle.Where(x => x.MasterVehicleYearId == yearId && x.MasterVehicleMakerId == makerId && x.MasterVehicleModelId == modelId)
                                        .Join(_context.MasterVehicleYear.Where(x => x.Id == yearId),
                                        x => x.MasterVehicleYearId,
                                        y => y.Id,
                                        (x, y) => new
                                        {
                                            Vehicle = x,
                                            MasterVehicleYear = y
                                        })
                                        .Join(_context.MasterVehicleMaker.Where(x => x.Id == makerId),
                                        x => x.Vehicle.MasterVehicleMakerId,
                                        y => y.Id,
                                        (x, y) => new
                                        {
                                            x.Vehicle,
                                            x.MasterVehicleYear,
                                            MasterVehicleMaker = y
                                        })
                                        .Join(_context.MasterVehicleModel.Where(x => x.Id == modelId),
                                        x => x.Vehicle.MasterVehicleModelId,
                                        y => y.Id,
                                        (x, y) => new
                                        {
                                            x.Vehicle,
                                            x.MasterVehicleYear,
                                            x.MasterVehicleMaker,
                                            MasterVehicleModel = y
                                        })
                                        .ToList()
                                        .Select(x => new
                                        {
                                            x.Vehicle.Id
                                        }).ToList();
            tempVehicleOnlyCookieModel.Id = vehicleDetails.Id;
            tempVehicleOnlyCookieModel.Model = false;
            tempVehicleOnlyCookieModel.MakerName = vehicleDetails.MakerName;
            tempVehicleOnlyCookieModel.ModelName = vehicleDetails.ModelName;
            tempVehicleOnlyCookieModel.Year = vehicleDetails.Year;
            tempVehicleOnlyCookieModel.HTMLId = Generator.RandomStringByLength(10);
            tempVehicleOnlyCookieModel.VehicleIdList = new List<Guid>();
            foreach (var item in vehicleIdList)
            {
                tempVehicleOnlyCookieModel.VehicleIdList.Add(item.Id);
            }

            return new JsonResult
            {
                Data = new
                {
                    TempVehicleOnlyCookieModel = tempVehicleOnlyCookieModel
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult SearchByVehicleWithSubModelAndEngine(Guid? yearId, Guid? makerId, Guid? modelId, Guid? subModelId, Guid? engineId)
        {
            TempVehiclePlusCookieModel tempVehiclePlusCookieModel = new TempVehiclePlusCookieModel();
            var vehicleDetails = _context.Vehicle.Where(x => x.MasterVehicleYearId == yearId && x.MasterVehicleMakerId == makerId && x.MasterVehicleModelId == modelId && subModelId != null ? x.MasterVehicleSubModelId == subModelId : true && engineId != null ? x.MasterVehicleEngineId == engineId : true)
                                        .Join(_context.MasterVehicleYear,
                                        x => x.MasterVehicleYearId,
                                        y => y.Id,
                                        (x, y) => new
                                        {
                                            Vehicle = x,
                                            MasterVehicleYear = y
                                        })
                                        .Join(_context.MasterVehicleMaker,
                                        x => x.Vehicle.MasterVehicleMakerId,
                                        y => y.Id,
                                        (x, y) => new
                                        {
                                            x.Vehicle,
                                            x.MasterVehicleYear,
                                            MasterVehicleMaker = y
                                        })
                                        .Join(_context.MasterVehicleModel,
                                        x => x.Vehicle.MasterVehicleModelId,
                                        y => y.Id,
                                        (x, y) => new
                                        {
                                            x.Vehicle,
                                            x.MasterVehicleYear,
                                            x.MasterVehicleMaker,
                                            MasterVehicleModel = y
                                        })
                                        .Join(_context.MasterVehicleSubModel,
                                        x => x.Vehicle.MasterVehicleSubModelId,
                                        y => y.Id,
                                        (x, y) => new
                                        {
                                            x.Vehicle,
                                            x.MasterVehicleYear,
                                            x.MasterVehicleMaker,
                                            x.MasterVehicleModel,
                                            MasterVehicleSubModel = y
                                        })
                                        .Join(_context.MasterVehicleEngine,
                                        x => x.Vehicle.MasterVehicleEngineId,
                                        y => y.Id,
                                        (x, y) => new
                                        {
                                            x.Vehicle,
                                            x.MasterVehicleYear,
                                            x.MasterVehicleMaker,
                                            x.MasterVehicleModel,
                                            x.MasterVehicleSubModel,
                                            MasterVehicleEngine = y
                                        })
                                        .ToList()
                                        .Select(x => new
                                        {
                                            Year = x.MasterVehicleYear.Year.ToString(),
                                            x.MasterVehicleMaker.MakerName,
                                            x.MasterVehicleModel.ModelName,
                                            x.Vehicle.Id,
                                            x.Vehicle.AddedOn,
                                            x.MasterVehicleSubModel.SubModelName,
                                            x.MasterVehicleEngine.EngineName
                                        }).FirstOrDefault();
            var vehicleIdList = _context.Vehicle.Where(x => x.MasterVehicleYearId == yearId && x.MasterVehicleMakerId == makerId && x.MasterVehicleModelId == modelId && subModelId != null ? x.MasterVehicleSubModelId == subModelId : true && engineId != null ? x.MasterVehicleEngineId == engineId : true)
                                        .Join(_context.MasterVehicleYear,
                                        x => x.MasterVehicleYearId,
                                        y => y.Id,
                                        (x, y) => new
                                        {
                                            Vehicle = x,
                                            MasterVehicleYear = y
                                        })
                                        .Join(_context.MasterVehicleMaker,
                                        x => x.Vehicle.MasterVehicleMakerId,
                                        y => y.Id,
                                        (x, y) => new
                                        {
                                            x.Vehicle,
                                            x.MasterVehicleYear,
                                            MasterVehicleMaker = y
                                        })
                                        .Join(_context.MasterVehicleModel,
                                        x => x.Vehicle.MasterVehicleModelId,
                                        y => y.Id,
                                        (x, y) => new
                                        {
                                            x.Vehicle,
                                            x.MasterVehicleYear,
                                            x.MasterVehicleMaker,
                                            MasterVehicleModel = y
                                        })
                                        .Join(_context.MasterVehicleSubModel,
                                        x => x.Vehicle.MasterVehicleSubModelId,
                                        y => y.Id,
                                        (x, y) => new
                                        {
                                            x.Vehicle,
                                            x.MasterVehicleYear,
                                            x.MasterVehicleMaker,
                                            x.MasterVehicleModel,
                                            MasterVehicleSubModel = y
                                        })
                                        .Join(_context.MasterVehicleEngine,
                                        x => x.Vehicle.MasterVehicleEngineId,
                                        y => y.Id,
                                        (x, y) => new
                                        {
                                            x.Vehicle,
                                            x.MasterVehicleYear,
                                            x.MasterVehicleMaker,
                                            x.MasterVehicleModel,
                                            x.MasterVehicleSubModel,
                                            MasterVehicleEngine = y
                                        })
                                        .ToList()
                                        .Select(x => new
                                        {
                                            x.Vehicle.Id
                                        }).ToList();
            tempVehiclePlusCookieModel.Id = vehicleDetails.Id;
            tempVehiclePlusCookieModel.Model = false;
            tempVehiclePlusCookieModel.MakerName = vehicleDetails.MakerName;
            tempVehiclePlusCookieModel.ModelName = vehicleDetails.ModelName;
            tempVehiclePlusCookieModel.Year = vehicleDetails.Year;
            tempVehiclePlusCookieModel.SubModelName = vehicleDetails.SubModelName;
            tempVehiclePlusCookieModel.EngineName = vehicleDetails.EngineName;
            tempVehiclePlusCookieModel.HTMLId = Generator.RandomStringByLength(10);
            tempVehiclePlusCookieModel.VehicleIdList = new List<Guid>();
            foreach (var item in vehicleIdList)
            {
                tempVehiclePlusCookieModel.VehicleIdList.Add(item.Id);
            }

            return new JsonResult
            {
                Data = new
                {
                    TempVehiclePlusCookieModel = tempVehiclePlusCookieModel
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }

    public interface IVehicleServices
    {
        JsonResult CreateVehicle(Vehicle vehicle);
        JsonResult DeletevehicleByVehicleId(Guid? vehicleId);
        JsonResult GetVehicleList();
        JsonResult GetMakerListByYearId(Guid? yearId);
        JsonResult GetModelListByMakerId(Guid? makerId);
        JsonResult GetSubModelListByModelId(Guid? modelId);
        JsonResult GetEngineListBySubModelId(Guid? subModelId);
        JsonResult SearchByVehicleWithOutSubModelAndEngine(Guid? yearId, Guid? makerId, Guid? modelId);
        JsonResult SearchByVehicleWithSubModelAndEngine(Guid? yearId, Guid? makerId, Guid? modelId, Guid? subModelId, Guid? engineId);
    }
}
