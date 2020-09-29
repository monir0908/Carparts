CarPartsApp.factory("masterproductspecificationlabelServices", ["$http", "$rootScope", "$window", function ($http, $rootScope, $window) {
    return {
        GetMasterProductSpecificationLabelList: function () {
            return $http({
                url: "/Api/MasterProductSpecificationLabel/GetMasterProductSpecificationLabelList",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                method: "GET",
                async: false
            });
        },
        CreateMasterProductSpecificationLabel: function (MasterProductSpecificationLabel) {
            return $http({
                url: "/Api/MasterProductSpecificationLabel/CreateMasterProductSpecificationLabel",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                data: MasterProductSpecificationLabel,
                method: "POST",
                async: false
            });
        },
        UpdateMasterProductSpecificationLabel: function (id, value) {
            return $http({
                url: "/Api/MasterProductSpecificationLabel/UpdateMasterProductSpecificationLabel",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                data: {
                    MasterProductSpecificationLabelId: id,
                    Value: value
                },
                method: "POST",
                async: false
            });
        },
        GetMasterProductSpecificationLabelList: function () {
            return $http({
                url: "/Api/MasterProductSpecificationLabel/GetMasterProductSpecificationLabelList",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                method: "GET",
                async: false
            });
        },
        GetMasterProductSpecificationLabelDetails: function (id) {
            return $http({
                url: "/Api/MasterProductSpecificationLabel/GetMasterProductSpecificationLabelDetails/" + id,
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                method: "GET",
                async: false
            });
        },
        ToggleCategory: function (id) {
            return $http({
                url: "/Api/MasterProductSpecificationLabel/ToggleCategory/" + id,
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                method: "POST",
                async: false
            });
        }
    };
}]);