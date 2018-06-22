(function() {
    'use strict';

    // Define the directive on the module.
    // Inject the dependencies. 
    // Point to the directive definition function.
    angular.module('beer-appreciation').directive('beverageStyleEditor', ['$window', beverageStyleDirective]);
    
    function beverageStyleDirective($window) {
        // Usage:
        // 
        // Creates:
        // 
        var directive = {
            link: link,
            restrict: 'AE',
            templateUrl: '/assets/app/beverage-style/beverage-style-editor.html',
            scope: {
                instance: "=",
                beverageStyles: "=",
                beverageStyleForm: "=",
                isDisabled: "="
            },
            resolve: {
                instance: function() {
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