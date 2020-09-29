/// <reference path="../app.js" />
//directive for Image info:
CarPartsApp.directive('ngFiles', [
    '$parse', function ($parse) {
        function fn_link(scope, element, attrs) {
            var onChange = $parse(attrs.ngFiles);
            element.on('change', function (event) {
                onChange(scope, { $files: event.target.files });
            });
        };

        return {
            link: fn_link
        }
    }
]);


//Custom Directive
CarPartsApp.directive('ignoreMouseWheel', function ($rootScope) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            element.bind('mousewheel', function (event) {
                element.blur();
            });
        }
    }
});

CarPartsApp.directive('tmpl', function ($compile) {
    return {
        restrict: 'A',
        templateUrl: 'app/package/PackageHistory.html',
        transclude: true,
        link: function (scope, element, attrs) {
        }
    }
});

//CarPartsApp.directive('dynamicFormElement', function ($compile) {
//    return {
//        restrict: "E",
//        link: function (scope, element) {
//            $templateRequest("app/order/manage_product/DynamicFormElement.html").then(function (html) {
//                var template = angular.element(html);
//                element.append(template);
//                $compile(template)(scope);
//            });
//        },
//        scope: {
//            content:'='
//        }
//    };
//})

CarPartsApp.directive('schrollBottom', function ($timeout) {
    return {
        scope: {
            schrollBottom: "="
        },
        link: function (scope, element) {
            scope.$watchCollection('schrollBottom', function (newValue) {
                if (newValue) {
                    $timeout(function () {
                        $(element).scrollTop($(element)[0].scrollHeight);
                    }, 1000)
                }
            });
        }
    }
});

CarPartsApp.filter('sumByKey', function () {
    return function (data, key) {
        if (typeof (data) === 'undefined' || typeof (key) === 'undefined') {
            return 0;
        }
        var sum = 0;
        for (var i = data.length - 1; i >= 0; i--) {
            if (data[i][key] !== null && typeof (data[i][key]) !== 'undefined' && !isNaN(data[i][key])) {
                sum += parseFloat(data[i][key]);
            }
        }
        return sum.toFixed(2);
    };
});