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
    public class MasterSubCategoryServices : IMasterSubCategoryServices
    {
        private readonly CarPartsDbContext _context;
        public MasterSubCategoryServices()
        {
            _context = new CarPartsDbContext();
        }

        public JsonResult CreateMasterSubCategory(MasterSubCategory masterSubCategory)
        {
            if (!String.IsNullOrEmpty(masterSubCategory.MasterSubCategoryName) && _context.MasterSubCategory.ToList().Where(x => x.MasterSubCategoryName.Replace(" ", "").ToLower() == masterSubCategory.MasterSubCategoryName.Replace(" ", "").ToLower() && x.MasterMainCategoryId == masterSubCategory.MasterMainCategoryId).Count() == 0)
            {
                masterSubCategory.Id = Guid.NewGuid();
                masterSubCategory.AddedOn = DateTime.UtcNow;
                masterSubCategory.LogoFileName = null;
                _context.MasterSubCategory.Add(masterSubCategory);
                _context.SaveChanges();
                Generator.IsReport = "Success";
                Generator.Message = "Record added successfully";
            }
            else
            {
                if (String.IsNullOrEmpty(masterSubCategory.MasterSubCategoryName))
                {
                    Generator.IsReport = "Error";
                    Generator.Message = "Failed to add record";
                }
                else if (_context.MasterSubCategory.ToList().Where(x => x.MasterSubCategoryName.Replace(" ", "").ToLower() == masterSubCategory.MasterSubCategoryName.Replace(" ", "").ToLower() && x.MasterMainCategoryId == masterSubCategory.MasterMainCategoryId).Count() > 0)
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
        public JsonResult UpdateMasterSubCategory(Guid? masterSubCategoryId, string value)
        {
            var masterSubCategory = _context.MasterSubCategory.Where(x => x.Id == masterSubCategoryId).Select(x => x).FirstOrDefault();
            if (masterSubCategory != null)
            {
                masterSubCategory.MasterSubCategoryName = value;
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
        public JsonResult GetMasterSubCategoryList()
        {
            return new JsonResult
            {
                Data = _context.MasterSubCategory
                                .Join(_context.MasterMainCategory,
                                x => x.MasterMainCategoryId,
                                y => y.Id,
                                (x, y) => new
                                {
                                    MasterSubCategory = x,
                                    MasterMainCategory = y
                                })
                                .GroupJoin(_context.MasterProductCategory,
                                x => x.MasterSubCategory.Id,
                                y => y.MasterSubCategoryId,
                                (x, y) => new
                                {
                                    x.MasterMainCategory,
                                    x.MasterSubCategory,
                                    MasterProductCategory = y
                                })
                                .Select(x => new
                                {
                                    x.MasterSubCategory.Id,
                                    x.MasterSubCategory.LogoFileName,
                                    x.MasterSubCategory.MasterMainCategoryId,
                                    x.MasterSubCategory.MasterSubCategoryName,
                                    x.MasterMainCategory.MasterMainCategoryName,
                                    ProductCategoryCount = x.MasterProductCategory.Count()
                                }).ToList(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult UploadMasterSubCategoryLogo(Guid? masterSubCategoryId)
        {
            var masterSubCategoryToUploadLogo = _context.MasterSubCategory.Where(x => x.Id == masterSubCategoryId).Select(x => x).FirstOrDefault();
            if (masterSubCategoryToUploadLogo != null)
            {
                var oldImageName = masterSubCategoryToUploadLogo.LogoFileName;
                string trimmedName = masterSubCategoryToUploadLogo.MasterSubCategoryName.Replace(".", "");
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
                                File.Delete(Generator.SubCategoryImagePath + oldImageName);
                            }
                            var newImageName = newImageNameWithoutExtension + extension;

                            hpf.SaveAs(Generator.SubCategoryImagePath + newImageName);

                            masterSubCategoryToUploadLogo.LogoFileName = newImageName;
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
        public JsonResult GetMasterSubCategoryDetails(Guid? masterSubCategoryId)
        {
            return new JsonResult
            {
                Data = _context.MasterSubCategory.Where(x => x.Id == masterSubCategoryId).ToList().Select(x => new
                {
                    x.Id,
                    CategoryLogo = !String.IsNullOrEmpty(x.LogoFileName) ? Generator.BaseURL() + "/Images/SubCategory_Images/" + x.LogoFileName : null,
                    x.MasterSubCategoryName,
                    x.AddedOn,
                    x.AdminId
                })
                .FirstOrDefault()
            };
        }
    }

    public interface IMasterSubCategoryServices
    {
        JsonResult CreateMasterSubCategory(MasterSubCategory masterSubCategory);
        JsonResult UpdateMasterSubCategory(Guid? masterSubCategoryId, string value);
        JsonResult GetMasterSubCategoryList();
        JsonResult UploadMasterSubCategoryLogo(Guid? masterSubCategoryId);
        JsonResult GetMasterSubCategoryDetails(Guid? masterSubCategoryId);
    }
}
