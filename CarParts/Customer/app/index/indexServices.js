CarPartsApp.factory("indexServices", function ($http) {
    return {
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
        GetPopularBrand: function () {
            return $http({
                url: "/Api/Product/GetPopularBrand",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                method: "GET",
                async: false
            });
        },
        GetPopularMaker: function () {
            return $http({
                url: "/Api/Product/GetPopularMaker",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                method: "GET",
                async: false
            });
        },
    };
});


