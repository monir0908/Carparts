CarPartsApp.factory("productServices", function ($http) {
    return {
        GetProductListByProductCategory: function (productCategoryId, tempVehicleFilter) {
            return $http({
                url: "/Api/Product/GetProductListByProductCategory",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                method: "POST",
                data: {
                    ProductCategoryId: productCategoryId,
                    TempVehicleFilter: tempVehicleFilter
                },
                async: false
            });
        },
    };
});


