CarPartsApp.controller("productController", function ($scope, productServices, appServices, customServices, $rootScope, $state, toastr, $stateParams, $compile, $cookies, $timeout, $location, $window) {
    $scope.TempVehicleFilter = {};
    if ($stateParams.productCategoryId != null && $stateParams.productCategoryId != "") {
        //====================================================================Declaration==============================================================================
        if (customServices.IsGuid($stateParams.productCategoryId)) {
            $scope.SubCategoryId = $stateParams.productCategoryId

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
            productServices.GetProductListByProductCategory($stateParams.productCategoryId, $scope.TempVehicleFilter).then(function (response) {
                $scope.ProductList = response.data.Result;
                $scope.Hierarchy = response.data.Hierarchy;
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