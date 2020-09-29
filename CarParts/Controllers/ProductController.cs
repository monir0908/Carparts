using CarParts.Models.Models_Shared;
using CarParts.Models.TempModels;
using CarParts.Services.Services_Shared;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CarParts.Controllers
{
    [RoutePrefix("Api/Product")]
    public class ProductController : ApiController
    {
        private readonly IProductServices _services;
        public ProductController()
        {
            _services = new ProductServices();
        }

        [Route("CreateProduct")]
        [HttpPost]
        public IHttpActionResult CreateProduct(JObject jObject)
        {
            dynamic jsonData = jObject;
            var JsonProduct = jsonData.Product;
            var product = JsonProduct.ToObject<Product>();

            var JsonTempVehicleFitmentList = jsonData.TempVehicleFitmentList;
            var tempVehicleFitmentList = JsonTempVehicleFitmentList.ToObject<List<TempVehicleFitment>>();

            var JsonTempMasterProductSpecificationLabelList = jsonData.TempMasterProductSpecificationLabelList;
            var tempMasterProductSpecificationLabelList = JsonTempMasterProductSpecificationLabelList.ToObject<List<TempMasterProductSpecificationLabel>>();


            return Ok(_services.CreateProduct(product, tempVehicleFitmentList, tempMasterProductSpecificationLabelList).Data);
        }

        [Route("UpdateProduct")]
        [HttpPost]
        public IHttpActionResult UpdateProduct(JObject jObject)
        {
            dynamic jsonData = jObject;
            var JsonProduct = jsonData.Product;
            var product = JsonProduct.ToObject<Product>();

            var JsonTempVehicleFitmentList = jsonData.TempVehicleFitmentList;
            var tempVehicleFitmentList = JsonTempVehicleFitmentList.ToObject<List<TempVehicleFitment>>();

            var JsonTempMasterProductSpecificationLabelList = jsonData.TempMasterProductSpecificationLabelList;
            var tempMasterProductSpecificationLabelList = JsonTempMasterProductSpecificationLabelList.ToObject<List<TempMasterProductSpecificationLabel>>();


            return Ok(_services.UpdateProduct(product, tempVehicleFitmentList, tempMasterProductSpecificationLabelList).Data);
        }

        [Route("UpdateProductStock/{productId}/{stock}/{adminId}")]
        [HttpPost]
        public IHttpActionResult UpdateProductStock(Guid? productId, double stock, Guid? adminId)
        {
            return Ok(_services.UpdateProductStock(productId, stock, adminId).Data);
        }

        [Route("UpdateDiscountPercentageByProductId/{productId}/{value}")]
        [HttpPost]
        public IHttpActionResult UpdateDiscountPercentageByProductId(Guid? productId, double value)
        {
            return Ok(_services.UpdateDiscountPercentageByProductId(productId, value).Data);
        }

        [Route("ToggleDiscountByProductId/{productId}")]
        [HttpPost]
        public IHttpActionResult ToggleDiscountByProductId(Guid? productId)
        {
            return Ok(_services.ToggleDiscountByProductId(productId).Data);
        }

        [Route("UpdateProductPrice")]
        [HttpPost]
        public IHttpActionResult UpdateProductPrice(ProductPrice productPrice)
        {
            return Ok(_services.UpdateProductPrice(productPrice).Data);
        }

        [Route("UpdateProductVehicleFitmentInformation")]
        [HttpPost]
        public IHttpActionResult UpdateProductVehicleFitmentInformation(JObject jObject)
        {
            dynamic jsonData = jObject;
            var JsonVehicleFitmentList = jsonData.VehicleFitmentList;
            var vehicleFitmentList = JsonVehicleFitmentList.ToObject<List<VehicleFitment>>();

            var JsonAdminId = jsonData.AdminId;
            var adminId = JsonAdminId.ToObject<Guid?>();


            return Ok(_services.UpdateProductVehicleFitmentInformation(vehicleFitmentList, adminId).Data);
        }

        [Route("UploadProductImage/{productId}/{adminId}")]
        [HttpPost]
        public IHttpActionResult UploadProductImage(Guid? productId, Guid? adminId)
        {
            return Ok(_services.UploadProductImage(productId, adminId).Data);
        }

        [Route("GetMasterSubCategoryList/{masterMainCategoryId}")]
        [HttpGet]
        public IHttpActionResult GetMasterSubCategoryList(Guid? masterMainCategoryId)
        {
            return Ok(_services.GetMasterSubCategoryList(masterMainCategoryId).Data);
        }

        [Route("GetMasterProductCategoryList/{masterMainCategoryId}/{masterSubCategoryId}")]
        [HttpGet]
        public IHttpActionResult GetMasterProductCategoryList(Guid? masterMainCategoryId, Guid? masterSubCategoryId)
        {
            return Ok(_services.GetMasterProductCategoryList(masterMainCategoryId, masterSubCategoryId).Data);
        }

        [Route("GenerateProductSKU/{masterProductBrandId}")]
        [HttpGet]
        public IHttpActionResult GenerateProductSKU(Guid? masterProductBrandId)
        {
            return Ok(_services.GenerateProductSKU(masterProductBrandId).Data);
        }

        [Route("GetProductHierarchy")]
        [HttpGet]
        public IHttpActionResult GetProductHierarchy()
        {
            return Ok(_services.GetProductHierarchy().Data);
        }

        [Route("GetFilteredProductList")]
        [HttpPost]
        public IHttpActionResult GetFilteredProductList(JArray jArray)
        {
            dynamic jsonData = jArray;
            var tempProductHierarchy = jsonData.ToObject<List<TempProductHierarchy>>();
            return Ok(_services.GetFilteredProductList(tempProductHierarchy).Data);
        }

        [Route("GetProductList")]
        [HttpGet]
        public IHttpActionResult GetProductList()
        {
            return Ok(_services.GetProductList().Data);
        }

        [Route("GetProductLabel")]
        [HttpGet]
        public IHttpActionResult GetProductLabel()
        {
            return Ok(_services.GetProductLabel().Data);
        }

        [Route("GetVehicleFitmentInfoList")]
        [HttpGet]
        public IHttpActionResult GetVehicleFitmentInfoList()
        {
            return Ok(_services.GetVehicleFitmentInfoList().Data);
        }

        [Route("GetproductDetailsForEdit/{productId}")]
        [HttpGet]
        public IHttpActionResult GetproductDetailsForEdit(Guid? productId)
        {
            return Ok(_services.GetproductDetailsForEdit(productId).Data);
        }

        [Route("DeleteImageByImageId/{productImageId}")]
        [HttpPost]
        public IHttpActionResult DeleteImageByImageId(Guid? productImageId)
        {
            return Ok(_services.DeleteImageByImageId(productImageId).Data);
        }

        [Route("SetAsProductHeaderImage/{productImageId}")]
        [HttpPost]
        public IHttpActionResult SetAsProductHeaderImage(Guid? productImageId)
        {
            return Ok(_services.SetAsProductHeaderImage(productImageId).Data);
        }

        [Route("DownloadFileByName/{filename}")]
        [HttpGet]
        public IHttpActionResult DownloadFileByName(string filename)
        {
            return Ok(_services.DownloadFileByName(filename).Data);
        }

        [Route("GetProductStockByProductId/{productId}")]
        [HttpGet]
        public IHttpActionResult GetProductStockByProductId(Guid? productId)
        {
            return Ok(_services.GetProductStockByProductId(productId).Data);
        }

        [Route("GetProductPriceByProductId/{productId}")]
        [HttpGet]
        public IHttpActionResult GetProductPriceByProductId(Guid? productId)
        {
            return Ok(_services.GetProductPriceByProductId(productId).Data);
        }

        [Route("GetProductUnitPriceByDateByProductId/{productId}")]
        [HttpGet]
        public IHttpActionResult GetProductUnitPriceByDateByProductId(Guid? productId)
        {
            return Ok(_services.GetProductUnitPriceByDateByProductId(productId).Data);
        }

        [Route("GetProductSalesPriceByDateByProductId/{productId}")]
        [HttpGet]
        public IHttpActionResult GetProductSalesPriceByDateByProductId(Guid? productId)
        {
            return Ok(_services.GetProductSalesPriceByDateByProductId(productId).Data);
        }

        [Route("GetPopularBrand")]
        [HttpGet]
        public IHttpActionResult GetPopularBrand()
        {
            return Ok(_services.GetPopularBrand().Data);
        }

        [Route("GetPopularMaker")]
        [HttpGet]
        public IHttpActionResult GetPopularMaker()
        {
            return Ok(_services.GetPopularMaker().Data);
        }

        [Route("GetAllSubCategoryListByCategoryId")]
        [HttpPost]
        public IHttpActionResult GetAllSubCategoryListByCategoryId(JObject jObject)
        {
            dynamic jsonData = jObject;
            var JsonCategoryId = jsonData.CategoryId;
            var categoryId = JsonCategoryId.ToObject<Guid?>();

            var JsonTempVehicleFilter = jsonData.TempVehicleFilter;
            var tempVehicleFilter = JsonTempVehicleFilter.ToObject<TempVehicleFilter>();


            return Ok(_services.GetAllSubCategoryListByCategoryId((Guid?)categoryId, tempVehicleFilter).Data);
        }

        [Route("GetAllProductCategoryListBySubCategoryId")]
        [HttpPost]
        public IHttpActionResult GetAllProductCategoryListBySubCategoryId(JObject jObject)
        {
            dynamic jsonData = jObject;
            var JsonSubCategoryId = jsonData.SubCategoryId;
            var subCategoryId = JsonSubCategoryId.ToObject<Guid?>();

            var JsonTempVehicleFilter = jsonData.TempVehicleFilter;
            var tempVehicleFilter = JsonTempVehicleFilter.ToObject<TempVehicleFilter>();


            return Ok(_services.GetAllProductCategoryListBySubCategoryId((Guid?)subCategoryId, tempVehicleFilter).Data);
        }

        [Route("GetProductListByProductCategory")]
        [HttpPost]
        public IHttpActionResult GetProductListByProductCategory(JObject jObject)
        {
            dynamic jsonData = jObject;
            var JsonProductCategoryId = jsonData.ProductCategoryId;
            var productCategoryId = JsonProductCategoryId.ToObject<Guid?>();

            var JsonTempVehicleFilter = jsonData.TempVehicleFilter;
            var tempVehicleFilter = JsonTempVehicleFilter.ToObject<TempVehicleFilter>();


            return Ok(_services.GetProductListByProductCategory((Guid?)productCategoryId, tempVehicleFilter).Data);
        }

    }
}
