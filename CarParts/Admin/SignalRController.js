CarPartsApp.controller('SignalRController', function ($scope, $rootScope, appServices, $cookies, blockUI, $window, $q, toastr, $compile, $timeout, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder, $state, homeServices) {

    //var audio = new Audio('/Notification Sound/Admin.mp3');
    var adminHub = $.connection.adminHub;

    //SignalR Hub Start
    if ($cookies.get('AdminToken') != null || $cookies.get('AdminToken') != undefined) {
        $.connection.hub.start().done(function () {
            //toastr.info("SignalR Connected From Admin", "SignalR");
        })
            .fail(function () {
                toastr.error("Web Socket Failed", { autoDismiss: false, timeOut: 30000, closeButton: true });
            })
    }
    else {
        $.connection.hub.stop();
    }
    $.connection.hub.logging = true;
    //$.connection.hub.start().done(function () {
    //    toastr.info("SignalR connected from Admin");
    //});


})