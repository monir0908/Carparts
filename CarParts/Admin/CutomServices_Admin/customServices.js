/// <reference path="Assets_Admin/angularjs/angular.js" />
/// <reference path="app.js" />
CarPartsApp.factory("customServices", ["$http", "$rootScope", "$window", "$q", "$cookies", function ($http, $rootScope, $window, $q, $cookies) {
    
    return {
        isLoggedIn: function () {
            var a = $cookies.get('AdminToken');
            if (a == null || a==='undefined') {
                return false;
            } else {
                return true;
            }
        },
        
    };


    //var isLoggedIn;
    //var AdmintokenFromCookies = $cookies.get('AdminToken');
    //if (AdmintokenFromCookies == null || AdmintokenFromCookies ==='undefined') {
    //    isLoggedIn = false;
    //} else {
    //    isLoggedIn = true;
    //}
    //return isLoggedIn;

}]);