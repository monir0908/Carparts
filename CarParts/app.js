var CarPartsApp = angular.module("CarPartsApp", ['ui.router', 'ngMessages', 'datatables', 'angular-loading-bar', 'toastr', 'ng-weekday-selector', 'ngSanitize', 'ngFileUpload', 'ngAnimate', "ngCookies", "base64", "ngFileUpload", "infinite-scroll", "chart.js", "timer", "xeditable", "720kb.tooltips"]);

CarPartsApp.service('authInterceptor', function ($q, $window, $rootScope, $timeout, $injector, $cookies) {
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
            $window.location.href = "/#/";
            $cookies.remove('CustomerToken', { path: '/' });
            $cookies.remove('CustomerInfo', { path: '/' });
            $cookies.remove('CustomerName', { path: '/' });
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
            //$state.reload();
            toastr.error("Invalid login credentials. Please try again !", {
                timeOut: 3000
            });
            $rootScope.UsernameOrPasswordNotMatched = true;
        }
        return $q.reject(response);
    };

});
CarPartsApp.config(function ($stateProvider, $urlRouterProvider, $httpProvider, toastrConfig, cfpLoadingBarProvider) {
    cfpLoadingBarProvider.includeSpinner = true;
    //cfpLoadingBarProvider.spinnerTemplate = '<div id="preloader"><span class="spinner"></span><div class="loader-section section-left"></div><div class="loader-section section-right"></div></div>';
    $httpProvider.interceptors.push('authInterceptor', function ($q, $cookies, $rootScope) {
        return {
            'request': function (config) {
                let Customertoken;
                let CustomertokenFromCookies = $cookies.get('CustomerToken');
                if (CustomertokenFromCookies) {
                    Customertoken = JSON.parse(CustomertokenFromCookies);
                } else {
                    Customertoken = null;
                }
                let Customerrole;
                let CustomerroleFromCookies = $cookies.get('CustomerInfo');
                if (CustomerroleFromCookies) {
                    Customerrole = JSON.parse(CustomerroleFromCookies);
                    Customerrole = Customerrole.Role;
                } else {
                    Customerrole = null;
                }


                config.headers['Token'] = Customertoken;
                config.headers['Role'] = Customerrole;
                // Update the Cookie time
                let date = new Date();
                let expireTime = date.getTime() + 10800000;
                date.setTime(expireTime);
                let CustomerInfoFromCookies = $cookies.get('CustomerInfo');
                if (CustomertokenFromCookies) {
                    $cookies.put('CustomerToken', CustomertokenFromCookies, { 'expires': date, 'path': '/' });
                }
                if (CustomerInfoFromCookies) {
                    $cookies.put('CustomerInfo', CustomerInfoFromCookies, { 'expires': date, 'path': '/' });
                }

                return config;
            }
        };
    });
    //CarPartsApp.config(function (ChartJsProvider) {
    //    ChartJsProvider.setOptions({ colors: ['#b18cff', '#0041ff', '#00b6ff', '#00fff5', '#00ff5c', '#f9ff00', '#ff7800'] });
    //});

    angular.extend(toastrConfig, {
        autoDismiss: false,
        containerId: 'toast-container',
        maxOpened: 0,
        newestOnTop: true,
        positionClass: 'toast-top-right',
        preventDuplicates: false,
        preventOpenDuplicates: false,
        target: 'body',
        progressBar: true,
        allowHtml: true
    });

    ////blockUIConfig.autoBlock = true;
    //blockUIConfig.message = 'Please Wait ...';

    //// Tell the blockUI service to ignore certain requests
    //blockUIConfig.requestFilter = function (config) {
    //    // If the request starts with '/api/quote' ...
    //    if (config.url.match(/^\/Api\/Tracking($|\/).*/)) {
    //        return false; // ... don't block it.
    //    }
    //    if (config.url.match(/^\/Api\/HallService\/GetHallServiceDetailsModel($|\/).*/)) {
    //        return false; // ... don't block it.
    //    }
    //};

    $urlRouterProvider.otherwise('/');
    $stateProvider
        //-------------------------------------------------------------404 PAGE NOT FOUND-----------------------------------------------------
        .state('404', {
            url: '/404',
            templateUrl: 'Customer/app/404/404.html',
            controller: '404Controller'
        })
        //-------------------------------------------------------------INDEX-----------------------------------------------------
        .state('index', {
            url: '/',
            templateUrl: 'Customer/app/index/index.html',
            controller: 'indexController'
        })
        //-------------------------------------------------------------CONTACT US-----------------------------------------------------
        .state('contactus', {
            url: '/contactus',
            templateUrl: 'Customer/app/contactus/contactus.html',
            controller: 'contactusController'
        })

        //-------------------------------------------------------------ABOUT US-----------------------------------------------------
        .state('aboutus', {
            url: '/aboutus',
            templateUrl: 'Customer/app/aboutus/aboutus.html',
            controller: 'aboutusController'
        })
        //-------------------------------------------------------------SUB CATEGORY LIST-----------------------------------------------------
        .state('subcategorylist', {
            url: '/subcategorylist/:categoryId',
            templateUrl: 'Customer/app/subcategorylist/subcategorylist.html',
            controller: 'subcategorylistController'
        })
        //-------------------------------------------------------------PRODUCT CATEGORY LIST-----------------------------------------------------
        .state('productcategorylist', {
            url: '/productcategorylist/:subCategoryId',
            templateUrl: 'Customer/app/productcategorylist/productcategorylist.html',
            controller: 'productcategorylistController'
        })
        //-------------------------------------------------------------PRODUCT-----------------------------------------------------
        .state('product', {
            url: '/product/:productCategoryId',
            templateUrl: 'Customer/app/product/product.html',
            controller: 'productController'
        })

    //-------------------------------------------------------------LOGIN-----------------------------------------------------
    //.state('logIn', {
    //    url: '/logIn',
    //    templateUrl: 'Customer/app/logIn/logIn.html',
    //    controller: 'logInController'
    //})

    //-------------------------------------------------------------HOME-----------------------------------------------------
    //.state('index', {
    //    url: '/index',
    //    templateUrl: 'app/index/index.html',
    //    controller: 'indexController',
    //    resolve: {
    //        isCustomerAuthenticated: checkAuthentication
    //    }
    //})


    function checkAuthentication($q, customServices, $state, $timeout, $window, toastr, $rootScope, $cookies) {
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
                if ($rootScope.CustomerInfo) {
                    toastr.error("Session expired, Please login again !", {
                        timeOut: 2000
                    });
                    $window.location.href = "/#/";
                    $cookies.remove('CustomerRole', { path: '/' });
                    $cookies.remove('CustomerInfo', { path: '/' });
                } else {
                    toastr.error("You seem to be unauthorized, Please login !", {
                        timeOut: 2000
                    });
                    $window.location.href = "/#/";
                }


            });
            // Reject the authentication promise to prevent the state from loading
            return $q.reject();
        }
    }
})

