(function () {
    'use strict';

    // Define the directive on the module.
    // Inject the dependencies. 
    // Point to the directive definition function.
    angular.module('beer-appreciation').directive('beverageTypeEditor', ['$window', beverageTypeDirective]);

    function beverageTypeDirective($window) {
        // Usage:
        // 
        // Creates:
        // 
        var directive = {
            link: link,
            restrict: 'AE',
            templateUrl: '/assets/app/beverage-type/beverage-type-editor.html',
            scope: {
                instance: "=",
                form: "=",
                isDisabled: "="
            },
            resolve: {
                instance: function () {
                    return $scope.instance;
                }
            }
        };
        return directive;

        function link(scope, element, attrs) {
            scope.$watch('instance', function (newValue) {
                if (newValue)
                    scope.instance = newValue;
            }, true);
        }
    }

})();