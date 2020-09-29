var akhtarApp = angular.module('akhtarApp', ['ui.router', 'datatables', 'ui.bootstrap', 'angular.filter', 'ui.bootstrap.modal', "ngSanitize", "ngAnimate", "base64", "blockUI", "ngIdle", "kendo.directives",
    "ngMap", "toastr", "720kb.datepicker", "ngCookies"]);


akhtarApp.service('authInterceptor', function ($q, $window, $rootScope, $timeout, $injector) {
    var service = this;


    service.request = function (response) {
        $rootScope.response = true;
        return $q.resolve(response);
    };
    service.response = function (response) {
        //console.log(response);
        $rootScope.response = false;
        return $q.resolve(response);
    };
    service.responseError = function (response) {
        $rootScope.UsernameOrPasswordNotMatched = false;

        function goToLoginPageAndClearLocalStorage() {
            $window.location.href = "/Dashboard/#/logIn";
            localStorage.clear();
        }

        var toastr = $injector.get('toastr');
        var $state = $injector.get('$state');


        //If token value expires or wrong token provided:
        if (response.status === 401) {
            goToLoginPageAndClearLocalStorage();
            $rootScope.UnauthorizedRequestFound = true;
        }
        else if (response.status === 500) {
            //goToLoginPageAndClearLocalStorage();
            toastr.error("Internal Server Error! Please try again !", {
                timeOut: 3000
            });
            $rootScope.InternalServerErrorFound = true;
        }

        else if (response.status === 403) {
            $state.reload();
            $rootScope.UsernameOrPasswordNotMatched = true;
        }
        return $q.reject(response);
    };

});