function isArray(a) {
    return (!!a) && (a.constructor === Array);
};

function isObject(a) {
    return (!!a) && (a.constructor === Object);
};


CarPartsApp.run(function ($rootScope, $http, $q, $state, toastr, $timeout, $cookies, $window, customServices, editableOptions, $stateParams) {

    editableOptions.theme = 'bs3'; // bootstrap3 theme. Can be also 'bs2', 'default'
    //===============================================On $stateChangeStart Event==========================================
    $rootScope.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams) {
        //--------------------Get Essential values from cookie and store then into $rootScope-------------------
        $rootScope.VehicleWithOutSubModelAndEngine = [];
        $rootScope.SelectedVehicle = {};
        $rootScope.VehicleWithSubModelAndEngine = [];
        if ($cookies.get('VehicleWithOutSubModelAndEngine')) {
            if (isObject(JSON.parse($cookies.get('VehicleWithOutSubModelAndEngine')))) {
                $rootScope.VehicleWithOutSubModelAndEngine.push(JSON.parse($cookies.get('VehicleWithOutSubModelAndEngine')));
            }
            else {
                $rootScope.VehicleWithOutSubModelAndEngine = JSON.parse($cookies.get('VehicleWithOutSubModelAndEngine'));
            }
        }
        if ($cookies.get('VehicleWithSubModelAndEngine')) {
            if (isObject(JSON.parse($cookies.get('VehicleWithSubModelAndEngine')))) {
                $rootScope.VehicleWithSubModelAndEngine.push(JSON.parse($cookies.get('VehicleWithSubModelAndEngine')));
            }
            else {
                $rootScope.VehicleWithSubModelAndEngine = JSON.parse($cookies.get('VehicleWithSubModelAndEngine'));
            }
        }
        if ($cookies.get('SelectedVehicle')) {
            $rootScope.SelectedVehicle = JSON.parse($cookies.get('SelectedVehicle'));
        }

        $rootScope.CustomerId = '';
        var customeridFromCookies = $cookies.get('CustomerInfo');
        if (customeridFromCookies) {
            var customeridFromCookiesJSON = JSON.parse(customeridFromCookies);
            $rootScope.CustomerId = customeridFromCookiesJSON.Id;
        } else {
            $rootScope.CustomerId = '';
        }

        $rootScope.CustomerRole = '';
        var CustomerRoleFromCookies = $cookies.get('CustomerInfo');
        if (CustomerRoleFromCookies) {
            var CustomerRoleFromCookiesJSON = JSON.parse(CustomerRoleFromCookies);
            $rootScope.CustomerRole = CustomerRoleFromCookiesJSON.Role;
        } else {
            $rootScope.CustomerRole = '';
        }

        $rootScope.CustomerEmail = '';
        var CustomerEmailFromCookies = $cookies.get('CustomerInfo');
        if (CustomerEmailFromCookies) {
            var CustomerEmailFromCookiesJSON = JSON.parse(CustomerEmailFromCookies);
            $rootScope.CustomerEmail = CustomerEmailFromCookiesJSON.Email;
        } else {
            $rootScope.CustomerEmail = '';
        }

        $rootScope.CustomerName = '';
        var CustomerNameFromCookies = $cookies.get('CustomerInfo');
        if (CustomerNameFromCookies) {
            var CustomerNameFromCookiesJSON = JSON.parse(CustomerNameFromCookies);
            $rootScope.CustomerName = CustomerNameFromCookiesJSON.Name;
        } else {
            $rootScope.CustomerName = '';
        }

        //-------------------Prevent a logged in user to go to login state----------------------
        //-------------------Prevent a logged in user to go to login state----------------------
        var userIsLoggedIn = customServices.isLoggedIn();
        var unKnownState = "^";
        if (fromState.url === unKnownState && toState.url === "/logIn" && userIsLoggedIn) {
            $window.location.href = "/#/";

        }
            //If user is logged-in and wants to go to logIn page again, To restrict him:
        else if (toState.url === "/logIn" && userIsLoggedIn) {
            event.preventDefault();
            toastr.error("You are already logged in, hence you can not go to <u>" + toState.url.slice(1) + "</u> page !", {
                timeOut: 3000
            });
            $window.location.href = "/#" + fromState.url;
        }

        //else if (toState.url === "/coursequiz/:courseQuizId/:customerId") {
        //    if (userIsLoggedIn) {
        //        if (!toParams.customerId) {
        //            toParams.customerId = $rootScope.CustomerId;
        //        }
        //    }
        //    else {
        //        if (toParams.customerId) {
        //            toParams.customerId = null;
        //            event.preventDefault();
        //            toastr.warning("Quiz Not Found");
        //            $window.location.href = "/#/";
        //        }
        //        else if (!toParams.customerId) {
        //            event.preventDefault();
        //            toastr.warning("Quiz Not Found");
        //            $window.location.href = "/#/";
        //        }
        //    }
        //}

        // Uib-Tooltip
        $rootScope.placement = {
            options: [
                'top',
                'top-left',
                'top-right',
                'bottom',
                'bottom-left',
                'bottom-right',
                'left',
                'left-top',
                'left-bottom',
                'right',
                'right-top',
                'right-bottom'
            ],
            selected: 'top'
        };

    });


    //=============================================On $stateChangeSuccess Event==========================================
    $rootScope.$on("$stateChangeSuccess", function (event, toState, toParams, fromState, fromParams) {
        $rootScope.LiveAPIEnabled = false;
        $rootScope.HTMLCollapseStatus = $cookies.get('HTMLCollapseStatus');
        if ($rootScope.HTMLCollapseStatus == null || $rootScope.HTMLCollapseStatus == undefined) {
            $rootScope.HTMLCollapseStatus = "fixed left-sidebar-top";
        }
        document.body.scrollTop = document.documentElement.scrollTop = 0;
        //$('html,body').animate({ scrollTop: 0 }, 'fast');
        $rootScope.LoggedInStatus = customServices.isLoggedIn();
    });

    //==================================================Miscelenous Function (app.run)==================================================

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

CarPartsApp.filter('smallize', function () {
    return function (input) {
        return (!!input) ? input.charAt(0).toLowerCase() + input.substr(1).toLowerCase() : '';
    }
});

CarPartsApp.filter('trusted', ['$sce', function ($sce) {
    return function (url) {
        return $sce.trustAsResourceUrl(url.toString());
    };
}]);