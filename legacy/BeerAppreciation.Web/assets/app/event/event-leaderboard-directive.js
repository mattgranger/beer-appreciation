(function () {
    'use strict';

    // Define the directive on the module.
    // Inject the dependencies. 
    // Point to the directive definition function.
    angular.module('beer-appreciation').directive('eventLeaderboard', ['$window', '$routeParams', eventLeaderboardDirective]);

    function eventLeaderboardDirective($window, $routeParams) {
        // Usage:
        // 
        // Creates:
        // 
        var directive = {
            link: link,
            restrict: 'AE',
            templateUrl: '/assets/app/event/event-leaderboard.html'
        };
        return directive;

        function link(scope, element, attrs) {
            scope.$on("Event_Loaded", function(args, eventInstance) {
                scope.eventInstance = eventInstance;
                scope.eventId = eventInstance.id;
            });
        }
    }

})();