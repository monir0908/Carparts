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
    public class MasterProductBrandServices : IMasterProductBrandServices
    {
        private readonly CarPartsDbContext _context;
        public MasterProductBrandServices()
        {
            _context = new CarPartsDbContext();
        }

        public JsonResult CreateMasterProductBrand(MasterProductBrand masterProductBrand)
        {
            if (!String.IsNullOrEmpty(masterProductBrand.MasterProductBrandName) && !_context.MasterProductBrand.ToList().Any(x => x.MasterProductBrandName.Replace(" ", "").ToLower() == masterProductBrand.MasterProductBrandName.Replace(" ", "").ToLower()))
            {
                masterProductBrand.Id = Guid.NewGuid();
                masterProductBrand.AddedOn = DateTime.UtcNow;
                masterProductBrand.LogoFileName = null;
                _context.MasterProductBrand.Add(masterProductBrand);
                _context.SaveChanges();
                Generator.IsReport = "Success";
                Generator.Message = "Record added successfully";
            }
            else
            {
                if (String.IsNullOrEmpty(masterProductBrand.MasterProductBrandName))
                {
                    Generator.IsReport = "Error";
                    Generator.Message = "Failed to add record";
                }
                else if (_context.MasterProductBrand.ToList().Any(x => x.MasterProductBrandName.ToLower() == masterProductBrand.MasterProductBrandName.ToLower()))
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
        public JsonResult UpdateMasterProductBrand(Guid? masterProductBrandId, string value)
        {
            var masterProductBrand = _context.MasterProductBrand.Where(x => x.Id == masterProductBrandId).Select(x => x).FirstOrDefault();
            if (masterProductBrand != null)
            {
                masterProductBrand.MasterProductBrandName = value;
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
        public JsonResult GetMasterProductBrandList()
        {
            return new JsonResult
            {
                Data = _context.MasterProductBrand.Select(x => x).ToList(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult UploadMasterProductBrandLogo(Guid? masterProductBrandId)
        {
            var masterProductBrandToUploadLogo = _context.MasterProductBrand.Where(x => x.Id == masterProductBrandId).Select(x => x).FirstOrDefault();
            if (masterProductBrandToUploadLogo != null)
            {
                var oldImageName = masterProductBrandToUploadLogo.LogoFileName;
                string trimmedName = masterProductBrandToUploadLogo.MasterProductBrandName.Replace(".", "");
                string randomString = Guid.NewGuid().ToString();

                System.Web.HttpFileCollection httpFileCollection = System.Web.HttpContext.Current.Request.Files;
                if (httpFileCollection.Count == 1)
                {
                    for (int i = 0; i < httpFileCollection.Count; i++)
                    {
                        System.Web.HttpPostedFile hpf = httpFileCollection[i];
                        var newImageNameWithoutExtension = trimmedName + randomString;
                        var extension = Path.GetExtension(hpf.FileName);
                        if ((hpf.ContentType == "image/jpeg" || hpf.ContentType == "image/png") && hpf.ContentLength <= 1024000)
                        {
                            if (extension.Length <= 0)
                            {
                                extension = ".jpg";
                            }
                            if (oldImageName != null)
                            {
                                File.Delete(Generator.ProductBrandImagePath + oldImageName);
                            }
                            var newImageName = newImageNameWithoutExtension + extension;

                            hpf.SaveAs(Generator.ProductBrandImagePath + newImageName);

                            masterProductBrandToUploadLogo.LogoFileName = newImageName;
                            _context.SaveChanges();
                            Generator.IsReport = "Success";
                            Generator.Message = "Logo uploaded successfully.";
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
        public JsonResult GetMasterProductBrandDetails(Guid? masterProductBrandId)
        {
            return new JsonResult
            {
                Data = _context.MasterProductBrand.Where(x => x.Id == masterProductBrandId).ToList().Select(x => new
                {
                    x.Id,
                    CategoryLogo = !String.IsNullOrEmpty(x.LogoFileName) ? Generator.BaseURL() + "/Images/ProductBrand_Images/" + x.LogoFileName : null,
                    x.MasterProductBrandName,
                    x.AddedOn,
                    x.AdminId
                })
                .FirstOrDefault()
            };
        }
    }

    public interface IMasterProductBrandServices
    {
        JsonResult CreateMasterProductBrand(MasterProductBrand masterProductBrand);
        JsonResult UpdateMasterProductBrand(Guid? masterProductBrandId, string value);
        JsonResult GetMasterProductBrandList();
        JsonResult UploadMasterProductBrandLogo(Guid? masterProductBrandId);
        JsonResult GetMasterProductBrandDetails(Guid? masterProductBrandId);
    }
}
