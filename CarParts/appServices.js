CarPartsApp.factory("appServices", ["$http", "$rootScope", "$q", "customServices", "$timeout", function ($http, $rootScope, $q, customServices, $timeout) {
    return {
        Logout: function (param) {
            return $http({
                url: "/Api/Common/LogOutByCustomerId/" + param,
                method: "POST",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                async: false
            });
        },
        GetAuthorizationToken: function (param1, param2) {
            return $http({
                url: "/Api/Authenticate/LogIn",
                method: "POST",
                headers: {
                    "authorization": "Basic " + param1,
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                data: param2,
                async: false
            });
        },

        RegisterCustomer: function (customer) {
            return $http({
                url: "/Api/Register/RegisterCustomer",
                method: "POST",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                data: customer,
                async: false
            });
        },
        GetCompanyDetailsForWebSite: function () {
            return $http({
                url: "/api/Company/GetCompanyDetailsForWebSite",
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
        GetMasterVehicleYearList: function () {
            return $http({
                url: "/Api/MasterVehicleYear/GetMasterVehicleYearList",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                method: "GET",
                async: false
            });
        },
        GetMakerListByYearId: function (yearId) {
            return $http({
                url: "/Api/Vehicle/GetMakerListByYearId/" + yearId,
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                method: "GET",
                async: false
            });
        },
        GetModelListByMakerId: function (makerId) {
            return $http({
                url: "/Api/Vehicle/GetModelListByMakerId/" + makerId,
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                method: "GET",
                async: false
            });
        },
        GetSubModelListByModelId: function (modelId) {
            return $http({
                url: "/Api/Vehicle/GetSubModelListByModelId/" + modelId,
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                method: "GET",
                async: false
            });
        },
        GetEngineListBySubModelId: function (subModelId) {
            return $http({
                url: "/Api/Vehicle/GetEngineListBySubModelId/" + subModelId,
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                method: "GET",
                async: false
            });
        },
        SearchByVehicleWithOutSubModelAndEngine: function (yearId, makerId, modelId) {
            return $http({
                url: "/Api/Vehicle/SearchByVehicleWithOutSubModelAndEngine/" + yearId + '/' + makerId + '/' + modelId,
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                method: "GET",
                async: false
            });
        },
        SearchByVehicleWithSubModelAndEngine: function (yearId, makerId, modelId, subModelId, engineId) {
            return $http({
                url: "/Api/Vehicle/SearchByVehicleWithSubModelAndEngine/" + yearId + '/' + makerId + '/' + modelId + '/' + subModelId + '/' + engineId,
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                method: "GET",
                async: false
            });
        },

    }

}]);