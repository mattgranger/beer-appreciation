(function () {
    'use strict';

    // Define the directive on the module.
    // Inject the dependencies. 
    // Point to the directive definition function.
    angular.module('beer-appreciation').directive('baSpinner', ['$window', baSpinner]);

    function baSpinner($window) {
        // Description:
        //  Creates a new Spinner and sets its options
        // Usage:
        //  <div data-ba-spinner="vm.spinnerOptions"></div>

        var directive = {
            link: link,
            restrict: 'A'
        };
        return directive;

        function link(scope, element, attrs) {
            scope.spinner = null;
            scope.$watch(attrs.baSpinner, function (options) {
                if (scope.spinner) {
                    scope.spinner.stop();
                }
                scope.spinner = new $window.Spinner(options);
                scope.spinner.spin(element[0]);
            }, true);
        }
    }

})();