CarPartsApp.factory("profileServices", ["$http", "$rootScope", "$window", function ($http, $rootScope, $window) {
    return {
        ChangeEmail: function (temporaryAuthentication) {
            return $http({
                url: "/Api/Admin/ChangeEmail",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                data: temporaryAuthentication,
                method: "POST",
                async: false
            });
        },
        ChangePassword: function (temporaryAuthentication) {
            return $http({
                url: "/Api/Admin/ChangePassword",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                data: temporaryAuthentication,
                method: "POST",
                async: false
            });
        },
        GetAdminProfileDetailsByAdminId: function (adminId) {
            return $http({
                url: "/Api/AdminProfile/GetAdminProfileDetailsByAdminId/" + adminId,
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                method: "GET",
                async: false
            });
        },

        GetAdminDetailsForCookies: function (adminId) {
            return $http({
                url: "/Api/AdminProfile/GetAdminDetailsForCookies/" + adminId,
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