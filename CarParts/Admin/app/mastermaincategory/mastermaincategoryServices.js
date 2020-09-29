CarPartsApp.factory("mastermaincategoryServices", ["$http", "$rootScope", "$window", function ($http, $rootScope, $window) {
    return {
        CreateMasterMainCategory: function (MasterMainCategory) {
            return $http({
                url: "/Api/MasterMainCategory/CreateMasterMainCategory",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                data: MasterMainCategory,
                method: "POST",
                async: false
            });
        },
        UpdateMasterMainCategory: function (id, value) {
            return $http({
                url: "/Api/MasterMainCategory/UpdateMasterMainCategory",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                data: {
                    MasterMainCategoryId: id,
                    Value: value
                },
                method: "POST",
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
        GetMasterMainCategoryDetails: function (id) {
            return $http({
                url: "/Api/MasterMainCategory/GetMasterMainCategoryDetails/" + id,
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