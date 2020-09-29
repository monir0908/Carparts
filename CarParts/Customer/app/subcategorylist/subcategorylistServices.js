CarPartsApp.factory("subcategorylistServices", function ($http) {
    return {
        GetAllSubCategoryListByCategoryId: function (categoryId, tempVehicleFilter) {
            return $http({
                url: "/Api/Product/GetAllSubCategoryListByCategoryId",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                method: "POST",
                data: {
                    CategoryId: categoryId,
                    TempVehicleFilter: tempVehicleFilter
                },
                async: false
            });
        },
    };
});


