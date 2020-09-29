var CarPartsApp = angular.module("CarPartsApp", ['ui.router', 'datatables', 'toastr', '720kb.datepicker', 'ngMap', 'ngAutocomplete', 'kendo.directives']);

CarPartsApp.constant("constant",
    {
        baseURL: "http://localhost:35128",
        headers: {
            "content-type": "application/json",
            "cache-control": "no-cache"
        }
    });

CarPartsApp.config(function ($stateProvider, $urlRouterProvider) {

    $urlRouterProvider.otherwise('/index');
    $stateProvider

    //-------------------------------------------------------------HOME-----------------------------------------------------
    .state('index', {
        url: '/index',
        templateUrl: 'app/index/index.html',
        controller: 'indexController',
        resolve: {
            LoadAllAuthDataAtDefaultStateResult: LoadAllAuthDataAtDefaultState
        }
    })


    //-------------------------------------------------------------PROFILE-----------------------------------------------------
    .state('profile', {
        url: '/profile',
        templateUrl: 'app/profile/profile.html',
        controller: 'profileController'
    })


    //-------------------------------------------------------------EMPLOYEE-----------------------------------------------------
    .state('customer', {
        url: '/customer',
        templateUrl: 'app/customer/customer.html',
        controller: 'customerController',
        data: {
            StateName: "customer"
        },
        resolve: {
            AuthToCustomerResult: CheckAuthToCustomer
        }
    })


    //-------------------------------------------------------------REPORT-----------------------------------------------------
    .state('report', {
        url: '/report',
        templateUrl: 'app/report/report.html',
        controller: 'reportController'
    })


    //-------------------------------------------------------------PACKAGE-----------------------------------------------------
    .state('package', {
        url: '/package',
        templateUrl: 'app/package/package.html',
        controller: 'packageController',
        resolve: {
            AuthToPackageResult: CheckAuthToPackage
        }
    })


    //-------------------------------------------------------------PAYMENT-----------------------------------------------------
    .state('addpayment', {
        url: '/addpayment',
        templateUrl: 'app/payment/add_payment/add payment.html',
        controller: 'add_paymentController',
        resolve: {
            AuthToPaymentResult: CheckAuthToPayment
        }
    })

    .state('pendingpayment', {
        url: '/pendingpayment',
        templateUrl: 'app/payment/pending_payment/pending payment.html',
        controller: 'pending_paymentController',
        resolve: {
            AuthToPaymentResult: CheckAuthToPayment
        }
    })

    .state('approvedpayment', {
        url: '/approvedpayment',
        templateUrl: 'app/payment/approved_payment/approved payment.html',
        controller: 'approved_paymentController',
        resolve: {
            AuthToPaymentResult: CheckAuthToPayment
        }
    })


    //-------------------------------------------------------------BILL-----------------------------------------------------
    .state('sessionbills', {
        url: '/sessionbills',
        templateUrl: 'app/bill/session_bills/session bills.html',
        controller: 'session_billsController',
        resolve: {
            AuthToPaymentResult: CheckAuthToPayment
        }
    })

    .state('oneoffbills', {
        url: '/oneoffbills',
        templateUrl: 'app/bill/oneoff_bills/one-off bills.html',
        controller: 'oneoff_billsController',
        resolve: {
            AuthToPaymentResult: CheckAuthToPayment
        }
    })


    //-------------------------------------------------------------TRACKING-----------------------------------------------------
    .state('trackcustomer', {
        url: '/trackcustomer',
        templateUrl: 'app/tracking/track_customer/track customer.html',
        controller: 'track_customerController',
        resolve: {
            AuthToTrackingResult: CheckAuthToTracking
        }
    })

    .state('currentlocation', {
        url: '/currentlocation',
        templateUrl: 'app/tracking/current_location/current location.html',
        controller: 'current_locationController',
        resolve: {
            AuthToTrackingResult: CheckAuthToTracking
        }
    });



    function CheckAuthToCustomer($q, appServices, $state, $timeout, $window, toastr, $rootScope) {
        //$q.when(appServices.IsAuthToCustomer());

        if (appServices.IsAuthToCustomer() == true) {
            // Resolve the promise successfully
            return $q.when();
        }
        else {
            $timeout(function () {
                //This code runs after the authentication promise has been rejected.
                //Reason of using $timeout below: Let's assume, unauthenticated user is in state A. They click a link to go to protected state 
                //B but you want to redirect them to logInPage. If there's no $timeout, ui-router will simply halt all state transitions, 
                //so the user would be stuck in state A. The $timeout allows ui-router to first prevent the initial transition to protected state 
                //B because the resolve was rejected and after that's done, it redirects to logInPage
                //if ($rootScope.User) {
                //    toastr.error("Session expired, Please login again !", {
                //        timeOut: 2000
                //    });
                //    $window.location.href = "http://localhost:35128";
                //} else {
                //    toastr.error("You seem to be unauthorized, Please login !", {
                //        timeOut: 2000
                //    });
                //    $window.location.href = "http://localhost:35128";

                //}

                if ($rootScope.CompanyProfileDetails.CompanyName == null || $rootScope.CompanyProfileDetails.CompanyEmail == null || $rootScope.CompanyProfileDetails.CompanyAddress == null || $rootScope.CompanyProfileDetails.CompanyBillingAddress == null || $rootScope.CompanyProfileDetails.ContactPerson == null || $rootScope.CompanyProfileDetails.ContactPersonEmail == null || ($rootScope.CompanyProfileDetails.CompanyPhone == null || $rootScope.CompanyProfileDetails.CompanyMobile == null) || ($rootScope.CompanyProfileDetails.ContactPersonPhone == null || $rootScope.CompanyProfileDetails.ContactPersonMobile == null)) {
                    toastr.error("Please fulfill your company info to get started", "Error");
                    if ($state.$current.self.name == "" || true) {
                        $state.go("index");
                        $state.reload();
                    }
                }
                else if ($rootScope.CompanyCurrentPriviliges.IsReport == "NoCurrentPackage") {
                    toastr.error("Please enroll a package to get started", "Error");
                    console.log("$state");
                    console.log($state);
                    if ($state.$current.self.name == "" || true) {
                        $state.go("index");
                        $state.reload();
                    }
                }
                console.log("Rejection Logged");


            }, 1);
            // Reject the authentication promise to prevent the state from loading
            return $q.reject();
        }
    }

    function CheckAuthToTracking($q, appServices, $state, $timeout, $window, toastr, $rootScope) {
        //$q.when(appServices.IsAuthToTracking());
        if (appServices.IsAuthToTracking() == true) {
            // Resolve the promise successfully
            return $q.when();
        }
        else {
            $timeout(function () {

                if ($rootScope.CompanyProfileDetails.CompanyName == null || $rootScope.CompanyProfileDetails.CompanyEmail == null || $rootScope.CompanyProfileDetails.CompanyAddress == null || $rootScope.CompanyProfileDetails.CompanyBillingAddress == null || $rootScope.CompanyProfileDetails.ContactPerson == null || $rootScope.CompanyProfileDetails.ContactPersonEmail == null || ($rootScope.CompanyProfileDetails.CompanyPhone == null || $rootScope.CompanyProfileDetails.CompanyMobile == null) || ($rootScope.CompanyProfileDetails.ContactPersonPhone == null || $rootScope.CompanyProfileDetails.ContactPersonMobile == null)) {
                    toastr.error("Please fulfill your company info to get started", "Error");
                    if ($state.$current.self.name == "" || true) {
                        $state.go("index");
                        $state.reload();
                    }
                }
                else if ($rootScope.CompanyCurrentPriviliges.IsReport == "NoCurrentPackage") {
                    toastr.error("Please enroll a package to get started", "Error");
                    if ($state.$current.self.name == "" || true) {
                        $state.go("index");
                        $state.reload();
                    }
                }
                else if ($rootScope.CustomerList.length <= 0) {
                    toastr.error("Please add customer to start tracking", "Error");
                    if ($state.$current.self.name == "" || true) {
                        $state.go("index");
                        $state.reload();
                    }
                }
                console.log("Rejection Logged");


            }, 1);
            // Reject the authentication promise to prevent the state from loading
            return $q.reject();
        }
    }

    function CheckAuthToPayment($q, appServices, $state, $timeout, $window, toastr, $rootScope) {
        //$q.when(appServices.IsAuthToPayment());
        if (appServices.IsAuthToPayment() == true) {
            // Resolve the promise successfully
            return $q.when();
        }
        else {
            $timeout(function () {

                if ($rootScope.CompanyProfileDetails.CompanyName == null || $rootScope.CompanyProfileDetails.CompanyEmail == null || $rootScope.CompanyProfileDetails.CompanyAddress == null || $rootScope.CompanyProfileDetails.CompanyBillingAddress == null || $rootScope.CompanyProfileDetails.ContactPerson == null || $rootScope.CompanyProfileDetails.ContactPersonEmail == null || ($rootScope.CompanyProfileDetails.CompanyPhone == null || $rootScope.CompanyProfileDetails.CompanyMobile == null) || ($rootScope.CompanyProfileDetails.ContactPersonPhone == null || $rootScope.CompanyProfileDetails.ContactPersonMobile == null)) {
                    toastr.error("Please fulfill your company info to get started", "Error");
                    if ($state.$current.self.name == "" || true) {
                        $state.go("index");
                        $state.reload();
                    }
                }
                else if ($rootScope.CompanyCurrentPriviliges.IsReport == "NoCurrentPackage") {
                    toastr.error("Please enroll a package to get started", "Error");
                    if ($state.$current.self.name == "" || true) {
                        $state.go("index");
                        $state.reload();
                    }
                }
                else if ($rootScope.AnyChargeablePackage == "Free" || $rootScope.AnyChargeablePackage == "Chargeable") {
                    toastr.error("You haven't used any chargeable package yet. Please buy any chargeable package to use Payment and Bill menu", "Error");
                    console.log($state);
                    if ($state.$current.self.name == "" || true) {
                        $state.go("index");
                        $state.reload();
                    }
                }
                console.log("Rejection Logged");


            }, 1);
            // Reject the authentication promise to prevent the state from loading
            return $q.reject();
        }
    }

    function CheckAuthToPackage($q, appServices, $state, $timeout, $window, toastr, $rootScope) {
        //$q.when(appServices.IsAuthToPackage());
        if (appServices.IsAuthToPackage() == true) {
            // Resolve the promise successfully
            return $q.when();
        }
        else {
            $timeout(function () {

                //if ($rootScope.CompanyProfileDetails.CompanyName != null && $rootScope.CompanyProfileDetails.CompanyEmail != null && $rootScope.CompanyProfileDetails.CompanyAddress != null && $rootScope.CompanyProfileDetails.CompanyBillingAddress != null && $rootScope.CompanyProfileDetails.ContactPerson != null && $rootScope.CompanyProfileDetails.ContactPersonEmail != null && ($rootScope.CompanyProfileDetails.CompanyMobile == null && $rootScope.CompanyProfileDetails.CompanyPhone == null ? false : $rootScope.CompanyProfileDetails.CompanyMobile == null && $rootScope.CompanyProfileDetails.CompanyPhone != null ? true : $rootScope.CompanyProfileDetails.CompanyMobile != null && $rootScope.CompanyProfileDetails.CompanyPhone == null ? true : $rootScope.CompanyProfileDetails.CompanyMobile != null && $rootScope.CompanyProfileDetails.CompanyPhone != null ? true : false) && ($rootScope.CompanyProfileDetails.ContactPersonMobile == null && $rootScope.CompanyProfileDetails.ContactPersonPhone == null ? false : $rootScope.CompanyProfileDetails.ContactPersonMobile == null && $rootScope.CompanyProfileDetails.ContactPersonPhone != null ? true : $rootScope.CompanyProfileDetails.ContactPersonMobile != null && $rootScope.CompanyProfileDetails.ContactPersonPhone == null ? true : $rootScope.CompanyProfileDetails.ContactPersonMobile != null && $rootScope.CompanyProfileDetails.ContactPersonPhone != null ? true : false)) {
                //    toastr.error("Please fulfill your company info to get started", "Error");
                //    if ($state.$current.self.name == "") {
                //        $state.go("index");
                //    }
                //}

                if ($rootScope.AuthToPackagePointer == false) {

                    toastr.error("Please fulfill your company info to get started", "Error");


                    if ($state.$current.self.name == "" || true) {
                        $state.go("index");
                        $state.reload();
                    }
                }
                console.log("Rejection Logged");


            }, 1);
            // Reject the authentication promise to prevent the state from loading
            return $q.reject();
        }
    }

    function LoadAllAuthDataAtDefaultState($q, appServices, $state, $timeout, $window, toastr, $rootScope) {
        $q.when(appServices.GetCompanyCurrentPriviliges().then(function (response) {
            $rootScope.CompanyCurrentPriviliges = response.data;
            console.log($rootScope.CompanyCurrentPriviliges);
            $rootScope.AnyChargeablePackage = response.data.Previleges.PckgType;
            console.log($rootScope.AnyChargeablePackage);
        }));

        $q.when(appServices.GetCompanyProfileDetails().then(function (response) {
            $rootScope.CompanyProfileDetails = response.data;
            console.log($rootScope.CompanyProfileDetails);
        }));

        $q.when(appServices.GetCP_CustomerList().then(function (response) {
            $rootScope.CustomerList = response.data;
            console.log($rootScope.CustomerList);
        }));

        return 0;
    }


});

