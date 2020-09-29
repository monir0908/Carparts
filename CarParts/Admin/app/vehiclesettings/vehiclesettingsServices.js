CarPartsApp.factory("vehiclesettingsServices", ["$http", "$rootScope", "$window", function ($http, $rootScope, $window) {
    return {
        CreateMasterVehicleYear: function (MasterVehicleYear) {
            return $http({
                url: "/Api/MasterVehicleYear/CreateMasterVehicleYear",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                data: MasterVehicleYear,
                method: "POST",
                async: false
            });
        },
        CreateMasterVehicleMaker: function (MasterVehicleMaker) {
            return $http({
                url: "/Api/MasterVehicleMaker/CreateMasterVehicleMaker",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                data: MasterVehicleMaker,
                method: "POST",
                async: false
            });
        },
        CreateMasterVehicleModel: function (MasterVehicleModel) {
            return $http({
                url: "/Api/MasterVehicleModel/CreateMasterVehicleModel",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                data: MasterVehicleModel,
                method: "POST",
                async: false
            });
        },
        CreateMasterVehicleSubModel: function (MasterVehicleSubModel) {
            return $http({
                url: "/Api/MasterVehicleSubModel/CreateMasterVehicleSubModel",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                data: MasterVehicleSubModel,
                method: "POST",
                async: false
            });
        },
        CreateMasterVehicleEngine: function (MasterVehicleEngine) {
            return $http({
                url: "/Api/MasterVehicleEngine/CreateMasterVehicleEngine",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                data: MasterVehicleEngine,
                method: "POST",
                async: false
            });
        },
        UpdateMasterVehicleYear: function (id, value) {
            return $http({
                url: "/Api/MasterVehicleYear/UpdateMasterVehicleYear",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                data: {
                    MasterVehicleYearId: id,
                    Value: value
                },
                method: "POST",
                async: false
            });
        },
        UpdateMasterVehicleMaker: function (id, value) {
            return $http({
                url: "/Api/MasterVehicleMaker/UpdateMasterVehicleMaker",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                data: {
                    MasterVehicleMakerId: id,
                    Value: value
                },
                method: "POST",
                async: false
            });
        },
        UpdateMasterVehicleModel: function (id, value) {
            return $http({
                url: "/Api/MasterVehicleModel/UpdateMasterVehicleModel",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                data: {
                    MasterVehicleModelId: id,
                    Value: value
                },
                method: "POST",
                async: false
            });
        },
        UpdateMasterVehicleSubModel: function (id, value) {
            return $http({
                url: "/Api/MasterVehicleSubModel/UpdateMasterVehicleSubModel",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                data: {
                    MasterVehicleSubModelId: id,
                    Value: value
                },
                method: "POST",
                async: false
            });
        },
        UpdateMasterVehicleEngine: function (id, value) {
            return $http({
                url: "/Api/MasterVehicleEngine/UpdateMasterVehicleEngine",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                data: {
                    MasterVehicleEngineId: id,
                    Value: value
                },
                method: "POST",
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
        GetMasterVehicleMakerList: function () {
            return $http({
                url: "/Api/MasterVehicleMaker/GetMasterVehicleMakerList",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                method: "GET",
                async: false
            });
        },
        GetMasterVehicleModelList: function () {
            return $http({
                url: "/Api/MasterVehicleModel/GetMasterVehicleModelList",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                method: "GET",
                async: false
            });
        },
        GetMasterVehicleSubModelList: function () {
            return $http({
                url: "/Api/MasterVehicleSubModel/GetMasterVehicleSubModelList",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                method: "GET",
                async: false
            });
        },
        GetMasterVehicleEngineList: function () {
            return $http({
                url: "/Api/MasterVehicleEngine/GetMasterVehicleEngineList",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                method: "GET",
                async: false
            });
        },
        GetMaxYear: function () {
            return $http({
                url: "/Api/MasterVehicleYear/GetMaxYear",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                method: "GET",
                async: false
            });
        },
        GetMasterVehicleSubModelFilteredList: function (MasterVehicleYearId, MasterVehicleMakerId, MasterVehicleModelId) {
            return $http({
                url: "/Api/MasterVehicleSubModel/GetMasterVehicleSubModelFilteredList/" + MasterVehicleYearId + "/" + MasterVehicleMakerId + "/" + MasterVehicleModelId,
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                method: "GET",
                async: false
            });
        },
        GetMasterVehicleEngineFilteredList: function (MasterVehicleYearId, MasterVehicleMakerId, MasterVehicleModelId, MasterVehicleSubModelId) {
            return $http({
                url: "/Api/MasterVehicleEngine/GetMasterVehicleEngineFilteredList/" + MasterVehicleYearId + "/" + MasterVehicleMakerId + "/" + MasterVehicleModelId + "/" + MasterVehicleSubModelId,
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                method: "GET",
                async: false
            });
        },
        CreateVehicle: function (vehicle) {
            return $http({
                url: "/Api/Vehicle/CreateVehicle",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                method: "POST",
                data: vehicle,
                async: false
            });
        },
        GetVehicleList: function () {
            return $http({
                url: "/Api/Vehicle/GetVehicleList",
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