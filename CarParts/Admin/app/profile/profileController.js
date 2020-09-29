/// <reference path="C:\Users\Bacbon-Lap\source\repos\addictioncare\AddictionCare\AddictionCare\Scripts/jquery.signalR-2.4.1.js" />
/// <reference path="app.js" />
CarPartsApp.controller('profileController', function ($scope, profileServices, $rootScope, appServices, $cookies, blockUI, $window, $q, toastr, $compile, $timeout, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder, $state) {
    //====================================================================Declaration=================================================================================    

    $scope.TemporaryAuthentication = {};



    //====================================================================Element Processing==========================================================================
    //Modal Scroll Stuck Issue Solve
    $('.modal').on('hidden.bs.modal', function (e) {
        if ($('.modal').hasClass('in')) {
            $('body').addClass('modal-open');
        }
    });

    //====================================================================Object Processing===========================================================================




    //====================================================================Modal Operation=============================================================================




    //====================================================================DB Operation================================================================================
    profileServices.GetAdminProfileDetailsByAdminId($rootScope.AdminId).then(function (response) {
        $scope.TemporaryAuthentication = response.data;
        $scope.TemporaryAuthentication.adminId = $rootScope.AdminId;
    })

    $scope.ChangeEmail = function () {
        profileServices.ChangeEmail($scope.TemporaryAuthentication).then(function (response) {
            if (response.data.IsReport == "Success") {
                toastr.success(response.data.Message, "Success!");
            }

            else if (response.data.IsReport == "Warning") {
                toastr.error(response.data.Message, "Error!");
            }
            else if (response.data.IsReport == "Error") {
                toastr.error(response.data.Message, "Error!");
            }
        })
        .then(function () {
            $state.reload();
        })
        //.then(function () {
        //    profileServices.GetAdminDetailsForCookies($rootScope.AdminId).then(function (response) {
        //        var date = new Date();
        //        var expireTime = date.getTime() + 31540000000000; // 3 hours
        //        date.setTime(expireTime);
        //        $cookies.put('AdminInfo', angular.toJson(response.data), { 'expires': date, 'path': '/' });
        //    })
        //})
    };

    $scope.ChangePassword = function () {
        profileServices.ChangePassword($scope.TemporaryAuthentication).then(function (response) {
            if (response.data.IsReport == "Success") {
                toastr.success(response.data.Message, "Success!");
            }

            else if (response.data.IsReport == "Warning") {
                toastr.error(response.data.Message, "Error!");
            }
            else if (response.data.IsReport == "Error") {
                toastr.error(response.data.Message, "Error!");
            }
        })
            .then(function () {
                $state.reload();
            })
    };

    //====================================================================Miscellaneous Function======================================================================


    //====================================================================Garbage Code================================================================================




});