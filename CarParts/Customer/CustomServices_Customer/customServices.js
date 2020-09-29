/// <reference path="Assets_Customer/angularjs/angular.js" />
/// <reference path="app.js" />
CarPartsApp.factory("customServices", ["$http", "$rootScope", "$window", "$q", "$cookies", function ($http, $rootScope, $window, $q, $cookies) {

    return {
        isLoggedIn: function () {
            var a = $cookies.get('CustomerToken');
            if (a == null || a === 'undefined') {
                return false;
            } else {
                return true;
            }
        },
        isValidCourse: function (courseId, employeeId) {
            return $http({
                url: "/Api/Course/isValidCourse/" + courseId + '/' + employeeId,
                method: "GET",
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                async: false
            });
        },
        IsGuid: function (stringToTest) {
            if (stringToTest[0] === "{") {
                stringToTest = stringToTest.substring(1, stringToTest.length - 1);
            }
            var regexGuid = /^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$/gi;
            return regexGuid.test(stringToTest);
        }

    };


    //var isLoggedIn;
    //var CustomertokenFromCookies = $cookies.get('CustomerToken');
    //if (CustomertokenFromCookies == null || CustomertokenFromCookies ==='undefined') {
    //    isLoggedIn = false;
    //} else {
    //    isLoggedIn = true;
    //}
    //return isLoggedIn;

}]);