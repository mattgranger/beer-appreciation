(function () {
    'use strict';

    // Define the directive on the module.
    // Inject the dependencies. 
    // Point to the directive definition function.
    angular.module('beer-appreciation').directive('serverTime', ['$window', serverTime]);

    function serverTime($window) {
        return {
            restrict: "EAC",
            templateUrl: '/assets/app/core/directives/server-time.html',
            controller: 'server-time-controller',
            link: function (scope, elem, attrs) {

            }
        };
    }

})();