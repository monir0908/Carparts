CarPartsApp.controller("manage_taskController", function (ManageTaskServices, ManageGroupServices, appServices, $scope, $rootScope, $q, $filter, $compile, $timeout, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder, $state, toastr) {
    $rootScope.CurrentState = $state.current.name;
    $rootScope.trackingOpen = false;
    $rootScope.paymentOpen = false;
    $rootScope.billOpen = false;
    $rootScope.taskOpen = true;
    $scope.AccordionOpen = true;
    $scope.TaskTypeList = [];
    $scope.TaskTypeList.push({ TaskType: 'Individual' }, { TaskType: 'Group' });
    //These are for Group Operation
    $scope.CustomerListFilter = [];
    $scope.GroupList = [];
    $scope.GroupMembers = [];
    $scope.CustomerDetails = {};
    //$scope.Group = null;
    //$scope.Customer = null;
    //These are for Group Operation
    //These are for Task Operation
    $scope.CustomerList = [];
    $scope.CopyOfCustomerList = [];
    $scope.Task = {};
    $scope.TaskMembers = [];
    $scope.CopyOfTaskMembers = [];
    $scope.NewTaskMembers = [];
    $scope.RemovedTaskMembers = [];
    $scope.TaskDetails = {};
    $scope.GroupMemberCustomer = {};
    $scope.Loader = false;
    $scope.GroupMemberTaskSecondaryStatusModel = null;
    //These are for Task Operation

    appServices.CheckCompanyHasToPayForNextSession().then(function (response) {
        $rootScope.PckgRenewWarning = response.data;

    })
    .then(function () {
        if ($rootScope.PckgRenewWarning.Result == true && $rootScope.PckgRenewWarning.AuthStatus != "Suspended") {
            //Prepare the duration
            if ($rootScope.PckgRenewWarning.PayingDuration.DurationInDay != 0) {
                $scope.Duration = $rootScope.PckgRenewWarning.PayingDuration.DurationInDay + ' day(s)';
            }
            else if ($rootScope.PckgRenewWarning.PayingDuration.DurationInHour != 0) {
                $scope.Duration = $rootScope.PckgRenewWarning.PayingDuration.DurationInHour + ' hour(s)';
            }
            else if ($rootScope.PckgRenewWarning.PayingDuration.DurationInMinute != 0) {
                $scope.Duration = $rootScope.PckgRenewWarning.PayingDuration.DurationInMinute + ' minute(s)';
            }
            else if ($rootScope.PckgRenewWarning.PayingDuration.DurationInSecond != 0) {
                $scope.Duration = $rootScope.PckgRenewWarning.PayingDuration.DurationInSecond + ' second(s)';
            }
            toastr.warning("Please pay within " + $scope.Duration + " to continue to avail current service. Failure to do so will result in package suspension.", "Caution!", {
                autoDismiss: false,
                containerId: 'toast-container',
                maxOpened: 0,
                newestOnTop: true,
                positionClass: 'toast-top-left',
                preventDuplicates: false,
                preventOpenDuplicates: false,
                target: 'body',
                progressBar: true,
                timeOut: 600000,
                closeButton: true,
                tapToDismiss: false,
            })

        }
    });


    //========================================================================Data table===========================================================================
    $scope.vm = {};
    $scope.vm.dtInstance = {};
    $scope.vm.dtColumnDefs = [DTColumnDefBuilder.newColumnDef(5).notSortable()
    ];
    $scope.vm.dtOptions = DTOptionsBuilder.newOptions()
                            .withOption('paging', true)
                            .withOption('searching', true)
                            .withOption('info', true);

    //==============Make Simple Table As Data-Table===============
    $scope.sort = function (keyname) {
        $scope.sortKey = keyname;   //set the sortKey to the param passed
        $scope.reverse = !$scope.reverse; //if true make it false and vice versa
    };


    //======================================================================Map Operation==========================================================================
    //------------------------------Reverse Geo Coding FUNC--------------------------------
    function geocodeLatLng(geocoder) {
        $scope.inputModel = $scope.Task.TaskLat + "," + $scope.Task.TaskLng;
        $scope.latlngStr = $scope.inputModel.split(',', 2);
        $scope.latlng = { lat: parseFloat($scope.latlngStr[0]), lng: parseFloat($scope.latlngStr[1]) };
        geocoder.geocode({ 'location': $scope.latlng }, function (results, status) {
            if (status === 'OK') {
                if (results[0]) {
                    $scope.SearchedPlace = results[0].formatted_address;
                    $scope.infowindow.setContent("<span class='badge x-darker-1'>" + $scope.SearchedPlace + "</span>");
                    $scope.infowindow.open($scope.map, $scope.marker);
                    $scope.PristinesCI = false;

                } else {
                    toastr.error('No result found', 'Error!');
                }
            } else {
                toastr.error('Geocoder failed due to: ' + status);
            }
        });
    };


    $scope.LoadMap = function () {
        $timeout(function () {
            if (($scope.Task.TaskLat > 0 || $scope.Task.TaskLng > 0)) {
                $scope.map = new google.maps.Map(document.getElementById('map-canvas'), {
                    center: { lat: $scope.Task.TaskLat, lng: $scope.Task.TaskLng },
                    zoom: 17,
                    draggable: true,
                    gestureHandling: 'greedy'
                });

                $scope.marker = new google.maps.Marker({
                    map: $scope.map,
                    position: {
                        lat: $scope.Task.TaskLat,
                        lng: $scope.Task.TaskLng
                    },
                    draggable: true
                });
            } else if (($scope.Task.TaskLat <= 0 || $scope.Task.TaskLng <= 0) || ($scope.Task.TaskLat == undefined || $scope.Task.TaskLng == undefined)) {
                $scope.map = new google.maps.Map(document.getElementById('map-canvas'), {
                    center: { lat: 23.915416, lng: 89.9129994 },
                    zoom: 6,
                    draggable: true,
                    gestureHandling: 'greedy'
                });
                $scope.infowindow = new google.maps.InfoWindow();
                $scope.marker = new google.maps.Marker({ map: $scope.map });
            }
            $scope.geocoder = new google.maps.Geocoder(); //Geocoder Defination
            $scope.infowindow = new google.maps.InfoWindow(); //InfoWindow Defination
            var input = document.getElementById('SearchLocation');
            var autocomplete = new google.maps.places.Autocomplete(input);

            // Bind the map's bounds (viewport) property to the autocomplete object,
            // so that the autocomplete requests use the current map bounds for the
            // bounds option in the request.
            autocomplete.bindTo('bounds', $scope.map);
            var contentString = "<span class='badge x-darker-1'>" + $scope.Task.CompanyName + "</span>";

            //Creating infoWindow
            $scope.infowindow.setContent(contentString);

            //Set Map and Marker to Dragable position while editing


            autocomplete.addListener('place_changed', function () {
                $scope.infowindow.close();
                $scope.marker.setVisible(false);
                var place = autocomplete.getPlace();
                if (!place.geometry) {
                    // User entered the name of a Place that was not suggested and
                    // pressed the Enter key, or the Place Details request failed.
                    window.alert("No details available for input: '" + place.name + "'");
                    return;
                }

                // If the place has a geometry, then present it on a map.
                if (place.geometry.viewport) {
                    $scope.map.fitBounds(place.geometry.viewport);
                    $scope.Task.TaskLat = place.geometry.location.lat();
                    $scope.Task.TaskLng = place.geometry.location.lng();
                    //toastr.info("Now you can drag the pin to get precious location", "Caution!");
                    //console.log("--After Binding New Location--");
                    //console.log($scope.Task.TaskLat);
                    //console.log($scope.Task.TaskLng);
                    //console.log(place.adr_address);
                } else {
                    $scope.map.setCenter(place.geometry.location);
                    $scope.map.setZoom(17);  // Why 17? Because it looks good.
                }
                $scope.marker.setPosition(place.geometry.location);
                $scope.marker.setVisible(true);
                $scope.marker.setOptions({ draggable: true });
                $scope.infowindow.setContent("<span class='badge x-darker-1'>" + place.adr_address + "</span>");
                $scope.infowindow.open($scope.map, $scope.marker);
                geocodeLatLng($scope.geocoder);
            });


            //Click On Empty Map

            //Map CLICK Event
            $scope.map.addListener('click', function (e) {
                if (($scope.Task.TaskLat == undefined || $scope.Task.TaskLat == "") && ($scope.Task.TaskLng == undefined || $scope.Task.TaskLng == "")) {
                    $scope.myLatLng = new google.maps.LatLng(e.latLng.lat(), e.latLng.lng());
                    $scope.Task.TaskLat = e.latLng.lat();
                    $scope.Task.TaskLng = e.latLng.lng();
                    $scope.marker.setVisible(true);
                    $scope.marker.setOptions({ draggable: true });
                    $scope.marker.setPosition($scope.myLatLng);
                    geocodeLatLng($scope.geocoder);
                }
                else {
                    toastr.warning('There is already a marker on this map available. Please drag position or search for precious location on search box', 'Warning!');
                }

                //console.log(e.latLng.lat());
                //console.log(e.latLng.lng());
            });

            //Marker DRAG Event
            $scope.marker.addListener('drag', function () {
                //console.log('Dragging...');
                $scope.infowindow.close();
                $scope.infowindow.setContent(null);
            });

            //Marker CLICK Event
            $scope.marker.addListener('click', function () {
                $scope.infowindow.open($scope.map, $scope.marker);
            });


            //Marker DRAGEND Event
            $scope.marker.addListener('dragend', function (e) {
                $scope.Task.TaskLat = e.latLng.lat();
                $scope.Task.TaskLng = e.latLng.lng();
                //console.log("--Only Draggin LAT")
                //console.log($scope.Task.TaskLat);
                //console.log("--Only Draggin LNG")
                //console.log($scope.Task.TaskLng);
                geocodeLatLng($scope.geocoder);
            });
        }, 3000)
        .then(function () {
        });

    };

    //====================================================================Accordion Operations======================================================================
    $('.collapse').on('shown.bs.collapse', function () {
        $(this).parent().find(".glyphicon-plus").removeClass("glyphicon-plus").addClass("glyphicon-minus");
    }).on('hidden.bs.collapse', function () {
        $(this).parent().find(".glyphicon-minus").removeClass("glyphicon-minus").addClass("glyphicon-plus");
    });

    $scope.Accordion = function () {
        if ($scope.AccordionOpen == true) {
            $scope.AccordionOpen = false;
        }
        else if ($scope.AccordionOpen == false) {
            $scope.AccordionOpen = true;
        }
    }

    //===================================================================Object Processing==========================================================================


    //---------------------------------------------------Multi-Step Form-------------------------------------------------
    $scope.Step = 1;
    $scope.StepUp = function () {
        if ($scope.Step == 1) {
            $scope.SwitchTab2();
        }
    }
    $scope.StepBack = function () {
        if ($scope.Step == 2) {
            $scope.SwitchTab1();
        }
        else if ($scope.Step == 1) {
            toastr.error('This is first step. You can not go back', 'Error!');
        }
    }

    $scope.SwitchTab1 = function () {
        $scope.Step = 1;
        $("li").prevAll().addClass('validated');
        $("li").nextAll().removeClass('validated');

    }
    $scope.SwitchTab2 = function () {
        if ($scope.TaskForm.$invalid == true) {
            toastr.error('You can not jump to next step without filling current form with valid data', 'Error!')
        }
        else {
            $scope.Step = 2;
            $("li").prevAll().addClass('validated');
            $("li").nextAll().removeClass('validated');
        }

    }

    //-------------TaskType Change---------------------------
    $scope.ChangeTaskType = function (taskType) {
        $scope.CustomerList = [];
        $scope.CustomerList = angular.copy($scope.CopyOfCustomerList);
        if (taskType == "Group") {
            for (var i = 0; i < $scope.CustomerListFilter.length; i++) {
                for (var k = 0; k < $scope.CustomerList.length; k++) {
                    if ($scope.CustomerList[k].Id == $scope.CustomerListFilter[i].Id) {
                        //$scope.CustomerList[k].AlreadyAdded = true;
                        var index = $scope.CustomerList.findIndex(x => x.Id == $scope.CustomerListFilter[i].Id);
                        if (index > -1) {
                            $scope.CustomerList.splice(index, 1);
                        }
                    }
                }
            }
        }
        if (taskType == "Individual") {
            $scope.CustomerList = [];
            $scope.CustomerList = angular.copy($scope.CopyOfCustomerList);
        }
    }

    //--------------Datetime picker--------------------------

    //Prevent Date and Time to be taken

    $('#ShortDayPickerForm').on('keypress', function (e) {
        e.preventDefault();
    });

    $('#ShortDayPickerTo').on('keypress', function (e) {
        e.preventDefault();
    });

    $('#SearchCustomer').on('keypress', function (e) {
        if ($scope.AccordionOpen == false) {
            e.preventDefault();
        }
    });

    //Datetime Picker Composing
    $scope.isOpenFrom = false;
    $scope.isOpenTo = false;

    $scope.openCalendarFrom = function (e) {
        e.preventDefault();
        //e.stopPropagation();

        $scope.isOpenFrom = true;
        $scope.isOpenTo = false;
    };

    $scope.openCalendarTo = function (e) {
        e.preventDefault();
        //e.stopPropagation();

        $scope.isOpenTo = true;
        $scope.isOpenFrom = false;
    };

    $scope.DateAndTimeFormObj = {
        date: new Date(),
        datepickerOptions: {
            showWeeks: false,
            minDate: new Date()
        }
    }

    $scope.DateAndTimeToObj = {
        date: new Date(),
        datepickerOptions: {
            showWeeks: false,
            minDate: new Date()
        }
    }


    $scope.TaskStartTimeClosed = function (args) {
        if ($scope.Task.TaskEndTime != undefined) {
            var endTime = moment($scope.Task.TaskEndTime);
            var startTime = moment(args.closeDate);
            var duration = moment.duration(endTime.diff(startTime));
            var days = duration.asDays();
            console.log(duration);
            $scope.durationInMillisecond = duration._milliseconds;
            if ($scope.durationInMillisecond < 0) {
                $scope.Task.TaskEndTime = null;
                $scope.TaskForm.$setPristine();
                $scope.TaskTimeDurationValidationMessage = "Task start time can't be greater than task end time";
            }
            else {
                $scope.TaskTimeDurationValidationMessage = "";
            }
        }
        $scope.DateAndTimeToObj = {
            date: new Date(),
            datepickerOptions: {
                showWeeks: false,
                minDate: args.closeDate
            }
        }
    };

    $scope.TaskEndTimeClosed = function (args) {
        if ($scope.Task.TaskStartTime != undefined) {
            var startTime = moment($scope.Task.TaskStartTime);
            var endTime = moment(args.closeDate);
            var duration = moment.duration(endTime.diff(startTime));
            var days = duration.asDays();
            console.log(duration);
            $scope.durationInMillisecond = duration._milliseconds;
            if ($scope.durationInMillisecond < 0) {
                $scope.Task.TaskEndTime = null;
                $scope.TaskForm.$setPristine();
                $scope.TaskTimeDurationValidationMessage = "Task start time can't be greater than task end time";
            }
            else {
                $scope.TaskTimeDurationValidationMessage = "";
            }
        }
    };

    //Process for Reading while creating/updating task
    function ProcessDateTimePikerForCU() {
        if ($scope.Task.TaskStartTime != null) {
            $scope.Task.TaskStartTime = moment($scope.Task.TaskStartTime).format("YYYY-MM-DD HH:mm:ss.sss")
        }
        if ($scope.Task.TaskEndTime != null) {
            $scope.Task.TaskEndTime = moment($scope.Task.TaskEndTime).format("YYYY-MM-DD HH:mm:ss.sss")
        }
    }

    //Process for Reading while editing task
    function ProcessDateTimePikerForReading() {
        if ($scope.Task.TaskStartTime != null) {
            $scope.Task.TaskStartTime = new Date($scope.Task.TaskStartTime);
        }
        if ($scope.Task.TaskEndTime != null) {
            $scope.Task.TaskEndTime = $scope.Task.TaskEndTime = new Date($scope.Task.TaskEndTime);
        }
    }







    //------------------TaskMemberObjs Processing C------------------

    //Push Customer One after another
    $scope.PushToCustomerListFilter = function (id, username, firstname, lastname, isactive) {
        if (id != null || id != undefined) {
            if ($scope.CustomerListFilter.length == 0) {
                $scope.CustomerListFilter.push({ Id: id, Username: username, FirstName: firstname, LastName: lastname, IsActive: isactive, Key: $scope.CustomerListFilter.length + 1 });
                var index = $scope.CustomerList.findIndex(x => x.Id == id);
                if (index > -1) {
                    $scope.CustomerList.splice(index, 1);
                }
            }
            else {
                for (var i = 0; i < $scope.CustomerListFilter.length; i++) {
                    if ($scope.CustomerListFilter[i].Id != id) {
                        var index = $scope.CustomerListFilter.findIndex(x => x.Id == id);
                        if (index == -1) {
                            $scope.CustomerListFilter.push({ Id: id, Username: username, FirstName: firstname, LastName: lastname, IsActive: isactive, Key: $scope.CustomerListFilter.length + 1 });
                            var index = $scope.CustomerList.findIndex(x => x.Id == id);
                            if (index > -1) {
                                $scope.CustomerList.splice(index, 1);
                                for (var k = 0; k < $scope.CustomerListFilter.length; k++) {
                                    $scope.CustomerListFilter[k].Key = k + 1;
                                }
                            }

                            //console.log($scope.CustomerListFilter);
                            return;
                        }
                        else {
                            //console.log($scope.CustomerListFilter);
                        }
                    }
                    else if ($scope.CustomerListFilter[i].Id == id) {
                        var index = $scope.CustomerListFilter.findIndex(x => x.Id == id);
                        if (index > -1) {
                            $scope.CustomerListFilter.splice(index, 1);
                            for (var j = 0; j < $scope.CopyOfCustomerList.length; j++) {
                                if ($scope.CopyOfCustomerList[j].Id == id) {
                                    $scope.CustomerList.push($scope.CopyOfCustomerList[j]);
                                }
                            }
                            for (var k = 0; k < $scope.CustomerListFilter.length; k++) {
                                $scope.CustomerListFilter[k].Key = k + 1;
                            }
                        }

                        return;
                    }
                }
            }
            console.log($scope.CustomerListFilter);
        }
    };


    //Add more customer into the CustomerListFilter (Pre TaskMemberObjs)
    $scope.addMoreCustomer = function (customerId) {
        ManageGroupServices.GetCP_CustomerDetails(customerId).then(function (response) {
            var id = customerId;
            var username = response.data.Username;
            var firstname = response.data.FirstName;
            var lastname = response.data.LastName;
            var isactive = response.data.IsActive;
            if (id != null || id != undefined) {
                if ($scope.CustomerListFilter.length == 0) {
                    $scope.CustomerListFilter.push({ Id: id, Username: username, FirstName: firstname, LastName: lastname, IsActive: isactive, Key: $scope.CustomerListFilter.length + 1 });
                    $('#AddCustomer').val('');
                    var index = $scope.CustomerList.findIndex(x => x.Id == id);
                    if (index > -1) {
                        $scope.CustomerList.splice(index, 1);
                    }
                }
                else {
                    for (var i = 0; i < $scope.CustomerListFilter.length; i++) {
                        if ($scope.CustomerListFilter[i].Id != id) {
                            var index = $scope.CustomerListFilter.findIndex(x => x.Id == id);
                            if (index == -1) {
                                $scope.CustomerListFilter.push({ Id: id, Username: username, FirstName: firstname, LastName: lastname, IsActive: isactive, Key: $scope.CustomerListFilter.length + 1 });
                                $('#AddCustomer').val('');
                                var index = $scope.CustomerList.findIndex(x => x.Id == id);
                                if (index > -1) {
                                    $scope.CustomerList.splice(index, 1);
                                }
                                return;
                            }
                            else {
                            }
                        }
                        else if ($scope.CustomerListFilter[i].Id == id) {
                            var index = $scope.CustomerListFilter.findIndex(x => x.Id == id);
                            $('#AddCustomer').val('');
                            //if (index > -1) {
                            //    $scope.CustomerListFilter.splice(index, 1);
                            //}
                            return;
                        }
                    }
                }
                console.log($scope.CustomerListFilter);
                $('#AddCustomer').val('');
            }
        });
    }


    //Final Procesing for C
    function TaskMemberObjsProcess() {
        if ($scope.Task.TaskType == "Group") {
            $scope.TaskMembers = [];
            for (var i = 0; i < $scope.CustomerListFilter.length; i++) {
                $scope.TaskMembers.push({ GroupId: $scope.Group.Id, CustomerId: $scope.CustomerListFilter[i].Id });
            }
        }
        else if ($scope.Task.TaskType == "Individual") {
            $scope.TaskMembers = [];
            $scope.TaskMembers.push({ GroupId: null, CustomerId: $scope.Customer.Id });
        }
    }



    //------------------TaskMemberObjs Processing U------------------

    //Push Customer One after another
    $scope.PushToTaskMembers = function (id) {

        if (CheckWeatherCustomerIsCompletelyNewMember(id) == true) {
            for (var i = 0; i < $scope.CustomerList.length; i++) {
                if (id == $scope.CustomerList[i].Id) {
                    //Push it into TaskMembers
                    $scope.TaskMembers.push({ CustomerId: $scope.CustomerList[i].Id, GroupMemberFirstName: $scope.CustomerList[i].FirstName, GroupMemberLastName: $scope.CustomerList[i].LastName })
                    //Push it into NewTaskMembers
                    if (CheckWeatherCustomerIsInNewTaskMembers(id) == false) {
                        $scope.NewTaskMembers.push({ CustomerId: $scope.CustomerList[i].Id })
                    }
                }
            }
            //Remove it from RemovedTaskMembers
            if (CheckWeatherCustomerIsInRemovedTaskMembers(id) == true) {
                var index = $scope.RemovedTaskMembers.findIndex(x => x.CustomerId == id);
                if (index > -1) {
                    $scope.RemovedTaskMembers.splice(index, 1);
                }
            }
        }

        else if (CheckWeatherCustomerIsCompletelyNewMember(id) == false) {
            for (var i = 0; i < $scope.CopyOfTaskMembers.length; i++) {
                if (id == $scope.CopyOfTaskMembers[i].CustomerId) {
                    //Push it into TaskMembers
                    $scope.TaskMembers.push($scope.CopyOfTaskMembers[i])
                    //Remove it From RemovedTaskMembers
                    if (CheckWeatherCustomerIsInRemovedTaskMembers(id) == true) {
                        var index = $scope.RemovedTaskMembers.findIndex(x => x.CustomerId == id);
                        if (index > -1) {
                            $scope.RemovedTaskMembers.splice(index, 1);
                        }
                    }
                }
            }
        }

        console.log("$scope.NewTaskMembers");
        console.log($scope.NewTaskMembers);
        console.log("$scope.RemovedTaskMembers");
        console.log($scope.RemovedTaskMembers)
        console.log("========================================================");
        ProcessFilteredCustomerListOnPushing(id);

    };

    $scope.RemoveFromTaskMember = function (id) {
        if (CheckWeatherCustomerIsCompletelyNewMember(id) == true) {
            //Remove it from NewTaskMembers
            if (CheckWeatherCustomerIsInNewTaskMembers(id) == true) {
                var index = $scope.NewTaskMembers.findIndex(x => x.CustomerId == id);
                if (index > -1) {
                    $scope.NewTaskMembers.splice(index, 1);
                }
            }
            //Add it into RemovedTaskMembers
            if (CheckWeatherCustomerIsInRemovedTaskMembers(id) == false) {
                $scope.RemovedTaskMembers.push({ CustomerId: id });
            }

            //Remove it from TaskMembers
            var index = $scope.TaskMembers.findIndex(x => x.CustomerId == id);
            if (index > -1) {
                $scope.TaskMembers.splice(index, 1);
            }
        }

        else if (CheckWeatherCustomerIsCompletelyNewMember(id) == false) {
            //Add it into RemovedTaskMembers
            if (CheckWeatherCustomerIsInRemovedTaskMembers(id) == false) {
                $scope.RemovedTaskMembers.push({ CustomerId: id });
            }
            //Remove it from TaskMembers
            var index = $scope.TaskMembers.findIndex(x => x.CustomerId == id);
            if (index > -1) {
                $scope.TaskMembers.splice(index, 1);
            }
        }

        console.log("$scope.NewTaskMembers");
        console.log($scope.NewTaskMembers);
        console.log("$scope.RemovedTaskMembers");
        console.log($scope.RemovedTaskMembers)
        console.log("========================================================");
        ProcessFilteredCustomerListOnPopping(id);
    };


    //Check Weather the customer was in CopyOfTaskMembers or not
    function CheckWeatherCustomerIsInCopyOfTaskMember(id) {
        var isThere = false;
        for (var i = 0; i < $scope.CopyOfTaskMembers.length; i++) {
            if ($scope.CopyOfTaskMembers[i].CustomerId == id) {
                isThere = true;
                break;
            }
            else {
                isThere = false;
            }
        }
        return isThere;
    }

    //Check Weather the customer was in RemovedTaskMembers or not
    function CheckWeatherCustomerIsInRemovedTaskMembers(id) {
        var isThere = false;
        for (var i = 0; i < $scope.RemovedTaskMembers.length; i++) {
            if ($scope.RemovedTaskMembers[i].CustomerId == id) {
                isThere = true;
                break;
            }
            else {
                isThere = false;
            }
        }
        return isThere;
    }

    //Check Weather the customer was in NewTaskMembers or not
    function CheckWeatherCustomerIsInNewTaskMembers(id) {
        var isThere = false;
        for (var i = 0; i < $scope.NewTaskMembers.length; i++) {
            if ($scope.NewTaskMembers[i].CustomerId == id) {
                isThere = true;
                break;
            }
            else {
                isThere = false;
            }
        }
        return isThere;
    }

    //Check Weather the customer in CompletelyNewMember or not
    function CheckWeatherCustomerIsCompletelyNewMember(id) {
        var isThere = false;
        for (var i = 0; i < $scope.CopyOfTaskMembers.length; i++) {
            if ($scope.CopyOfTaskMembers[i].CustomerId == id) {
                isThere = false;
                break;
            }
            else {
                isThere = true;
            }
        }
        return isThere;
    }

    //Process Filter Customer List
    function ProcessFilteredCustomerList() {
        for (var i = 0; i < $scope.TaskMembers.length; i++) {
            for (var k = 0; k < $scope.CustomerList.length; k++) {
                if ($scope.CustomerList[k].Id == $scope.TaskMembers[i].CustomerId) {
                    var index = $scope.CustomerList.findIndex(x => x.Id == $scope.TaskMembers[i].CustomerId);
                    if (index > -1) {
                        $scope.CustomerList.splice(index, 1);
                    }
                }
            }
        }
        $('#AddCustomer').val('');
        $scope.AtLeastOneGroupMemberCheckerWhileEditing();
    }

    function ProcessFilteredCustomerListOnPushing(id) {
        var index = $scope.CustomerList.findIndex(x => x.Id == id);
        if (index > -1) {
            $scope.CustomerList.splice(index, 1);
        }
        $('#AddCustomer').val('');
        $scope.AtLeastOneGroupMemberCheckerWhileEditing();
    }


    function ProcessFilteredCustomerListOnPopping(id) {
        for (var j = 0; j < $scope.CopyOfCustomerList.length; j++) {
            if ($scope.CopyOfCustomerList[j].Id == id) {
                $scope.CustomerList.push($scope.CopyOfCustomerList[j]);
            }
        }
        $('#AddCustomer').val('');
        $scope.AtLeastOneGroupMemberCheckerWhileEditing();
    }

    $scope.AtLeastOneGroupMemberCheckerWhileEditing = function () {
        if ($scope.TaskMembers.length > 0) {
            return true;
        }
        else {
            return false;
        }
    }


    //------------------TaskDetailObjs Processing For C & U------------------
    function TaskDescriptionObjProcess() {
        $scope.TaskDetails = {};
        $scope.TaskDetails = {
            Description: $scope.TaskDetail
        }
    }


    //------------------TaskObj Processing For C & U------------------
    function TaskObjProcess() {
        $scope.Task.CompanyId = $rootScope.CompanyId;
    }

    //GroupMemberTaskSecondary Object processing For U
    $scope.GroupMemberTaskSecondaryStatusList = [{ value: 'In-Progress' }, { value: 'Complete' }]; //For ng-options
    $scope.FilterGroupMemberTaskSecondaryStatusList = function (model) {
        return function (item) {

            if (model == "Complete") {
                return false;
            }
            return true;
        };
    } //ng-options filter



    //========================================================================Modal Operation======================================================================

    //Modal Scroll Stuck Issue Solve
    $('.modal').on('hidden.bs.modal', function (e) {
        if ($('.modal').hasClass('in')) {
            $('body').addClass('modal-open');
        }
    });


    //-----------Task Modal--------------
    $scope.editTaskModal = function (TaskId) {
        ManageTaskServices.GetTaskDetailsByTaskId(TaskId).then(function (response) {
            $scope.Task = response.data;
            $scope.TaskMembers = response.data.TaskMembers;
            //Process for group data
            if ($scope.Task.TaskType == "Group") {
                for (var i = 0; i < $scope.TaskMembers.length; i++) {
                    $scope.TaskMembers[i].Key = i;
                }
            }
                //Processing for individual data
            else if ($scope.Task.TaskType == "Individual") {

                ManageGroupServices.GetCP_CustomerDetails($scope.TaskMembers[0].CustomerId).then(function (response) {
                    $scope.CustomerDetails = response.data;
                    if ($scope.CustomerDetails.Photo == null) {
                        if ($scope.CustomerDetails.Gender == "Male") {
                            $scope.src = "../Company_Images/Customer_Images/male.png";
                        }
                        else if ($scope.CustomerDetails.Gender == "Female") {
                            $scope.src = "../Company_Images/Customer_Images/female.jpg";
                        }
                    }
                    else {
                        $scope.src = "../Company_Images/Customer_Images/" + $scope.CustomerDetails.Photo;
                    }
                });
            }
            $scope.CopyOfTaskMembers = angular.copy($scope.TaskMembers);
            $scope.TaskDetails = response.data.TaskDescriptions;
            ProcessDateTimePikerForReading();
            ProcessFilteredCustomerList();
        })
        .then(function () {
            $('#TaskModal').modal('show');
        })
    };

    $scope.cancelTaskModal = function () {
        $('#TaskModal').modal('hide');
        $scope.TaskForm.$setPristine();
        $scope.TaskForm.$setUntouched();
        $timeout(function () {
            $scope.Task = {};
            $scope.CustomerDetails = {};
            $scope.CustomerListFilter = [];
            $scope.TaskDetails = {};
            $scope.TaskDetail = null;
            $scope.AddCustomerId = null;
            $scope.Group = null;
            $scope.Step = 1;
            $scope.SwitchTab1();
            $scope.TaskMembers = [];
            $scope.CopyOfTaskMembers = [];
            $scope.NewTaskMembers = [];
            $scope.RemovedTaskMembers = [];
            $scope.GroupMemberCustomer = {};
            $scope.Loader = false;
            for (var j = 0; j < $scope.CustomerList.length; j++) {
                //$scope.CustomerList[j].AlreadyAdded = false;
            }
            $('#SearchLocation').val("");
            $('#a_taskDetails').trigger('click');
            $scope.LoadMap();//This is the culprit of firstChild error

        }, 200)
    };
    //-----------Task Modal--------------------------


    //-----------TaskDescription Modal---------------    

    $scope.cancelTaskDescriptionModal = function () {
        $('#AddTaskDescriptionModal').modal('hide');
        $scope.TaskForm.$setPristine();
        $scope.TaskForm.$setUntouched();
        $timeout(function () {
            $scope.TaskDetail = null;
            $scope.TaskDescriptionsForm.$setPristine();
            $scope.TaskDescriptionsForm.$setUntouched();
            $scope.Loader = false;
        }, 200)
    }



    //-----------Customer Details Modal--------------
    $scope.openCustomerDetailsModal = function (customerId) {
        ManageGroupServices.GetCP_CustomerDetails(customerId).then(function (response) {
            $scope.CustomerDetails = response.data;
            if ($scope.CustomerDetails.Photo == null) {
                if ($scope.CustomerDetails.Gender == "Male") {
                    $scope.src = "../Company_Images/Customer_Images/male.png";
                }
                else if ($scope.CustomerDetails.Gender == "Female") {
                    $scope.src = "../Company_Images/Customer_Images/female.jpg";
                }
            }
            else {
                $scope.src = "../Company_Images/Customer_Images/" + $scope.CustomerDetails.Photo;
            }
        })
        .then(function () {
            $('#CustomerDetailsModal').modal('show');
        })
    };

    $scope.cancelCustomerDetailsModal = function () {
        $('#CustomerDetailsModal').modal('hide');
        $timeout(function () {
            $scope.CustomerDetails = {};
        }, 200);
    };


    //===============================================================================DBOperations==================================================================

    //--------------Getting List------------
    ManageTaskServices.GetTaskListByCompanyId($rootScope.CompanyId).then(function (response) {
        $scope.TaskList = response.data;
    });
    ManageGroupServices.GetGroupListByCompanyId($rootScope.CompanyId).then(function (response) {
        $scope.GroupList = response.data;
    });
    ManageGroupServices.GetGroupCreatableCustomerListByCompanyId($rootScope.CompanyId).then(function (response) {
        $scope.CustomerList = response.data;

        for (var i = 0; i < $scope.CustomerList.length; i++) {
            $scope.CustomerList[i].FullName = response.data[i].FirstName + ' ' + response.data[i].LastName;
            $scope.src = "../Company_Images/Customer_Images/" + response.data[i].Photo;
        }
        $scope.CopyOfCustomerList = angular.copy($scope.CustomerList);
    });


    //---------------Getting Propagation Details------------

    $scope.typeaheadonselectgroup = function ($item, $model, $label, $event) {
        $scope.CustomerListFilter = [];


        ManageGroupServices.GetGroupCreatableCustomerListByCompanyId($rootScope.CompanyId).then(function (response) {
            $scope.CustomerList = response.data;

            for (var i = 0; i < $scope.CustomerList.length; i++) {
                $scope.CustomerList[i].FullName = response.data[i].FirstName + ' ' + response.data[i].LastName;
                $scope.src = "../Company_Images/Customer_Images/" + response.data[i].Photo;
            }
            $scope.CopyOfCustomerList = angular.copy($scope.CustomerList);
        })
        .then(function () {
            ManageGroupServices.GetGroupDetails($model.Id).then(function (response) {
                for (var i = 0; i < response.data.GroupMembers.length; i++) {
                    $scope.CustomerListFilter.push({ Id: response.data.GroupMembers[i].CustomerId, Username: response.data.GroupMembers[i].Username, FirstName: response.data.GroupMembers[i].FirstName, LastName: response.data.GroupMembers[i].LastName, IsActive: response.data.GroupMembers[i].IsActive, Key: $scope.CustomerListFilter.length + 1 });
                    for (var k = 0; k < $scope.CustomerList.length; k++) {
                        if ($scope.CustomerList[k].Id == response.data.GroupMembers[i].CustomerId) {
                            var index = $scope.CustomerList.findIndex(x => x.Id == response.data.GroupMembers[i].CustomerId);
                            if (index > -1) {
                                $scope.CustomerList.splice(index, 1);
                            }
                        }
                    }
                }
            });
        });
    };

    $scope.typeaheadonselectcustomer = function ($item, $model, $label, $event) {
        ManageGroupServices.GetCP_CustomerDetails($model.Id).then(function (response) {
            $scope.CustomerDetails = response.data;
            if ($scope.CustomerDetails.Photo == null) {
                if ($scope.CustomerDetails.Gender == "Male") {
                    $scope.src = "../Company_Images/Customer_Images/male.png";
                }
                else if ($scope.CustomerDetails.Gender == "Female") {
                    $scope.src = "../Company_Images/Customer_Images/female.jpg";
                }
            }
            else {
                $scope.src = "../Company_Images/Customer_Images/" + $scope.CustomerDetails.Photo;
            }
        })
    };


    //-------------------------Save---------------------

    $scope.CreateTask = function () {
        var ident = false;
        if ($scope.Task.TaskType == "Group") {
            ident = true;
        }
        else if ($scope.Task.TaskType == "Individual") {
            ident = true;
        }
        if ($scope.TaskForm.$invalid == false && ident == true) {
            $scope.Loader = true;
            TaskObjProcess();
            ProcessDateTimePikerForCU();
            TaskMemberObjsProcess();
            TaskDescriptionObjProcess();

            if ($scope.Task.Id == undefined || $scope.Task.Id == null) {
                ManageTaskServices.CreateTask($scope).then(function (response) {
                    if (response.data.IsReport == "TaskNameExists") {
                        toastr.warning(response.data.Message, "Warning!");
                    }
                    else if (response.data.IsReport == "Ok") {
                        toastr.success(response.data.Message, "Success!");
                    }
                    else if (response.data.IsReport == "NotOk") {
                        toastr.error(response.data.Message, "Error!");
                    }
                })
                .then(function () {
                    ManageTaskServices.GetTaskListByCompanyId($rootScope.CompanyId).then(function (response) {
                        $scope.TaskList = response.data;
                    });
                })
                .then(function () {
                    $scope.cancelTaskModal();
                });
            }
        }
        else {
            toastr.error("This form contains invalid data. Can not be submitted", 'Error!');
            $scope.Loader = false;
        }
    };

    //------------Update--------------


    //Update Task
    $scope.UpdateTask = function () {
        if ($scope.TaskDetailsForm.$invalid == false) {
            $scope.Loader = true;
            TaskObjProcess();
            ProcessDateTimePikerForCU();
            TaskMemberObjsProcess();
            TaskDescriptionObjProcess();
            ManageTaskServices.UpdateTask($scope.Task).then(function (response) {
                if (response.data.IsReport == "Ok") {
                    toastr.success(response.data.Message, "Success!");
                }
                else if (response.data.IsReport == "NotOk") {
                    toastr.error(response.data.Message, "Error!");
                }
            })
        .then(function () {
            //Reload The Task List
            ManageTaskServices.GetTaskListByCompanyId($rootScope.CompanyId).then(function (response) {
                $scope.TaskList = response.data;
            });
        })
        .then(function () {
            //Reload The TaskDetails
            ManageTaskServices.GetTaskDetailsByTaskId($scope.Task.Id).then(function (response) {
                $scope.Task = response.data;
                $scope.TaskMembers = response.data.TaskMembers;
                //Process for group data
                if ($scope.Task.TaskType == "Group") {
                    for (var i = 0; i < $scope.TaskMembers.length; i++) {
                        $scope.TaskMembers[i].Key = i;
                    }
                }
                    //Processing for individual data
                else if ($scope.Task.TaskType == "Individual") {

                    ManageGroupServices.GetCP_CustomerDetails($scope.TaskMembers[0].CustomerId).then(function (response) {
                        $scope.CustomerDetails = response.data;
                        if ($scope.CustomerDetails.Photo == null) {
                            if ($scope.CustomerDetails.Gender == "Male") {
                                $scope.src = "../Company_Images/Customer_Images/male.png";
                            }
                            else if ($scope.CustomerDetails.Gender == "Female") {
                                $scope.src = "../Company_Images/Customer_Images/female.jpg";
                            }
                        }
                        else {
                            $scope.src = "../Company_Images/Customer_Images/" + $scope.CustomerDetails.Photo;
                        }
                    });
                }
                $scope.CopyOfTaskMembers = angular.copy($scope.TaskMembers);
                $scope.TaskDetails = response.data.TaskDescriptions;
                ProcessDateTimePikerForReading();
                ProcessFilteredCustomerList();
                $scope.Loader = false;
            })
        });
        }
    }

    //Update Description
    $scope.AddTaskDescription = function () {
        TaskObjProcess();
        TaskDescriptionObjProcess();
        $scope.Loader = true;
        ManageTaskServices.AddTaskDescription($scope).then(function (response) {
            if (response.data.IsReport == "Ok") {
                toastr.success(response.data.Message, "Success!");
            }
            else if (response.data.IsReport == "NotOk") {
                toastr.error(response.data.Message, "Error!");
            }
        })
        .then(function () {
            //Reload The Task List
            ManageTaskServices.GetTaskListByCompanyId($rootScope.CompanyId).then(function (response) {
                $scope.TaskList = response.data;
            });
        })
        .then(function () {
            //Reload The TaskDetails
            ManageTaskServices.GetTaskDetailsByTaskId($scope.Task.Id).then(function (response) {
                $scope.Task = response.data;
                $scope.TaskMembers = response.data.TaskMembers;
                $scope.TaskDetails = response.data.TaskDescriptions;
                for (var i = 0; i < $scope.TaskMembers.length; i++) {
                    $scope.TaskMembers[i].Key = i;
                }
                ProcessDateTimePikerForReading();
            })
        })
        .then(function () {
            $scope.cancelTaskDescriptionModal();
        })
    };

    //Update Task Members
    $scope.UpdateTaskMembers = function () {
        if (($scope.RemovedTaskMembers.length > 0 || $scope.NewTaskMembers.length > 0) && ($scope.AtLeastOneGroupMemberCheckerWhileEditing() == true)) {
            $scope.Loader = true;
            ManageTaskServices.UpdateTaskMember($scope).then(function (response) {
                if (response.data.IsReport == "Ok") {
                    toastr.success(response.data.Message, "Success!");
                }
                else if (response.data.IsReport == "NotOk") {
                    toastr.error(response.data.Message, "Error!");
                }
            })
            .then(function () {
                //Reload The TaskDetails
                ManageTaskServices.GetTaskDetailsByTaskId($scope.Task.Id).then(function (response) {
                    $scope.TaskMembers = [];
                    $scope.CopyOfTaskMembers = [];
                    $scope.RemovedTaskMembers = [];
                    $scope.NewTaskMembers = [];
                    $scope.TaskMembers = response.data.TaskMembers;
                    for (var i = 0; i < $scope.TaskMembers.length; i++) {
                        $scope.TaskMembers[i].Key = i;
                    }
                    $scope.Loader = false;
                    $scope.CopyOfTaskMembers = angular.copy($scope.TaskMembers);
                    ProcessFilteredCustomerList();
                })
            })
        }
        else {
            toastr.error("This form contains invalid data. Can not be submitted", 'Error!');
            $scope.Loader = false;
        }
    }

    //Update Group Member Work Progress
    $scope.UpdateGroupMemberWorkProgress = function (TaskId, CustomerId, model) {
        console.log(model);
        $scope.Loader = true;
        ManageTaskServices.UpdateGroupMemberWorkProgress(TaskId, CustomerId, model).then(function (response) {
            if (response.data.IsReport == "Ok") {
                toastr.success(response.data.Message, "Success!");
            }
            else if (response.data.IsReport == "ArgumentNotPassed") {
                toastr.error(response.data.Message, "Error!");
            }
        })
        .then(function () {
            //Reload The Task List
            ManageTaskServices.GetTaskListByCompanyId($rootScope.CompanyId).then(function (response) {
                $scope.TaskList = response.data;
            });
        })
        .then(function () {
            //Reload The TaskDetails
            ManageTaskServices.GetTaskDetailsByTaskId($scope.Task.Id).then(function (response) {
                $scope.TaskMembers = response.data.TaskMembers;
            })
        })
        .then(function () {
            ManageTaskServices.GetTaskDetailsByTaskId($scope.Task.Id).then(function (response) {
                $scope.TaskMembers = [];
                $scope.CopyOfTaskMembers = [];
                $scope.RemovedTaskMembers = [];
                $scope.NewTaskMembers = [];
                $scope.TaskMembers = response.data.TaskMembers;
                for (var i = 0; i < $scope.TaskMembers.length; i++) {
                    $scope.TaskMembers[i].Key = i;
                }
                $scope.Loader = false;
                $scope.CopyOfTaskMembers = angular.copy($scope.TaskMembers);
                ProcessFilteredCustomerList();
            })
        })
    }

    //-------------Delete-------------
    $scope.deleteTaskAlert = function (TaskId) {
        if (TaskId != null || TaskName != null) {
            swal({
                title: "Are you sure?",
                text: "You are going to delete this Task",
                type: "warning",
                showCancelButton: true,
                confirmButtonClass: "btn-danger",
                confirmButtonText: "Yes, delete it!",
                closeOnConfirm: true
            },
        function () {
            ManageTaskServices.DeleteTask(TaskId).then(function (response) {
                if (response.data.IsReport === "Ok") {
                    toastr.success(response.data.Message, 'Successful');
                }
                else if (response.data.IsReport === "HasOnGoingTask") {
                    toastr.warning(response.data.Message, 'Warning!');
                }
                else if (response.data.IsReport === "NotOk") {
                    toastr.error(response.data.Message, 'Failed');
                }
            })
            .then(function () {
                ManageTaskServices.GetTaskListByCompanyId($rootScope.CompanyId).then(function (response) {
                    $scope.TaskList = response.data;
                });
            })

        });
        }
        else {
            toastr.error("An error occurred. Deletion is not possible.", "Error!")
        }
    };

    //=======================Live Validation==============================



});