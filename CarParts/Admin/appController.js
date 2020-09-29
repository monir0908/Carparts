/// <reference path="Assets_Admin/angularjs/angular.js" />
/// <reference path="app.js" />
CarPartsApp.controller("appController", function ($scope, appServices, $q, toastr, $filter, blockUI, $compile, $cookies, $timeout, $location, $window, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder, $state, $rootScope) {

    $scope.LogOut = function () {
        console.log($rootScope.AdminId);
        appServices.Logout($rootScope.AdminId).then(function (response) {
            $.connection.hub.stop();
            $rootScope.AdminToken = null;
            if (response.data == true) {
                $cookies.remove('AdminToken', { path: '/' });
                $cookies.remove('AdminInfo', { path: '/' });
                $cookies.remove('AdminFullName', { path: '/' });
                //Reset the HTML collapser;
                $cookies.remove('HTMLCollapseStatus', { path: '/' });
                //$window.location.href = "/Admin/#/logIn";
                $state.go("logIn");

                toastr.info("You have logged out !", {
                    timeOut: 2000
                });
            }
        });
    }



    //For HTML Collapsing
    function updateCookie(name, value) {
        document.cookie = name + '=' + value + '; Path=/; Expires=' + new Date() + ';';;

    };


    $scope.HTMLCollapser = function () {
        if ($rootScope.HTMLCollapseStatus == "fixed left-sidebar-top") {
            updateCookie('HTMLCollapseStatus', "fixed left-sidebar-top left-sidebar-collapsed");
            $rootScope.HTMLCollapseStatus = "fixed left-sidebar-top left-sidebar-collapsed";
        }
        else if ($rootScope.HTMLCollapseStatus == "fixed left-sidebar-top left-sidebar-collapsed") {
            updateCookie('HTMLCollapseStatus', "fixed left-sidebar-top");
            $rootScope.HTMLCollapseStatus = "fixed left-sidebar-top";
        }
    }


    //Modal Scroll Stuck Issue Solve
    $('.modal').on('hidden.bs.modal', function (e) {
        if ($('.modal').hasClass('in')) {
            $('body').addClass('modal-open');
        }
    });
});