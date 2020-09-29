CarPartsApp.factory("masterproductbrandServices", ["$http", "$rootScope", "$window", function ($http, $rootScope, $window) {
    return {
        GetMasterProductBrandList: function () {
            return $http({
                url: "/Api/MasterProductBrand/GetMasterProductBrandList",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                method: "GET",
                async: false
            });
        },
        CreateMasterProductBrand: function (MasterProductBrand) {
            return $http({
                url: "/Api/MasterProductBrand/CreateMasterProductBrand",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                data: MasterProductBrand,
                method: "POST",
                async: false
            });
        },
        UpdateMasterProductBrand: function (id, value) {
            return $http({
                url: "/Api/MasterProductBrand/UpdateMasterProductBrand",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                data: {
                    MasterProductBrandId: id,
                    Value: value
                },
                method: "POST",
                async: false
            });
        },
        GetMasterProductBrandList: function () {
            return $http({
                url: "/Api/MasterProductBrand/GetMasterProductBrandList",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                method: "GET",
                async: false
            });
        },
        GetMasterProductBrandDetails: function (id) {
            return $http({
                url: "/Api/MasterProductBrand/GetMasterProductBrandDetails/" + id,
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