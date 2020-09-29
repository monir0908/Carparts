/// <reference path="C:\Users\Bacbon-Lap\source\repos\addictioncare\AddictionCare\AddictionCare\Scripts/jquery.signalR-2.4.1.js" />
/// <reference path="app.js" />
CarPartsApp.controller('mastermaincategoryController', function ($scope, $http, mastermaincategoryServices, $rootScope, appServices, $cookies, blockUI, $window, $q, toastr, $compile, $timeout, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder, $state) {
    //====================================================================Declaration=================================================================================    
    $rootScope.categoryOpen = true;
    $scope.MasterMainCategory = {};
    $scope.TemporaryAuthentication = {};
    $scope.vm = {};
    $scope.vm.dtInstance = {};
    $scope.vm.dtColumnDefs = [DTColumnDefBuilder.newColumnDef(2).notSortable()
    ];
    $scope.vm.dtOptions = DTOptionsBuilder.newOptions()
                            .withOption('paging', true)
                            .withOption('searching', true)
                            .withOption('info', true)
                            .withOption('order', [1, 'desc']);


    //====================================================================Element Processing==========================================================================
    //Modal Scroll Stuck Issue Solve
    $('.modal').on('hidden.bs.modal', function (e) {
        if ($('.modal').hasClass('in')) {
            $('body').addClass('modal-open');
        }
    });

    //====================================================================Object Processing===========================================================================




    //====================================================================Modal Operation=============================================================================
    $scope.openCreateMasterMainCategoryModal = function () {
        $('#MasterMainCategoryModal').modal('show');
    }

    $scope.cancelCreateMasterMainCategoryModal = function () {
        $('#MasterMainCategoryModal').modal('hide');
        $timeout(function () {
            $scope.MasterMainCategory = {};
            $scope.MasterMainCategoryForm.$setPristine();
            $scope.MasterMainCategoryForm.$setUntouched();
        }, 200)

    }

    $scope.openCategoryLogoModal = function (id) {
        mastermaincategoryServices.GetMasterMainCategoryDetails(id).then(function (response) {
            $scope.MasterMainCategory = response.data;
        })
        .then(function () {
            $('#CategoryLogoModal').modal('show');
        })
    }

    $scope.cancelCategoryLogoModal = function () {
        $('#CategoryLogoModal').modal('hide');
        $timeout(function () {
            $scope.MasterMainCategory = {};
            if ($scope.filesCategoryLogo != undefined) {
                if ($scope.filesCategoryLogo.length > 0) {
                    let i = 0;
                    while (i == 0) {
                        $scope.removeFileCategoryLogo($scope.filesCategoryLogo[i]);
                        if ($scope.filesCategoryLogo.length == 0) {
                            break;
                        }
                    }
                }
            }
            if ($scope.errFilesCategoryLogo != undefined) {
                if ($scope.errFilesCategoryLogo.length > 0) {
                    let i = 0;
                    while (i == 0) {
                        $scope.removeFileCategoryLogo($scope.errFilesCategoryLogo[i]);
                        if ($scope.errFilesCategoryLogo.length == 0) {
                            break;
                        }
                    }
                }
            }
        }, 200)
    }


    //====================================================================DB Operation================================================================================    
    function LoadMasterMainCategoryList() {
        mastermaincategoryServices.GetMasterMainCategoryList().then(function (response) {
            $scope.MasterMainCategoryList = response.data;
            for (let i = 0; i < $scope.MasterMainCategoryList.length; i++) {
                $scope.MasterMainCategoryList[i].AddedOn = moment.utc($scope.MasterMainCategoryList[i].AddedOn).local().format();
            }
        })
    }
    LoadMasterMainCategoryList();


    //====================================================================Event Binding===============================================================================
    $scope.UpdateMasterMainCategory = function (id, data) {
        mastermaincategoryServices.UpdateMasterMainCategory(id, data).then(function (response) {
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
            LoadMasterMainCategoryList();
        })
    }

    $scope.CreateMasterMainCategory = function () {
        $scope.MasterMainCategory.AdminId = $rootScope.AdminId;
        mastermaincategoryServices.CreateMasterMainCategory($scope.MasterMainCategory).then(function (response) {
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
            mastermaincategoryServices.GetMasterMainCategoryList().then(function (response) {
                $scope.MasterMainCategoryList = response.data;
                for (let i = 0; i < $scope.MasterMainCategoryList.length; i++) {
                    $scope.MasterMainCategoryList[i].AddedOn = moment.utc($scope.MasterMainCategoryList[i].AddedOn).local().format();
                }
            })
            .then(function () {
                $scope.cancelCreateMasterMainCategoryModal();
            })
        })
    }

    //====================================================================Miscellaneous Function======================================================================



    // ================================================== File Operation =============================================

    //---File Operation
    $scope.ValidFilesCategoryLogo = [];
    $scope.InValidFilesCategoryLogo = [];
    var formdataCategoryLogo = new FormData();
    var xhrCategoryLogo;
    $scope.filesCategoryLogo = [];
    $scope.errFilesCategoryLogo = [];
    $scope.LoaderCategoryLogo = false;
    $scope.progressVisibleCategoryLogo = false;
    $scope.progressCategoryLogo = 0;
    $scope.fileCategoryLogo = {};
    //Watchman for ng-model 'files'
    $scope.$watch('filesCategoryLogo', function () {
        $scope.addFilesIntoFileStackCategoryLogo($scope.filesCategoryLogo, $scope.errFilesCategoryLogo);
    });
    $scope.addFilesIntoFileStackCategoryLogo = function (filesCategoryLogo, errFilesCategoryLogo) {
        $scope.progressCategoryLogo = 0;
        $scope.filesCategoryLogo = filesCategoryLogo;
        $scope.errFilesCategoryLogo = errFilesCategoryLogo;


        //Precess FormData();
        formdataCategoryLogo = new FormData();
        if ($scope.filesCategoryLogo != undefined) {
            for (var i = 0; i < $scope.filesCategoryLogo.length; i++) {
                formdataCategoryLogo.append(0, $scope.filesCategoryLogo[i]);
            }
        }
        for (var pair of formdataCategoryLogo.entries()) {
            console.log(pair[0] + ', ' + pair[1]);
        }
    };
    $scope.removeFileCategoryLogo = function (modelCategoryLogo) {
        //Remove Valid Files
        //blockUI.instances.get('myBlock').start();
        //$scope.ImageMessage = undefined;
        var validIndexCategoryLogo = $scope.filesCategoryLogo.findIndex(x => x.$$hashKey == modelCategoryLogo.$$hashKey);
        if (validIndexCategoryLogo > -1) {
            $scope.filesCategoryLogo.splice(validIndexCategoryLogo, 1);
            formdataCategoryLogo = new FormData();
            if ($scope.filesCategoryLogo != undefined) {
                for (var i = 0; i < $scope.filesCategoryLogo.length; i++) {
                    formdataCategoryLogo.append(0, $scope.filesCategoryLogo[i]);
                }
            }
        }
        //Removbe Invalid Files
        var inValidIndexCategoryLogo = $scope.errFilesCategoryLogo.findIndex(x => x.$$hashKey == modelCategoryLogo.$$hashKey);
        if (inValidIndexCategoryLogo > -1) {
            $scope.errFilesCategoryLogo.splice(inValidIndexCategoryLogo, 1);
        }
        //blockUI.instances.get('myBlock').stop();
    }
    $scope.UploadMasterMainCategoryLogo = function () {
        $http({
            method: 'POST',
            url: "/Api/MasterMainCategory/UploadMasterMainCategoryLogo/" + $scope.MasterMainCategory.Id,
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
                    //$scope.progressCategoryLogo = parseInt(e.loaded / e.total * 100, 10);
                }
            },
            data: formdataCategoryLogo,
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
            mastermaincategoryServices.GetMasterMainCategoryList().then(function (response) {
                $scope.MasterMainCategoryList = response.data;
                for (let i = 0; i < $scope.MasterMainCategoryList.length; i++) {
                    $scope.MasterMainCategoryList[i].AddedOn = moment.utc($scope.MasterMainCategoryList[i].AddedOn).local().format();
                }
            }).then(function () {
                if ($scope.filesCategoryLogo != undefined) {
                    if ($scope.filesCategoryLogo.length > 0) {
                        let i = 0;
                        while (i == 0) {
                            $scope.removeFileCategoryLogo($scope.filesCategoryLogo[i]);
                            if ($scope.filesCategoryLogo.length == 0) {
                                break;
                            }
                        }
                    }
                }
            })
            .then(function () {
                if ($scope.errFilesCategoryLogo != undefined) {
                    if ($scope.errFilesCategoryLogo.length > 0) {
                        let i = 0;
                        while (i == 0) {
                            $scope.removeFileCategoryLogo($scope.errFilesCategoryLogo[i]);
                            if ($scope.errFilesCategoryLogo.length == 0) {
                                break;
                            }
                        }
                    }
                }
            })
            .then(function () {
                mastermaincategoryServices.GetMasterMainCategoryDetails($scope.MasterMainCategory.Id).then(function (response) {
                    $scope.MasterMainCategory = response.data;
                })
            })
        })
    }

    //====================================================================Garbage Code================================================================================




});