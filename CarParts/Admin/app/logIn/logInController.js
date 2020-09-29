CarPartsApp.controller("logInController", function ($scope, $rootScope, $state, blockUI, logInServices, $cookies, toastr, $base64, $window, $timeout) {

    $scope.Loader = false;

    //Load LogInDataAdmin From Cookie
    var LogInDataCookie = $cookies.get('LogInDataAdmin');
    //If Cookie Exists
    if (LogInDataCookie != null) {
        var LogInDataTemp = JSON.parse($cookies.get('LogInDataAdmin'));
        $rootScope.LogInDataAdmin = {
            Email: LogInDataTemp.Email,
            Password: LogInDataTemp.Password
        }
        $scope.Remember = 1;
    }
    //Else Cookie not found
    else {
        $rootScope.LogInDataAdmin = {
            Email: null,
            Password: null
        }
        $scope.Remember = 0;
    }

    //Control Remember Me checkbox while formdata in invalid
    $scope.ControlRememberMe = function () {
        if ($scope.logInForm.$invalid == true) {
            $scope.Remember = 0;
        }
    }

    //Login Function
    $scope.LogIn = function () {
        $scope.Loader = true;

        //Take care of remember me
        if ($scope.Remember == 1) {
            $cookies.put('LogInDataAdmin', angular.toJson($rootScope.LogInDataAdmin), { 'path': '/', 'samesite': 'lax' });
        }
        else if ($scope.Remember == 0) {
            $cookies.remove('LogInDataAdmin', { 'path': '/', 'samesite': 'lax' });
        }


        if ($scope.logInForm.$invalid == false) {
            $scope.Submited = true;
            var key = $rootScope.LogInDataAdmin.Email + ':' + $rootScope.LogInDataAdmin.Password;

            //console.log($base64.encode(key));
            // Create LoginType Object
            $scope.LoginType = {};
            $scope.LoginType.Type = "Admin";


            logInServices.GetAuthorizationToken($base64.encode(key), $scope.LoginType).then(function (response) {

                //console.log(response.data);
                function redirectingToDashboard() {
                    return new Promise(function (done) {
                        $window.location.href = "/Admin/#/home";
                        done();
                    });
                }
                function tasksAfterSuccessfulLogin() {
                    redirectingToDashboard()
                        .then(function () {
                            $timeout(function () {
                                toastr.success("Successfully logged in ! </br>Welcome", {
                                    timeOut: 2000
                                });
                            }, 3000);
                        });
                }


                if (response.data.Success) {
                    tasksAfterSuccessfulLogin();

                    console.log(angular.toJson(response.data.Token));


                    //Setting a cookie
                    var date = new Date();
                    var expireTime = date.getTime() + 10800000; // 3 hours
                    date.setTime(expireTime);

                    //$cookies.put('Token', angular.toJson(response.data.Token), { 'expires': date });
                    //$cookies.put('User', angular.toJson(response.data.User), { 'expires': date });
                    $cookies.put('AdminToken', angular.toJson(response.data.Token), { 'expires': date, 'path': '/' });
                    $cookies.put('AdminInfo', angular.toJson(response.data.AdminInfo), { 'expires': date, 'path': '/' });

                    //Make HTML Collapser Value Dynamic Per-User
                    $cookies.put('HTMLCollapseStatus', "fixed left-sidebar-top", { 'expires': date, 'path': '/' });
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




    //Signing in through Enter key :
    //$("body").keypress(function (e) {
    //    var code = e.keyCode || e.which;
    //    if (code === 13) {
    //        $("#btnLogin").click();
    //    }
    //});

    if ($rootScope.UnauthorizedRequestFound == true) {
        //$('.modal-backdrop').remove();
        toastr.error("Session expired, Please log in again !", {
            timeOut: 3000
        });
        $rootScope.UnauthorizedRequestFound = false;
        $scope.Loader = false;
    }
    else if ($rootScope.InternalServerErrorFound == true) {
        //$('.modal-backdrop').remove();
        toastr.error("Internal Server Error! Please log in again", {
            timeOut: 3000
        });
        $rootScope.InternalServerErrorFound = false;
        $scope.Loader = false;
    }
    else if ($rootScope.UsernameOrPasswordNotMatched == true) {
        //$('.modal-backdrop').remove();
        toastr.error("Invalid Username or Password !", {
            timeOut: 3000
        });
        $rootScope.loading = false;

        $rootScope.UsernameOrPasswordNotMatched = false;
        //Idle.unwatch();
    }

});