CarPartsApp.controller("companyController", function ($scope, companyServices, appServices, $timeout, blockUI, $http, $rootScope, $state, toastr) {
    // ====================================== Variable Declaration ========================================

    $scope.HasAccessToMasterCompany = false;
    $scope.HasAnyMasterCompany = false;

    //---File Operation
    $scope.ValidFiles = [];
    $scope.InValidFiles = [];
    var formdata = new FormData();
    var xhr;
    $scope.files = [];
    $scope.errFiles = [];
    $scope.Loader = false;
    $scope.progressVisible = false;
    $scope.progress = 0;
    $scope.file = {};

    //---File Operation
    $scope.ValidFilesLogo = [];
    $scope.InValidFilesLogo = [];
    var formdataLogo = new FormData();
    var xhrLogo;
    $scope.filesLogo = [];
    $scope.errFilesLogo = [];
    $scope.LoaderLogo = false;
    $scope.progressVisibleLogo = false;
    $scope.progressLogo = 0;
    $scope.fileLogo = {};
    $scope.ImageMessage = undefined;

    //---File Operation
    $scope.ValidFilesMasterCompany = [];
    $scope.InValidFilesMasterCompany = [];
    var formdataMasterCompany = new FormData();
    var xhrMasterCompany;
    $scope.filesMasterCompany = [];
    $scope.errFilesMasterCompany = [];
    $scope.LoaderMasterCompany = false;
    $scope.progressVisibleMasterCompany = false;
    $scope.progressMasterCompany = 0;
    $scope.fileMasterCompany = {};
    $scope.ImageMessage = undefined;


    // ====================================== Object Processing ===========================================

    //Watchman for ng-model 'files'
    $scope.$watch('files', function () {
        $scope.addFilesIntoFileStack($scope.files, $scope.errFiles);
    });
    $scope.addFilesIntoFileStack = function (files, errFiles) {
        $scope.progress = 0;
        $scope.files = files;
        $scope.errFiles = errFiles;


        //Precess FormData();
        formdata = new FormData();
        if ($scope.files != undefined) {
            for (var i = 0; i < $scope.files.length; i++) {
                formdata.append(0, $scope.files[i]);
            }
        }
        for (var pair of formdata.entries()) {
            console.log(pair[0] + ', ' + pair[1]);
        }
    };
    $scope.removeFile = function (model) {
        //Remove Valid Files
        //blockUI.instances.get('myBlock').start();
        //$scope.ImageMessage = undefined;
        var validIndex = $scope.files.findIndex(x => x.$$hashKey == model.$$hashKey);
        if (validIndex > -1) {
            $scope.files.splice(validIndex, 1);
            formdata = new FormData();
            if ($scope.files != undefined) {
                for (var i = 0; i < $scope.files.length; i++) {
                    formdata.append(0, $scope.files[i]);
                }
            }
        }
        //Removbe Invalid Files
        var inValidIndex = $scope.errFiles.findIndex(x => x.$$hashKey == model.$$hashKey);
        if (inValidIndex > -1) {
            $scope.errFiles.splice(inValidIndex, 1);
        }
        //blockUI.instances.get('myBlock').stop();
    }

    //Watchman for ng-model 'filesLogo'
    $scope.$watch('filesLogo', function () {
        $scope.addFilesIntoFileStackLogo($scope.filesLogo, $scope.errFilesLogo);
    });
    $scope.addFilesIntoFileStackLogo = function (filesLogo, errFilesLogo) {
        $scope.progressLogo = 0;
        $scope.filesLogo = filesLogo;
        $scope.errFilesLogo = errFilesLogo;


        //Precess FormData();
        formdataLogo = new FormData();
        if ($scope.filesLogo != undefined) {
            for (var i = 0; i < $scope.filesLogo.length; i++) {
                formdataLogo.append(0, $scope.filesLogo[i]);
            }
        }
        for (var pair of formdataLogo.entries()) {
            console.log(pair[0] + ', ' + pair[1]);
        }
    };
    $scope.removeFileLogo = function (modelLogo) {
        //Remove Valid Files
        //blockUI.instances.get('myBlock').start();
        $scope.ImageMessage = undefined;
        var validIndexLogo = $scope.filesLogo.findIndex(x => x.$$hashKey == modelLogo.$$hashKey);
        if (validIndexLogo > -1) {
            $scope.filesLogo.splice(validIndexLogo, 1);
            formdataLogo = new FormData();
            if ($scope.filesLogo != undefined) {
                for (var i = 0; i < $scope.filesLogo.length; i++) {
                    formdataLogo.append(0, $scope.filesLogo[i]);
                }
            }
        }
        //Removbe Invalid Files
        var inValidIndexLogo = $scope.errFilesLogo.findIndex(x => x.$$hashKey == modelLogo.$$hashKey);
        if (inValidIndexLogo > -1) {
            $scope.errFilesLogo.splice(inValidIndexLogo, 1);
        }
        //blockUI.instances.get('myBlock').stop();
    }


    //Watchman for ng-model 'filesMasterCompany'
    $scope.$watch('filesMasterCompany', function () {
        $scope.addFilesIntoFileStackMasterCompany($scope.filesMasterCompany, $scope.errFilesMasterCompany);
    });
    $scope.addFilesIntoFileStackMasterCompany = function (filesMasterCompany, errFilesMasterCompany) {
        $scope.progressMasterCompany = 0;
        $scope.filesMasterCompany = filesMasterCompany;
        $scope.errFilesMasterCompany = errFilesMasterCompany;


        //Precess FormData();
        formdataMasterCompany = new FormData();
        if ($scope.filesMasterCompany != undefined) {
            for (var i = 0; i < $scope.filesMasterCompany.length; i++) {
                formdataMasterCompany.append(0, $scope.filesMasterCompany[i]);
            }
        }
        for (var pair of formdataMasterCompany.entries()) {
            console.log(pair[0] + ', ' + pair[1]);
        }
    };
    $scope.removeFileMasterCompany = function (modelMasterCompany) {
        //Remove Valid Files
        //blockUI.instances.get('myBlock').start();
        //$scope.ImageMessage = undefined;
        var validIndexMasterCompany = $scope.filesMasterCompany.findIndex(x => x.$$hashKey == modelMasterCompany.$$hashKey);
        if (validIndexMasterCompany > -1) {
            $scope.filesMasterCompany.splice(validIndexMasterCompany, 1);
            formdataMasterCompany = new FormData();
            if ($scope.filesMasterCompany != undefined) {
                for (var i = 0; i < $scope.filesMasterCompany.length; i++) {
                    formdataMasterCompany.append(0, $scope.filesMasterCompany[i]);
                }
            }
        }
        //Removbe Invalid Files
        var inValidIndexMasterCompany = $scope.errFilesMasterCompany.findIndex(x => x.$$hashKey == modelMasterCompany.$$hashKey);
        if (inValidIndexMasterCompany > -1) {
            $scope.errFilesMasterCompany.splice(inValidIndexMasterCompany, 1);
        }
        //blockUI.instances.get('myBlock').stop();
    }


    // ====================================== Automated HTTP Requests =====================================

    companyServices.GetCompanyDetails().then(function (response) {
        $scope.Company = response.data;
    })
    .then(function () {
        companyServices.HasAccessToMasterCompany($rootScope.AdminId).then(function (response) {
            $scope.HasAccessToMasterCompany = response.data;
        })
        .then(function () {
            if ($scope.HasAccessToMasterCompany) {
                companyServices.HasAnyMasterCompany().then(function (response) {
                    $scope.HasAnyMasterCompany = response.data;
                })
            }
        })
    })


    // ====================================== Modal Operation =============================================

    $scope.openMasterCompanyModal = function () {
        companyServices.HasAccessToMasterCompany($rootScope.AdminId).then(function (response) {
            $scope.HasAccessToMasterCompany = response.data;
        })
        .then(function () {
            if ($scope.HasAccessToMasterCompany) {
                companyServices.GetMasterCompanyDetails().then(function (response) {
                    $scope.MasterCompany = response.data;
                })
                .then(function () {
                    $('#MasterCompanyModal').modal("show");
                })
            }
        })

    }

    $scope.cancelMasterCompanyModal = function () {
        $scope.MasterCompany = {};
        if ($scope.filesMasterCompany != undefined) {
            if ($scope.filesMasterCompany.length > 0) {
                let i = 0;
                while (i == 0) {
                    $scope.removeFileMasterCompany($scope.filesMasterCompany[i]);
                    if ($scope.filesMasterCompany.length == 0) {
                        break;
                    }
                }
            }
        }

        if ($scope.errFilesMasterCompany != undefined) {
            if ($scope.errFilesMasterCompany.length > 0) {
                let i = 0;
                while (i == 0) {
                    $scope.removeFileMasterCompany($scope.errFilesMasterCompany[i]);
                    if ($scope.errFilesMasterCompany.length == 0) {
                        break;
                    }
                }
            }
        }
        $('#MasterCompanyModal').modal("hide");
    }

    // ====================================== Event Based Operation =======================================

    $scope.CreateCompany = function () {
        $scope.Company.AdminId = $rootScope.AdminId;
        companyServices.CreateCompany($scope.Company).then(function (response) {
            if (response.data.IsReport == "Success") {
                toastr.success(response.data.Message, "Success!");
            }
            else if (response.data.IsReport == "Warning") {
                toastr.warning(response.data.Message, "Warning!");
            }
            else if (response.data.IsReport == "Error") {
                toastr.error(response.data.Message, "Error!");
            }
        })
        .then(function () {
            companyServices.GetCompanyDetails().then(function (response) {
                $scope.Company = response.data;
            })
        })
    }

    $scope.UpdateCompany = function () {
        companyServices.UpdateCompany($scope.Company).then(function (response) {
            if (response.data.IsReport == "Success") {
                toastr.success(response.data.Message, "Success!");
            }
            else if (response.data.IsReport == "Warning") {
                toastr.warning(response.data.Message, "Warning!");
            }
            else if (response.data.IsReport == "Error") {
                toastr.error(response.data.Message, "Error!");
            }
        })
        .then(function () {
            companyServices.GetCompanyDetails().then(function (response) {
                $scope.Company = response.data;
            })
        })
    }

    $scope.CreateMasterCompany = function () {
        $scope.MasterCompany.AdminId = $rootScope.AdminId;
        companyServices.CreateMasterCompany($scope.MasterCompany).then(function (response) {
            if (response.data.IsReport == "Success") {
                toastr.success(response.data.Message, "Success!");
            }
            else if (response.data.IsReport == "Warning") {
                toastr.warning(response.data.Message, "Warning!");
            }
            else if (response.data.IsReport == "Error") {
                toastr.error(response.data.Message, "Error!");
            }
        })
        .then(function () {
            $scope.cancelMasterCompanyModal();
        })
    }

    $scope.UpdateMasterCompany = function () {
        companyServices.UpdateMasterCompany($scope.MasterCompany).then(function (response) {
            if (response.data.IsReport == "Success") {
                toastr.success(response.data.Message, "Success!");
            }
            else if (response.data.IsReport == "Warning") {
                toastr.warning(response.data.Message, "Warning!");
            }
            else if (response.data.IsReport == "Error") {
                toastr.error(response.data.Message, "Error!");
            }
        })
        .then(function () {
            $scope.cancelMasterCompanyModal();
        })
    }

    $scope.ToggleMasterSettingsApperance = function () {
        companyServices.ToggleMasterSettingsApperance($scope.MasterCompany.Id).then(function (response) {
            if (response.data.IsReport == "Success") {
                toastr.success(response.data.Message, "Success!");
            }
            else if (response.data.IsReport == "Warning") {
                toastr.warning(response.data.Message, "Warning!");
            }
            else if (response.data.IsReport == "Error") {
                toastr.error(response.data.Message, "Error!");
            }
        })
        .then(function () {
            companyServices.GetMasterCompanyDetails().then(function (response) {
                $scope.MasterCompany = response.data;
            })
        })
    }


    //Upload Attachment (Update)

    function CheckImageAspect() {
        $http({
            method: 'POST',
            url: "/Api/Company/CheckImageRatio",
            headers: {
                'Content-Type': undefined
            },
            data: formdataLogo,
            transformRequest: angular.identity
        }).success(function (data) {
            if (data.IsReport == "Success") {
                $scope.ImageMessage = data.Message;
                //toastr.success(data.Message, data.IsReport);
            }
            else if (data.IsReport == "Warning") {
                //toastr.error(data.Message, data.IsReport);
                $scope.ImageMessage = data.Message;
            }
            else if (data.IsReport == "Error") {
                //toastr.error(data.Message, data.IsReport);
                $scope.ImageMessage = data.Message;
            }
        }).error(function (data, status) {
            //toastr.error(data, status);
            $scope.ImageMessage = data.Message;
        })
    }

    $scope.UploadCompanyLogo = function () {
        $http({
            method: 'POST',
            url: "/Api/Company/UploadCompanyLogo/" + $scope.Company.Id,
            headers: {
                'Content-Type': undefined
            },
            eventHandlers: {
                progress: function (c) {
                }
            },
            uploadEventHandlers: {
                progress: function (e) {
                    console.log('UploadProgress -> ' + e);
                    //blockUI.start("Uploading ... " + parseInt(e.loaded / e.total * 100, 10) + "%")
                    //$scope.progress = parseInt(e.loaded / e.total * 100, 10);
                }
            },
            data: formdata,
            transformRequest: angular.identity
        }).success(function (data) {
            if (data.IsReport == "Success") {
                toastr.success(data.Message, data.IsReport);
            }
            else if (data.IsReport == "Warning") {
                toastr.error(data.Message, data.IsReport);
            }
            else if (data.IsReport == "Error") {
                toastr.error(data.Message, data.IsReport);
            }
        }).error(function (data, status) {
            toastr.error(data, status);
        })
        .then(function () {
            if ($scope.files != undefined) {
                if ($scope.files.length > 0) {
                    let i = 0;
                    while (i == 0) {
                        $scope.removeFile($scope.files[i]);
                        if ($scope.files.length == 0) {
                            break;
                        }
                    }
                }
            }
        })
        .then(function () {
            if ($scope.errFiles != undefined) {
                if ($scope.errFiles.length > 0) {
                    let i = 0;
                    while (i == 0) {
                        $scope.removeFile($scope.errFiles[i]);
                        if ($scope.errFiles.length == 0) {
                            break;
                        }
                    }
                }
            }
        })
        .then(function () {
            companyServices.GetCompanyDetails().then(function (response) {
                $scope.Company = response.data;
            })
        })
    }

    $scope.UploadCompanySignature = function () {
        $http({
            method: 'POST',
            url: "/Api/Company/UploadCompanySignature/" + $scope.Company.Id,
            headers: {
                'Content-Type': undefined
            },
            eventHandlers: {
                progress: function (c) {
                }
            },
            uploadEventHandlers: {
                progress: function (e) {
                    console.log('UploadProgress -> ' + e);
                    //blockUI.start("Uploading ... " + parseInt(e.loaded / e.total * 100, 10) + "%")
                    //$scope.progress = parseInt(e.loaded / e.total * 100, 10);
                }
            },
            data: formdataLogo,
            transformRequest: angular.identity
        }).success(function (data) {
            if (data.IsReport == "Success") {
                toastr.success(data.Message, data.IsReport);
            }
            else if (data.IsReport == "Warning") {
                toastr.error(data.Message, data.IsReport);
            }
            else if (data.IsReport == "Error") {
                toastr.error(data.Message, data.IsReport);
            }
        }).error(function (data, status) {
            toastr.error(data, status);
        })
        .then(function () {
            if ($scope.filesLogo != undefined) {
                if ($scope.filesLogo.length > 0) {
                    let i = 0;
                    while (i == 0) {
                        $scope.removeFileLogo($scope.filesLogo[i]);
                        if ($scope.filesLogo.length == 0) {
                            break;
                        }
                    }
                }
            }
        })
        .then(function () {
            if ($scope.errFilesLogo != undefined) {
                if ($scope.errFilesLogo.length > 0) {
                    let i = 0;
                    while (i == 0) {
                        $scope.removeFileLogo($scope.errFilesLogo[i]);
                        if ($scope.errFilesLogo.length == 0) {
                            break;
                        }
                    }
                }
            }
        })
        .then(function () {
            companyServices.GetCompanyDetails().then(function (response) {
                $scope.Company = response.data;
            })
        })
    }

    $scope.UploadMasterCompanyLogo = function () {
        $http({
            method: 'POST',
            url: "/Api/MasterCompany/UploadMasterCompanyLogo/" + $scope.Company.Id,
            headers: {
                'Content-Type': undefined
            },
            eventHandlers: {
                progress: function (c) {
                }
            },
            uploadEventHandlers: {
                progress: function (e) {
                    console.log('UploadProgress -> ' + e);
                    //blockUI.start("Uploading ... " + parseInt(e.loaded / e.total * 100, 10) + "%")
                    //$scope.progress = parseInt(e.loaded / e.total * 100, 10);
                }
            },
            data: formdataMasterCompany,
            transformRequest: angular.identity
        }).success(function (data) {
            if (data.IsReport == "Success") {
                toastr.success(data.Message, data.IsReport);
            }
            else if (data.IsReport == "Warning") {
                toastr.error(data.Message, data.IsReport);
            }
            else if (data.IsReport == "Error") {
                toastr.error(data.Message, data.IsReport);
            }
        }).error(function (data, status) {
            toastr.error(data, status);
        })
        .then(function () {
            if ($scope.filesMasterCompany != undefined) {
                if ($scope.filesMasterCompany.length > 0) {
                    let i = 0;
                    while (i == 0) {
                        $scope.removeFileMasterCompany($scope.filesMasterCompany[i]);
                        if ($scope.filesMasterCompany.length == 0) {
                            break;
                        }
                    }
                }
            }
        })
        .then(function () {
            if ($scope.errFilesMasterCompany != undefined) {
                if ($scope.errFilesMasterCompany.length > 0) {
                    let i = 0;
                    while (i == 0) {
                        $scope.removeFileMasterCompany($scope.errFilesMasterCompany[i]);
                        if ($scope.errFilesMasterCompany.length == 0) {
                            break;
                        }
                    }
                }
            }
        })
        .then(function () {
            companyServices.GetMasterCompanyDetails().then(function (response) {
                $scope.MasterCompany = response.data;
            })
        })
    }
    // ====================================== Helper Fuctions =============================================
});