CarPartsApp.config(function (toastrConfig) {
    angular.extend(toastrConfig, {
        autodismiss: false,
        containerid: 'toast-container',
        maxopened: 0,
        newestontop: true,
        positionclass: 'toast-top-right',
        preventduplicates: false,
        preventopenduplicates: false,
        target: 'body',
        progressbar: true
    });
});
CarPartsApp.run(function ($rootScope, $http, toastr) {
    $rootScope.divId = "1";

    //Get Company Current Priviliges
    $rootScope.CompanyCurrentPriviliges = {};
    $http.get("/api/CompanyPackage/GetCompanyPckgPrivilegeByCompanyId/1").then(function (response) {
        $rootScope.CompanyCurrentPriviliges = response.data;
        console.log($rootScope.CompanyCurrentPriviliges);
    });


    //Get Company Profile Details
    $rootScope.CompanyProfileDetails = {};
    $http.get("/Api/CP_Profile/GetCompanyProfileDetails/1").then(function (response) {
        $rootScope.CompanyProfileDetails = response.data;
        console.log($rootScope.CompanyProfileDetails);
    });

    //Get Customer List Of a Company
    $rootScope.CustomerList = {};
    $http.get("/Api/CP_Customer/GetCP_CustomerList").then(function (response) {
        $rootScope.CustomerList = response.data;
        console.log($rootScope.CustomerList);
    });

    //Get Information about any Chargeable Package
    $rootScope.AnyChargeablePackage = null;
    if ($rootScope.CompanyCurrentPriviliges.Previleges != null) {
        $http.get("/api/CompanyPackage/GetCompanyPckgPrivilegeByCompanyId/1").then(function (response) {
            $rootScope.AnyChargeablePackage = response.data.Previleges.PckgType;
            console.log($rootScope.AnyChargeablePackage);
        });
    }



    //Get All The Information Againg on StateChageStartEvent
    $rootScope.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams) {
        //Get Company Current Priviliges
        $rootScope.CompanyCurrentPriviliges = {};
        $http.get("/api/CompanyPackage/GetCompanyPckgPrivilegeByCompanyId/1").then(function (response) {
            $rootScope.CompanyCurrentPriviliges = response.data;
            console.log($rootScope.CompanyCurrentPriviliges);
        });


        //Get Company Profile Details
        $rootScope.CompanyProfileDetails = {};
        $http.get("/Api/CP_Profile/GetCompanyProfileDetails/1").then(function (response) {
            $rootScope.CompanyProfileDetails = response.data;
            console.log($rootScope.CompanyProfileDetails);
        });

        //Get Customer List Of a Company
        $rootScope.CustomerList = {};
        $http.get("/Api/CP_Customer/GetCP_CustomerList").then(function (response) {
            $rootScope.CustomerList = response.data;
            console.log($rootScope.CustomerList);
        });

        //Get Information about any Chargeable Package
        $rootScope.AnyChargeablePackage = null;
        if ($rootScope.CompanyCurrentPriviliges.Previleges != null) {
            $http.get("/api/CompanyPackage/GetCompanyPckgPrivilegeByCompanyId/1").then(function (response) {
                $rootScope.AnyChargeablePackage = response.data.Previleges.PckgType;
                console.log($rootScope.AnyChargeablePackage);
            });
        }



        if (toState.name === "customer") {
            if ($rootScope.CompanyProfileDetails.CompanyName == null || $rootScope.CompanyProfileDetails.CompanyEmail == null || $rootScope.CompanyProfileDetails.CompanyAddress == null || $rootScope.CompanyProfileDetails.CompanyBillingAddress == null || $rootScope.CompanyProfileDetails.ContactPerson == null || $rootScope.CompanyProfileDetails.ContactPersonEmail == null || ($rootScope.CompanyProfileDetails.CompanyPhone == null || $rootScope.CompanyProfileDetails.CompanyMobile == null) || ($rootScope.CompanyProfileDetails.ContactPersonPhone == null || $rootScope.CompanyProfileDetails.ContactPersonMobile == null)) {
                toastr.error("Please fulfill your company info to get started", "Error");
                event.preventDefault();
            }
            else if ($rootScope.CompanyCurrentPriviliges.IsReport == "NoCurrentPackage") {
                toastr.error("Please enroll a package to get started", "Error");
                console.log("$state");
                console.log($state);
                event.preventDefault();
            }
            console.log("Rejection Logged");
        }

        //// check the destination is active
        //if (toState.data.isClosed) { // note that the 'closed' state has isClosed set to false
        //    event.preventDefault();
        //    $state.go('closed');
        //    return;
        //}
        //$rootScope.data = toState.data; // getting executed twice after our state transition to 'closed'
        //console.debug(toState);
    });


});
//Custom Filter
CarPartsApp.filter('replace', [function () {
    return function (input, from, to) {
        if (input === undefined) {
            return;
        }
        var regex = new RegExp(from, 'g');
        return input.replace(regex, to);
    };
}]);
//Custom Directive
CarPartsApp.directive('ignoreMouseWheel', function ($rootScope) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            element.bind('mousewheel', function (event) {
                element.blur();
            });
        }
    }
});





