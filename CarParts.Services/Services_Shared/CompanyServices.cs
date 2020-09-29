using CarParts.Common;
using CarParts.Models;
using CarParts.Models.Models_Sahred;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CarParts.Services
{
    public class CompanyServices : ICompanyServices
    {
        private readonly CarPartsDbContext _context;
        public CompanyServices()
        {
            _context = new CarPartsDbContext();
        }

        public JsonResult CreateCompany(Company company)
        {
            var curretnCompanyList = _context.Company.Select(x => x).ToList().Count();
            if (curretnCompanyList == 0)
            {
                company.Id = Guid.NewGuid();
                company.AddedOn = DateTime.UtcNow;
                _context.Company.Add(company);
                _context.SaveChanges();
                Generator.IsReport = "Success";
                Generator.Message = "Company created successfully.";
            }
            else
            {
                Generator.IsReport = "Error";
                Generator.Message = "You can't create more than one company.";
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
        public JsonResult UploadCompanyLogo(Guid? companyId)
        {
            var companyToUploadImage = _context.Company.Where(x => x.Id == companyId).Select(x => x).FirstOrDefault();
            if (companyToUploadImage != null)
            {
                var oldImageName = companyToUploadImage.CompanyLogo;
                string trimmedEmail = companyToUploadImage.CompanyEmail.Replace(".", "");
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
                                File.Delete(Generator.CompanyImagePath + oldImageName);
                            }
                            var newImageName = newImageNameWithoutExtension + extension;

                            hpf.SaveAs(Generator.CompanyImagePath + newImageName);

                            companyToUploadImage.CompanyLogo = newImageName;
                            _context.SaveChanges();
                            Generator.IsReport = "Success";
                            Generator.Message = "Company logo uploaded successfully.";
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
        public JsonResult UploadCompanySignature(Guid? adminId)
        {
            var companyToUploadImage = _context.Company.Where(x => x.AdminId == adminId).Select(x => x).FirstOrDefault();
            if (companyToUploadImage != null)
            {
                var oldImageName = companyToUploadImage.CompanySignature;
                string trimmedEmail = companyToUploadImage.CompanyEmail.Replace(".", "");
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
                                File.Delete(Generator.CompanySignaturePath + oldImageName);
                            }
                            var newImageName = newImageNameWithoutExtension + extension;

                            hpf.SaveAs(Generator.CompanySignaturePath + newImageName);

                            companyToUploadImage.CompanySignature = newImageName;
                            _context.SaveChanges();
                            Generator.IsReport = "Success";
                            Generator.Message = "Company signature uploaded successfully.";
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
        public JsonResult GetCompanyDetails()
        {
            return new JsonResult
            {
                Data = _context.Company.ToList().Select(x => new
                {
                    x.Id,
                    x.AddedOn,
                    x.Address,
                    x.AdminId,
                    x.CompanyEmail,
                    CompanySignature = x.CompanySignature != null ? Generator.BaseURL() + "/Images/Company_Signature/" + x.CompanySignature : null,
                    CompanyLogo = x.CompanyLogo != null ? Generator.BaseURL() + "/Images/Company_Images/" + x.CompanyLogo : null,
                    x.CompanyName,
                    x.CompanyWebsite,
                    x.ContactPersonDesignation,
                    x.ContactPersonEmail,
                    x.ContactPersonName,
                    x.ContactPersonPhone,
                    x.Mobile,
                    x.Phone_1,
                    x.Phone_2
                }).FirstOrDefault(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult GetCompanyDetailsForWebSite()
        {
            return new JsonResult
            {
                Data = _context.Company.ToList().Select(x => new
                {
                    x.Id,
                    x.Mobile,
                    x.Phone_1,
                    x.Phone_2,
                    x.AddedOn,
                    x.Address,
                    x.AdminId,
                    x.CompanyEmail,
                    x.CompanySignature,
                    CompanyLogo = x.CompanyLogo != null ? Generator.BaseURL() + "/Images/Company_Images/" + x.CompanyLogo : null,
                    x.CompanyName,
                    x.CompanyWebsite,
                    x.ContactPersonDesignation,
                    x.ContactPersonEmail,
                    x.ContactPersonName,
                    x.ContactPersonPhone,
                    Date = DateTime.UtcNow.Year
                }).FirstOrDefault(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult GetCompanyList()
        {
            return new JsonResult
            {
                Data = _context.Company.Select(x => x).ToList(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult UpdateCompany(Company company)
        {
            var companyToUpdate = _context.Company.Where(x => x.Id == company.Id).Select(x => x).FirstOrDefault();
            if (companyToUpdate != null)
            {
                companyToUpdate.CompanyName = company.CompanyName;
                companyToUpdate.Address = company.Address;
                companyToUpdate.Phone_1 = company.Phone_1;
                companyToUpdate.Phone_2 = company.Phone_2;
                companyToUpdate.Mobile = company.Mobile;
                companyToUpdate.ContactPersonName = company.ContactPersonName;
                companyToUpdate.ContactPersonPhone = company.ContactPersonPhone;
                companyToUpdate.ContactPersonEmail = company.ContactPersonEmail;
                companyToUpdate.ContactPersonDesignation = company.ContactPersonDesignation;
                companyToUpdate.CompanyEmail = company.CompanyEmail;
                companyToUpdate.CompanyWebsite = company.CompanyWebsite;
                _context.SaveChanges();
                Generator.IsReport = "Success";
                Generator.Message = "Company information updated successfully.";
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

    public interface ICompanyServices
    {
        JsonResult CreateCompany(Company company);
        JsonResult UploadCompanyLogo(Guid? adminId);
        JsonResult UploadCompanySignature(Guid? adminId);
        JsonResult GetCompanyDetails();
        JsonResult GetCompanyDetailsForWebSite();
        JsonResult GetCompanyList();
        JsonResult UpdateCompany(Company company);
    }
}
