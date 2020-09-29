using System;
using System.IO;
using CarParts.Models;
using System.Linq;

namespace CarParts.Services
{
    public static class GenericServices
    {
        private static readonly CarPartsDbContext _context;

        static GenericServices()
        {
            _context = new CarPartsDbContext();
        }


        public static string GenerateCustomereCode()
        {
            var lastUserCode = _context.Customer.OrderByDescending(x => x.CreatedOn).Select(x => x.UserCode).FirstOrDefault();
            var userCode = "";
            if (lastUserCode != null)
            {
                string onlyLastPartofApplicantCode = lastUserCode.Substring(7);
                int onlyLastPart = Int32.Parse(onlyLastPartofApplicantCode);
                int newId = onlyLastPart + 1;
                userCode = "C900000" + newId;
            }
            else
            {
                userCode = "C900000" + 1;
            }
            return userCode;
        }

        public static string GenerateProductSKU(Guid? brandId)
        {
            string brandName = _context.MasterProductBrand.Where(x => x.Id == brandId).Select(x => x.MasterProductBrandName).FirstOrDefault();
            string brandShortName = brandName.Length > 3 ? brandName.Substring(0, 3).ToUpper() : brandName.ToUpper();
            var lastBrandSKU = _context.Product.Where(x => x.MasterProductBrandId == brandId).OrderByDescending(x => x.AddedOn).Select(x => x.ProductSKU).FirstOrDefault();
            var brandSKU = "";
            if (lastBrandSKU != null)
            {
                string onlyNeumericalOfBrandSKU =lastBrandSKU.Substring(brandShortName.Length);
                int onlyLatsPart = Int32.Parse(onlyNeumericalOfBrandSKU);
                int newId = onlyLatsPart + 1;
                brandSKU = brandShortName + "000000" + newId;

            }
            else
            {
                brandSKU = brandShortName + "000000" + 1;
            }
            return brandSKU;
        }
    }
}


