using CarParts.Common;
using CarParts.Models;
using CarParts.Models.Models_Sahred;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CarParts.Services
{
    public class MasterCompanyServices: IMasterCompanyServices
    {
        private readonly CarPartsDbContext _context;
        public MasterCompanyServices()
        {
            _context = new CarPartsDbContext();
        }

        public JsonResult CreateMasterCompany(MasterCompany masterCompany)
        {
            var curretnCompanyList = _context.MasterCompany.Select(x => x).ToList().Count();
            if (curretnCompanyList == 0)
            {
                masterCompany.Id = Guid.NewGuid();
                masterCompany.AddedOn = DateTime.UtcNow;
                _context.MasterCompany.Add(masterCompany);
                _context.SaveChanges();
                Generator.IsReport = "Success";
                Generator.Message = "Master Company created successfully.";
            }
            else
            {
                Generator.IsReport = "Error";
                Generator.Message = "You can't create more than one master company.";
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
        public JsonResult UploadMasterCompanyLogo(Guid? adminId)
        {
            var companyToUploadImage = _context.MasterCompany.Where(x => x.AdminId == adminId).Select(x => x).FirstOrDefault();
            if (companyToUploadImage != null)
            {
                var oldImageName = companyToUploadImage.MasterCompanyLogo;
                string trimmedEmail = companyToUploadImage.MasterCompanyName.Replace(".", "");
                trimmedEmail = trimmedEmail.Replace("@", "");
                string randomString = Guid.NewGuid().ToString();

                System.Web.HttpFileCollection httpFileCollection = System.Web.HttpContext.Current.Request.Files;
                if (httpFileCollection.Count == 1)
                {
                    for (int i = 0; i < httpFileCollection.Count; i++)
                    {
                        System.Web.HttpPostedFile hpf = httpFileCollection[i];
                        var newImageNameWithoutExtension = trimmedEmail + randomString;
                        var extension = Path.GetExtension(hpf.FileName);
                        if ((hpf.ContentType == "image/jpeg" || hpf.ContentType == "image/png") && hpf.ContentLength <= 1024000)
                        {
                            if (extension.Length <= 0)
                            {
                                extension = ".jpg";
                            }
                            if (oldImageName != null)
                            {
                                File.Delete(Generator.MasterCompanyImagePath + oldImageName);
                            }
                            var newImageName = newImageNameWithoutExtension + extension;

                            hpf.SaveAs(Generator.MasterCompanyImagePath + newImageName);

                            companyToUploadImage.MasterCompanyLogo = newImageName;
                            _context.SaveChanges();
                            Generator.IsReport = "Success";
                            Generator.Message = "Master Company logo uploaded successfully.";
                        }
                        else
                        {
                            if (hpf.ContentType != "image/jpeg" || hpf.ContentType != "image/png")
                            {
                                Generator.IsReport = "Error";
                                Generator.Message = "Only jpeg images are allowed to upload. Your selected file format is " + hpf.ContentType + ".";
                            }
                            else if (hpf.ContentLength > 1024000)
                            {
                                Generator.IsReport = "Error";
                                Generator.Message = "File size exceeded. Max file size is 1MB. Your selected file size is " + hpf.ContentLength / 1000 + ".";
                            }
                        }
                    }
                }
                else
                {
                    Generator.IsReport = "Error";
                    Generator.Message = "You can not upload more than one image.";
                }
            }
            else
            {
                Generator.IsReport = "Error";
                Generator.Message = "Company information not found.";
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
        public JsonResult GetMasterCompanyDetails()
        {
            return new JsonResult
            {
                Data = _context.MasterCompany.Select(x => x).FirstOrDefault(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult GetCompanyDetailsForWebSite()
        {
            return new JsonResult
            {
                Data = _context.MasterCompany.Select(x => x).FirstOrDefault(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult GetMasterCompanyList()
        {
            return new JsonResult
            {
                Data = _context.MasterCompany.Select(x => x).ToList(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult UpdateMasterCompany(MasterCompany masterCompany)
        {
            var companyToUpdate = _context.MasterCompany.Where(x => x.Id == masterCompany.Id).Select(x => x).FirstOrDefault();
            if (companyToUpdate != null)
            {
                companyToUpdate.MasterCompanyName = masterCompany.MasterCompanyName;
                _context.SaveChanges();
                Generator.IsReport = "Success";
                Generator.Message = "Master Company information updated successfully.";
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
        public JsonResult HasAccessToMasterCompany(Guid? adminId)
        {
            return new JsonResult
            {
                Data = _context.Admin.Where(x => x.Id == adminId).Select(x => x.Role).FirstOrDefault() == _EnumObjects.AdminRole.Super_Admin.ToString() ? true : false,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult HasAnyMasterCompany()
        {
            return new JsonResult
            {
                Data = _context.MasterCompany.Select(x => x).FirstOrDefault() != null ? true : false,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult ToggleMasterSettingsApperance(Guid? masterCompanyid)
        {
            var masterCompanyToToggle = _context.MasterCompany.Where(x => x.Id == masterCompanyid).Select(x => x).FirstOrDefault();
            if (masterCompanyToToggle != null)
            {
                if (String.IsNullOrEmpty(masterCompanyToToggle.ShowOnReport))
                {
                    masterCompanyToToggle.ShowOnReport = _EnumObjects.ShowOnReportStatus.Show.ToString();
                    _context.SaveChanges();
                    Generator.IsReport = "Success";
                    Generator.Message = "Details will be shown on  report";
                }
                else if (masterCompanyToToggle.ShowOnReport == _EnumObjects.ShowOnReportStatus.Show.ToString())
                {
                    masterCompanyToToggle.ShowOnReport = _EnumObjects.ShowOnReportStatus.Hide.ToString();
                    _context.SaveChanges();
                    Generator.IsReport = "Success";
                    Generator.Message = "Details will be hidden on  report";
                }
                else if (masterCompanyToToggle.ShowOnReport == _EnumObjects.ShowOnReportStatus.Hide.ToString())
                {
                    masterCompanyToToggle.ShowOnReport = _EnumObjects.ShowOnReportStatus.Show.ToString();
                    _context.SaveChanges();
                    Generator.IsReport = "Success";
                    Generator.Message = "Details will be shown on  report";
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
    }

    public interface IMasterCompanyServices
    {
        JsonResult CreateMasterCompany(MasterCompany masterCompany);
        JsonResult UploadMasterCompanyLogo(Guid? adminId);
        JsonResult GetMasterCompanyDetails();
        JsonResult GetMasterCompanyList();
        JsonResult UpdateMasterCompany(MasterCompany masterCompany);
        JsonResult HasAccessToMasterCompany(Guid? adminId);
        JsonResult HasAnyMasterCompany();
        JsonResult ToggleMasterSettingsApperance(Guid? masterCompanyid);
    }
}
