CarPartsApp.factory("logInServices", ["$http", "$rootScope", "$window", function ($http, $rootScope, $window) {
    return {

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

    };
}]);
