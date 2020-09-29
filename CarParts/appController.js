/// <reference path="Assets_Customer/angularjs/angular.js" />
/// <reference path="app.js" />
CarPartsApp.controller("appController", function ($scope, appServices, customServices, indexServices, $base64, $q, toastr, $filter, $compile, $cookies, $timeout, $location, $window, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder, $state, $rootScope) {
    //=================================================== GENERAL SETTINGS =====================================================


    appServices.GetCompanyDetailsForWebSite().then(function (response) {
        $rootScope.Company = response.data;
    })
    .then(function () {
        indexServices.GetProductHierarchy().then(function (response) {
            $scope.ProductHierarchy = response.data;
        })
        .then(function () {
            appServices.GetMasterVehicleYearList().then(function (response) {
                $scope.YearList = response.data;
            })
        })
    })
    $scope.GetMakerListByYearId = function (yearId) {
        appServices.GetMakerListByYearId(yearId).then(function (response) {
            $scope.MakerList = response.data;
        })
    }
    $scope.GetModelListByMakerId = function (makerId) {
        appServices.GetModelListByMakerId(makerId).then(function (response) {
            $scope.ModelList = response.data;
        })
    }
    $scope.GetSubModelListByModelId = function (modelId) {
        appServices.GetSubModelListByModelId(modelId).then(function (response) {
            $scope.SubModelList = response.data;
        })
    }
    $scope.GetEngineListBySubModelId = function (subModelId) {
        appServices.GetEngineListBySubModelId(subModelId).then(function (response) {
            $scope.EngineList = response.data;
        })
    }

    $scope.SearchByVehicleWithOutSubModelAndEngine = function () {
        let msg = "";
        appServices.SearchByVehicleWithOutSubModelAndEngine($scope.YearId, $scope.MakerId, $scope.ModelId).then(function (response) {
            let existingVehicleWithOutSubModelAndEngineCookie = [];
            var date = new Date();
            var expireTime = date.getTime() + 10800000; // 3 hours
            date.setTime(expireTime);
            if ($cookies.get('VehicleWithOutSubModelAndEngine')) {
                let json = JSON.parse($cookies.get('VehicleWithOutSubModelAndEngine'));
                if (!isArray(json) && isObject(json)) {
                    existingVehicleWithOutSubModelAndEngineCookie.push(json)
                }
                else if (isArray(json) && !isObject(json)) {
                    for (let i = 0; i < json.length; i++) {
                        existingVehicleWithOutSubModelAndEngineCookie.push(json[i])
                    }
                }
                let validIndex = existingVehicleWithOutSubModelAndEngineCookie.findIndex(x=>x.Id == response.data.TempVehicleOnlyCookieModel.Id);
                if (validIndex == -1) {
                    existingVehicleWithOutSubModelAndEngineCookie.push(response.data.TempVehicleOnlyCookieModel);
                    msg = "Vehicle information added successully for search";
                }
                else {
                    msg = "Vehicle information already exists";
                }
                $cookies.remove('VehicleWithOutSubModelAndEngine', { path: '/' });
                var date = new Date();
                var expireTime = date.getTime() + 10800000; // 3 hours
                date.setTime(expireTime);
                $cookies.put('VehicleWithOutSubModelAndEngine', angular.toJson(existingVehicleWithOutSubModelAndEngineCookie), { 'expires': date, 'path': '/' });
            }
            else {
                $cookies.put('VehicleWithOutSubModelAndEngine', angular.toJson(response.data.TempVehicleOnlyCookieModel), { 'expires': date, 'path': '/' });
                msg = "Vehicle information added successully for search";
                existingVehicleWithOutSubModelAndEngineCookie.push(response.data.TempVehicleOnlyCookieModel);
            }
            $rootScope.VehicleWithOutSubModelAndEngine = existingVehicleWithOutSubModelAndEngineCookie;
            //response.data.TempVehicleOnlyCookieModel.Model = true;
            //$cookies.put('SelectedVehicle', angular.toJson(response.data.TempVehicleOnlyCookieModel), { 'expires': date, 'path': '/' });            
            //if ($cookies.get('SelectedVehicle')) {
            //    $rootScope.SelectedVehicle = JSON.parse($cookies.get('SelectedVehicle'));
            //    //$timeout(function () {
            //    //    $("#" + $rootScope.SelectedVehicle.Id).attr('checked', 'checked');
            //    //}, 1)
            //}
        })
        .then(function () {
            $scope.YearId = undefined;
            $scope.MakerId = undefined;
            $scope.ModelId = undefined;
            $scope.SearchByVehicleForm.$setPristine();
            $scope.SearchByVehicleForm.$setUntouched();
            if (msg == "Vehicle information added successully for search") {
                toastr.success(msg);
            }
            else {
                toastr.warning(msg);
            }
        })
    }


    $scope.SearchByVehicleWithSubModelAndEngine = function () {
        let msg = "";
        if (!$scope.SubModelId) {
            $scope.SubModelId = null;
        }
        if (!$scope.EngineId) {
            $scope.EngineId = null;
        }
        appServices.SearchByVehicleWithSubModelAndEngine($scope.YearId, $scope.MakerId, $scope.ModelId, $scope.SubModelId, $scope.EngineId).then(function (response) {
            let existingVehicleWithSubModelAndEngineCookie = [];
            var date = new Date();
            var expireTime = date.getTime() + 10800000; // 3 hours
            date.setTime(expireTime);
            if ($cookies.get('VehicleWithSubModelAndEngine')) {
                let json = JSON.parse($cookies.get('VehicleWithSubModelAndEngine'));
                if (!isArray(json) && isObject(json)) {
                    existingVehicleWithSubModelAndEngineCookie.push(json)
                }
                else if (isArray(json) && !isObject(json)) {
                    for (let i = 0; i < json.length; i++) {
                        existingVehicleWithSubModelAndEngineCookie.push(json[i])
                    }
                }
                let validIndex = existingVehicleWithSubModelAndEngineCookie.findIndex(x=>x.Id == response.data.TempVehiclePlusCookieModel.Id);
                if (validIndex == -1) {
                    existingVehicleWithSubModelAndEngineCookie.push(response.data.TempVehiclePlusCookieModel);
                    msg = "Vehicle information added successully for search";
                }
                else {
                    msg = "Vehicle information already exists";
                }
                $cookies.remove('VehicleWithSubModelAndEngine', { path: '/' });
                $cookies.put('VehicleWithSubModelAndEngine', angular.toJson(existingVehicleWithSubModelAndEngineCookie), { 'expires': date, 'path': '/' });
            }
            else {
                $cookies.put('VehicleWithSubModelAndEngine', angular.toJson(response.data.TempVehiclePlusCookieModel), { 'expires': date, 'path': '/' });
                msg = "Vehicle information added successully for search";
                existingVehicleWithSubModelAndEngineCookie.push(response.data.TempVehiclePlusCookieModel);
            }
            $rootScope.VehicleWithSubModelAndEngine = existingVehicleWithSubModelAndEngineCookie;
            //response.data.TempVehiclePlusCookieModel.Model = true;
            //$cookies.put('SelectedVehicle', angular.toJson(response.data.TempVehiclePlusCookieModel), { 'expires': date, 'path': '/' });
            //if ($cookies.get('SelectedVehicle')) {
            //    $rootScope.SelectedVehicle = JSON.parse($cookies.get('SelectedVehicle'));
            //    //$timeout(function () {
            //    //    $("#" + $rootScope.SelectedVehicle.Id).attr('checked', 'checked');
            //    //}, 1)
            //}
        })
        .then(function () {
            $scope.YearId = undefined;
            $scope.MakerId = undefined;
            $scope.ModelId = undefined;
            $scope.SubModelId = undefined;
            $scope.EngineId = undefined;
            $scope.SearchByVehicleForm.$setPristine();
            $scope.SearchByVehicleForm.$setUntouched();
            if (msg == "Vehicle information added successully for search") {
                toastr.success(msg);
            }
            else {
                toastr.warning(msg);
            }
        })
    }

    //Modal Scroll Stuck Issue Solve
    $('.modal').on('hidden.bs.modal', function (e) {
        if ($('.modal').hasClass('in')) {
            $('body').addClass('modal-open');
        }
    });
    $('body').removeClass('modal-open');
    $('.modal-backdrop').remove();



    $scope.ShowRespectiveMegaMenuById = function (idList, id) {
        for (let i = 0; i < idList.length; i++) {
            if (idList[i].ParentId != id) {
                $("#" + idList[i].ParentId).removeClass("show-mega-menu");
                $("#" + idList[i].ParentId).addClass("mega_menu");
            }
        }
        if ($("#" + id).hasClass("show-mega-menu")) {
            $("#" + id).removeClass("show-mega-menu");
            $("#" + id).addClass("mega_menu");
        }
        else {
            $("#" + id).removeClass("mega_menu");
            $("#" + id).addClass("show-mega-menu");
        }
    }

    $scope.openmegamenu = function () {
        if ($("#fghjk").hasClass("show-mega-menu")) {
            $("#fghjk").removeClass("show-mega-menu");
            $("#fghjk").addClass("mega_menu");
        }
        else {
            $("#fghjk").removeClass("mega_menu");
            $("#fghjk").addClass("show-mega-menu");
        }
    }

    $scope.LoadSelectedVehicleWithOutSubModelAndEngine = function (id, model) {
        let validIndex = $rootScope.VehicleWithOutSubModelAndEngine.findIndex(x=>x.Id == id);
        let thisObj = $rootScope.VehicleWithOutSubModelAndEngine[validIndex];
        thisObj.Model = model;
        if ($cookies.get('SelectedVehicle')) {
            $cookies.remove('SelectedVehicle')
        }
        var date = new Date();
        var expireTime = date.getTime() + 10800000; // 3 hours
        date.setTime(expireTime);

        for (var i = 0; i < $rootScope.VehicleWithOutSubModelAndEngine.length; i++) {
            $rootScope.VehicleWithOutSubModelAndEngine[i].Model = false;
        }
        $rootScope.VehicleWithOutSubModelAndEngine[validIndex].Model = model;

        $cookies.remove('VehicleWithOutSubModelAndEngine', {
            path: '/'
        });
        $cookies.put('VehicleWithOutSubModelAndEngine',
            angular.toJson($rootScope.VehicleWithOutSubModelAndEngine), { 'expires': date, 'path': '/' });

        if (model == true) {
            $cookies.put('SelectedVehicle', angular.toJson(thisObj), {
                'expires': date, 'path': '/'
            });
            $rootScope.SelectedVehicle = JSON.parse($cookies.get('SelectedVehicle'))
        }



        for (var i = 0; i < $rootScope.VehicleWithSubModelAndEngine.length; i++) {
            $rootScope.VehicleWithSubModelAndEngine[i].Model = false;
        }
        $cookies.remove('VehicleWithSubModelAndEngine', {
            path: '/'
        });
        $cookies.put('VehicleWithSubModelAndEngine',
            angular.toJson($rootScope.VehicleWithSubModelAndEngine), {
                'expires': date, 'path': '/'
            });

        if ($state.current.name == 'subcategorylist' || $state.current.name == 'productcategorylist' || $state.current.name == 'product') {
            $scope.openmegamenu();
            $timeout(function () {
                $state.reload();
            }, 300)
        }
    }

    $scope.LoadSelectedVehicleWithSubModelAndEngine = function (id, model) {
        let validIndex = $rootScope.VehicleWithSubModelAndEngine.findIndex(x=>x.Id == id);
        let thisObj = $rootScope.VehicleWithSubModelAndEngine[validIndex];
        thisObj.Model = model;
        if ($cookies.get('SelectedVehicle')) {
            $cookies.remove('SelectedVehicle')
        }
        var date = new Date();
        var expireTime = date.getTime() + 10800000; // 3 hours
        date.setTime(expireTime);

        console.log($rootScope.VehicleWithSubModelAndEngine);
        for (var i = 0; i < $rootScope.VehicleWithSubModelAndEngine.length; i++) {
            $rootScope.VehicleWithSubModelAndEngine[i].Model = false;
        }
        $rootScope.VehicleWithSubModelAndEngine[validIndex].Model = model;

        $cookies.remove('VehicleWithSubModelAndEngine', {
            path: '/'
        });
        $cookies.put('VehicleWithSubModelAndEngine',
            angular.toJson($rootScope.VehicleWithSubModelAndEngine), { 'expires': date, 'path': '/' });


        if (model == true) {
            $cookies.put('SelectedVehicle', angular.toJson(thisObj), {
                'expires': date, 'path': '/'
            });
            $rootScope.SelectedVehicle = JSON.parse($cookies.get('SelectedVehicle'))
        }


        for (var i = 0; i < $rootScope.VehicleWithOutSubModelAndEngine.length; i++) {
            $rootScope.VehicleWithOutSubModelAndEngine[i].Model = false;
        }
        $cookies.remove('VehicleWithOutSubModelAndEngine', {
            path: '/'
        });
        $cookies.put('VehicleWithOutSubModelAndEngine',
            angular.toJson($rootScope.VehicleWithOutSubModelAndEngine), {
                'expires': date, 'path': '/'
            });
        if ($state.current.name == 'subcategorylist' || $state.current.name == 'productcategorylist' || $state.current.name == 'product') {
            $scope.openmegamenu();
            $timeout(function () {
                $state.reload();
            }, 300)
        }
    }

    $scope.RemoveSelectedVehicleWithOutSubModelAndEngine = function (id) {
        if (true) {
            let validIndex = $rootScope.VehicleWithOutSubModelAndEngine.findIndex(x=>x.Id == id);
            if (validIndex != -1) {
                $rootScope.VehicleWithOutSubModelAndEngine.splice(validIndex, 1);
                $cookies.remove('VehicleWithOutSubModelAndEngine', {
                    path: '/'
                });
                if ($rootScope.VehicleWithOutSubModelAndEngine.length > 0) {
                    var date = new Date();
                    var expireTime = date.getTime() + 10800000; // 3 hours
                    date.setTime(expireTime);
                    $cookies.put('VehicleWithOutSubModelAndEngine', angular.toJson($rootScope.VehicleWithOutSubModelAndEngine), { 'expires': date, 'path': '/' });
                }
                else {
                    $cookies.remove('VehicleWithOutSubModelAndEngine', {
                        path: '/'
                    });
                }

                if ($rootScope.SelectedVehicle.Id == id) {
                    $rootScope.SelectedVehicle = {};
                    $cookies.remove('SelectedVehicle', { path: '/' });
                }
            }
        }
        if ($state.current.name == 'subcategorylist' || $state.current.name == 'productcategorylist') {
            let validIndex = $rootScope.VehicleWithOutSubModelAndEngine.findIndex(x=>x.Model == true && x.Id == id)
            if (validIndex == -1) {
                $state.reload();
            }
        }
    }

    $scope.RemoveSelectedVehicleWithSubModelAndEngine = function (id) {
        if (true) {
            var validIndex = $rootScope.VehicleWithSubModelAndEngine.findIndex(x=>x.Id == id);
            if (validIndex != -1) {
                $rootScope.VehicleWithSubModelAndEngine.splice(validIndex, 1);
                $cookies.remove('VehicleWithSubModelAndEngine', {
                    path: '/'
                });
                if ($rootScope.VehicleWithSubModelAndEngine.length > 0) {
                    var date = new Date();
                    var expireTime = date.getTime() + 10800000; // 3 hours
                    date.setTime(expireTime);
                    $cookies.put('VehicleWithSubModelAndEngine', angular.toJson($rootScope.VehicleWithSubModelAndEngine), { 'expires': date, 'path': '/' });
                }
                else {
                    $cookies.remove('VehicleWithSubModelAndEngine', {
                        path: '/'
                    });
                }

                if ($rootScope.SelectedVehicle.Id == id) {
                    $rootScope.SelectedVehicle = {};
                    $cookies.remove('SelectedVehicle', { path: '/' });
                }
            }
        }
        if ($state.current.name == 'subcategorylist' || $state.current.name == 'productcategorylist') {
            let validIndex = $rootScope.VehicleWithSubModelAndEngine.findIndex(x=>x.Model == true && x.Id == id)
            if (validIndex == -1) {
                $state.reload();
            }
        }
    }

    $scope.radioclicktest = function (id, model) {
        console.log("Id " + id)
        console.log("Model " + model)
    }

    //==================================================== ANONYMOUS FUNCTION ========================================================
    function isArray(a) {
        return (!!a) && (a.constructor === Array);
    };

    function isObject(a) {
        return (!!a) && (a.constructor === Object);
    };
































    //==================================================== LOGIN SETTINGS =============================================================
    $scope.Loader = false;
    var LogInDataCookie = $cookies.get('LogInDataCustomer');

    if (LogInDataCookie != null) {
        var LogInDataTemp = JSON.parse($cookies.get('LogInDataCustomer'));
        $rootScope.LogInDataCustomer = {
            Email: LogInDataTemp.Email,
            Password: LogInDataTemp.Password
        }
        $scope.Remember = 1;
    }
    else {
        $rootScope.LogInDataCustomer = {
            Email: null,
            Password: null
        }
        $scope.Remember = 0;
    }
    $scope.openLogInModal = function () {
        if (LogInDataCookie != null) {
            $rootScope.LogInDataCustomer = {
                Email: LogInDataTemp.Email,
                Password: LogInDataTemp.Password
            }
            $scope.Remember = 1;
        }
        $('#LogIn').modal('show');
    }

    $scope.cancelLogInModal = function () {
        $('#LogIn').modal('hide');
        $timeout(function () {
            if (LogInDataCookie != null) {
                $rootScope.LogInDataCustomer = {
                    Email: LogInDataTemp.Email,
                    Password: LogInDataTemp.Password
                }
                $scope.Remember = 1;
            }
            else {
                $rootScope.LogInDataCustomer = {
                };
            }
        }, 200)
        .then(function () {
            $scope.Customer = {
            };
            $scope.IsReport = null;
        })
        .then(function () {
            $scope.logInForm.$setPristine();
            $scope.logInForm.$setUntouched();
        })
    }
    $scope.ControlRememberMe = function () {
        if ($scope.logInForm.$invalid == true) {
            $scope.Remember = 0;
        }
    }
    //Login Function
    $scope.LogIn = function () {
        $scope.Loader = true;
        if ($scope.Remember == 1) {
            $cookies.put('LogInDataCustomer', angular.toJson($rootScope.LogInDataCustomer), {
                'path': '/', 'samesite': 'lax'
            });
        }
        else if ($scope.Remember == 0) {
            $cookies.remove('LogInDataCustomer', {
                'path': '/', 'samesite': 'lax'
            });
        }
        if ($scope.logInForm.$invalid == false) {
            $scope.Submited = true;
            var key = $rootScope.LogInDataCustomer.Email + ':' + $rootScope.LogInDataCustomer.Password;
            $scope.LoginType = {};
            $scope.LoginType.Type = "Customer";
            appServices.GetAuthorizationToken($base64.encode(key), $scope.LoginType).then(function (response) {
                function redirectingToDashboard() {
                    return new Promise(function (done) {
                        //$window.location.href = "/Customer/#//";
                        done();
                    });
                }
                function tasksAfterSuccessfulLogin() {
                    redirectingToDashboard()
                        .then(function () {
                            $timeout(function () {
                                toastr.success("Successfully logged in ! </br>Welcome " + $rootScope.CustomerName, {
                                    timeOut: 2000
                                });
                            }, 500)
                            .then(function () {
                                $scope.cancelLogInModal();
                            })
                            .then(function () {
                                $timeout(function () {
                                    $state.reload();
                                }, 500)
                            })
                        });
                }
                if (response.data.Success) {
                    tasksAfterSuccessfulLogin();
                    console.log(angular.toJson(response.data.Token));
                    var date = new Date();
                    var expireTime = date.getTime() + 10800000; // 3 hours
                    date.setTime(expireTime);
                    $cookies.put('CustomerToken', angular.toJson(response.data.Token), {
                        'expires': date, 'path': '/'
                    });
                    $cookies.put('CustomerInfo', angular.toJson(response.data.CustomerInfo), {
                        'expires': date, 'path': '/'
                    });
                    $scope.Loader = false;
                } else {
                    $scope.Loader = false;
                    console.log("login is unsuccesfull !");
                }
            },
                function (reject) {
                    $scope.Loader = false;
                });
        }
        else {
            $scope.Loader = false;
            toastr.error("This form contains invalid data. Can not be submitted", 'Error!');
        }
    }

    $scope.LogOut = function () {
        console.log($rootScope.CustomerId);
        appServices.Logout($rootScope.CustomerId).then(function (response) {
            //$.connection.hub.stop();            
            $rootScope.CustomerToken = null;
            if (response.data == true) {
                $cookies.remove('CustomerToken', {
                    path: '/'
                });
                $cookies.remove('CustomerInfo', {
                    path: '/'
                });
                $cookies.remove('CustomerName', {
                    path: '/'
                });
                toastr.info("You have logged out !", {
                    timeOut: 2000
                });
            }
        })
        .then(function () {
            $rootScope.LoggedInStatus = customServices.isLoggedIn();
        })
        .then(function () {
            $state.reload();
        })
    }

    $scope.RegisterCustomer = function () {
        $scope.Loader = true;
        $scope.IsReport = null;
        appServices.RegisterCustomer($scope.Customer).then(function (response) {
            if (response.data.IsReport == "Success") {
                toastr.success(response.data.Message, "Success!");
            }
            else if (response.data.IsReport == "Warning") {
                toastr.warning(response.data.Message, "Warning!");
            }
            else if (response.data.IsReport == "Error") {
                toastr.error(response.data.Message, "Error!");
            }
            $scope.IsReport = response.data.IsReport;
            $scope.Loader = false;
        })
        .then(function () {
            if ($scope.IsReport == "Success") {
                $timeout(function () {
                    $('#login-tab1').trigger("click");
                }, 500)
            }
        })
    }
});