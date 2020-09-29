CarPartsApp.controller("subcategorylistController", function ($scope, subcategorylistServices, appServices, customServices, $rootScope, $state, toastr, $stateParams, $compile, $cookies, $timeout, $location, $window) {
    $scope.TempVehicleFilter = {};
    if ($stateParams.categoryId != null && $stateParams.categoryId != "") {
        //====================================================================Declaration==============================================================================
        if (customServices.IsGuid($stateParams.categoryId)) {
            $scope.CategoryId = $stateParams.categoryId

            //====================================================================Automated HTTP Calls=================================================================
            if ($cookies.get("SelectedVehicle")) {
                let data = JSON.parse($cookies.get("SelectedVehicle"));
                $scope.TempVehicleFilter.Id = data.Id;
                $scope.TempVehicleFilter.Model = data.Model;
                $scope.TempVehicleFilter.Year = data.Year;
                $scope.TempVehicleFilter.MakerName = data.MakerName;
                $scope.TempVehicleFilter.ModelName = data.ModelName;
                $scope.TempVehicleFilter.VehicleIdList = data.VehicleIdList;
                if (data.SubModelName) {
                    $scope.TempVehicleFilter.SubModelName = data.SubModelName;
                }
                else {
                    $scope.TempVehicleFilter.SubModelName = null;
                }
                if (data.EngineName) {
                    $scope.TempVehicleFilter.EngineName = data.EngineName;
                }
                else {
                    $scope.TempVehicleFilter.EngineName = null;
                }
            }
            else {
                $scope.TempVehicleFilter = null;
            }
            subcategorylistServices.GetAllSubCategoryListByCategoryId($stateParams.categoryId, $scope.TempVehicleFilter).then(function (response) {
                $scope.SubCategoryList = response.data;
            })
        }
        else {
            toastr.error("Invalid Parameter");
            $state.go("404");
        }
    }
    else {
        $state.go("404");
    }

    //====================================================================Element Processing==========================================================================



    //====================================================================Object Processing===========================================================================



    //====================================================================Modal Operation=============================================================================



    //====================================================================DB Operation================================================================================



    //====================================================================Miscellaneous Function======================================================================



    //====================================================================Garbage Code================================================================================

});