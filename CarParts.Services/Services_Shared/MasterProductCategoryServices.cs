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
    public class MasterProductCategoryServices : IMasterProductCategoryServices
    {
        private readonly CarPartsDbContext _context;
        public MasterProductCategoryServices()
        {
            _context = new CarPartsDbContext();
        }

        public JsonResult CreateMasterProductCategory(MasterProductCategory masterProductCategory)
        {
            if (!String.IsNullOrEmpty(masterProductCategory.MasterProductCategoryName) && !_context.MasterProductCategory.ToList().Any(x => x.MasterProductCategoryName.Replace(" ", "").ToLower() == masterProductCategory.MasterProductCategoryName.Replace(" ", "").ToLower()))
            {
                masterProductCategory.Id = Guid.NewGuid();
                masterProductCategory.AddedOn = DateTime.UtcNow;
                masterProductCategory.LogoFileName = null;
                _context.MasterProductCategory.Add(masterProductCategory);
                _context.SaveChanges();
                Generator.IsReport = "Success";
                Generator.Message = "Record added successfully";
            }
            else
            {
                if (String.IsNullOrEmpty(masterProductCategory.MasterProductCategoryName))
                {
                    Generator.IsReport = "Error";
                    Generator.Message = "Failed to add record";
                }
                else if (_context.MasterProductCategory.ToList().Any(x => x.MasterProductCategoryName.ToLower() == masterProductCategory.MasterProductCategoryName.ToLower()))
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
        public JsonResult UpdateMasterProductCategory(Guid? masterProductCategoryId, string value)
        {
            var masterProductCategory = _context.MasterProductCategory.Where(x => x.Id == masterProductCategoryId).Select(x => x).FirstOrDefault();
            if (masterProductCategory != null)
            {
                masterProductCategory.MasterProductCategoryName = value;
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
        public JsonResult GetMasterProductCategoryList()
        {
            return new JsonResult
            {
                Data = _context.MasterProductCategory.Select(x => x).ToList(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult UploadMasterProductCategoryLogo(Guid? masterProductCategoryId)
        {
            var masterProductCategoryToUploadLogo = _context.MasterProductCategory.Where(x => x.Id == masterProductCategoryId).Select(x => x).FirstOrDefault();
            if (masterProductCategoryToUploadLogo != null)
            {
                var oldImageName = masterProductCategoryToUploadLogo.LogoFileName;
                string trimmedName = masterProductCategoryToUploadLogo.MasterProductCategoryName.Replace(".", "");
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
                                File.Delete(Generator.ProductCategoryImagePath + oldImageName);
                            }
                            var newImageName = newImageNameWithoutExtension + extension;

                            hpf.SaveAs(Generator.ProductCategoryImagePath + newImageName);

                            masterProductCategoryToUploadLogo.LogoFileName = newImageName;
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
        public JsonResult GetMasterProductCategoryDetails(Guid? masterProductCategoryId)
        {
            return new JsonResult
            {
                Data = _context.MasterProductCategory.Where(x => x.Id == masterProductCategoryId).ToList().Select(x => new
                {
                    x.Id,
                    CategoryLogo = !String.IsNullOrEmpty(x.LogoFileName) ? Generator.BaseURL() + "/Images/ProductCategory_Images/" + x.LogoFileName : null,
                    x.MasterProductCategoryName,
                    x.AddedOn,
                    x.AdminId
                })
                .FirstOrDefault()
            };
        }
    }

    public interface IMasterProductCategoryServices
    {
        JsonResult CreateMasterProductCategory(MasterProductCategory masterProductCategory);
        JsonResult UpdateMasterProductCategory(Guid? masterProductCategoryId, string value);
        JsonResult GetMasterProductCategoryList();
        JsonResult UploadMasterProductCategoryLogo(Guid? masterProductCategoryId);
        JsonResult GetMasterProductCategoryDetails(Guid? masterProductCategoryId);
    }
}
