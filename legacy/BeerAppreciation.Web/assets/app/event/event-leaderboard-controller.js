(function () {
    'use strict';

    // Controller name is handy for logging
    var controllerId = 'event-leaderboard-controller';

    // Define the controller on the module.
    // Inject the dependencies. 
    // Point to the controller definition function.
    angular.module('beer-appreciation').controller(controllerId,
        ['$scope', "$q", '$routeParams', "$window", "bootstrap.dialog", "$modal", 'common', "config", "event-service", eventLeaderboardController]);

    function eventLeaderboardController($scope, $q, $routeParams, $window, dialog, $modal, common, config, eventService) {
        // Using 'Controller As' syntax, so we assign this to the vm variable (for viewmodel).
        var logError = common.logger.getLogFn(controllerId, 'error');
        var logSuccess = common.logger.getLogFn(controllerId, 'success');
        var log = common.logger.getLogFn;

        // Bindable properties and functions are placed on vm.
        $scope.title = 'Event Leaderboard';
        $scope.predicate = "score";
        $scope.updateScores = updateScores;

        activate();

        function activate() {

        }

        function updateScores() {
            if ($scope.eventId) {
                eventService.updateScores($scope.eventId)
                    .then(function (data) {
                        $scope.eventInstance.beverages = data.beverages;
                    }, function (error) {
                        logError('Unable to get event ' + $scope.eventId);
                    });
            }

        }
    }

})();