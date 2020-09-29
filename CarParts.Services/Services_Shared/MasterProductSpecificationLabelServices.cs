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
    public class MasterProductSpecificationLabelServices : IMasterProductSpecificationLabelServices
    {
        private readonly CarPartsDbContext _context;
        public MasterProductSpecificationLabelServices()
        {
            _context = new CarPartsDbContext();
        }

        public JsonResult CreateMasterProductSpecificationLabel(MasterProductSpecificationLabel masterProductSpecificationLabel)
        {
            if (!String.IsNullOrEmpty(masterProductSpecificationLabel.Label) && !_context.MasterProductSpecificationLabel.ToList().Any(x => x.Label.Replace(" ", "").ToLower() == masterProductSpecificationLabel.Label.Replace(" ", "").ToLower()))
            {
                masterProductSpecificationLabel.Id = Guid.NewGuid();
                masterProductSpecificationLabel.AddedOn = DateTime.UtcNow;
                _context.MasterProductSpecificationLabel.Add(masterProductSpecificationLabel);
                _context.SaveChanges();
                Generator.IsReport = "Success";
                Generator.Message = "Record added successfully";
            }
            else
            {
                if (String.IsNullOrEmpty(masterProductSpecificationLabel.Label))
                {
                    Generator.IsReport = "Error";
                    Generator.Message = "Failed to add record";
                }
                else if (_context.MasterProductSpecificationLabel.ToList().Any(x => x.Label.ToLower() == masterProductSpecificationLabel.Label.ToLower()))
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
                    Generator.Message
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult UpdateMasterProductSpecificationLabel(Guid? masterProductSpecificationLabelId, string value)
        {
            var masterProductSpecificationLabel = _context.MasterProductSpecificationLabel.Where(x => x.Id == masterProductSpecificationLabelId).Select(x => x).FirstOrDefault();
            if (masterProductSpecificationLabel != null)
            {
                masterProductSpecificationLabel.Label = value;
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
        public JsonResult GetMasterProductSpecificationLabelList()
        {
            return new JsonResult
            {
                Data = _context.MasterProductSpecificationLabel.Select(x => x).ToList(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult ToggleCategory(Guid? masterProductSpecificationLabelId)
        {
            var masterProductSpecificationToToggle = _context.MasterProductSpecificationLabel.Where(x => x.Id == masterProductSpecificationLabelId).Select(x => x).FirstOrDefault();
            if (masterProductSpecificationToToggle != null)
            {
                if (masterProductSpecificationToToggle.IsCategorical)
                {
                    masterProductSpecificationToToggle.IsCategorical = false;
                    _context.SaveChanges();
                    Generator.IsReport = "Success";
                    Generator.Message = "Label updated as non categorical";
                }
                else
                {
                    masterProductSpecificationToToggle.IsCategorical = true;
                    _context.SaveChanges();
                    Generator.IsReport = "Success";
                    Generator.Message = "Label updated as categorical";
                }
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
    }

    public interface IMasterProductSpecificationLabelServices
    {
        JsonResult CreateMasterProductSpecificationLabel(MasterProductSpecificationLabel masterProductSpecificationLabel);
        JsonResult UpdateMasterProductSpecificationLabel(Guid? masterProductSpecificationLabelId, string value);
        JsonResult GetMasterProductSpecificationLabelList();
        JsonResult ToggleCategory(Guid? masterProductSpecificationLabelId);
    }
}
