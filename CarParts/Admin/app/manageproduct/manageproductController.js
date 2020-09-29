/// <reference path="C:\Users\Bacbon-Lap\source\repos\addictioncare\AddictionCare\AddictionCare\Scripts/jquery.signalR-2.4.1.js" />
/// <reference path="app.js" />
CarPartsApp.controller('manageproductController', function ($scope, $http, manageproductServices, masterproductbrandServices, $rootScope, appServices, $cookies, blockUI, $window, $q, toastr, $compile, $timeout, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder, $state) {
    //====================================================================Declaration=================================================================================    
    $scope.Product = {};
    $scope.TemporaryAuthentication = {};
    $scope.vm = {};
    $scope.vm.dtInstance = {};
    $scope.vm.dtColumnDefs = [DTColumnDefBuilder.newColumnDef(2).notSortable()
    ];
    $scope.vm.dtOptions = DTOptionsBuilder.newOptions()
                            .withOption('paging', true)
                            .withOption('searching', true)
                            .withOption('info', true)
                            .withOption('order', [1, 'desc']);
    $scope.AllControls = [];
    $scope.FilteredControls = [];

    $scope.AllFitmentInfo = [];
    $scope.FilteredFitmentInfo = [];

    $scope.ProductUnitPriceByDateByProductIdDates = [];
    $scope.ProductUnitPriceByDateByProductIdData = [];


    //====================================================================Automated HTTP Operation================================================================================    
    function LoadProductList() {
        manageproductServices.GetProductHierarchy().then(function (response) {
            $scope.ProductHierarchy = response.data;
        })
        .then(function () {
            manageproductServices.GetProductList().then(function (response) {
                $scope.ProductList = response.data;
            })
        })
    }
    LoadProductList();

    //====================================================================Element Processing==========================================================================
    //Modal Scroll Stuck Issue Solve
    $('.modal').on('hidden.bs.modal', function (e) {
        if ($('.modal').hasClass('in')) {
            $('body').addClass('modal-open');
        }
    });

    //====================================================================Object Processing===========================================================================
    $scope.ProcessMasterMainCategory = function (item, model, label) {
        $scope.Product.MasterMainCategoryId = model.Id;
        // Get The Sub Category By The Main Category Id
        manageproductServices.GetMasterSubCategoryList($scope.Product.MasterMainCategoryId).then(function (response) {
            $scope.MasterSubCategoryList = response.data;
        })
    }
    $scope.ProcessMasterSubCategory = function (item, model, label) {
        $scope.Product.MasterSubCategoryId = model.Id;
        // Get The Product Category By The Main Category Id and Sub Category Id
        manageproductServices.GetMasterProductCategoryList($scope.Product.MasterMainCategoryId, $scope.Product.MasterSubCategoryId).then(function (response) {
            $scope.MasterProductCategoryList = response.data;
        })
    }
    $scope.ProcessMasterProductCategory = function (item, model, label) {
        $scope.Product.MasterProductCategoryId = model.Id;
    }
    $scope.ProcessMasterProductBrand = function (item, model, label) {
        $scope.Product.MasterProductBrandId = model.Id;
        // Generate Brand SKU From Brand Id

    }

    $scope.addControl = function (model) {
        if (model != undefined && model != "") {
            //Push that selected control to our filtered control
            $scope.FilteredControls.push(model);

            //Then remove that control from the $scope.AllControls
            var index = $scope.AllControls.findIndex(x => x.Id == model.Id);
            if (index > -1) {
                $scope.AllControls.splice(index, 1);
            }
        }
        else {
            toastr.error("Please select a label by searching", "Error!")
        }
    };
    $scope.removeControl = function (model) {
        var index = $scope.FilteredControls.findIndex(x => x.Id == model.Id);
        if (index > -1) {
            $scope.FilteredControls.splice(index, 1);
        }
        $scope.AllControls.push(model);
    };

    $scope.addFitmentControl = function (model) {
        if (model != undefined && model != "") {
            //Push that selected control to our filtered control
            $scope.FilteredFitmentInfo.push(model);

            //Then remove that control from the $scope.AllControls
            var index = $scope.AllFitmentInfo.findIndex(x => x.VehicleId == model.VehicleId);
            if (index > -1) {
                $scope.AllFitmentInfo.splice(index, 1);
            }
        }
        else {
            toastr.error("Please select a label by searching", "Error!")
        }
    };
    $scope.removeFitmentControl = function (model) {
        var index = $scope.FilteredFitmentInfo.findIndex(x => x.VehicleId == model.VehicleId);
        if (index > -1) {
            $scope.FilteredFitmentInfo.splice(index, 1);
        }
        $scope.AllFitmentInfo.push(model);
    };

    $scope.AppendToProductName = function (value) {
        if ($scope.Product.Name != undefined) {
            if (!$scope.Product.Name.includes(value.toString())) {
                $scope.Product.Name = $scope.Product.Name + ' ' + value.toString();
                toastr.success("Value Appended");
            }
            else {
                $scope.Product.Name = $scope.Product.Name.toString().replace(' ' + value.toString(), "")
                toastr.warning("Value Removed");
            }
        }
        else if ($scope.Product.Name == undefined) {
            $scope.Product.Name = ' ' + value.toString();
            toastr.success("Value Appended");
        }
    }

    $scope.FilterGrandParentProduct = function (data) {
        $timeout(function () {
            let itemIndexToOperateOn = $scope.ProductHierarchy.findIndex(x=>x.ParentId == data.ParentId);
            if (itemIndexToOperateOn != -1) {
                if (data.Model == false) {
                    let item = $scope.ProductHierarchy[itemIndexToOperateOn];
                    item.Model = false;
                    for (let i = 0; i < item.TempProductHierarchyList.length; i++) {
                        let _item = item.TempProductHierarchyList[i];
                        _item.Model = false;
                        for (let j = 0; j < _item.TempProductHierarchyList.length; j++) {
                            _item.TempProductHierarchyList[j].Model = false;
                        }
                    }
                }
                else if (data.Model == true) {
                    let item = $scope.ProductHierarchy[itemIndexToOperateOn];
                    item.Model = true;
                    for (let i = 0; i < item.TempProductHierarchyList.length; i++) {
                        let _item = item.TempProductHierarchyList[i];
                        _item.Model = true;
                        for (let j = 0; j < _item.TempProductHierarchyList.length; j++) {
                            _item.TempProductHierarchyList[j].Model = true;
                        }
                    }
                }
            }
        }, 1)
        .then(function () {
            manageproductServices.GetFilteredProductList($scope).then(function (response) {
                $scope.ProductList = response.data;
                for (let i = 0; i < $scope.ProductList.length; i++) {
                    $scope.ProductList[i].AddedOn = moment.utc($scope.ProductList[i].AddedOn).local().format();
                }
            })
        })
    }
    $scope.FilterParentProduct = function (subData, data) {
        $timeout(function () {
            if (subData.Model == true) {
                let _item = subData.TempProductHierarchyList;
                _item.Model = true;
                for (let i = 0; i < _item.length; i++) {
                    _item[i].Model = true;
                }
            }
            else if (subData.Model == false) {
                let _item = subData.TempProductHierarchyList;
                _item.Model = false;
                for (let i = 0; i < _item.length; i++) {
                    _item[i].Model = false;
                }
            }

            // While clicking on Parent; Allow him to work on his Grand Parent
            let _item = data.TempProductHierarchyList;
            let trueIndex = _item.findIndex(x=>x.Model == true);
            let falseIndex = _item.findIndex(x=>x.Model == false);
            if (trueIndex != -1) {
                data.Model = true;
            }
            if (falseIndex != -1 && trueIndex == -1) {
                data.Model = false;
            }
        }, 1)
        .then(function () {
            manageproductServices.GetFilteredProductList($scope).then(function (response) {
                $scope.ProductList = response.data;
                for (let i = 0; i < $scope.ProductList.length; i++) {
                    $scope.ProductList[i].AddedOn = moment.utc($scope.ProductList[i].AddedOn).local().format();
                }
            })
        })
    }
    $scope.FilterChildProduct = function (subData, data) {
        $timeout(function () {
            // While clicking on grand child; Allow him to work on his Parent
            let _item = subData.TempProductHierarchyList;
            let trueIndex = _item.findIndex(x=>x.Model == true);
            let falseIndex = _item.findIndex(x=>x.Model == false);
            if (trueIndex != -1) {
                subData.Model = true;
                data.Model = true;
            }
            if (falseIndex != -1 && trueIndex == -1) {
                subData.Model = false;
            }

            // While clicking on grand child; Allow him to work on his Grand parent
            let item = data.TempProductHierarchyList;
            let trueParentIndex = item.findIndex(x=>x.Model == true);
            let falseParentIndex = item.findIndex(x=>x.Model == false);
            if (trueParentIndex != -1) {
                data.Model = true;
            }
            if (falseParentIndex != -1 && trueParentIndex == -1) {
                data.Model = false;
            }
        }, 1)
        .then(function () {
            manageproductServices.GetFilteredProductList($scope).then(function (response) {
                $scope.ProductList = response.data;
                for (let i = 0; i < $scope.ProductList.length; i++) {
                    $scope.ProductList[i].AddedOn = moment.utc($scope.ProductList[i].AddedOn).local().format();
                }
            })
        })
    }

    //====================================================================Modal Operation=============================================================================
    $scope.openCreateProductModal = function () {
        manageproductServices.GetMasterMainCategoryList().then(function (response) {
            $scope.MasterMainCategoryList = response.data;
        })
        .then(function () {
            masterproductbrandServices.GetMasterProductBrandList().then(function (response) {
                $scope.MasterProductBrandList = response.data;
            })
                .then(function () {
                    manageproductServices.GetProductLabel().then(function (response) {
                        $scope.AllControls = response.data;
                    })
                    .then(function () {
                        manageproductServices.GetVehicleFitmentInfoList().then(function (response) {
                            $scope.AllFitmentInfo = response.data;
                        })
                        .then(function () {
                            $('#a_ProductDetails').trigger('click');
                        })
                        .then(function () {
                            $('#ProductModal').modal('show');
                        })
                    })
                })
        })
    }
    $scope.cancelCreateProductModal = function () {
        $('#ProductModal').modal('hide');
        $timeout(function () {
            $scope.MasterMainCategoryList = [];
            $scope.MasterProductBrandList = [];
            $scope.MasterSubCategoryList = [];
            $scope.MasterProductCategoryList = [];
            $scope.Product = {};
            $scope.AddControl = undefined;
            $scope.AddVehicleFitmentControl = undefined;
            $scope.MasterMainCategoryId = undefined;
            $scope.MasterSubCategoryId = undefined;
            $scope.MasterProductCategoryId = undefined;
            $scope.MasterProductBrandId = undefined;
            $scope.AllControls = [];
            $scope.AllFitmentInfo = [];
            $scope.FilteredControls = [];
            $scope.FilteredFitmentInfo = [];
            $scope.ProductForm.$setPristine();
            $scope.ProductForm.$setUntouched();
            $scope.ProductImageUploadForm.$setPristine();
            $scope.ProductImageUploadForm.$setUntouched();
            $scope.Loader = false;
            $scope.progressVisible = false;
            $scope.progress = 0;
        }, 200)
        .then(function () {
            if ($scope.files != undefined) {
                if ($scope.files.length > 0) {
                    let i = 0;
                    while (i == 0) {
                        $scope.removeFile($scope.files[i]);
                        if ($scope.files.length == 0) {
                            break;
                        }
                    }
                }
            }

            if ($scope.errFiles != undefined) {
                if ($scope.errFiles.length > 0) {
                    let i = 0;
                    while (i == 0) {
                        $scope.removeFile($scope.errFiles[i]);
                        if ($scope.errFiles.length == 0) {
                            break;
                        }
                    }
                }
            }
        })

    }

    $scope.openEditProductModal = function (productId) {
        GetProductDetailsForEditByProductIdWithModalOpen(productId);
    }

    $scope.openProductStockAndPriceModal = function (productId) {
        manageproductServices.GetProductStockByProductId(productId).then(function (response) {
            $scope.ProductStock = response.data;
        })
        .then(function () {
            manageproductServices.GetProductPriceByProductId(productId).then(function (response) {
                $scope.ProductPrice = response.data;
            })
        })
        .then(function () {
            manageproductServices.GetProductUnitPriceByDateByProductId(productId).then(function (response) {
                $scope.ProductUnitPriceByDateByProductIdDates = response.data.Dates;
                $scope.ProductUnitPriceByDateByProductIdData = response.data.Data;
            })
            .then(function () {
                manageproductServices.GetProductSalesPriceByDateByProductId(productId).then(function (response) {
                    $scope.ProductSalesPriceByDateByProductIdDates = response.data.Dates;
                    $scope.ProductSalesPriceByDateByProductIdData = response.data.Data;
                })
                .then(function () {
                    $('#a_ProductPrice').trigger('click');
                })
                .then(function () {
                    $('#ProductStockAndPriceModal').modal('show');
                })
            })
        })
    }
    $scope.cancelProductStockAndPriceModal = function () {
        $('#ProductStockAndPriceModal').modal('hide');
        $timeout(function () {
            $scope.ProductStock = {};
            $scope.ProductPrice = {};
            $scope.NewStock = undefined;
            $scope.ProductPriceForm.$setPristine();
            $scope.ProductPriceForm.$setUntouched();
            $scope.ProductStockForm.$setPristine();
            $scope.ProductStockForm.$setUntouched();

        })
    }

    $scope.openFilePreviewModal = function (fileUrl, filename) {
        //$scope.filePreview.ChatId = chatId;

        var $image = $("#imageAsync").first();
        var $downloadingImage = $("#imageAsync");
        $downloadingImage.load(function () {
            $image.attr("src", $(this).attr("src"));
        })
        $downloadingImage.attr("src", fileUrl);

        $scope.filePreview.FileName = filename;
        $scope.filePreview.FileURL = fileUrl;
        $('#FilePreviewModal').modal('show');
    }
    $scope.cancelFilePreviewModal = function () {
        //$downloadingImage.attr("src", "../../../Images/load.gif");
        $scope.filePreview = {};
        $('#FilePreviewModal').modal('hide');
    }

    //====================================================================Event Binding===============================================================================    
    $scope.CreateProduct = function () {
        $scope.Product.AdminId = $rootScope.AdminId;
        manageproductServices.CreateProduct($scope).then(function (response) {
            $scope.ProductIdMax = response.data.ProductIdMax;
            if (response.data.IsReport == "Success") {
                toastr.success(response.data.Message, "Success!");
            }
            else if (response.data.IsReport == "Warning") {
                toastr.warning(response.data.Message, "Warning!");
            }
            else if (response.data.IsReport == "Error") {
                toastr.error(response.data.Message, "Error!");
            }
        })
        .then(function () {
            $scope.MasterMainCategoryList = [];
            $scope.MasterProductBrandList = [];
            $scope.MasterSubCategoryList = [];
            $scope.MasterProductCategoryList = [];
            $scope.Product = {};
            $scope.AddControl = undefined;
            $scope.AddVehicleFitmentControl = undefined;
            $scope.MasterMainCategoryId = undefined;
            $scope.MasterSubCategoryId = undefined;
            $scope.MasterProductCategoryId = undefined;
            $scope.MasterProductBrandId = undefined;
            $scope.AllControls = [];
            $scope.AllFitmentInfo = [];
            $scope.FilteredControls = [];
            $scope.FilteredFitmentInfo = [];
            $scope.ProductForm.$setPristine();
            $scope.ProductForm.$setUntouched();
            $scope.ProductImageUploadForm.$setPristine();
            $scope.ProductImageUploadForm.$setUntouched();
        })
        .then(function () {
            if ($scope.files != undefined) {
                if ($scope.files.length > 0) {
                    let i = 0;
                    while (i == 0) {
                        $scope.removeFile($scope.files[i]);
                        if ($scope.files.length == 0) {
                            break;
                        }
                    }
                }
            }

            if ($scope.errFiles != undefined) {
                if ($scope.errFiles.length > 0) {
                    let i = 0;
                    while (i == 0) {
                        $scope.removeFile($scope.errFiles[i]);
                        if ($scope.errFiles.length == 0) {
                            break;
                        }
                    }
                }
            }
        })
        .then(function () {
            manageproductServices.GetFilteredProductList($scope).then(function (response) {
                $scope.ProductList = response.data;
                for (let i = 0; i < $scope.ProductList.length; i++) {
                    $scope.ProductList[i].AddedOn = moment.utc($scope.ProductList[i].AddedOn).local().format();
                }
            })
            .then(function () {
                // Reload Data
                GetProductDetailsForEditByProductId($scope.ProductIdMax);
            })
        })
    }

    $scope.UpdateProduct = function () {
        manageproductServices.UpdateProduct($scope).then(function (response) {
            if (response.data.IsReport == "Success") {
                toastr.success(response.data.Message, "Success!");
            }
            else if (response.data.IsReport == "Warning") {
                toastr.warning(response.data.Message, "Warning!");
            }
            else if (response.data.IsReport == "Error") {
                toastr.error(response.data.Message, "Error!");
            }
        })
        .then(function () {
            manageproductServices.GetFilteredProductList($scope).then(function (response) {
                $scope.ProductList = response.data;
                for (let i = 0; i < $scope.ProductList.length; i++) {
                    $scope.ProductList[i].AddedOn = moment.utc($scope.ProductList[i].AddedOn).local().format();
                }
            })
            .then(function () {
                $scope.cancelCreateProductModal();
            })
        })
    }

    $scope.UpdateProductStock = function () {
        manageproductServices.UpdateProductStock($scope.ProductStock.ProductId, $scope.NewStock, $rootScope.AdminId).then(function (response) {
            if (response.data.IsReport == "Success") {
                toastr.success(response.data.Message, "Success!");
                $scope.NewStock = undefined;
            }
            else if (response.data.IsReport == "Warning") {
                toastr.warning(response.data.Message, "Warning!");
            }
            else if (response.data.IsReport == "Error") {
                toastr.error(response.data.Message, "Error!");
            }
        })
        .then(function () {
            manageproductServices.GetProductStockByProductId($scope.ProductPrice.ProductId).then(function (response) {
                $scope.ProductStock = response.data;
            })
            .then(function () {
                $scope.ProductStockForm.$setPristine();
                $scope.ProductStockForm.$setUntouched();
            })
            .then(function () {
                LoadProductList();
            })
        })
    }

    $scope.UpdateProductPrice = function () {
        manageproductServices.UpdateProductPrice($scope.ProductPrice).then(function (response) {
            if (response.data.IsReport == "Success") {
                toastr.success(response.data.Message, "Success!");
            }
            else if (response.data.IsReport == "Warning") {
                toastr.warning(response.data.Message, "Warning!");
            }
            else if (response.data.IsReport == "Error") {
                toastr.error(response.data.Message, "Error!");
            }
        })
        .then(function () {
            manageproductServices.GetProductPriceByProductId($scope.ProductPrice.ProductId).then(function (response) {
                $scope.ProductPrice = response.data;
            })
            .then(function () {
                $scope.RefreshProductUnitPriceByDateByProductId()
            })
            .then(function () {
                $scope.RefreshProductSalesPriceByDateByProductId()
            })
            .then(function () {
                LoadProductList();
            })
        })
    }

    $scope.UpdateProductStockAndPrice = function () {
        manageproductServices.UpdateProductStock($scope.ProductStock.ProductId, $scope.NewStock, $rootScope.AdminId).then(function (response) {
            if (response.data.IsReport == "Success") {
                toastr.success(response.data.Message, "Success!");
            }
            else if (response.data.IsReport == "Warning") {
                toastr.warning(response.data.Message, "Warning!");
            }
            else if (response.data.IsReport == "Error") {
                toastr.error(response.data.Message, "Error!");
            }
        })
        .then(function () {
            manageproductServices.UpdateProductPrice($scope.ProductPrice).then(function (response) {
                if (response.data.IsReport == "Success") {
                    toastr.success(response.data.Message, "Success!");
                }
                else if (response.data.IsReport == "Warning") {
                    toastr.warning(response.data.Message, "Warning!");
                }
                else if (response.data.IsReport == "Error") {
                    toastr.error(response.data.Message, "Error!");
                }
            })
            .then(function () {
                LoadProductList();
            })
            .then(function () {
                $scope.cancelProductStockAndPriceModal();
            })
        })
    }

    $scope.RefreshProductUnitPriceByDateByProductId = function () {
        manageproductServices.GetProductUnitPriceByDateByProductId($scope.ProductPrice.ProductId).then(function (response) {
            $scope.ProductUnitPriceByDateByProductIdDates = response.data.Dates;
            $scope.ProductUnitPriceByDateByProductIdData = response.data.Data;
        })
    }

    $scope.RefreshProductSalesPriceByDateByProductId = function () {
        manageproductServices.GetProductSalesPriceByDateByProductId($scope.ProductPrice.ProductId).then(function (response) {
            $scope.ProductSalesPriceByDateByProductIdDates = response.data.Dates;
            $scope.ProductSalesPriceByDateByProductIdData = response.data.Data;
        })
    }

    $scope.DeleteImageByImageId = function (productImageId, productId) {
        swal({
            title: "Are you sure?",
            text: "You are going to delete this image",
            type: "error",
            showCancelButton: true,
            confirmButtonClass: "btn-danger",
            confirmButtonText: "Yes, delete it!",
            closeOnConfirm: true
        },
        function () {
            manageproductServices.DeleteImageByImageId(productImageId).then(function (response) {
                if (response.data.IsReport == "Success") {
                    toastr.success(response.data.Message, "Success!");
                }
                else if (response.data.IsReport == "Warning") {
                    toastr.warning(response.data.Message, "Warning!");
                }
                else if (response.data.IsReport == "Error") {
                    toastr.error(response.data.Message, "Error!");
                }
            })
            .then(function () {
                GetProductDetailsForEditByProductId(productId);
            })
        })
    }

    $scope.SetAsProductHeaderImage = function (productImageId, productId) {
        manageproductServices.SetAsProductHeaderImage(productImageId).then(function (response) {
            if (response.data.IsReport == "Success") {
                toastr.success(response.data.Message, "Success!");
            }
            else if (response.data.IsReport == "Warning") {
                toastr.warning(response.data.Message, "Warning!");
            }
            else if (response.data.IsReport == "Error") {
                toastr.error(response.data.Message, "Error!");
            }
        })
        .then(function () {
            GetProductDetailsForEditByProductId(productId);
        })
    }

    $scope.UpdateDiscountPercentageByProductId = function (id, value) {
        manageproductServices.UpdateDiscountPercentageByProductId(id, value).then(function (response) {
            if (response.data.IsReport == "Success") {
                toastr.success(response.data.Message, "Success!");
            }
            else if (response.data.IsReport == "Warning") {
                toastr.warning(response.data.Message, "Warning!");
            }
            else if (response.data.IsReport == "Error") {
                toastr.error(response.data.Message, "Error!");
            }
        })
        .then(function () {
            LoadProductList();
        })
    }

    $scope.ToggleDiscountByProductId = function (id, isDiscountActive) {
        console.log(isDiscountActive);
        swal({
            title: "Are you sure?",
            text: isDiscountActive ? "You are going to enable discount for this product" : "You are going to disable discount for this product",
            type: "warning",
            showCancelButton: true,
            confirmButtonClass: "btn-success",
            confirmButtonText: isDiscountActive ? "Yes! Enable Discount" : "Yes! Disable Discount",
            closeOnConfirm: true,
            closeOnCancel: true
        },
        function (isConfirm) {
            if (isConfirm) {
                manageproductServices.ToggleDiscountByProductId(id).then(function (response) {
                    if (response.data.IsReport == "Success") {
                        toastr.success(response.data.Message, "Success!");
                    }
                    else if (response.data.IsReport == "Warning") {
                        toastr.warning(response.data.Message, "Warning!");
                    }
                    else if (response.data.IsReport == "Error") {
                        toastr.error(response.data.Message, "Error!");
                    }
                })
                        .then(function () {
                            LoadProductList();
                        })
            }
            else {
                LoadProductList();
            }
        })
    }
    //--------------Download------------
    $scope.downloadFile = function (filename) {
        manageproductServices.DownloadFileByName(filename).then(function (response) {
            if (response.status == 200) {
                download("data:" + response.data.mimetype + ";base64," + response.data.base64File, filename, response.data.mimetype);
            }
            else if (response.status == 204) {
                toastr.error("Can't download file due to " + response.statusText + ". Please disable IDM extension/add-on for downloading file from our site", "Error!");
            }
        })
    }


    //====================================================================Miscellaneous Function======================================================================
    function GetProductDetailsForEditByProductId(productId) {
        manageproductServices.GetproductDetailsForEdit(productId).then(function (response) {
            $scope.Product = response.data.Product;
            $scope.FilteredControls = response.data.TempMasterProductSpecificationLabelList;
            $scope.FilteredFitmentInfo = response.data.TempVehicleFitmentList;
            $scope.ImageList = response.data.ImageList;
        })
        .then(function () {
            manageproductServices.GetMasterMainCategoryList().then(function (response) {
                $scope.MasterMainCategoryList = response.data;
            })
            .then(function () {
                masterproductbrandServices.GetMasterProductBrandList().then(function (response) {
                    $scope.MasterProductBrandList = response.data;
                })
                .then(function () {
                    manageproductServices.GetProductLabel().then(function (response) {
                        $scope.AllControls = response.data;
                    })
                    .then(function () {
                        manageproductServices.GetVehicleFitmentInfoList().then(function (response) {
                            $scope.AllFitmentInfo = response.data;
                        })
                        .then(function () {
                            // Process AllControls
                            for (let i = 0; i < $scope.FilteredControls.length; i++) {
                                let thisAllControlIndex = $scope.AllControls.findIndex(x=>x.Id == $scope.FilteredControls[i].Id);
                                if (thisAllControlIndex != -1) {
                                    $scope.AllControls.splice(thisAllControlIndex, 1);
                                }
                            }
                        })
                        .then(function () {
                            // Process Fitment Information
                            for (let i = 0; i < $scope.FilteredFitmentInfo.length; i++) {
                                let thisAllFitmentInfoIndex = $scope.FilteredFitmentInfo.findIndex(x=>x.Id == $scope.FilteredFitmentInfo[i].Id);
                                if (thisAllFitmentInfoIndex != -1) {
                                    $scope.AllFitmentInfo.splice(thisAllFitmentInfoIndex, 1);
                                }
                            }
                        })
                        .then(function () {
                            // Process MasterMainCategory
                            let thisMasterMainCategoryId = $scope.MasterMainCategoryList.findIndex(x=>x.Id == $scope.Product.MasterMainCategoryId);
                            $scope.MasterMainCategoryId = $scope.MasterMainCategoryList[thisMasterMainCategoryId];
                        })
                        .then(function () {
                            manageproductServices.GetMasterSubCategoryList($scope.Product.MasterMainCategoryId).then(function (response) {
                                $scope.MasterSubCategoryList = response.data;
                            })
                            .then(function () {
                                // Process MasterSubCategory
                                let thisMasterSubCategoryId = $scope.MasterSubCategoryList.findIndex(x=>x.Id == $scope.Product.MasterSubCategoryId);
                                $scope.MasterSubCategoryId = $scope.MasterSubCategoryList[thisMasterSubCategoryId];
                            })
                            .then(function () {
                                manageproductServices.GetMasterProductCategoryList($scope.Product.MasterMainCategoryId, $scope.Product.MasterSubCategoryId).then(function (response) {
                                    $scope.MasterProductCategoryList = response.data;
                                })
                                .then(function () {
                                    // Process MasterProdductCategory
                                    let thisMasterProductCategoryId = $scope.MasterProductCategoryList.findIndex(x=>x.Id == $scope.Product.MasterProductCategoryId);
                                    $scope.MasterProductCategoryId = $scope.MasterProductCategoryList[thisMasterProductCategoryId];
                                })
                                .then(function () {
                                    // Process MasterBrand
                                    let thisMasterProductBrandId = $scope.MasterProductBrandList.findIndex(x=>x.Id == $scope.Product.MasterProductBrandId);
                                    $scope.MasterProductBrandId = $scope.MasterProductBrandList[thisMasterProductBrandId];
                                })
                            })
                        })
                    })
                })
            })
        })
    }

    function GetProductDetailsForEditByProductIdWithModalOpen(productId) {
        manageproductServices.GetproductDetailsForEdit(productId).then(function (response) {
            $scope.Product = response.data.Product;
            $scope.FilteredControls = response.data.TempMasterProductSpecificationLabelList;
            $scope.FilteredFitmentInfo = response.data.TempVehicleFitmentList;
            $scope.ImageList = response.data.ImageList;
        })
        .then(function () {
            manageproductServices.GetMasterMainCategoryList().then(function (response) {
                $scope.MasterMainCategoryList = response.data;
            })
            .then(function () {
                masterproductbrandServices.GetMasterProductBrandList().then(function (response) {
                    $scope.MasterProductBrandList = response.data;
                })
                .then(function () {
                    manageproductServices.GetProductLabel().then(function (response) {
                        $scope.AllControls = response.data;
                    })
                    .then(function () {
                        manageproductServices.GetVehicleFitmentInfoList().then(function (response) {
                            $scope.AllFitmentInfo = response.data;
                        })
                        .then(function () {
                            // Process AllControls
                            for (let i = 0; i < $scope.FilteredControls.length; i++) {
                                let thisAllControlIndex = $scope.AllControls.findIndex(x=>x.Id == $scope.FilteredControls[i].Id);
                                if (thisAllControlIndex != -1) {
                                    $scope.AllControls.splice(thisAllControlIndex, 1);
                                }
                            }
                        })
                        .then(function () {
                            // Process Fitment Information
                            for (let i = 0; i < $scope.FilteredFitmentInfo.length; i++) {
                                let thisAllFitmentInfoIndex = $scope.FilteredFitmentInfo.findIndex(x=>x.Id == $scope.FilteredFitmentInfo[i].Id);
                                if (thisAllFitmentInfoIndex != -1) {
                                    $scope.AllFitmentInfo.splice(thisAllFitmentInfoIndex, 1);
                                }
                            }
                        })
                        .then(function () {
                            // Process MasterMainCategory
                            let thisMasterMainCategoryId = $scope.MasterMainCategoryList.findIndex(x=>x.Id == $scope.Product.MasterMainCategoryId);
                            $scope.MasterMainCategoryId = $scope.MasterMainCategoryList[thisMasterMainCategoryId];
                        })
                        .then(function () {
                            manageproductServices.GetMasterSubCategoryList($scope.Product.MasterMainCategoryId).then(function (response) {
                                $scope.MasterSubCategoryList = response.data;
                            })
                            .then(function () {
                                // Process MasterSubCategory
                                let thisMasterSubCategoryId = $scope.MasterSubCategoryList.findIndex(x=>x.Id == $scope.Product.MasterSubCategoryId);
                                $scope.MasterSubCategoryId = $scope.MasterSubCategoryList[thisMasterSubCategoryId];
                            })
                            .then(function () {
                                manageproductServices.GetMasterProductCategoryList($scope.Product.MasterMainCategoryId, $scope.Product.MasterSubCategoryId).then(function (response) {
                                    $scope.MasterProductCategoryList = response.data;
                                })
                                .then(function () {
                                    // Process MasterProdductCategory
                                    let thisMasterProductCategoryId = $scope.MasterProductCategoryList.findIndex(x=>x.Id == $scope.Product.MasterProductCategoryId);
                                    $scope.MasterProductCategoryId = $scope.MasterProductCategoryList[thisMasterProductCategoryId];
                                })
                                .then(function () {
                                    // Process MasterBrand
                                    let thisMasterProductBrandId = $scope.MasterProductBrandList.findIndex(x=>x.Id == $scope.Product.MasterProductBrandId);
                                    $scope.MasterProductBrandId = $scope.MasterProductBrandList[thisMasterProductBrandId];
                                })
                                .then(function () {
                                    $('#a_ProductDetails').trigger('click');
                                })
                                .then(function () {
                                    $('#ProductModal').modal('show');
                                })
                            })
                        })
                    })
                })
            })
        })
    }


    // ================================================== File Operation =============================================

    //---File Operation
    $scope.ValidFiles = [];
    $scope.InValidFiles = [];
    var formdata = new FormData();
    var xhr;
    $scope.files = [];
    $scope.errFiles = [];
    $scope.Loader = false;
    $scope.progressVisible = false;
    $scope.progress = 0;
    $scope.TaskAttachments = [];
    $scope.file = {};
    //$scope.MaxLimitToUpload = '10KB';
    $scope.filePreview = {};

    //Watchman for ng-model 'files'
    $scope.$watch('files', function () {
        $scope.addFilesIntoFileStack($scope.files, $scope.errFiles);
    });

    $scope.addFilesIntoFileStack = function (files, errFiles) {
        $scope.progress = 0;
        $scope.files = files;
        $scope.errFiles = errFiles;


        //Precess FormData();
        formdata = new FormData();
        if ($scope.files != undefined) {
            for (var i = 0; i < $scope.files.length; i++) {
                formdata.append(0, $scope.files[i]);
            }
        }
        for (var pair of formdata.entries()) {
            console.log(pair[0] + ', ' + pair[1]);
        }
    };

    $scope.removeFile = function (model) {
        //Remove Valid Files
        var validIndex = $scope.files.findIndex(x => x.$$hashKey == model.$$hashKey);
        if (validIndex > -1) {
            $scope.files.splice(validIndex, 1);
            formdata = new FormData();
            if ($scope.files != undefined) {
                for (var i = 0; i < $scope.files.length; i++) {
                    formdata.append(0, $scope.files[i]);
                }
            }
        }
        //Removbe Invalid Files
        var inValidIndex = $scope.errFiles.findIndex(x => x.$$hashKey == model.$$hashKey);
        if (inValidIndex > -1) {
            $scope.errFiles.splice(inValidIndex, 1);
        }
    }

    //Upload Attachment (Update)
    $scope.uploadFiles = function () {
        if ($scope.files != undefined && $scope.files.length != 0 && $scope.Loader == false && $scope.files.length <= 10) {
            $scope.Loader = true;
            $scope.progressVisible = true;
            $http({
                method: 'POST',
                url: "/Api/Product/UploadProductImage/" + $scope.Product.Id + '/' + $rootScope.AdminId,
                headers: {
                    'Content-Type': undefined
                },
                eventHandlers: {
                    progress: function (c) {
                    }
                },
                uploadEventHandlers: {
                    progress: function (e) {
                        console.log('UploadProgress -> ' + e);
                        $scope.progress = parseInt(e.loaded / e.total * 100, 10);
                        //blockUI.instances.get('myBlock').start();
                        //blockUI.instances.get('myBlock').message("Uploading ... " + $scope.progress + "%");
                    }
                },
                data: formdata,
                transformRequest: angular.identity
            })
            .success(function (data) {
                if (data.IsReport != "Warning" && data.IsReport != "Error") {
                    let Successful_Upload_text = data.Successful_Upload.toString() + " Images uploaded successfully.";
                    let Failed_Upload_Format_text = data.Failed_Upload_Format.toString() + " Images upload failed due to invalid format.";
                    let Failed_Upload_Size_text = data.Failed_Upload_Size.toString() + " Images upload failed due to invalid size";
                    let Failed_Upload_text = data.Failed_Upload.toString() + " Images  upload failed due to internal server issue.";
                    if (data.Successful_Upload > 0) {
                        toastr.success(Successful_Upload_text);
                    }
                    else {
                        toastr.error(data.Failed_Upload_Format > 0 ? Failed_Upload_Format_text : "" + data.Failed_Upload_Size > 0 ? Failed_Upload_Size_text : "" + data.Failed_Upload > 0 ? Failed_Upload_text : "")
                    }
                }

                if (data.IsReport == "Warning") {
                    toastr.error(data.Message, "Error!");
                }
                else if (data.IsReport == "Error") {
                    toastr.error(data.Message, "Error!");
                }
            })
            .error(function (data, status) {
                toastr.error(data, status);
            })
            .then(function () {
                //blockUI.instances.get('myBlock').stop();
            })
            .then(function () {
                $timeout(function () {
                    $scope.ProductImageUploadForm.$setPristine();
                    $scope.ProductImageUploadForm.$setUntouched();
                    $scope.Loader = false;
                    $scope.progressVisible = false;
                    $scope.progress = 0;
                }, 200)
                .then(function () {
                    if ($scope.files != undefined) {
                        if ($scope.files.length > 0) {
                            let i = 0;
                            while (i == 0) {
                                $scope.removeFile($scope.files[i]);
                                if ($scope.files.length == 0) {
                                    break;
                                }
                            }
                        }
                    }

                    if ($scope.errFiles != undefined) {
                        if ($scope.errFiles.length > 0) {
                            let i = 0;
                            while (i == 0) {
                                $scope.removeFile($scope.errFiles[i]);
                                if ($scope.errFiles.length == 0) {
                                    break;
                                }
                            }
                        }
                    }
                })
                .then(function () {
                    GetProductDetailsForEditByProductId($scope.Product.Id);
                })
            })
        }
        else {
            toastr.info("There are some invalid parameter associated with upload process. Please try again later", "Error!");
        }
    }

    //====================================================================Garbage Code================================================================================




});