akhtarApp.config(function ($stateProvider, $urlRouterProvider, toastrConfig, $httpProvider, blockUIConfig, KeepaliveProvider, IdleProvider) {

    //The following piece of code stop "Loding" animation for every http call/ route chage
    blockUIConfig.autoBlock = false;

    //Idle Session:
    //IdleProvider.idle(1200);        //1200 == 20 minutes
    IdleProvider.idle(2400);        //2400 == 40 minutes
    IdleProvider.timeout(5);        //gives time to user for moving mouse
    KeepaliveProvider.interval(2);  //If I want to keep the user's session alive

    //Toastr:
    angular.extend(toastrConfig, {
        autoDismiss: false,
        containerId: 'toast-container',
        maxOpened: 0,
        newestOnTop: true,
        positionClass: 'toast-top-right',
        preventDuplicates: false,
        preventOpenDuplicates: false,
        target: 'body',
        allowHtml: true
    });
    $httpProvider.interceptors.push('authInterceptor');
    $urlRouterProvider.otherwise('/logIn');
    $stateProvider
        .state('logIn', {
            url: '/logIn',
            templateUrl: 'app/logIn/logIn.html',
            controller: 'logInController'
        })
        .state('index', {
            url: '/index',
            templateUrl: 'app/index/index.html',
            controller: 'indexController',
            resolve: {
                isUserAuthenticated: checkCarParts
            }

        })
        .state('settings', {
            url: '/settings',
            templateUrl: 'app/settings/settings.html',
            controller: 'settingsController',
            resolve: {
                isUserAuthenticated: checkCarParts
            }
        })
        .state('dealer', {
            url: '/dealer',
            templateUrl: 'app/dealer/dealer.html',
            controller: 'dealerController',
            resolve: {
                isUserAuthenticated: checkCarParts
            }
        })
        .state('salesOrder', {
            url: '/salesOrder',
            templateUrl: 'app/salesOrder/salesOrder.html',
            controller: 'salesOrderController',
            resolve: {
                isUserAuthenticated: checkCarParts
            }
        })
        .state('deliveryOrder', {
            url: '/deliveryOrder',
            templateUrl: 'app/deliveryOrder/deliveryOrder.html',
            controller: 'deliveryOrderController',
            resolve: {
                isUserAuthenticated: checkCarParts
            }
        })
        .state('driver', {
            url: '/driver',
            templateUrl: 'app/driver/driver.html',
            controller: 'driverController',
            resolve: {
                isUserAuthenticated: checkCarParts
            }
        })
        .state('dealerLocation', {
            url: '/dealerLocation',
            templateUrl: 'app/tracking/dealerLocation.html',
            controller: 'dealerLocationController',
            resolve: {
                isUserAuthenticated: checkCarParts
            }
        })
        .state('trackCustomer', {
            url: '/trackCustomer',
            templateUrl: 'app/tracking/trackCustomer.html',
            controller: 'trackCustomerController',
            resolve: {
                isUserAuthenticated: checkCarParts
            }
        })
        .state('permission', {
            url: '/permission',
            templateUrl: 'app/permission/permission.html',
            controller: 'permissionController',
            resolve: {
                isUserAuthenticated: checkCarParts
            }
        })
        .state('payment', {
            url: '/payment',
            templateUrl: 'app/payment/payment.html',
            controller: 'paymentController',
            resolve: {
                isUserAuthenticated: checkCarParts
            }
        })
        .state('dealerReport', {
            url: '/dealerReport',
            templateUrl: 'app/report/dealerReport/dealerReport.html',
            controller: 'dealerReportController',
            resolve: {
                isUserAuthenticated: checkCarParts
            }
        })
        .state('salesReport', {
            url: '/salesReport',
            templateUrl: 'app/report/salesReport/salesReport.html',
            controller: 'salesReportController',
            resolve: {
                isUserAuthenticated: checkCarParts
            }
        })
        .state('deliveryReport', {
            url: '/deliveryReport',
            templateUrl: 'app/report/deliveryReport/deliveryReport.html',
            controller: 'deliveryReportController',
            resolve: {
                isUserAuthenticated: checkCarParts
            }
        })
        .state('virtualDriver', {
            url: '/virtualDriver',
            templateUrl: 'app/virtualDriver/virtualDriver.html',
            controller: 'virtualDriverController',
            resolve: {
                isUserAuthenticated: checkCarParts,
                isUserVirtualDriver: checkVirtualDriverAccess
            }
        });

    function checkCarParts($q, customServices, $state, $timeout, $window, toastr, $rootScope, $cookies) {
        if (customServices.isLoggedIn()) {
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
                if ($rootScope.User) {
                    toastr.error("Session expired, Please login again !", {
                        timeOut: 2000
                    });
                    $window.location.href = "/Dashboard/#/logIn";
                    $cookies.remove('User', { path: '/' });
                } else {
                    toastr.error("You seem to be unauthorized, Please login !", {
                        timeOut: 2000
                    });
                    $window.location.href = "/Dashboard/#/logIn";
                }


            });
            // Reject the authentication promise to prevent the state from loading
            return $q.reject();
        }
    }
    function checkVirtualDriverAccess($q, customServices, $state, $timeout, $window, toastr, $rootScope, $cookies) {
        if (customServices.hasVirtualDriverPrevileges()) {
            // Resolve the promise successfully
            return $q.when();
        }
        else {
            $timeout(function () {
                toastr.error("You are not permitted for this url !", {
                    timeOut: 2000
                });
            });
            // Reject the authentication promise to prevent the state from loading
            return $q.reject();
        }
    }
})
    .run(function ($http, $rootScope, $window, $location, $state, toastr, customServices, $timeout, Idle, $q, $cookies) {
        $rootScope.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams) {


            //Getting Token and User object :
            $rootScope.Token = $cookies.get('Token');
            $rootScope.User = $cookies.get('User');
            if ($rootScope.User) {
                $rootScope.UserObj = JSON.parse($rootScope.User);
                $rootScope.LoggedInUserId = $rootScope.UserObj.Id;
                $rootScope.UserName = $rootScope.UserObj.UserId;
                $rootScope.UserFullName = $rootScope.UserObj.FullName;
                $rootScope.IsVirtualDriver = $rootScope.UserObj.IsVirtualDriver;
            }

            //Watching Starts if user is idle or not:
            Idle.watch();


            //Cheking Online or Offline:
            $rootScope.online = navigator.onLine;
            $window.addEventListener("offline", function () {
                $rootScope.$apply(function () {
                    $rootScope.online = false;
                });
            }, false);
            $window.addEventListener("online", function () {
                $rootScope.$apply(function () {
                    $rootScope.online = true;
                });
            }, false);

            //Assume, user is logged-in and closes his browser and now he reopens the browser
            //three phrases are found:
            //1. he starts his journey from unknown state
            //2. our app will commence journey from logIn state
            //1. he is logged in

            var userIsLoggedIn = customServices.isLoggedIn();

            var unKnownState = "^";
            if (fromState.url === unKnownState && toState.url === "/logIn" && userIsLoggedIn) {
                $window.location.href = "/Dashboard/#/index";

            }

                //If user is logged-in and wants to go to logIn page again, To restrict him:
            else if (toState.url === "/logIn" && userIsLoggedIn) {
                event.preventDefault();
                toastr.error("You are already logged in, hence you can not go to login page !", {
                    timeOut: 3000
                });
                $window.location.href = "/Dashboard/#" + fromState.url;
            }

            //Cookies time extension:

            //Here, we are getting date from system (keeping it in: 'extendedTime' variable)
            //Then we are adding +6 GMT with it. Because, browser supports only +6 GMT time for expiration
            //After that, we will add expiration time (keeping it in expireTime variable)
            //Then converting it into toUTCString format ; Because cookies can only read toUTCString format

            var extendedTime = new Date();
            var universalTime = new Date(extendedTime.getTime() + extendedTime.getTimezoneOffset() * 60000);
            var expireTime = universalTime.getTime() + 10800000; // 3 hours
            //var expireTime = universalTime.getTime() + 10000; // 10 seconds
            extendedTime.setTime(expireTime);
            extendedTime.toUTCString();
            //alert(extendedTime);

            function updateCookie(name, value) {
                document.cookie = name + '=' + value + '; Path=/; Expires=' + extendedTime + ';';;

            };
            //The updateCookie function will only be fired if Token found in Cookies; 
            if ($rootScope.Token) {
                updateCookie('Token', $rootScope.Token);
            }
        });

        $rootScope.$on('$stateChangeSuccess', function (response) {

        });
    });



