(function () {
    'use strict';

    // Define the directive on the module.
    // Inject the dependencies. 
    // Point to the directive definition function.
    angular.module('beer-appreciation').directive('beverageSelector', ['$window', beverageSelectorDirective]);

    function beverageSelectorDirective($window) {
        // Usage:
        // 
        // Creates:
        // 
        var directive = {
            link: link,
            restrict: 'AE',
            templateUrl: '/assets/app/beverage/beverage-selector.html'
        };
        return directive;

        function link(scope, element, attrs) {
        }
    }

})();