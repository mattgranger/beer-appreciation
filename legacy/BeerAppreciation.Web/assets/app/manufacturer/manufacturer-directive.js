(function() {
    'use strict';

    // Define the directive on the module.
    // Inject the dependencies. 
    // Point to the directive definition function.
    angular.module('beer-appreciation').directive('manufacturerEditor', ['$window', manufacturerDetails]);
    
    function manufacturerDetails($window) {
        // Usage:
        // 
        // Creates:
        // 
        var directive = {
            link: link,
            restrict: 'AE',
            templateUrl: '/assets/app/manufacturer/manufacturer-editor.html',
            scope: {
                instance: "=",
                isDisabled: "="
            }
        };
        return directive;

        function link(scope, element, attrs) {
            //scope.$watch('instance', function (newValue) {
            //    if (newValue)
            //        scope.instance = newValue;
            //}, true);
        }
    }

})();