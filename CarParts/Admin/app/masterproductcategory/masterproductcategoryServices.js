CarPartsApp.factory("masterproductcategoryServices", ["$http", "$rootScope", "$window", function ($http, $rootScope, $window) {
    return {
        GetMasterProductCategoryList: function () {
            return $http({
                url: "/Api/MasterProductCategory/GetMasterProductCategoryList",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                method: "GET",
                async: false
            });
        },
        CreateMasterProductCategory: function (MasterProductCategory) {
            return $http({
                url: "/Api/MasterProductCategory/CreateMasterProductCategory",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                data: MasterProductCategory,
                method: "POST",
                async: false
            });
        },
        UpdateMasterProductCategory: function (id, value) {
            return $http({
                url: "/Api/MasterProductCategory/UpdateMasterProductCategory",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                data: {
                    MasterProductCategoryId: id,
                    Value: value
                },
                method: "POST",
                async: false
            });
        },
        GetMasterProductCategoryList: function () {
            return $http({
                url: "/Api/MasterProductCategory/GetMasterProductCategoryList",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                method: "GET",
                async: false
            });
        },
        GetMasterProductCategoryDetails: function (id) {
            return $http({
                url: "/Api/MasterProductCategory/GetMasterProductCategoryDetails/" + id,
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