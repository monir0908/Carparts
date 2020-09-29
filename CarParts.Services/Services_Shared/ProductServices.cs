using CarParts.Common;
using CarParts.Models;
using CarParts.Models.Common;
using CarParts.Services;
using CarParts.Models.Models_Shared;
using CarParts.Models.TempModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CarParts.Services.Services_Shared
{
    public class ProductServices : IProductServices
    {
        private readonly CarPartsDbContext _context;
        public ProductServices()
        {
            _context = new CarPartsDbContext();
        }

        public JsonResult CreateProduct(Product product, List<TempVehicleFitment> tempVehicleFitmentList, List<TempMasterProductSpecificationLabel> tempMasterProductSpecificationLabelList)
        {
            if (product.MasterMainCategoryId != null && product.MasterMainCategoryId != Guid.Empty && product.MasterSubCategoryId != null && product.MasterSubCategoryId != Guid.Empty && product.MasterProductCategoryId != null && product.MasterProductCategoryId != Guid.Empty)
            {
                // Add Product
                product.Id = Guid.NewGuid();
                product.AddedOn = DateTime.UtcNow;
                product.ProductSKU = GenericServices.GenerateProductSKU(product.MasterProductBrandId);
                product.IsDiscountActive = false;
                product.DiscountPercentage = 0;
                _context.Product.Add(product);

                // Add Product Specification
                foreach (var item in tempMasterProductSpecificationLabelList)
                {
                    _context.ProductSpecificationLabelDetails.Add(new ProductSpecificationLabelDetails
                    {
                        AddedOn = DateTime.UtcNow,
                        AdminId = product.AdminId,
                        MasterProductSpecificationLabelId = item.Id,
                        ProductId = product.Id,
                        Value = item.Model,
                        IsHighlightedFeature = item.IsHighlightedFeature
                    });
                }

                // Add Product Vehicle Fitment Information
                foreach (var item in tempVehicleFitmentList)
                {
                    _context.VehicleFitment.Add(new VehicleFitment
                    {
                        Id = Guid.NewGuid(),
                        AddedOn = DateTime.UtcNow,
                        AdminId = product.AdminId,
                        FitmentInfo = item.FitmentInfo,
                        ProductId = product.Id,
                        VehicleId = item.VehicleId
                    });
                }

                // Create New Product Stock
                _context.ProductStock.Add(new ProductStock
                {
                    Id = Guid.NewGuid(),
                    AdminId = product.AdminId,
                    AddedOn = DateTime.UtcNow,
                    CurrentStock = 0,
                    ProductId = product.Id
                });

                // Create New Product Price
                _context.ProductPrice.Add(new ProductPrice
                {
                    Id = Guid.NewGuid(),
                    ProductId = product.Id,
                    AddedOn = DateTime.UtcNow,
                    AdminId = product.AdminId,
                    CurrentSalesPrice = 0,
                    CurrentUnitPrice = 0
                });

                _context.SaveChanges();
                Generator.IsReport = "Success";
                Generator.Message = "Product added successfully";

            }
            else
            {
                Generator.IsReport = "Error";
                Generator.Message = "Product record addition failed as all necessary information is not provided";
            }
            return new JsonResult
            {
                Data = new
                {
                    Generator.IsReport,
                    Generator.Message,
                    ProductIdMax = product.Id
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult UpdateProduct(Product product, List<TempVehicleFitment> tempVehicleFitmentList, List<TempMasterProductSpecificationLabel> tempMasterProductSpecificationLabelList)
        {
            var productToUpdate = _context.Product.Where(x => x.Id == product.Id).Select(x => x).FirstOrDefault();
            if (productToUpdate.MasterMainCategoryId != null && productToUpdate.MasterMainCategoryId != Guid.Empty && productToUpdate.MasterSubCategoryId != null && productToUpdate.MasterSubCategoryId != Guid.Empty && productToUpdate.MasterProductCategoryId != null && productToUpdate.MasterProductCategoryId != Guid.Empty)
            {
                // Update Product
                productToUpdate.Name = product.Name;
                productToUpdate.Description = product.Description;
                productToUpdate.ModifiedOn = DateTime.UtcNow;
                productToUpdate.MasterMainCategoryId = product.MasterMainCategoryId;
                productToUpdate.MasterProductBrandId = product.MasterProductBrandId;
                productToUpdate.MasterProductCategoryId = product.MasterProductCategoryId;
                productToUpdate.MasterSubCategoryId = product.MasterSubCategoryId;

                // Update Product Specification
                var productSpecificationLabelDetailsListToDelete = _context.ProductSpecificationLabelDetails.Where(x => x.ProductId == product.Id).Select(x => x).ToList();
                _context.ProductSpecificationLabelDetails.RemoveRange(productSpecificationLabelDetailsListToDelete);
                foreach (var item in tempMasterProductSpecificationLabelList)
                {
                    _context.ProductSpecificationLabelDetails.Add(new ProductSpecificationLabelDetails
                    {
                        AddedOn = DateTime.UtcNow,
                        AdminId = product.AdminId,
                        MasterProductSpecificationLabelId = item.Id,
                        ProductId = product.Id,
                        Value = item.Model,
                        IsHighlightedFeature = item.IsHighlightedFeature
                    });
                }

                // Update Product Vehicle Fitment Information
                var vehicleFitmentListToDelete = _context.VehicleFitment.Where(x => x.ProductId == product.Id).Select(x => x).ToList();
                _context.VehicleFitment.RemoveRange(vehicleFitmentListToDelete);
                foreach (var item in tempVehicleFitmentList)
                {
                    _context.VehicleFitment.Add(new VehicleFitment
                    {
                        Id = Guid.NewGuid(),
                        AddedOn = DateTime.UtcNow,
                        AdminId = product.AdminId,
                        FitmentInfo = item.FitmentInfo,
                        ProductId = product.Id,
                        VehicleId = item.VehicleId
                    });
                }
                _context.SaveChanges();
                Generator.IsReport = "Success";
                Generator.Message = "Product updated successfully";

            }
            else
            {
                Generator.IsReport = "Error";
                Generator.Message = "Product record update failed as all necessary information is not provided";
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
        public JsonResult UpdateDiscountPercentageByProductId(Guid? productId, double value)
        {
            var productToUpdate = _context.Product.Where(x => x.Id == productId).Select(x => x).FirstOrDefault();
            if (productToUpdate != null)
            {
                productToUpdate.DiscountPercentage = value;
                if (value <= 0)
                {
                    productToUpdate.IsDiscountActive = false;
                    _context.SaveChanges();
                    Generator.IsReport = "Success";
                    Generator.Message = "Product Discount percentage updated successfully. As discount percentage is set to zero thus discount status has been switched to disabled.";
                }
                else
                {
                    _context.SaveChanges();
                    Generator.IsReport = "Success";
                    Generator.Message = "Product Discount percentage updated successfully.";
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
        public JsonResult ToggleDiscountByProductId(Guid? productId)
        {
            var productToToggle = _context.Product.Where(x => x.Id == productId).Select(x => x).FirstOrDefault();
            if (productToToggle != null)
            {
                if (productToToggle.IsDiscountActive)
                {
                    productToToggle.IsDiscountActive = false;
                    _context.SaveChanges();
                    Generator.IsReport = "Success";
                    Generator.Message = "Discount de-activated successfully for this product.";
                }
                else
                {
                    if (productToToggle.DiscountPercentage > 0)
                    {
                        productToToggle.IsDiscountActive = true;
                        _context.SaveChanges();
                        Generator.IsReport = "Success";
                        Generator.Message = "Discount activated successfully for this product.";
                    }
                    else
                    {
                        Generator.IsReport = "Warning";
                        Generator.Message = "Please update discount percentage before you can toggle discount status";
                    }
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
        public JsonResult GetproductDetailsForEdit(Guid? productId)
        {
            var product = _context.Product.Where(x => x.Id == productId).Select(x => x).FirstOrDefault();
            var productSpecificationLabelList = _context.ProductSpecificationLabelDetails.Where(x => x.ProductId == productId)
                                                        .Join(_context.MasterProductSpecificationLabel,
                                                        x => x.MasterProductSpecificationLabelId,
                                                        y => y.Id,
                                                        (x, y) => new
                                                        {
                                                            ProductSpecificationLabelDetails = x,
                                                            MasterProductSpecificationLabel = y
                                                        })
                                                        .Select(x => new
                                                        {
                                                            x.MasterProductSpecificationLabel,
                                                            x.ProductSpecificationLabelDetails
                                                        })
                                                        .ToList();
            List<TempMasterProductSpecificationLabel> tempMasterProductSpecificationLabelList = new List<TempMasterProductSpecificationLabel>();
            foreach (var item in productSpecificationLabelList)
            {
                tempMasterProductSpecificationLabelList.Add(new TempMasterProductSpecificationLabel
                {
                    AddedOn = item.ProductSpecificationLabelDetails.AddedOn,
                    AdminId = item.ProductSpecificationLabelDetails.AdminId,
                    Id = item.MasterProductSpecificationLabel.Id,
                    IsCategorical = item.MasterProductSpecificationLabel.IsCategorical,
                    Label = item.MasterProductSpecificationLabel.Label,
                    LabelName = Generator.RandomStringByLength(10),
                    Model = item.ProductSpecificationLabelDetails.Value,
                    IsHighlightedFeature = item.ProductSpecificationLabelDetails.IsHighlightedFeature,
                    IsHighlightedFeatureId = Generator.RandomStringByLength(10)
                });
            }
            var vehicleFitmentList = _context.VehicleFitment.Where(x => x.ProductId == productId)
                                            .Join(_context.Vehicle,
                                            x => x.VehicleId,
                                            y => y.Id,
                                            (x, y) => new
                                            {
                                                VehicleFitment = x,
                                                Vehicle = y
                                            })
                                            .Join(_context.MasterVehicleYear,
                                            x => x.Vehicle.MasterVehicleYearId,
                                            y => y.Id,
                                            (x, y) => new
                                            {
                                                x.Vehicle,
                                                x.VehicleFitment,
                                                MasterVehicleYear = y
                                            })
                                            .Join(_context.MasterVehicleMaker,
                                            x => x.Vehicle.MasterVehicleMakerId,
                                            y => y.Id,
                                            (x, y) => new
                                            {
                                                x.Vehicle,
                                                x.VehicleFitment,
                                                x.MasterVehicleYear,
                                                MasterVehicleMaker = y
                                            })
                                            .Join(_context.MasterVehicleModel,
                                            x => x.Vehicle.MasterVehicleModelId,
                                            y => y.Id,
                                            (x, y) => new
                                            {
                                                x.Vehicle,
                                                x.VehicleFitment,
                                                x.MasterVehicleYear,
                                                x.MasterVehicleMaker,
                                                MasterVehicleModel = y
                                            })
                                            .Join(_context.MasterVehicleSubModel,
                                            x => x.Vehicle.MasterVehicleSubModelId,
                                            y => y.Id,
                                            (x, y) => new
                                            {
                                                x.Vehicle,
                                                x.VehicleFitment,
                                                x.MasterVehicleYear,
                                                x.MasterVehicleMaker,
                                                x.MasterVehicleModel,
                                                MasterVehicleSubModel = y
                                            })
                                            .Join(_context.MasterVehicleEngine,
                                            x => x.Vehicle.MasterVehicleEngineId,
                                            y => y.Id,
                                            (x, y) => new
                                            {
                                                x.Vehicle,
                                                x.VehicleFitment,
                                                x.MasterVehicleYear,
                                                x.MasterVehicleMaker,
                                                x.MasterVehicleModel,
                                                x.MasterVehicleSubModel,
                                                MasterVehicleEngine = y
                                            })
                                            .Select(x => new
                                            {
                                                Year = x.MasterVehicleYear.Year.ToString(),
                                                x.MasterVehicleMaker.MakerName,
                                                x.MasterVehicleModel.ModelName,
                                                x.MasterVehicleSubModel.SubModelName,
                                                x.MasterVehicleEngine.EngineName,
                                                x.Vehicle.Id,
                                                x.Vehicle.MasterVehicleEngineId,
                                                x.Vehicle.MasterVehicleMakerId,
                                                x.Vehicle.MasterVehicleModelId,
                                                x.Vehicle.MasterVehicleSubModelId,
                                                x.Vehicle.MasterVehicleYearId,
                                                x.Vehicle.AddedOn,
                                                x.VehicleFitment.FitmentInfo,
                                            });
            List<TempVehicleFitment> tempVehicleFitmentList = new List<TempVehicleFitment>();
            foreach (var item in vehicleFitmentList)
            {
                tempVehicleFitmentList.Add(new TempVehicleFitment
                {
                    FitmentInfo = item.FitmentInfo,
                    VehicleId = item.Id,
                    VehicleName = item.Year.ToString() + " " + item.MakerName + " " + item.ModelName,
                    Engine = item.EngineName,
                    SubModel = item.SubModelName,
                    LabelName = Generator.RandomStringByLength(10)
                });
            }
            var imageList = _context.ProductImage.Where(x => x.ProductId == productId).ToList().Select(x => new
            {
                x.Id,
                x.ProductId,
                x.AdminId,
                x.AddedOn,
                x.FileName,
                x.IsHeader,
                FileURL = !String.IsNullOrEmpty(x.FileName) ? Generator.BaseURL() + "/Images/Product_Images/" + x.FileName : null
            })
            .ToList().OrderByDescending(x => x.AddedOn);
            return new JsonResult
            {
                Data = new
                {
                    Product = product,
                    TempMasterProductSpecificationLabelList = tempMasterProductSpecificationLabelList,
                    TempVehicleFitmentList = tempVehicleFitmentList,
                    ImageList = imageList
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult GetFilteredProductList(List<TempProductHierarchy> tempProductHierarchyList)
        {
            var mainCategoryIdList = tempProductHierarchyList.Where(x => x.Model == true).Select(x => x.ParentId).ToList();
            var subCategoryIdList = new List<Guid?>();
            var productCategoryIdList = new List<Guid?>();
            foreach (var item in mainCategoryIdList)
            {
                var subCategoryList = tempProductHierarchyList.Where(x => x.ParentId == item && x.Model == true).Select(x => x.TempProductHierarchyList.Where(e => e.Model == true).Select(e => e).ToList()).FirstOrDefault();
                foreach (var _item in subCategoryList)
                {
                    subCategoryIdList.Add(_item.ParentId);
                    var productCategoryList = _item.TempProductHierarchyList.Where(x => x.Model == true).Select(x => x).ToList();
                    foreach (var __item in productCategoryList)
                    {
                        productCategoryIdList.Add(__item.ParentId);
                    }

                }
            }

            // Reform the tempProductHierarchyList
            //foreach (var item in tempProductHierarchyList)
            //{
            //    if (item.Model)
            //    {
            //        var thisSubCategoryList = item.TempProductHierarchyList;
            //        item.Model = true;
            //        foreach (var _item in thisSubCategoryList)
            //        {
            //            if (item.Model == true)
            //            {
            //                //_item.Model = true;
            //                //var thisProductCategoryList = _item.TempProductHierarchyList;
            //                //foreach (var __item in thisProductCategoryList)
            //                //{
            //                //    __item.Model = true;
            //                //}
            //                if (_item.Model == false && item.Model == true)
            //                {
            //                    var thisProductCategoryList = _item.TempProductHierarchyList;
            //                    foreach (var __item in thisProductCategoryList)
            //                    {
            //                        __item.Model = false;
            //                    }
            //                }
            //                else if (_item.Model == true && item.Model == true)
            //                {
            //                    var thisProductCategoryList = _item.TempProductHierarchyList;
            //                    foreach (var __item in thisProductCategoryList)
            //                    {
            //                        __item.Model = true;
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    else if (item.Model == false)
            //    {
            //        var thisSubCategoryList = item.TempProductHierarchyList;
            //        item.Model = false;
            //        foreach (var _item in thisSubCategoryList)
            //        {
            //            _item.Model = false;
            //            var thisProductCategoryList = _item.TempProductHierarchyList;
            //            foreach (var __item in thisProductCategoryList)
            //            {
            //                __item.Model = false;
            //            }
            //        }
            //    }
            //}
            return new JsonResult
            {
                Data = _context.Product.Where(x => mainCategoryIdList.Contains(x.MasterMainCategoryId) && subCategoryIdList.Contains(x.MasterSubCategoryId) && productCategoryIdList.Contains(x.MasterProductCategoryId))
                                        .Join(_context.MasterMainCategory,
                                        x => x.MasterMainCategoryId,
                                        y => y.Id,
                                        (x, y) => new
                                        {
                                            Product = x,
                                            MasterMainCategory = y
                                        })
                                        .Join(_context.MasterSubCategory,
                                        x => x.Product.MasterSubCategoryId,
                                        y => y.Id,
                                        (x, y) => new
                                        {
                                            x.Product,
                                            x.MasterMainCategory,
                                            MasterSubCategory = y
                                        })
                                        .Join(_context.MasterProductCategory,
                                        x => x.Product.MasterProductCategoryId,
                                        y => y.Id,
                                        (x, y) => new
                                        {
                                            x.Product,
                                            x.MasterMainCategory,
                                            x.MasterSubCategory,
                                            MasterProductCategory = y
                                        })
                                        .Join(_context.MasterProductBrand,
                                        x => x.Product.MasterProductBrandId,
                                        y => y.Id,
                                        (x, y) => new
                                        {
                                            x.Product,
                                            x.MasterMainCategory,
                                            x.MasterSubCategory,
                                            x.MasterProductCategory,
                                            MasterProductBrand = y
                                        })
                                        .Join(_context.ProductPrice,
                                        x => x.Product.Id,
                                        y => y.ProductId,
                                        (x, y) => new
                                        {
                                            x.Product,
                                            x.MasterMainCategory,
                                            x.MasterSubCategory,
                                            x.MasterProductCategory,
                                            x.MasterProductBrand,
                                            ProductPrice = y
                                        })
                                        .Join(_context.ProductStock,
                                        x => x.Product.Id,
                                        y => y.ProductId,
                                        (x, y) => new
                                        {
                                            x.Product,
                                            x.MasterMainCategory,
                                            x.MasterSubCategory,
                                            x.MasterProductCategory,
                                            x.MasterProductBrand,
                                            x.ProductPrice,
                                            ProductStock = y
                                        })
                                        .Select(x => new
                                        {
                                            x.Product.ProductSKU,
                                            x.Product.Id,
                                            x.Product.Name,
                                            x.Product.AddedOn,
                                            x.Product.MasterMainCategoryId,
                                            x.Product.MasterSubCategoryId,
                                            x.Product.MasterProductCategoryId,
                                            x.Product.MasterProductBrandId,
                                            x.ProductStock.CurrentStock,
                                            x.ProductPrice.CurrentSalesPrice,
                                            x.ProductPrice.CurrentUnitPrice
                                        }).ToList(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult GetProductList()
        {
            return new JsonResult
            {
                Data = _context.Product
                                .Join(_context.MasterMainCategory,
                                x => x.MasterMainCategoryId,
                                y => y.Id,
                                (x, y) => new
                                {
                                    Product = x,
                                    MasterMainCategory = y
                                })
                                .Join(_context.MasterSubCategory,
                                x => x.Product.MasterSubCategoryId,
                                y => y.Id,
                                (x, y) => new
                                {
                                    x.Product,
                                    x.MasterMainCategory,
                                    MasterSubCategory = y
                                })
                                .Join(_context.MasterProductCategory,
                                x => x.Product.MasterProductCategoryId,
                                y => y.Id,
                                (x, y) => new
                                {
                                    x.Product,
                                    x.MasterMainCategory,
                                    x.MasterSubCategory,
                                    MasterProductCategory = y
                                })
                                .Join(_context.MasterProductBrand,
                                x => x.Product.MasterProductBrandId,
                                y => y.Id,
                                (x, y) => new
                                {
                                    x.Product,
                                    x.MasterMainCategory,
                                    x.MasterSubCategory,
                                    x.MasterProductCategory,
                                    MasterProductBrand = y
                                })
                                .Join(_context.ProductPrice,
                                x => x.Product.Id,
                                y => y.ProductId,
                                (x, y) => new
                                {
                                    x.Product,
                                    x.MasterMainCategory,
                                    x.MasterSubCategory,
                                    x.MasterProductCategory,
                                    x.MasterProductBrand,
                                    ProductPrice = y
                                })
                                .Join(_context.ProductStock,
                                x => x.Product.Id,
                                y => y.ProductId,
                                (x, y) => new
                                {
                                    x.Product,
                                    x.MasterMainCategory,
                                    x.MasterSubCategory,
                                    x.MasterProductCategory,
                                    x.MasterProductBrand,
                                    x.ProductPrice,
                                    ProductStock = y
                                })
                                .Select(x => new
                                {
                                    x.Product.ProductSKU,
                                    x.Product.Id,
                                    x.Product.Name,
                                    x.Product.IsDiscountActive,
                                    x.Product.DiscountPercentage,
                                    x.Product.AddedOn,
                                    x.Product.MasterMainCategoryId,
                                    x.Product.MasterSubCategoryId,
                                    x.Product.MasterProductCategoryId,
                                    x.Product.MasterProductBrandId,
                                    x.ProductStock.CurrentStock,
                                    x.ProductPrice.CurrentSalesPrice,
                                    x.ProductPrice.CurrentUnitPrice
                                }).ToList(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult GetProductStockByProductId(Guid? productId)
        {
            return new JsonResult
            {
                Data = _context.ProductStock.Where(x => x.ProductId == productId)
                                .Join(_context.Product,
                                x => x.ProductId,
                                y => y.Id,
                                (x, y) => new
                                {
                                    ProductStock = x,
                                    Product = y
                                })
                                .Select(x => new
                                {
                                    x.ProductStock.ProductId,
                                    x.ProductStock.Id,
                                    x.ProductStock.CurrentStock,
                                    x.ProductStock.AdminId,
                                    x.ProductStock.AddedOn,
                                    x.Product.ProductSKU,
                                    x.Product.Name
                                }).FirstOrDefault(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult GetProductPriceByProductId(Guid? productId)
        {
            return new JsonResult
            {
                Data = _context.ProductPrice.Where(x => x.ProductId == productId)
                                            .Join(_context.Product,
                                            x => x.ProductId,
                                            y => y.Id,
                                            (x, y) => new
                                            {
                                                ProductPrice = x,
                                                Product = y
                                            })
                                            .Select(x => new
                                            {
                                                x.ProductPrice.ProductId,
                                                x.ProductPrice.Id,
                                                x.ProductPrice.CurrentSalesPrice,
                                                x.ProductPrice.CurrentUnitPrice,
                                                x.ProductPrice.AdminId,
                                                x.ProductPrice.AddedOn,
                                                x.Product.ProductSKU,
                                                x.Product.Name
                                            }).FirstOrDefault(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult UpdateProductStock(Guid? productId, double stock, Guid? adminId)
        {
            var productStockToUpdate = _context.ProductStock.Where(x => x.ProductId == productId).Select(x => x).FirstOrDefault();
            if (productStockToUpdate != null)
            {
                // Update Product Stock
                productStockToUpdate.CurrentStock = productStockToUpdate.CurrentStock + stock;

                // Add Product Stock Log
                _context.ProductStockLog.Add(new ProductStockLog
                {
                    AddedOn = DateTime.UtcNow,
                    ProductId = productId,
                    Value = stock,
                    AdminId = adminId,
                    Status = _EnumObjects.StockLogStatus.Increase.ToString()
                });

                _context.SaveChanges();
                Generator.IsReport = "Success";
                Generator.Message = "Product stock updated successfully";
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
        public JsonResult UpdateProductPrice(ProductPrice productPrice)
        {
            var productPriceToUpdate = _context.ProductPrice.Where(x => x.Id == productPrice.Id).Select(x => x).FirstOrDefault();
            if (productPriceToUpdate != null)
            {
                var salesPriceUpdateStatus = productPrice.CurrentSalesPrice > productPriceToUpdate.CurrentSalesPrice ? _EnumObjects.PriceLogStatus.Increase.ToString() : productPrice.CurrentSalesPrice < productPriceToUpdate.CurrentSalesPrice ? _EnumObjects.PriceLogStatus.Decrease.ToString() : _EnumObjects.PriceLogStatus.Constant.ToString();
                var unitPriceUpdateStatus = productPrice.CurrentUnitPrice > productPriceToUpdate.CurrentUnitPrice ? _EnumObjects.PriceLogStatus.Increase.ToString() : productPrice.CurrentUnitPrice < productPriceToUpdate.CurrentUnitPrice ? _EnumObjects.PriceLogStatus.Decrease.ToString() : _EnumObjects.PriceLogStatus.Constant.ToString();
                // Update Product Price
                productPriceToUpdate.CurrentSalesPrice = productPrice.CurrentSalesPrice;
                productPriceToUpdate.CurrentUnitPrice = productPrice.CurrentUnitPrice;

                // Add Product Price Log
                _context.ProductPriceLog.Add(new ProductPriceLog
                {
                    AddedOn = DateTime.UtcNow,
                    ProductId = productPrice.ProductId,
                    SalesValue = productPrice.CurrentSalesPrice,
                    UnitValue = productPrice.CurrentUnitPrice,
                    AdminId = productPrice.AdminId,
                    SalesPriceUpdateStatus = salesPriceUpdateStatus,
                    UnitPriceUpdateStatus = unitPriceUpdateStatus
                });

                _context.SaveChanges();
                Generator.IsReport = "Success";
                Generator.Message = "Product price updated successfully";
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
        public JsonResult UpdateProductVehicleFitmentInformation(List<VehicleFitment> vehicleFitmentList, Guid? adminId)
        {
            if (vehicleFitmentList.Count() > 0)
            {
                var productToUpdateVechicleFitmentInfo = vehicleFitmentList.Select(x => x.ProductId).FirstOrDefault();
                if (productToUpdateVechicleFitmentInfo != null)
                {
                    var oldFitmentInfoList = _context.VehicleFitment.Where(x => x.ProductId == productToUpdateVechicleFitmentInfo).Select(x => x).ToList();
                    if (oldFitmentInfoList.Count() > 0)
                    {
                        // Remove the olders
                        _context.VehicleFitment.RemoveRange(oldFitmentInfoList);
                        _context.SaveChanges();
                    }
                    // Add the newers
                    foreach (var item in vehicleFitmentList)
                    {
                        _context.VehicleFitment.Add(new VehicleFitment
                        {
                            Id = Guid.NewGuid(),
                            AddedOn = DateTime.UtcNow,
                            AdminId = adminId,
                            FitmentInfo = item.FitmentInfo,
                            ProductId = item.ProductId,
                            VehicleId = item.VehicleId
                        });
                    }

                    _context.SaveChanges();
                    Generator.IsReport = "Success";
                    Generator.Message = "vehicle fitment information has been updated successfully";
                }
                else
                {
                    Generator.IsReport = "Error";
                    Generator.Message = "404 Not Found!";
                }
            }
            else
            {
                Generator.IsReport = "Error";
                Generator.Message = "No fitment information provided";
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
        public JsonResult UploadProductImage(Guid? productId, Guid? adminId)
        {
            var successful_upload = 0;
            var failed_upload_format = 0;
            var failed_upload_size = 0;
            var failed_upload = 0;
            var thisProduct = _context.Product.Where(x => x.Id == productId).Select(x => x).FirstOrDefault();
            if (thisProduct != null)
            {
                System.Web.HttpFileCollection httpFileCollection = System.Web.HttpContext.Current.Request.Files;
                if (httpFileCollection.Count != 0 && httpFileCollection.Count <= 10)
                {
                    for (int i = 0; i < httpFileCollection.Count; i++)
                    {
                        try
                        {
                            string randomString = Guid.NewGuid().ToString().ToUpper();
                            randomString = randomString.Replace("-", "");
                            System.Web.HttpPostedFile hpf = httpFileCollection[i];
                            var newImageNameWithoutExtension = System.Convert.ToBase64String(System.Text.UTF8Encoding.ASCII.GetBytes(thisProduct.ProductSKU)).ToString().ToUpper() + "_" + randomString;
                            var extension = Path.GetExtension(hpf.FileName);
                            if (hpf.ContentType == "image/jpeg" && hpf.ContentLength <= 10485760)
                            {
                                if (extension.Length <= 0)
                                {
                                    extension = ".jpg";
                                }
                                var newImageName = newImageNameWithoutExtension + extension;

                                // Save Image into folder
                                hpf.SaveAs(Generator.ProductImagePath + newImageName);

                                // Save image details into DB
                                _context.ProductImage.Add(new ProductImage
                                {
                                    Id = Guid.NewGuid(),
                                    AdminId = adminId,
                                    AddedOn = DateTime.UtcNow,
                                    FileName = newImageName,
                                    ProductId = productId,
                                    IsHeader = successful_upload == 0 ? true : false
                                });
                                _context.SaveChanges();
                                successful_upload++;
                            }
                            else
                            {
                                if (hpf.ContentType != "image/jpeg")
                                {
                                    failed_upload_format++;
                                }
                                else if (hpf.ContentLength > 10485760)
                                {
                                    failed_upload_size++;
                                    Generator.IsReport = "Error";
                                    Generator.Message = "File size exceeded. Max file size is 10MB. Your selected file size is " + hpf.ContentLength / 1000 + "KB.";
                                }
                            }
                        }
                        catch (Exception)
                        {
                            failed_upload++;
                        }
                    }
                }
                else
                {
                    if (httpFileCollection.Count == 0)
                    {
                        Generator.IsReport = "Error";
                        Generator.Message = "No image to upload.";
                    }
                    else if (httpFileCollection.Count > 10)
                    {
                        Generator.IsReport = "Error";
                        Generator.Message = "You can not upload more than 10 image at same time.";
                    }
                }
            }
            else
            {
                Generator.IsReport = "Error";
                Generator.Message = "Floor information not found.";
            }
            return new JsonResult
            {
                Data = new
                {
                    Generator.IsReport,
                    Generator.Message,
                    Successful_Upload = successful_upload,
                    Failed_Upload_Format = failed_upload_format,
                    Failed_Upload_Size = failed_upload_size,
                    Failed_Upload = failed_upload
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult DeleteImageByImageId(Guid? productImageId)
        {
            var productImageToDelete = _context.ProductImage.Where(x => x.Id == productImageId).Select(x => x).FirstOrDefault();
            if (productImageToDelete != null)
            {
                if (File.Exists(Generator.ProductImagePath + productImageToDelete.FileName))
                {
                    File.Delete(Generator.ProductImagePath + productImageToDelete.FileName);
                    _context.ProductImage.Remove(productImageToDelete);
                    _context.SaveChanges();
                    Generator.IsReport = "Success";
                    Generator.Message = "Image deleted successfully.";
                }
                else
                {
                    _context.ProductImage.Remove(productImageToDelete);
                    _context.SaveChanges();
                    Generator.IsReport = "Success";
                    Generator.Message = "Image deleted successfully.";
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
        public JsonResult SetAsProductHeaderImage(Guid? productImageId)
        {
            var thisProductImage = _context.ProductImage.Where(x => x.Id == productImageId).Select(x => x).FirstOrDefault();
            if (thisProductImage != null)
            {
                if (thisProductImage.IsHeader == false)
                {
                    if (_context.ProductImage.Where(x => x.ProductId == thisProductImage.ProductId).Any(x => x.IsHeader == true))
                    {
                        var listOfProductImageToUnsetAsHeader = _context.ProductImage.Where(x => x.ProductId == thisProductImage.ProductId).Select(x => x).ToList();
                        foreach (var item in listOfProductImageToUnsetAsHeader)
                        {
                            item.IsHeader = false;
                        }
                        // Set This Product Image as Header
                        thisProductImage.IsHeader = true;
                        _context.SaveChanges();
                        Generator.IsReport = "Success";
                        Generator.Message = "Product image cover has been set successfully.";
                    }
                    else
                    {
                        // Set This Product Image as Header
                        thisProductImage.IsHeader = true;
                        _context.SaveChanges();
                        Generator.IsReport = "Success";
                        Generator.Message = "Product image cover has been set successfully.";
                    }
                }
                else
                {
                    Generator.IsReport = "Warning";
                    Generator.Message = "This image has already been set as product image cover. Please try some different image.";
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
        public JsonResult GetProductUnitPriceByDateByProductId(Guid? productId)
        {
            var result = _context.ProductPriceLog.Where(x => x.ProductId == productId).Select(x => new
            {
                x.UnitValue,
                x.UnitPriceUpdateStatus,
                x.AddedOn
            })
            .OrderByDescending(x => x.AddedOn)
            .ToList();
            var dates = new List<string>();
            var data = new List<string>();
            foreach (var item in result)
            {
                data.Add(item.UnitValue.ToString());
                dates.Add(item.AddedOn.ToString());
            }
            return new JsonResult
            {
                Data = new
                {
                    Dates = dates,
                    Data = data
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult GetProductSalesPriceByDateByProductId(Guid? productId)
        {
            var result = _context.ProductPriceLog.Where(x => x.ProductId == productId).Select(x => new
            {
                x.SalesValue,
                x.SalesPriceUpdateStatus,
                x.AddedOn
            })
            .OrderByDescending(x => x.AddedOn)
            .ToList();
            var dates = new List<string>();
            var data = new List<string>();
            foreach (var item in result)
            {
                data.Add(item.SalesValue.ToString());
                dates.Add(item.AddedOn.ToString());
            }
            return new JsonResult
            {
                Data = new
                {
                    Dates = dates,
                    Data = data
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult GetMasterSubCategoryList(Guid? masterMainCategoryId)
        {

            return new JsonResult
            {
                Data = _context.MasterSubCategory.Where(x => x.MasterMainCategoryId == masterMainCategoryId).Select(x => x).ToList(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult GetMasterProductCategoryList(Guid? masterMainCategoryId, Guid? masterSubCategoryId)
        {
            return new JsonResult
            {
                Data = _context.MasterProductCategory.Where(x => x.MasterMainCategoryId == masterMainCategoryId && x.MasterSubCategoryId == masterSubCategoryId).Select(x => x).ToList(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult GetPopularBrand()
        {
            return new JsonResult
            {
                Data = _context.MasterProductBrand.ToList().Select(x => new
                {
                    x.AddedOn,
                    x.AdminId,
                    x.Id,
                    Name = x.MasterProductBrandName,
                    Banner = !String.IsNullOrEmpty(x.LogoFileName) ? Generator.BaseURL() + "/Images/ProductBrand_Images/" + x.LogoFileName : null
                })
                .OrderBy(x => x.AddedOn)
                .ToList(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult GetPopularMaker()
        {
            return new JsonResult
            {
                Data = _context.MasterVehicleMaker.ToList().Select(x => new
                {
                    x.AddedOn,
                    x.AdminId,
                    x.Id,
                    Name = x.MakerName
                })
                .ToList().OrderBy(x => x.AddedOn),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult GenerateProductSKU(Guid? masterProductBrandId)
        {
            return new JsonResult
            {
                Data = GenericServices.GenerateProductSKU(masterProductBrandId),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult GetProductHierarchy()
        {
            List<TempProductHierarchy> tempProductHierarchyList = new List<TempProductHierarchy>();



            var result = _context.MasterMainCategory
                                .GroupJoin(_context.MasterSubCategory,
                                x => x.Id,
                                y => y.MasterMainCategoryId,
                                (x, y) => new
                                {
                                    MasterMainCategory = x,
                                    MasterSubCategory = y
                                })
                                .ToList()
                                .Select(x => new
                                {
                                    MasterMainCategoryLogo = !String.IsNullOrEmpty(x.MasterMainCategory.LogoFileName) ? Generator.BaseURL() + "/Images/MainCategory_Images/" + x.MasterMainCategory.LogoFileName : null,
                                    x.MasterMainCategory.MasterMainCategoryName,
                                    x.MasterMainCategory.Id,
                                    SubCategoryList = x.MasterSubCategory
                                                        .GroupJoin(_context.MasterProductCategory,
                                                        f => f.Id,
                                                        g => g.MasterSubCategoryId,
                                                        (f, g) => new
                                                        {
                                                            MasterSubCategory = f,
                                                            MasterProductCategory = g
                                                        })
                                                        .ToList()
                                                        .Select(e => new
                                                        {
                                                            MasterSubCategoryLogo = !String.IsNullOrEmpty(e.MasterSubCategory.LogoFileName) ? Generator.BaseURL() + "/Images/SubCategory_Images/" + e.MasterSubCategory.LogoFileName : null,
                                                            e.MasterSubCategory.MasterSubCategoryName,
                                                            e.MasterSubCategory.Id,
                                                            ProductCategoryList = e.MasterProductCategory
                                                                                    .GroupJoin(_context.Product,
                                                                                    f => f.Id,
                                                                                    g => g.MasterProductCategoryId,
                                                                                    (f, g) => new
                                                                                    {
                                                                                        MasterProductCategory = f,
                                                                                        Product = g
                                                                                    })
                                                                                    .ToList()
                                                                                    .Select(a => new
                                                                                    {
                                                                                        MasterProductCategoryLogo = !String.IsNullOrEmpty(a.MasterProductCategory.LogoFileName) ? Generator.BaseURL() + "/Images/SubCategory_Images/" + a.MasterProductCategory.LogoFileName : null,
                                                                                        a.MasterProductCategory.MasterProductCategoryName,
                                                                                        a.MasterProductCategory.Id,
                                                                                        //ProductList = a.Product.Select(h => new
                                                                                        //{
                                                                                        //    h.Name,
                                                                                        //    h.Id
                                                                                        //}).ToList().OrderBy(h => h.Name)
                                                                                    }).ToList().OrderBy(a => a.MasterProductCategoryName),
                                                        }).ToList().OrderBy(e => e.MasterSubCategoryName),
                                }).ToList().OrderBy(x => x.MasterMainCategoryName);

            foreach (var item in result)
            {
                var tempProductHierarchy = new TempProductHierarchy();
                tempProductHierarchy.TempProductHierarchyList = new List<TempProductHierarchy>();
                tempProductHierarchy.Model = true;
                tempProductHierarchy.Name = item.MasterMainCategoryName;
                tempProductHierarchy.ParentId = item.Id;
                tempProductHierarchy.Banner = item.MasterMainCategoryLogo;
                foreach (var _item in item.SubCategoryList)
                {
                    var _tempProductHierarchy = new TempProductHierarchy();
                    _tempProductHierarchy.TempProductHierarchyList = new List<TempProductHierarchy>();
                    _tempProductHierarchy.Model = true;
                    _tempProductHierarchy.Name = _item.MasterSubCategoryName;
                    _tempProductHierarchy.ParentId = _item.Id;
                    _tempProductHierarchy.Banner = _item.MasterSubCategoryLogo;
                    foreach (var __item in _item.ProductCategoryList)
                    {
                        var __tempProductHierarchy = new TempProductHierarchy();
                        __tempProductHierarchy.TempProductHierarchyList = new List<TempProductHierarchy>();
                        __tempProductHierarchy.Model = true;
                        __tempProductHierarchy.Name = __item.MasterProductCategoryName;
                        __tempProductHierarchy.ParentId = __item.Id;
                        __tempProductHierarchy.Banner = __item.MasterProductCategoryLogo;
                        _tempProductHierarchy.TempProductHierarchyList.Add(__tempProductHierarchy);
                    }
                    tempProductHierarchy.TempProductHierarchyList.Add(_tempProductHierarchy);
                }
                tempProductHierarchyList.Add(tempProductHierarchy);
            }
            return new JsonResult
            {
                Data = tempProductHierarchyList,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult GetProductLabel()
        {
            List<TempMasterProductSpecificationLabel> tempMasterProductSpecificationLabelList = new List<TempMasterProductSpecificationLabel>();
            var dataList = _context.MasterProductSpecificationLabel.Select(x => x).ToList();
            foreach (var item in dataList)
            {
                tempMasterProductSpecificationLabelList.Add(new TempMasterProductSpecificationLabel
                {
                    Id = item.Id,
                    AdminId = item.AdminId,
                    Label = item.Label,
                    LabelName = Generator.RandomStringByLength(10),
                    Model = null,
                    IsCategorical = item.IsCategorical,
                    IsHighlightedFeature = false,
                    IsHighlightedFeatureId = Generator.RandomStringByLength(10)
                });
            }
            return new JsonResult
            {
                Data = tempMasterProductSpecificationLabelList,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult GetVehicleFitmentInfoList()
        {
            var data = _context.Vehicle.Join(_context.MasterVehicleYear,
                                        x => x.MasterVehicleYearId,
                                        y => y.Id,
                                        (x, y) => new
                                        {
                                            Vehicle = x,
                                            MasterVehicleYear = y
                                        })
                                        .Join(_context.MasterVehicleMaker,
                                        x => x.Vehicle.MasterVehicleMakerId,
                                        y => y.Id,
                                        (x, y) => new
                                        {
                                            x.Vehicle,
                                            x.MasterVehicleYear,
                                            MasterVehicleMaker = y
                                        })
                                        .Join(_context.MasterVehicleModel,
                                        x => x.Vehicle.MasterVehicleModelId,
                                        y => y.Id,
                                        (x, y) => new
                                        {
                                            x.Vehicle,
                                            x.MasterVehicleYear,
                                            x.MasterVehicleMaker,
                                            MasterVehicleModel = y
                                        })
                                        .Join(_context.MasterVehicleSubModel,
                                        x => x.Vehicle.MasterVehicleSubModelId,
                                        y => y.Id,
                                        (x, y) => new
                                        {
                                            x.Vehicle,
                                            x.MasterVehicleYear,
                                            x.MasterVehicleMaker,
                                            x.MasterVehicleModel,
                                            MasterVehicleSubModel = y
                                        })
                                        .Join(_context.MasterVehicleEngine,
                                        x => x.Vehicle.MasterVehicleEngineId,
                                        y => y.Id,
                                        (x, y) => new
                                        {
                                            x.Vehicle,
                                            x.MasterVehicleYear,
                                            x.MasterVehicleMaker,
                                            x.MasterVehicleModel,
                                            x.MasterVehicleSubModel,
                                            MasterVehicleEngine = y
                                        })
                                        .ToList()
                                        .Select(x => new
                                        {
                                            Year = x.MasterVehicleYear.Year.ToString(),
                                            x.MasterVehicleMaker.MakerName,
                                            x.MasterVehicleModel.ModelName,
                                            x.MasterVehicleSubModel.SubModelName,
                                            x.MasterVehicleEngine.EngineName,
                                            x.Vehicle.Id,
                                            x.Vehicle.MasterVehicleEngineId,
                                            x.Vehicle.MasterVehicleMakerId,
                                            x.Vehicle.MasterVehicleModelId,
                                            x.Vehicle.MasterVehicleSubModelId,
                                            x.Vehicle.MasterVehicleYearId,
                                            x.Vehicle.AddedOn
                                        }).ToList();
            List<TempVehicleFitment> tempVehicleFitment = new List<TempVehicleFitment>();
            foreach (var item in data)
            {
                tempVehicleFitment.Add(new TempVehicleFitment
                {
                    FitmentInfo = null,
                    VehicleId = item.Id,
                    VehicleName = item.Year.ToString() + " " + item.MakerName + " " + item.ModelName,
                    Engine = item.EngineName,
                    SubModel = item.SubModelName,
                    LabelName = Generator.RandomStringByLength(10)
                });
            }
            return new JsonResult
            {
                Data = tempVehicleFitment,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult DownloadFileByName(string filename)
        {
            string filePath = Generator.ProductImagePath + filename;
            byte[] bytes = File.ReadAllBytes(filePath);
            string base64File = Convert.ToBase64String(bytes);
            string mimetype = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(filename)).ToString();
            return new JsonResult
            {
                Data = new
                {
                    base64File,
                    mimetype
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult GetAllSubCategoryListByCategoryId(Guid? categoryId, TempVehicleFilter tempVehicleFilter)
        {
            var result = _context.MasterMainCategory.Where(x => x.Id == categoryId)
                                .GroupJoin(_context.MasterSubCategory,
                                x => x.Id,
                                y => y.MasterMainCategoryId,
                                (x, y) => new
                                {
                                    MasterMainCategory = x,
                                    MasterSubCategory = y
                                })
                                .ToList()
                                .Select(x => new
                                {
                                    x.MasterMainCategory.Id,
                                    x.MasterMainCategory.MasterMainCategoryName,
                                    MasterMainCategoryLogo = !String.IsNullOrEmpty(x.MasterMainCategory.LogoFileName) ? Generator.BaseURL() + "/Images/MainCategory_Images/" + x.MasterMainCategory.LogoFileName : null,
                                    SubCategoryList = x.MasterSubCategory.Select(e => new
                                    {
                                        e.MasterSubCategoryName,
                                        e.Id,
                                        MasterSubCategoryLogo = !String.IsNullOrEmpty(e.LogoFileName) ? Generator.BaseURL() + "/Images/SubCategory_Images/" + e.LogoFileName : null,
                                    }).ToList()
                                }).FirstOrDefault();
            if (tempVehicleFilter != null)
            {
                var vehicleIds = tempVehicleFilter.VehicleIdList;
                var productIds = _context.VehicleFitment.Where(x => vehicleIds.Contains(x.VehicleId)).Select(x => x.ProductId).Distinct().ToList();
                List<Guid?> filteredSubCategoryList = new List<Guid?>();
                foreach (var item in productIds)
                {
                    filteredSubCategoryList.Add(_context.Product.Where(x => x.Id == item).Select(x => x.MasterSubCategoryId).FirstOrDefault());
                }
                foreach (var item in result.SubCategoryList.ToList())
                {
                    if (!filteredSubCategoryList.Contains(item.Id))
                    {
                        result.SubCategoryList.Remove(item);
                    }
                }
            }
            return new JsonResult
            {
                Data = result,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult GetAllProductCategoryListBySubCategoryId(Guid? subCategoryId, TempVehicleFilter tempVehicleFilter)
        {
            var result = _context.MasterSubCategory.Where(x => x.Id == subCategoryId)
                                .GroupJoin(_context.MasterProductCategory,
                                x => x.Id,
                                y => y.MasterSubCategoryId,
                                (x, y) => new
                                {
                                    MasterSubCategory = x,
                                    MasterProductCategory = y
                                })
                                .ToList()
                                .Select(x => new
                                {
                                    x.MasterSubCategory.Id,
                                    x.MasterSubCategory.MasterSubCategoryName,
                                    MasterSubCategoryLogo = !String.IsNullOrEmpty(x.MasterSubCategory.LogoFileName) ? Generator.BaseURL() + "/Images/SubCategory_Images/" + x.MasterSubCategory.LogoFileName : null,
                                    ProductCategoryList = x.MasterProductCategory.Select(e => new
                                    {
                                        e.MasterProductCategoryName,
                                        e.Id,
                                        MasterProductCategoryLogo = !String.IsNullOrEmpty(e.LogoFileName) ? Generator.BaseURL() + "/Images/ProductCategory_Images/" + e.LogoFileName : null,
                                    }).ToList()
                                }).FirstOrDefault();
            if (tempVehicleFilter != null)
            {
                var vehicleIds = tempVehicleFilter.VehicleIdList;
                var productIds = _context.VehicleFitment.Where(x => vehicleIds.Contains(x.VehicleId)).Select(x => x.ProductId).Distinct().ToList();
                List<Guid?> filteredProductCategoryList = new List<Guid?>();
                foreach (var item in productIds)
                {
                    filteredProductCategoryList.Add(_context.Product.Where(x => x.Id == item).Select(x => x.MasterProductCategoryId).FirstOrDefault());
                }
                if (result != null)
                {
                    foreach (var item in result.ProductCategoryList.ToList())
                    {
                        if (!filteredProductCategoryList.Contains(item.Id))
                        {
                            result.ProductCategoryList.Remove(item);
                        }
                    }
                }
            }
            return new JsonResult
            {
                Data = result,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult GetProductListByProductCategory(Guid? productCategoryId, TempVehicleFilter tempVehicleFilter)
        {
            var hierarchy = _context.MasterProductCategory.Where(x => x.Id == productCategoryId)
                                    .Join(_context.MasterSubCategory,
                                    x => x.MasterSubCategoryId,
                                    y => y.Id,
                                    (x, y) => new
                                    {
                                        MasterProductCategory = x,
                                        MasterSubCategory = y
                                    })
                                    .Join(_context.MasterMainCategory,
                                    x => x.MasterSubCategory.MasterMainCategoryId,
                                    y => y.Id,
                                    (x, y) => new
                                    {
                                        x.MasterSubCategory,
                                        x.MasterProductCategory,
                                        MasterMainCategory = y
                                    })
                                    .Select(x => new
                                    {
                                        MasterMainCategoryId = x.MasterMainCategory.Id,
                                        x.MasterMainCategory.MasterMainCategoryName,
                                        MasterSubCategoryId = x.MasterSubCategory.Id,
                                        x.MasterSubCategory.MasterSubCategoryName,
                                        MasterProductCategoryId = x.MasterProductCategory.Id,
                                        x.MasterProductCategory.MasterProductCategoryName
                                    }).FirstOrDefault();
            var result = _context.Product.Where(x => x.MasterProductCategoryId == productCategoryId)
                                .Join(_context.MasterProductCategory,
                                x => x.MasterProductCategoryId,
                                y => y.Id,
                                (x, y) => new
                                {
                                    Product = x,
                                    MasterProductCategory = y
                                })
                                .Join(_context.MasterProductBrand,
                                x => x.Product.MasterProductBrandId,
                                y => y.Id,
                                (x, y) => new
                                {
                                    x.Product,
                                    x.MasterProductCategory,
                                    MasterProductBrand = y
                                })
                                .Join(_context.ProductPrice,
                                x => x.Product.Id,
                                y => y.ProductId,
                                (x, y) => new
                                {
                                    x.Product,
                                    x.MasterProductCategory,
                                    x.MasterProductBrand,
                                    ProductPrice = y
                                })
                                .GroupJoin(_context.ProductSpecificationLabelDetails.Where(x => x.IsHighlightedFeature),
                                x => x.Product.Id,
                                y => y.ProductId,
                                (x, y) => new
                                {
                                    x.Product,
                                    x.MasterProductCategory,
                                    x.MasterProductBrand,
                                    x.ProductPrice,
                                    ProductSpecificationLabelDetails = y
                                })
                                .GroupJoin(_context.ProductImage,
                                x => x.Product.Id,
                                y => y.ProductId,
                                (x, y) => new
                                {
                                    x.Product,
                                    x.MasterProductCategory,
                                    x.MasterProductBrand,
                                    x.ProductPrice,
                                    x.ProductSpecificationLabelDetails,
                                    ProductImage = y
                                })
                                .GroupJoin(_context.VehicleFitment,
                                x => x.Product.Id,
                                y => y.ProductId,
                                (x, y) => new
                                {
                                    x.Product,
                                    x.MasterProductCategory,
                                    x.MasterProductBrand,
                                    x.ProductPrice,
                                    x.ProductSpecificationLabelDetails,
                                    x.ProductImage,
                                    VehicleFitment = y
                                })
                                .ToList()
                                .Select(x => new
                                {
                                    x.MasterProductCategory.MasterProductCategoryName,
                                    Price = x.Product.IsDiscountActive ? x.ProductPrice.CurrentSalesPrice - (x.ProductPrice.CurrentSalesPrice * (x.Product.DiscountPercentage / 100)) : x.ProductPrice.CurrentSalesPrice,
                                    x.Product.Id,
                                    x.Product.MasterMainCategoryId,
                                    x.Product.MasterProductBrandId,
                                    x.Product.MasterProductCategoryId,
                                    x.Product.MasterSubCategoryId,
                                    x.Product.Name,
                                    x.Product.ProductSKU,
                                    x.Product.Description,
                                    x.MasterProductBrand.MasterProductBrandName,
                                    ProductSpecificationLabelDetailsList = x.ProductSpecificationLabelDetails
                                                                            .Join(_context.MasterProductSpecificationLabel,
                                                                            m => m.MasterProductSpecificationLabelId,
                                                                            n => n.Id,
                                                                            (m, n) => new
                                                                            {
                                                                                ProductSpecificationLabelDetails = m,
                                                                                MasterProductSpecificationLabel = n
                                                                            })
                                                                            .ToList()
                                                                            .Select(e => new
                                                                            {
                                                                                e.ProductSpecificationLabelDetails.Id,
                                                                                e.ProductSpecificationLabelDetails.Value,
                                                                                e.MasterProductSpecificationLabel.Label
                                                                            }).ToList(),
                                    ProductHeaderBanner = !String.IsNullOrEmpty(x.ProductImage.Where(e => e.IsHeader || true).Select(e => e.FileName).FirstOrDefault()) ? Generator.BaseURL() + "/Images/Product_Images/" + x.ProductImage.Where(e => e.IsHeader || true).Select(e => e.FileName).FirstOrDefault() : null,
                                    VehicleIds = x.VehicleFitment.Select(e => e.Id).ToList()
                                }).ToList();
            // Filter By Vehicle Fitment
            if (tempVehicleFilter != null)
            {
                tempVehicleFilter.VehicleIdList = tempVehicleFilter.VehicleIdList.Select(x => x).Distinct().ToList();
                var vehicleFitmentProductIds = _context.VehicleFitment.Where(x => tempVehicleFilter.VehicleIdList.Contains(x.VehicleId)).Select(x => x.ProductId).Distinct().ToList();
                if (vehicleFitmentProductIds != null && vehicleFitmentProductIds.Count() > 0)
                {
                    foreach (var item in result.ToList())
                    {
                        if (!vehicleFitmentProductIds.Contains(item.Id))
                        {
                            result.Remove(item);
                        }
                    }
                }
                // As we can't found any product fitted to this vehicle thus we should vacate the product list (result)
                else
                {
                    foreach (var item in result.ToList())
                    {
                        result.Remove(item);
                    }
                }
            }
            return new JsonResult
            {
                Data = new
                {
                    Result = result,
                    Hierarchy = hierarchy
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult GetProductDetailsByProductId(Guid? productId)
        {
            var result = _context.Product.Where(x => x.Id == productId)
                                .Join(_context.MasterMainCategory,
                                x => x.MasterMainCategoryId,
                                y => y.Id,
                                (x, y) => new
                                {
                                    Product = x,
                                    MasterMainCategory = y
                                })
                                .Join(_context.MasterSubCategory,
                                x => x.Product.MasterSubCategoryId,
                                y => y.Id,
                                (x, y) => new
                                {
                                    x.Product,
                                    x.MasterMainCategory,
                                    MasterSubCategory = y
                                })
                                .Join(_context.MasterProductCategory,
                                x => x.Product.MasterProductCategoryId,
                                y => y.Id,
                                (x, y) => new
                                {
                                    x.Product,
                                    x.MasterMainCategory,
                                    x.MasterSubCategory,
                                    MasterProductCategory = y
                                })
                                .Join(_context.MasterProductBrand,
                                x => x.Product.MasterProductBrandId,
                                y => y.Id,
                                (x, y) => new
                                {
                                    x.Product,
                                    x.MasterMainCategory,
                                    x.MasterSubCategory,
                                    x.MasterProductCategory,
                                    MasterProductBrand = y
                                })
                                .Join(_context.ProductPrice,
                                x => x.Product.Id,
                                y => y.ProductId,
                                (x, y) => new
                                {
                                    x.Product,
                                    x.MasterMainCategory,
                                    x.MasterSubCategory,
                                    x.MasterProductCategory,
                                    x.MasterProductBrand,
                                    ProductPrice = y
                                })
                                .Join(_context.ProductStock,
                                x => x.Product.Id,
                                y => y.ProductId,
                                (x, y) => new
                                {
                                    x.Product,
                                    x.MasterMainCategory,
                                    x.MasterSubCategory,
                                    x.MasterProductCategory,
                                    x.MasterProductBrand,
                                    x.ProductPrice,
                                    ProductStock = y
                                })
                                .GroupJoin(_context.ProductSpecificationLabelDetails, // further joins with MasterProductSpecificationLabel
                                x => x.Product.Id,
                                y => y.ProductId,
                                (x, y) => new
                                {
                                    x.Product,
                                    x.MasterMainCategory,
                                    x.MasterSubCategory,
                                    x.MasterProductCategory,
                                    x.MasterProductBrand,
                                    x.ProductPrice,
                                    x.ProductStock,
                                    ProductSpecificationLabelDetails = y
                                })
                                .GroupJoin(_context.ProductImage,
                                x => x.Product.Id,
                                y => y.ProductId,
                                (x, y) => new
                                {
                                    x.Product,
                                    x.MasterMainCategory,
                                    x.MasterSubCategory,
                                    x.MasterProductCategory,
                                    x.MasterProductBrand,
                                    x.ProductPrice,
                                    x.ProductStock,
                                    x.ProductSpecificationLabelDetails,
                                    ProductImage = y
                                }) // further need to do to list
                                .GroupJoin(_context.VehicleFitment, // further need to join with 
                                x => x.Product.Id,
                                y => y.ProductId,
                                (x, y) => new
                                {
                                    x.Product,
                                    x.MasterMainCategory,
                                    x.MasterSubCategory,
                                    x.MasterProductCategory,
                                    x.MasterProductBrand,
                                    x.ProductPrice,
                                    x.ProductStock,
                                    x.ProductSpecificationLabelDetails,
                                    x.ProductImage,
                                    VehicleFitment = y
                                })
                                .Select(x => new
                                {
                                    // Product Details (Only Product)
                                    x.Product.Id,
                                    x.Product.IsDiscountActive,
                                    x.MasterMainCategory.MasterMainCategoryName,
                                    x.MasterProductBrand.MasterProductBrandName,
                                    x.MasterProductCategory.MasterProductCategoryName,
                                    x.MasterSubCategory.MasterSubCategoryName,
                                    x.Product.ModifiedOn,
                                    x.Product.Name,
                                    x.Product.ProductSKU,
                                    x.Product.AddedOn,
                                    x.Product.AdminId,
                                    x.Product.Description,
                                    x.Product.DiscountPercentage,
                                    x.ProductStock.CurrentStock,
                                    Price = x.Product.IsDiscountActive ? x.ProductPrice.CurrentSalesPrice - (x.ProductPrice.CurrentSalesPrice * (x.Product.DiscountPercentage / 100)) : x.ProductPrice.CurrentSalesPrice,

                                    // ProductSpecificationLabelDetailsList
                                    ProductSpecificationLabelDetailsList = x.ProductSpecificationLabelDetails
                                                                            .Join(_context.MasterProductSpecificationLabel,
                                                                            a => a.MasterProductSpecificationLabelId,
                                                                            b => b.Id,
                                                                            (a, b) => new
                                                                            {
                                                                                ProductSpecificationLabelDetails = a,
                                                                                MasterProductSpecificationLabel = b
                                                                            })
                                                                            .Select(e => new
                                                                            {
                                                                                e.MasterProductSpecificationLabel.Label,
                                                                                e.ProductSpecificationLabelDetails.Value,
                                                                            })
                                                                            .ToList(),

                                    // ProductImageList
                                    ProductImageList = x.ProductImage.Select(e => new
                                    {
                                        e.IsHeader,
                                        e.AddedOn,
                                        FileURL = !String.IsNullOrEmpty(e.FileName) ? Generator.BaseURL() + "/Images/Product_Images/" + e.FileName : null,
                                    })
                                    .OrderBy(e => e.AddedOn)
                                    .OrderBy(e => e.IsHeader)
                                    .ToList(),

                                    // VehicleFitmentList
                                    VehicleFitmentList = x.VehicleFitment
                                                        .Join(_context.Vehicle,
                                                        a => a.VehicleId,
                                                        b => b.Id,
                                                        (a, b) => new
                                                        {
                                                            VehicleFitment = a,
                                                            Vehicle = b
                                                        })
                                                        .Join(_context.MasterVehicleYear,
                                                        a => a.Vehicle.MasterVehicleYearId,
                                                        b => b.Id,
                                                        (a, b) => new
                                                        {
                                                            a.Vehicle,
                                                            a.VehicleFitment,
                                                            MasterVehicleYear = b
                                                        })
                                                        .Join(_context.MasterVehicleMaker,
                                                        a => a.Vehicle.MasterVehicleMakerId,
                                                        b => b.Id,
                                                        (a, b) => new
                                                        {
                                                            a.Vehicle,
                                                            a.VehicleFitment,
                                                            a.MasterVehicleYear,
                                                            MasterVehicleMaker = b
                                                        })
                                                        .Join(_context.MasterVehicleModel,
                                                        a => a.Vehicle.MasterVehicleModelId,
                                                        b => b.Id,
                                                        (a, b) => new
                                                        {
                                                            a.Vehicle,
                                                            a.VehicleFitment,
                                                            a.MasterVehicleYear,
                                                            a.MasterVehicleMaker,
                                                            MasterVehicleModel = b
                                                        })
                                                        // To be cotinued with join
                                });
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
    public interface IProductServices
    {
        JsonResult CreateProduct(Product product, List<TempVehicleFitment> tempVehicleFitmentList, List<TempMasterProductSpecificationLabel> tempMasterProductSpecificationLabelList);
        JsonResult UpdateProduct(Product product, List<TempVehicleFitment> tempVehicleFitmentList, List<TempMasterProductSpecificationLabel> tempMasterProductSpecificationLabelList);
        JsonResult UpdateDiscountPercentageByProductId(Guid? productId, double value);
        JsonResult ToggleDiscountByProductId(Guid? productId);
        JsonResult GetproductDetailsForEdit(Guid? productId);
        JsonResult GetFilteredProductList(List<TempProductHierarchy> tempProductHierarchyList);
        JsonResult GetProductList();
        JsonResult GetProductStockByProductId(Guid? productId);
        JsonResult GetProductPriceByProductId(Guid? productId);
        JsonResult UpdateProductStock(Guid? productId, double stock, Guid? adminId);
        JsonResult UpdateProductPrice(ProductPrice productPrice);
        JsonResult UpdateProductVehicleFitmentInformation(List<VehicleFitment> vehicleFitmentList, Guid? adminId);
        JsonResult UploadProductImage(Guid? productId, Guid? adminId);
        JsonResult DeleteImageByImageId(Guid? productImageId);
        JsonResult SetAsProductHeaderImage(Guid? productImageId);
        JsonResult GetProductUnitPriceByDateByProductId(Guid? productId);
        JsonResult GetProductSalesPriceByDateByProductId(Guid? productId);
        JsonResult GetMasterSubCategoryList(Guid? masterMainCategoryId); // By MasterMainCategory
        JsonResult GetMasterProductCategoryList(Guid? masterMainCategoryId, Guid? masterSubCategoryId); // By MasterMainCategory && MasterSubCategory
        JsonResult GetPopularBrand();
        JsonResult GetPopularMaker();
        JsonResult GenerateProductSKU(Guid? masterProductBrandId);
        JsonResult GetProductHierarchy();
        JsonResult GetProductLabel();
        JsonResult GetVehicleFitmentInfoList();
        JsonResult DownloadFileByName(string filename);
        JsonResult GetAllSubCategoryListByCategoryId(Guid? categoryId, TempVehicleFilter tempVehicleFilter);
        JsonResult GetAllProductCategoryListBySubCategoryId(Guid? subCategoryId, TempVehicleFilter tempVehicleFilter);
        JsonResult GetProductListByProductCategory(Guid? productCategoryId, TempVehicleFilter tempVehicleFilter);
        JsonResult GetProductDetailsByProductId(Guid? productId);
    }
}
