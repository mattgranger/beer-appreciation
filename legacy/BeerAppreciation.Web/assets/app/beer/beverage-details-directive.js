(function () {
    'use strict';

    // Define the directive on the module.
    // Inject the dependencies. 
    // Point to the directive definition function.
    angular.module('beer-appreciation').directive('beverageDetails', ['$window', beverageDetailsDirective]);

    function beverageDetailsDirective($window) {
        // Usage:
        // 
        // Creates:
        // 
        var directive = {
            link: link,
            restrict: 'AE',
            templateUrl: '/assets/app/beer/beverage-details.html',
            scope: {
                instance: "="
            }
        };
        return directive;

        function link(scope, element, attrs) {
        }
    }

})();