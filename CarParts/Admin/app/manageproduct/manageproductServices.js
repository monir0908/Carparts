CarPartsApp.factory("manageproductServices", ["$http", "$rootScope", "$window", function ($http, $rootScope, $window) {
    return {
        CreateProduct: function ($scope) {
            return $http({
                url: "/Api/Product/CreateProduct",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                data: {
                    Product: $scope.Product,
                    TempVehicleFitmentList: $scope.FilteredFitmentInfo,
                    TempMasterProductSpecificationLabelList: $scope.FilteredControls
                },
                method: "POST",
                async: false
            });
        },
        UpdateProduct: function ($scope) {
            return $http({
                url: "/Api/Product/UpdateProduct",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                data: {
                    Product: $scope.Product,
                    TempVehicleFitmentList: $scope.FilteredFitmentInfo,
                    TempMasterProductSpecificationLabelList: $scope.FilteredControls
                },
                method: "POST",
                async: false
            });
        },
        //UpdateMasterMainCategory: function (id, value) {
        //    return $http({
        //        url: "/Api/MasterMainCategory/UpdateMasterMainCategory",
        //        headers: {
        //            "content-type": "application/json",
        //            "cache-control": "no-cache"
        //        },
        //        data: {
        //            MasterMainCategoryId: id,
        //            Value: value
        //        },
        //        method: "POST",
        //        async: false
        //    });
        //},
        UpdateDiscountPercentageByProductId: function (id, value) {
            return $http({
                url: "/Api/Product/UpdateDiscountPercentageByProductId/" + id + '/' + value,
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                method: "POST",
                async: false
            });
        },
        ToggleDiscountByProductId: function (id) {
            return $http({
                url: "/Api/Product/ToggleDiscountByProductId/" + id,
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                method: "POST",
                async: false
            });
        },
        GetProductList: function () {
            return $http({
                url: "/Api/Product/GetProductList",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                method: "GET",
                async: false
            });
        },
        GetMasterMainCategoryList: function () {
            return $http({
                url: "/Api/MasterMainCategory/GetMasterMainCategoryList",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                method: "GET",
                async: false
            });
        },
        GetMasterSubCategoryList: function (masterMainCategoryId) {
            return $http({
                url: "/Api/Product/GetMasterSubCategoryList/" + masterMainCategoryId,
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                method: "GET",
                async: false
            });
        },
        GetMasterProductCategoryList: function (masterMainCategoryId, masterSubCategoryId) {
            return $http({
                url: "/Api/Product/GetMasterProductCategoryList/" + masterMainCategoryId + '/' + masterSubCategoryId,
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                method: "GET",
                async: false
            });
        },
        GetProductHierarchy: function () {
            return $http({
                url: "/Api/Product/GetProductHierarchy",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                method: "GET",
                async: false
            });
        },
        GetProductLabel: function () {
            return $http({
                url: "/Api/Product/GetProductLabel",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                method: "GET",
                async: false
            });
        },
        GetVehicleFitmentInfoList: function () {
            return $http({
                url: "/Api/Product/GetVehicleFitmentInfoList",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                method: "GET",
                async: false
            });
        },
        GetFilteredProductList: function ($scope) {
            return $http({
                url: "/Api/Product/GetFilteredProductList",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                method: "POST",
                data: $scope.ProductHierarchy,
                async: false
            });
        },
        GetProductList: function () {
            return $http({
                url: "/Api/Product/GetProductList",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                method: "GET",
                async: false
            });
        },
        GetproductDetailsForEdit: function (productId) {
            return $http({
                url: "/Api/Product/GetproductDetailsForEdit/" + productId,
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                method: "GET",
                async: false
            });
        },
        DeleteImageByImageId: function (productImageId) {
            return $http({
                url: "/api/Product/DeleteImageByImageId/" + productImageId,
                method: "POST",
                async: false
            });
        },
        SetAsProductHeaderImage: function (productImageId) {
            return $http({
                url: "/api/Product/SetAsProductHeaderImage/" + productImageId,
                method: "POST",
                async: false
            });
        },
        DownloadFileByName: function (filename) {
            return $http({
                url: "/api/Product/DownloadFileByName/" + filename,
                method: "GET",
                async: false
            });
        },
        GetProductStockByProductId: function (productId) {
            return $http({
                url: "/Api/Product/GetProductStockByProductId/" + productId,
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                method: "GET",
                async: false
            });
        },
        GetProductPriceByProductId: function (productId) {
            return $http({
                url: "/Api/Product/GetProductPriceByProductId/" + productId,
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                method: "GET",
                async: false
            });
        },
        UpdateProductStock: function (productId, stock, adminId) {
            return $http({
                url: "/Api/Product/UpdateProductStock/" + productId + "/" + stock + "/" + adminId,
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                method: "POST",
                async: false
            });
        },
        UpdateProductPrice: function (productPrice) {
            return $http({
                url: "/Api/Product/UpdateProductPrice",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                data: productPrice,
                method: "POST",
                async: false
            });
        },
        GetProductUnitPriceByDateByProductId: function (productId) {
            return $http({
                url: "/Api/Product/GetProductUnitPriceByDateByProductId/" + productId,
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                method: "GET",
                async: false
            });
        },
        GetProductSalesPriceByDateByProductId: function (productId) {
            return $http({
                url: "/Api/Product/GetProductSalesPriceByDateByProductId/" + productId,
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                method: "GET",
                async: false
            });
        },


    };
}]);