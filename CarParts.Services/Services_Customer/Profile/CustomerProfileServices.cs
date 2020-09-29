using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using CarParts.Common;
using CarParts.Models;
using CarParts.Models.Models_Customer;
using CarParts.Models.TempModels;
using Microsoft.AspNet.SignalR;

namespace CarParts.Services.Services_Customer.Profile
{
    public class CustomerProfileServices : ICustomerProfileServices
    {
        private readonly CarPartsDbContext _context;
        private readonly IHubContext _adminHubContext;
        public CustomerProfileServices()
        {
            _context = new CarPartsDbContext();
            _adminHubContext = GlobalHost.ConnectionManager.GetHubContext("adminHub");
        }

        public JsonResult RegisterCustomer(Customer user)
        {
            if (!String.IsNullOrEmpty(user.Email) && !String.IsNullOrEmpty(user.Phone))
            {
                var _IsEmailUsed = _context.Customer.Any(x => x.Email == user.Email);
                var _IsPhoneNumberUsed = _context.Customer.Any(x => x.Phone == user.Phone);

                if (!_IsEmailUsed && !_IsPhoneNumberUsed)
                {
                    user.Id = Guid.NewGuid();
                    user.UserCode = GenericServices.GenerateCustomereCode();
                    user.CreatedOn = DateTime.UtcNow;
                    _context.Customer.Add(user);
                    _context.SaveChanges();
                    _adminHubContext.Clients.All.clientRecievedByAdminOnRegisterApplicant();
                    Generator.IsReport = "Success";
                    Generator.Message = "Registration completed";
                }
                else
                {
                    if (_IsEmailUsed)
                    {
                        Generator.IsReport = "Error";
                        Generator.Message = "Another account is being associated with this email. Please try a different email.";
                    }
                    else if (_IsPhoneNumberUsed)
                    {
                        Generator.IsReport = "Error";
                        Generator.Message = "Another account is being associated with this phone number. Please try a different phone number.";
                    }
                }
            }
            else
            {
                if (String.IsNullOrEmpty(user.Email))
                {
                    Generator.IsReport = "Error";
                    Generator.Message = "Please enter your phone number";
                }
                else if (String.IsNullOrEmpty(user.Phone))
                {
                    Generator.IsReport = "Error";
                    Generator.Message = "Please enter your phone number";
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
        public JsonResult GetCustomerDetailsByCustomerId(Guid customerId)
        {
            return new JsonResult
            {
                Data = _context.Customer.Where(x => x.Id == customerId).ToList().Select(x => new
                {
                    x.Email,
                    x.Name,
                    x.Phone,
                    x.CreatedOn,
                    ProfilePicture = x.ProfilePicture,
                    x.Address,
                    x.Id
                }).FirstOrDefault(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult UploadProfilePictureByCustomerId(Guid customerId)
        {
            var userToUploadPhoto = _context.Customer.Where(x => x.Id == customerId).Select(x => x).FirstOrDefault();
            if (userToUploadPhoto != null)
            {
                var oldImageName = userToUploadPhoto.ProfilePicture;
                string trimmedEmail = userToUploadPhoto.Email.Replace(".", "");
                trimmedEmail = trimmedEmail.Replace("@", "");
                string randomString = Generator.GenerateRandomCodeStringByByteSize(6);

                System.Web.HttpFileCollection httpFileCollection = System.Web.HttpContext.Current.Request.Files;
                if (httpFileCollection.Count == 1)
                {
                    for (int i = 0; i < httpFileCollection.Count; i++)
                    {
                        System.Web.HttpPostedFile hpf = httpFileCollection[i];
                        var newImageNameWithoutExtension = trimmedEmail + randomString;
                        var extension = Path.GetExtension(hpf.FileName);
                        if (hpf.ContentType == "image/jpeg" && hpf.ContentLength <= 1024000)
                        {
                            if (extension.Length <= 0)
                            {
                                extension = ".jpg";
                            }
                            var newImageName = newImageNameWithoutExtension + extension;

                            if (oldImageName != null)
                            {
                                File.Delete(Generator.CustomerPhotoPath + oldImageName);
                            }

                            hpf.SaveAs(Generator.CustomerPhotoPath + newImageName);
                            userToUploadPhoto.ProfilePicture = newImageName;
                            _context.SaveChanges();
                            Generator.IsReport = "Success";
                            Generator.Message = "Profile picture uploaded successfully";
                        }
                        else
                        {
                            if (hpf.ContentType != "image/jpeg")
                            {
                                Generator.IsReport = "Error";
                                Generator.Message = "Only jpeg images are allowed to upload. Your selected file format is " + hpf.ContentType;
                            }
                            else if (hpf.ContentLength > 1024000)
                            {
                                Generator.IsReport = "Error";
                                Generator.Message = "File size exceeded. Max file size is 1MB. Your selected file size is " + hpf.ContentLength / 1000;
                            }
                        }
                    }
                }
                else
                {
                    Generator.IsReport = "Error";
                    Generator.Message = "You can not upload more than on e image";
                }
            }
            else
            {
                Generator.IsReport = "Error";
                Generator.Message = "Record not found";
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
        public JsonResult UpdateCustomerProfile(Customer user)
        {
            var _userToUpdate = _context.Customer.Where(x => x.Id == user.Id).Select(x => x).FirstOrDefault();
            if (_userToUpdate != null)
            {
                _userToUpdate.Name = user.Name;
                _userToUpdate.Address = user.Address;
                _context.SaveChanges();
                //_adminHubContext.Clients.All.clientRecievedByAdminOnUpdateApplicantProfileAt_user_State(_userToUpdate.Id, _userToUpdate.Name);
                //_adminHubContext.Clients.All.clientRecievedByAdminOnUpdateApplicantProfileAt_query_State(_userToUpdate.Id);
                Generator.IsReport = "Success";
                Generator.Message = "Profile updated successfully";
            }
            else
            {
                Generator.IsReport = "Error";
                Generator.Message = "Record not found";
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

    public interface ICustomerProfileServices
    {
        JsonResult RegisterCustomer(Customer user);
        JsonResult GetCustomerDetailsByCustomerId(Guid customerId);
        JsonResult UploadProfilePictureByCustomerId(Guid customerId);
        JsonResult UpdateCustomerProfile(Customer user);
    }
}
