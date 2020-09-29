CarPartsApp.factory("mastersubcategoryServices", ["$http", "$rootScope", "$window", function ($http, $rootScope, $window) {
    return {
        GetMasterSubCategoryList: function () {
            return $http({
                url: "/Api/MasterSubCategory/GetMasterSubCategoryList",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                method: "GET",
                async: false
            });
        },
        CreateMasterSubCategory: function (MasterSubCategory) {
            return $http({
                url: "/Api/MasterSubCategory/CreateMasterSubCategory",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                data: MasterSubCategory,
                method: "POST",
                async: false
            });
        },
        UpdateMasterSubCategory: function (id, value) {
            return $http({
                url: "/Api/MasterSubCategory/UpdateMasterSubCategory",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                data: {
                    MasterSubCategoryId: id,
                    Value: value
                },
                method: "POST",
                async: false
            });
        },
        GetMasterSubCategoryList: function () {
            return $http({
                url: "/Api/MasterSubCategory/GetMasterSubCategoryList",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                method: "GET",
                async: false
            });
        },
        GetMasterSubCategoryDetails: function (id) {
            return $http({
                url: "/Api/MasterSubCategory/GetMasterSubCategoryDetails/" + id,
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                method: "GET",
                async: false
            });
        }
    };
}]);