var CarPartsApp = angular.module("CarPartsApp", ['ui.router', 'ngMessages', 'datatables', 'toastr', 'ng-weekday-selector', 'ngSanitize', 'ngFileUpload', 'ngMap', 'ngAutocomplete', 'ngAnimate', 'ui.bootstrap', "ngCookies", "base64", "ngFileUpload", "blockUI", "infinite-scroll", "chart.js", "timer", "xeditable"]);

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
            $window.location.href = "/Admin/#/logIn";
            $cookies.remove('AdminToken', { path: '/' });
            $cookies.remove('AdminInfo', { path: '/' });
            $cookies.remove('AdminFullName', { path: '/' });
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
CarPartsApp.config(function ($stateProvider, $urlRouterProvider, $httpProvider, toastrConfig, blockUIConfig) {

    $httpProvider.interceptors.push('authInterceptor', function ($q, $cookies, $rootScope) {
        return {
            'request': function (config) {
                let Admintoken;
                let AdmintokenFromCookies = $cookies.get('AdminToken');
                if (AdmintokenFromCookies) {
                    Admintoken = JSON.parse(AdmintokenFromCookies);
                } else {
                    Admintoken = null;
                }
                config.headers['Token'] = Admintoken;
                // Update the Cookie time
                let date = new Date();
                let expireTime = date.getTime() + 10800000;
                date.setTime(expireTime);
                let AdminInfoFromCookies = $cookies.get('AdminInfo');
                if (AdmintokenFromCookies) {
                    $cookies.put('AdminToken', AdmintokenFromCookies, { 'expires': date, 'path': '/' });
                }
                if (AdminInfoFromCookies) {
                    $cookies.put('AdminInfo', AdminInfoFromCookies, { 'expires': date, 'path': '/' });
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

    //blockUIConfig.autoBlock = true;
    blockUIConfig.message = 'Please Wait ...';

    // Tell the blockUI service to ignore certain requests
    blockUIConfig.requestFilter = function (config) {
        // If the request starts with '/api/quote' ...
        if (config.url.match(/^\/Api\/Tracking($|\/).*/)) {
            return false; // ... don't block it.
        }
        if (config.url.match(/^\/Api\/HallService\/GetHallServiceDetailsModel($|\/).*/)) {
            return false; // ... don't block it.
        }
    };

    $urlRouterProvider.otherwise('/logIn');
    $stateProvider

        //-------------------------------------------------------------LOGIN-----------------------------------------------------
        .state('logIn', {
            url: '/logIn',
            templateUrl: 'app/logIn/logIn.html',
            controller: 'logInController'
        })

        //-------------------------------------------------------------HOME-----------------------------------------------------
        .state('home', {
            url: '/home',
            templateUrl: 'app/home/home.html',
            controller: 'homeController',
            resolve: {
                isAdminAuthenticated: checkAuthentication
            }
        })
        //-------------------------------------------------------------PROFILE-----------------------------------------------------
        .state('profile', {
            url: '/profile',
            templateUrl: 'app/profile/profile.html',
            controller: 'profileController',
            resolve: {
                isAdminAuthenticated: checkAuthentication
            }
        })

        //-------------------------------------------------------------MASTER MAIN CATEGORY-----------------------------------------------------
        .state('mastermaincategory', {
            url: '/mastermaincategory',
            templateUrl: 'app/mastermaincategory/mastermaincategory.html',
            controller: 'mastermaincategoryController',
            resolve: {
                isAdminAuthenticated: checkAuthentication
            }
        })
        //-------------------------------------------------------------MASTER SUB CATEGORY-----------------------------------------------------
        .state('mastersubcategory', {
            url: '/mastersubcategory',
            templateUrl: 'app/mastersubcategory/mastersubcategory.html',
            controller: 'mastersubcategoryController',
            resolve: {
                isAdminAuthenticated: checkAuthentication
            }
        })
        //-------------------------------------------------------------MASTER SUB CATEGORY-----------------------------------------------------
        .state('masterproductcategory', {
            url: '/masterproductcategory',
            templateUrl: 'app/masterproductcategory/masterproductcategory.html',
            controller: 'masterproductcategoryController',
            resolve: {
                isAdminAuthenticated: checkAuthentication
            }
        })
        //-------------------------------------------------------------MASTER SUB CATEGORY-----------------------------------------------------
        .state('masterproductbrand', {
            url: '/masterproductbrand',
            templateUrl: 'app/masterproductbrand/masterproductbrand.html',
            controller: 'masterproductbrandController',
            resolve: {
                isAdminAuthenticated: checkAuthentication
            }
        })
        //-------------------------------------------------------------VEHICLE SETTING-----------------------------------------------------
        .state('vehiclesettings', {
            url: '/vehiclesettings',
            templateUrl: 'app/vehiclesettings/vehiclesettings.html',
            controller: 'vehiclesettingsController',
            resolve: {
                isAdminAuthenticated: checkAuthentication
            }
        })
        //-------------------------------------------------------------PRODUCT SPECIFICATION LABEL-----------------------------------------------------
        .state('masterproductspecificationlabel', {
            url: '/masterproductspecificationlabel',
            templateUrl: 'app/masterproductspecificationlabel/masterproductspecificationlabel.html',
            controller: 'masterproductspecificationlabelController',
            resolve: {
                isAdminAuthenticated: checkAuthentication
            }
        })
        //-------------------------------------------------------------MANAGE PRODUCT-----------------------------------------------------
        .state('manageproduct', {
            url: '/manageproduct',
            templateUrl: 'app/manageproduct/manageproduct.html',
            controller: 'manageproductController',
            resolve: {
                isAdminAuthenticated: checkAuthentication
            }
        })
        //-------------------------------------------------------------COMPANY-----------------------------------------------------
        .state('company', {
            url: '/company',
            templateUrl: 'app/company/company.html',
            controller: 'companyController',
            resolve: {
                isAdminAuthenticated: checkAuthentication
            }
        })

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
                if ($rootScope.AdminInfo) {
                    toastr.error("Session expired, Please login again !", {
                        timeOut: 2000
                    });
                    $window.location.href = "/Admin/#/logIn";
                    $cookies.remove('AdminRole', { path: '/' });
                    $cookies.remove('AdminInfo', { path: '/' });
                } else {
                    toastr.error("You seem to be unauthorized, Please login !", {
                        timeOut: 2000
                    });
                    $window.location.href = "/Admin/#/logIn";
                }


            });
            // Reject the authentication promise to prevent the state from loading
            return $q.reject();
        }
    }
})


CarPartsApp.run(function ($rootScope, $http, $q, $state, toastr, $timeout, $cookies, $window, customServices, editableOptions) {
    editableOptions.theme = 'bs3'; // bootstrap3 theme. Can be also 'bs2', 'default'
    //===============================================On $stateChangeStart Event==========================================
    $rootScope.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams) {
        //-------------------Control Left Sidebar child menu------------------------------------
        $rootScope.categoryOpen = false;
        $rootScope.vehicleOpen = false;


        //-------------------Prevent a logged in user to go to login state----------------------
        var userIsLoggedIn = customServices.isLoggedIn();
        var unKnownState = "^";
        if (fromState.url === unKnownState && toState.url === "/logIn" && userIsLoggedIn) {
            $window.location.href = "/Admin/#/home";

        }
            //If user is logged-in and wants to go to logIn page again, To restrict him:
        else if (toState.url === "/logIn" && userIsLoggedIn) {
            event.preventDefault();
            toastr.error("You are already logged in, hence you can not go to <u>" + toState.url.slice(1) + "</u> page !", {
                timeOut: 3000
            });
            $window.location.href = "/Admin/#" + fromState.url;
        }

        //--------------------Get Essential values from cookie and store then into $rootScope-------------------

        $rootScope.AdminId = '';
        var adminidFromCookies = $cookies.get('AdminInfo');
        if (adminidFromCookies) {
            var adminidFromCookiesJSON = JSON.parse(adminidFromCookies);
            $rootScope.AdminId = adminidFromCookiesJSON.Id;
        } else {
            $rootScope.AdminId = '';
        }

        $rootScope.AdminRole = '';
        var AdminRoleFromCookies = $cookies.get('AdminInfo');
        if (AdminRoleFromCookies) {
            var AdminRoleFromCookiesJSON = JSON.parse(AdminRoleFromCookies);
            $rootScope.AdminRole = AdminRoleFromCookiesJSON.Role;
        } else {
            $rootScope.AdminRole = '';
        }

        $rootScope.AdminEmail = '';
        var AdminEmailFromCookies = $cookies.get('AdminInfo');
        if (AdminEmailFromCookies) {
            var AdminEmailFromCookiesJSON = JSON.parse(AdminEmailFromCookies);
            $rootScope.AdminEmail = AdminEmailFromCookiesJSON.Email;
        } else {
            $rootScope.AdminEmail = '';
        }

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