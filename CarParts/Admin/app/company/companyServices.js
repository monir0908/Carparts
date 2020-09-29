CarPartsApp.factory("companyServices", function ($http) {
    return {
        GetCompanyDetails: function () {
            return $http({
                url: "/api/Company/GetCompanyDetails",
                method: "GET",
                async: false
            });
        },
        CreateCompany: function (company) {
            return $http({
                url: "/api/Company/CreateCompany",
                method: "POST",
                async: false,
                data: company
            });
        },
        UpdateCompany: function (company) {
            return $http({
                url: "/api/Company/UpdateCompany",
                method: "POST",
                async: false,
                data: company
            });
        },

        GetMasterCompanyDetails: function () {
            return $http({
                url: "/api/MasterCompany/GetMasterCompanyDetails",
                method: "GET",
                async: false
            });
        },
        CreateMasterCompany: function (masterCompany) {
            return $http({
                url: "/api/MasterCompany/CreateMasterCompany",
                method: "POST",
                async: false,
                data: masterCompany
            });
        },
        UpdateMasterCompany: function (masterCompany) {
            return $http({
                url: "/api/MasterCompany/UpdateMasterCompany",
                method: "POST",
                async: false,
                data: masterCompany
            });
        },
        HasAccessToMasterCompany: function (adminId) {
            return $http({
                url: "/api/MasterCompany/HasAccessToMasterCompany/"+adminId,
                method: "GET",
                async: false
            });
        },
        HasAnyMasterCompany: function () {
            return $http({
                url: "/api/MasterCompany/HasAnyMasterCompany",
                method: "GET",
                async: false
            });
        },
        ToggleMasterSettingsApperance: function (masterCompanyid) {
            return $http({
                url: "/api/MasterCompany/ToggleMasterSettingsApperance/" + masterCompanyid,
                method: "POST",
                async: false
            });
        },
    };
});


