CarPartsApp.factory("productcategorylistServices", function ($http) {
    return {
        GetAllProductCategoryListBySubCategoryId: function (subCategoryId, tempVehicleFilter) {
            return $http({
                url: "/Api/Product/GetAllProductCategoryListBySubCategoryId",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                method: "POST",
                data: {
                    SubCategoryId: subCategoryId,
                    TempVehicleFilter: tempVehicleFilter
                },
                async: false
            });
        },
    };
});


