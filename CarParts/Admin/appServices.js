CarPartsApp.factory("appServices", ["$http", "$rootScope", "$q", "$timeout", function ($http, $rootScope, $q, $timeout) {
    return {
        Logout: function (param) {
            return $http({
                url: "/Api/Common/LogOutByAdminId/" + param,
                method: "POST",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                async: false
            });
        }
    }

}]);