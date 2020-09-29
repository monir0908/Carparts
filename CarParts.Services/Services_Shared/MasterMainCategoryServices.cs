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
    public class MasterMainCategoryServices : IMasterMainCategoryServices
    {
        private readonly CarPartsDbContext _context;
        public MasterMainCategoryServices()
        {
            _context = new CarPartsDbContext();
        }

        public JsonResult CreateMasterMainCategory(MasterMainCategory masterMainCategory)
        {
            if (!String.IsNullOrEmpty(masterMainCategory.MasterMainCategoryName) && !_context.MasterMainCategory.ToList().Any(x => x.MasterMainCategoryName.Replace(" ", "").ToLower() == masterMainCategory.MasterMainCategoryName.Replace(" ", "").ToLower()))
            {
                masterMainCategory.Id = Guid.NewGuid();
                masterMainCategory.AddedOn = DateTime.UtcNow;
                masterMainCategory.LogoFileName = null;
                _context.MasterMainCategory.Add(masterMainCategory);
                _context.SaveChanges();
                Generator.IsReport = "Success";
                Generator.Message = "Record added successfully";
            }
            else
            {
                if (String.IsNullOrEmpty(masterMainCategory.MasterMainCategoryName))
                {
                    Generator.IsReport = "Error";
                    Generator.Message = "Failed to add record";
                }
                else if (_context.MasterMainCategory.ToList().Any(x => x.MasterMainCategoryName.ToLower() == masterMainCategory.MasterMainCategoryName.ToLower()))
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
        public JsonResult UpdateMasterMainCategory(Guid? masterMainCategoryId, string value)
        {
            var masterMainCategory = _context.MasterMainCategory.Where(x => x.Id == masterMainCategoryId).Select(x => x).FirstOrDefault();
            if (masterMainCategory != null)
            {
                masterMainCategory.MasterMainCategoryName = value;
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
        public JsonResult GetMasterMainCategoryList()
        {
            return new JsonResult
            {
                Data = _context.MasterMainCategory
                                .GroupJoin(_context.MasterSubCategory,
                                x => x.Id,
                                y => y.MasterMainCategoryId,
                                (x, y) => new
                                {
                                    MasterMainCategory = x,
                                    MasterSubCategory = y
                                })
                                .Select(x => new
                                {
                                    x.MasterMainCategory.Id,
                                    x.MasterMainCategory.LogoFileName,
                                    x.MasterMainCategory.MasterMainCategoryName,
                                    x.MasterMainCategory.AddedOn,
                                    x.MasterMainCategory.AdminId,
                                    SubCategoryCount = x.MasterSubCategory.Count()
                                }).ToList(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult GetMasterMainCategoryDetails(Guid? masterMainCategoryId)
        {
            return new JsonResult
            {
                Data = _context.MasterMainCategory.Where(x => x.Id == masterMainCategoryId).ToList().Select(x => new
                {
                    x.Id,
                    CategoryLogo = !String.IsNullOrEmpty(x.LogoFileName) ? Generator.BaseURL() + "/Images/MainCategory_Images/" + x.LogoFileName : null,
                    x.MasterMainCategoryName,
                    x.AddedOn,
                    x.AdminId
                })
                .FirstOrDefault()
            };
        }
        public JsonResult UploadMasterMainCategoryLogo(Guid? masterMainCategoryId)
        {
            var masterMainCategoryToUploadLogo = _context.MasterMainCategory.Where(x => x.Id == masterMainCategoryId).Select(x => x).FirstOrDefault();
            if (masterMainCategoryToUploadLogo != null)
            {
                var oldImageName = masterMainCategoryToUploadLogo.LogoFileName;
                string trimmedName = masterMainCategoryToUploadLogo.MasterMainCategoryName.Replace(".", "");
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
                                File.Delete(Generator.MainCategoryImagePath + oldImageName);
                            }
                            var newImageName = newImageNameWithoutExtension + extension;

                            hpf.SaveAs(Generator.MainCategoryImagePath + newImageName);

                            masterMainCategoryToUploadLogo.LogoFileName = newImageName;
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
    }

    public interface IMasterMainCategoryServices
    {
        JsonResult CreateMasterMainCategory(MasterMainCategory masterMainCategory);
        JsonResult UpdateMasterMainCategory(Guid? masterMainCategoryId, string value);
        JsonResult GetMasterMainCategoryList();
        JsonResult GetMasterMainCategoryDetails(Guid? masterMainCategoryId);
        JsonResult UploadMasterMainCategoryLogo(Guid? masterMainCategoryId);
    }
}
