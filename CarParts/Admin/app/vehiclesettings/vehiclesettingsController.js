/// <reference path="C:\Users\Bacbon-Lap\source\repos\addictioncare\AddictionCare\AddictionCare\Scripts/jquery.signalR-2.4.1.js" />
/// <reference path="app.js" />
CarPartsApp.controller('vehiclesettingsController', function ($scope, $http, vehiclesettingsServices, $rootScope, appServices, $cookies, blockUI, $window, $q, toastr, $compile, $timeout, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder, $state) {
    //====================================================================Declaration=================================================================================    
    $rootScope.vehicleOpen = true;
    $scope.MasterVehicleYear = {};
    $scope.Vehicle = {};
    $scope.vm = {};
    $scope.vm.dtInstance = {};
    $scope.vm.dtColumnDefs = [
    ];
    //DTColumnDefBuilder.newColumnDef(2).notSortable()
    $scope.vm.dtOptions = DTOptionsBuilder.newOptions()
                            .withOption('paging', true)
                            .withOption('searching', true)
                            .withOption('info', true)
                            .withOption('order', [0, 'asc']);


    //====================================================================Element Processing==========================================================================
    //Modal Scroll Stuck Issue Solve
    $('.modal').on('hidden.bs.modal', function (e) {
        if ($('.modal').hasClass('in')) {
            $('body').addClass('modal-open');
        }
    });

    //====================================================================Object Processing===========================================================================
    $scope.ProcessMasterYear = function (item, model, label) {
        $scope.Vehicle.MasterVehicleYearId = model.Id;
    }
    $scope.ProcessMasterMaker = function (item, model, label) {
        $scope.Vehicle.MasterVehicleMakerId = model.Id;
    }
    $scope.ProcessMasterModel = function (item, model, label) {
        $scope.Vehicle.MasterVehicleModelId = model.Id;
    }
    $scope.ProcessMasterSubModel = function (item, model, label) {
        $scope.Vehicle.MasterVehicleSubModelId = model.Id;
        vehiclesettingsServices.GetMasterVehicleEngineFilteredList($scope.Vehicle.MasterVehicleYearId, $scope.Vehicle.MasterVehicleMakerId, $scope.Vehicle.MasterVehicleModelId, $scope.Vehicle.MasterVehicleSubModelId).then(function (response) {
            $scope.VehicleEngineList = response.data;
        })
    }
    $scope.ProcessMasterEngine = function (item, model, label) {
        $scope.Vehicle.MasterVehicleEngineId = model.Id;
    }



    //====================================================================Modal Operation=============================================================================
    $scope.openCreateMasterVehicleYearModal = function () {
        $('#MasterVehicleYearModal').modal('show');
    }
    $scope.cancelCreateMasterVehicleYearModal = function () {
        $('#MasterVehicleYearModal').modal('hide');
        $timeout(function () {
            $scope.MasterVehicleYear = {};
            $scope.MasterVehicleYearForm.$setPristine();
            $scope.MasterVehicleYearForm.$setUntouched();
        }, 200)

    }

    $scope.openCreateMasterVehicleMakerModal = function () {
        $('#MasterVehicleMakerModal').modal('show');
    }
    $scope.cancelCreateMasterVehicleMakerModal = function () {
        $('#MasterVehicleMakerModal').modal('hide');
        $timeout(function () {
            $scope.MasterVehicleMaker = {};
            $scope.MasterVehicleMakerForm.$setPristine();
            $scope.MasterVehicleMakerForm.$setUntouched();
        }, 200)

    }

    $scope.openCreateMasterVehicleModelModal = function () {
        $('#MasterVehicleModelModal').modal('show');
    }
    $scope.cancelCreateMasterVehicleModelModal = function () {
        $('#MasterVehicleModelModal').modal('hide');
        $timeout(function () {
            $scope.MasterVehicleModel = {};
            $scope.MasterVehicleModelForm.$setPristine();
            $scope.MasterVehicleModelForm.$setUntouched();
        }, 200)

    }

    $scope.openCreateMasterVehicleSubModelModal = function () {
        $('#MasterVehicleSubModelModal').modal('show');
    }
    $scope.cancelCreateMasterVehicleSubModelModal = function () {
        $('#MasterVehicleSubModelModal').modal('hide');
        $timeout(function () {
            $scope.MasterVehicleSubModel = {};
            $scope.MasterVehicleSubModelForm.$setPristine();
            $scope.MasterVehicleSubModelForm.$setUntouched();
        }, 200)

    }

    $scope.openCreateMasterVehicleEngineModal = function () {
        $('#MasterVehicleEngineModal').modal('show');
    }
    $scope.cancelCreateMasterVehicleEngineModal = function () {
        $('#MasterVehicleEngineModal').modal('hide');
        $timeout(function () {
            $scope.MasterVehicleEngine = {};
            $scope.MasterVehicleEngineForm.$setPristine();
            $scope.MasterVehicleEngineForm.$setUntouched();
        }, 200)

    }

    $scope.openCreateVehicleModal = function () {
        $('#VehicleModal').modal('show');
    }
    $scope.cancelCreateVehicleModal = function () {
        $('#VehicleModal').modal('hide');
        $timeout(function () {
            $scope.Vehicle = {};
            $scope.MasterVehicleYearId = undefined;
            $scope.MasterVehicleMakerId = undefined;
            $scope.MasterVehicleModelId = undefined;
            $scope.MasterVehicleSubModelId = undefined;
            $scope.MasterVehicleEngineId = undefined;
            $scope.VehicleSubModelList = [];
            $scope.VehicleEngineList = [];
            $scope.VehicleForm.$setPristine();
            $scope.VehicleForm.$setUntouched();
        }, 200)

    }

    $scope.openCategoryLogoModal = function (id) {
        vehiclesettingsServices.GetMasterVehicleYearDetails(id).then(function (response) {
            $scope.MasterVehicleYear = response.data;
        })
        .then(function () {
            $('#CategoryLogoModal').modal('show');
        })
    }
    $scope.cancelCategoryLogoModal = function () {
        $('#CategoryLogoModal').modal('hide');
        $timeout(function () {
            $scope.MasterVehicleYear = {};
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
    function LoadAllmasterSettingsData() {
        vehiclesettingsServices.GetMasterVehicleYearList().then(function (response) {
            $scope.MasterVehicleYearList = response.data;
            for (let i = 0; i < $scope.MasterVehicleYearList.length; i++) {
                $scope.MasterVehicleYearList[i].AddedOn = moment.utc($scope.MasterVehicleYearList[i].AddedOn).local().format();
            }
        })
        .then(vehiclesettingsServices.GetMaxYear().then(function (response) {
            $scope.MaxYear = response.data;
        }))
        .then(function () {
            vehiclesettingsServices.GetMasterVehicleMakerList().then(function (response) {
                $scope.MasterVehicleMakerList = response.data;
                for (let i = 0; i < $scope.MasterVehicleMakerList.length; i++) {
                    $scope.MasterVehicleMakerList[i].AddedOn = moment.utc($scope.MasterVehicleMakerList[i].AddedOn).local().format();
                }
            })
            .then(function () {
                vehiclesettingsServices.GetMasterVehicleModelList().then(function (response) {
                    $scope.MasterVehicleModelList = response.data;
                    for (let i = 0; i < $scope.MasterVehicleModelList.length; i++) {
                        $scope.MasterVehicleModelList[i].AddedOn = moment.utc($scope.MasterVehicleModelList[i].AddedOn).local().format();
                    }
                })
                .then(function () {
                    vehiclesettingsServices.GetMasterVehicleSubModelList().then(function (response) {
                        $scope.MasterVehicleSubModelList = response.data;
                        for (let i = 0; i < $scope.MasterVehicleSubModelList.length; i++) {
                            $scope.MasterVehicleSubModelList[i].AddedOn = moment.utc($scope.MasterVehicleSubModelList[i].AddedOn).local().format();
                        }
                    })
                    .then(function () {
                        vehiclesettingsServices.GetMasterVehicleEngineList().then(function (response) {
                            $scope.MasterVehicleEngineList = response.data;
                            for (let i = 0; i < $scope.MasterVehicleEngineList.length; i++) {
                                $scope.MasterVehicleEngineList[i].AddedOn = moment.utc($scope.MasterVehicleEngineList[i].AddedOn).local().format();
                            }
                        })
                        .then(function () {
                            vehiclesettingsServices.GetVehicleList().then(function (response) {
                                $scope.VehicleList = response.data;
                                for (let i = 0; i < $scope.VehicleList.length; i++) {
                                    $scope.VehicleList[i].AddedOn = moment.utc($scope.VehicleList[i].AddedOn).local().format();
                                }
                            })
                        })
                    })
                })
            })
        })
    }
    LoadAllmasterSettingsData();


    //====================================================================Event Binding===============================================================================
    $scope.UpdateMasterVehicleYear = function (id, data) {
        vehiclesettingsServices.UpdateMasterVehicleYear(id, data).then(function (response) {
            if (response.data.IsReport == "Success") {
                toastr.success(response.data.Message, "Success!");
            }
            else if (response.data.IsReport == "Warning") {
                toastr.warning(response.data.Message, "Warning!");
            }
            else if (response.data.IsReport == "Error") {
                toastr.error(response.data.Message, "Error!");
            }
            else {
                toastr.warning("Update failed", "Warning!");
            }
        })
        .then(function () {
            LoadAllmasterSettingsData();
        })
    }
    $scope.CreateMasterVehicleYear = function () {
        $scope.MasterVehicleYear.AdminId = $rootScope.AdminId;
        vehiclesettingsServices.CreateMasterVehicleYear($scope.MasterVehicleYear).then(function (response) {
            if (response.data.IsReport == "Success") {
                toastr.success(response.data.Message, "Success!");
                if ($('#VehicleModal').hasClass('in')) {
                    $scope.MasterVehicleYearId = response.data.MasterVehicleYearId;
                }
            }
            else if (response.data.IsReport == "Warning") {
                toastr.warning(response.data.Message, "Warning!");
            }
            else if (response.data.IsReport == "Error") {
                toastr.error(response.data.Message, "Error!");
            }
        })
        .then(function () {
            vehiclesettingsServices.GetMasterVehicleYearList().then(function (response) {
                $scope.MasterVehicleYearList = response.data;
                for (let i = 0; i < $scope.MasterVehicleYearList.length; i++) {
                    $scope.MasterVehicleYearList[i].AddedOn = moment.utc($scope.MasterVehicleYearList[i].AddedOn).local().format();
                }
            })
            .then(function () {
                $scope.cancelCreateMasterVehicleYearModal();
            })
            .then(function () {
                if ($('#VehicleModal').hasClass('in')) {
                    $scope.ProcessMasterYear(null, $scope.MasterVehicleYearId, null);
                }
            })
        })
    }

    $scope.UpdateMasterVehicleMaker = function (id, data) {
        vehiclesettingsServices.UpdateMasterVehicleMaker(id, data).then(function (response) {
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
            LoadAllmasterSettingsData();
        })
    }
    $scope.CreateMasterVehicleMaker = function () {
        $scope.MasterVehicleMaker.AdminId = $rootScope.AdminId;
        vehiclesettingsServices.CreateMasterVehicleMaker($scope.MasterVehicleMaker).then(function (response) {
            if (response.data.IsReport == "Success") {
                toastr.success(response.data.Message, "Success!");
                if ($('#VehicleModal').hasClass('in')) {
                    $scope.MasterVehicleMakerId = response.data.MasterVehicleMakerId;
                }
            }
            else if (response.data.IsReport == "Warning") {
                toastr.warning(response.data.Message, "Warning!");
            }
            else if (response.data.IsReport == "Error") {
                toastr.error(response.data.Message, "Error!");
            }
        })
        .then(function () {
            vehiclesettingsServices.GetMasterVehicleMakerList().then(function (response) {
                $scope.MasterVehicleMakerList = response.data;
                for (let i = 0; i < $scope.MasterVehicleMakerList.length; i++) {
                    $scope.MasterVehicleMakerList[i].AddedOn = moment.utc($scope.MasterVehicleMakerList[i].AddedOn).local().format();
                }
            })
            .then(function () {
                $scope.cancelCreateMasterVehicleMakerModal();
            })
            .then(function () {
                if ($('#VehicleModal').hasClass('in')) {
                    $scope.ProcessMasterMaker(null, $scope.MasterVehicleMakerId, null);
                }
            })
        })
    }

    $scope.UpdateMasterVehicleModel = function (id, data) {
        vehiclesettingsServices.UpdateMasterVehicleModel(id, data).then(function (response) {
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
            LoadAllmasterSettingsData();
        })
    }
    $scope.CreateMasterVehicleModel = function () {
        $scope.MasterVehicleModel.AdminId = $rootScope.AdminId;
        vehiclesettingsServices.CreateMasterVehicleModel($scope.MasterVehicleModel).then(function (response) {
            if (response.data.IsReport == "Success") {
                toastr.success(response.data.Message, "Success!");
                if ($('#VehicleModal').hasClass('in')) {
                    $scope.MasterVehicleModelId = response.data.MasterVehicleModelId;
                }
            }
            else if (response.data.IsReport == "Warning") {
                toastr.warning(response.data.Message, "Warning!");
            }
            else if (response.data.IsReport == "Error") {
                toastr.error(response.data.Message, "Error!");
            }
        })
        .then(function () {
            vehiclesettingsServices.GetMasterVehicleModelList().then(function (response) {
                $scope.MasterVehicleModelList = response.data;
                for (let i = 0; i < $scope.MasterVehicleModelList.length; i++) {
                    $scope.MasterVehicleModelList[i].AddedOn = moment.utc($scope.MasterVehicleModelList[i].AddedOn).local().format();
                }
            })
            .then(function () {
                $scope.cancelCreateMasterVehicleModelModal();
            })
            .then(function () {
                if ($('#VehicleModal').hasClass('in')) {
                    $scope.ProcessMasterModel(null, $scope.MasterVehicleModelId, null);
                }
            })
        })
    }

    $scope.UpdateMasterVehicleSubModel = function (id, data) {
        vehiclesettingsServices.UpdateMasterVehicleSubModel(id, data).then(function (response) {
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
            LoadAllmasterSettingsData();
        })
    }
    $scope.CreateMasterVehicleSubModel = function () {
        $scope.MasterVehicleSubModel.AdminId = $rootScope.AdminId;
        vehiclesettingsServices.CreateMasterVehicleSubModel($scope.MasterVehicleSubModel).then(function (response) {
            if (response.data.IsReport == "Success") {
                toastr.success(response.data.Message, "Success!");
                if ($('#VehicleModal').hasClass('in')) {
                    $scope.MasterVehicleSubModelId = response.data.MasterVehicleSubModelId;
                }
            }
            else if (response.data.IsReport == "Warning") {
                toastr.warning(response.data.Message, "Warning!");
            }
            else if (response.data.IsReport == "Error") {
                toastr.error(response.data.Message, "Error!");
            }
        })
        .then(function () {
            vehiclesettingsServices.GetMasterVehicleSubModelList().then(function (response) {
                $scope.MasterVehicleSubModelList = response.data;
                for (let i = 0; i < $scope.MasterVehicleSubModelList.length; i++) {
                    $scope.MasterVehicleSubModelList[i].AddedOn = moment.utc($scope.MasterVehicleSubModelList[i].AddedOn).local().format();
                }
            })
            .then(function () {
                $scope.cancelCreateMasterVehicleSubModelModal();
            })
            .then(function () {
                if ($('#VehicleModal').hasClass('in')) {
                    $scope.ProcessMasterSubModel(null, $scope.MasterVehicleSubModelId, null);
                }
            })
        })
    }

    $scope.UpdateMasterVehicleEngine = function (id, data) {
        vehiclesettingsServices.UpdateMasterVehicleEngine(id, data).then(function (response) {
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
            LoadAllmasterSettingsData();
        })
    }
    $scope.CreateMasterVehicleEngine = function () {
        $scope.MasterVehicleEngine.AdminId = $rootScope.AdminId;
        vehiclesettingsServices.CreateMasterVehicleEngine($scope.MasterVehicleEngine).then(function (response) {
            if (response.data.IsReport == "Success") {
                toastr.success(response.data.Message, "Success!");
                if ($('#VehicleModal').hasClass('in')) {
                    $scope.MasterVehicleEngineId = response.data.MasterVehicleEngineId;
                }
            }
            else if (response.data.IsReport == "Warning") {
                toastr.warning(response.data.Message, "Warning!");
            }
            else if (response.data.IsReport == "Error") {
                toastr.error(response.data.Message, "Error!");
            }
        })
        .then(function () {
            vehiclesettingsServices.GetMasterVehicleEngineList().then(function (response) {
                $scope.MasterVehicleEngineList = response.data;
                for (let i = 0; i < $scope.MasterVehicleEngineList.length; i++) {
                    $scope.MasterVehicleEngineList[i].AddedOn = moment.utc($scope.MasterVehicleEngineList[i].AddedOn).local().format();
                }
            })
            .then(function () {
                $scope.cancelCreateMasterVehicleEngineModal();
            })
            .then(function () {
                if ($('#VehicleModal').hasClass('in')) {
                    vehiclesettingsServices.GetMasterVehicleEngineFilteredList($scope.Vehicle.MasterVehicleYearId, $scope.Vehicle.MasterVehicleMakerId, $scope.Vehicle.MasterVehicleModelId, $scope.Vehicle.MasterVehicleSubModelId).then(function (response) {
                        $scope.VehicleEngineList = response.data;
                    })
                    .then(function () {
                        $scope.ProcessMasterEngine(null, $scope.MasterVehicleEngineId, null);
                    })
                }
            })
        })
    }

    $scope.CreateVehicle = function () {
        if ($scope.Vehicle.MasterVehicleYearId && $scope.Vehicle.MasterVehicleMakerId && $scope.Vehicle.MasterVehicleModelId && $scope.Vehicle.MasterVehicleSubModelId && $scope.Vehicle.MasterVehicleEngineId) {
            vehiclesettingsServices.CreateVehicle($scope.Vehicle).then(function (response) {
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
                LoadAllmasterSettingsData();
            })
            .then(function () {
                $scope.cancelCreateVehicleModal();
            })
        }
    }

    //====================================================================Miscellaneous Function======================================================================





    //====================================================================Garbage Code================================================================================




});