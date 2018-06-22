(function () {
    'use strict';

    // Controller name is handy for logging
    var controllerId = 'start-controller';

    // Define the controller on the module.
    // Inject the dependencies. 
    // Point to the controller definition function.
    angular.module('beer-appreciation').controller(controllerId,
        ['$scope', "$location", 'common', 'drinking-club-service', 'registration-service', startController]);

    function startController($scope, $location, common, drinkingClubService, registrationService) {
        // setup shortcuts to common functions
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);
        // Using 'Controller As' syntax, so we assign this to the vm variable (for viewmodel).
        var vm = this;

        // Bindable properties and functions are placed on vm.
        vm.activate = activate;
        vm.title = 'Start Appreciating';
        vm.createEvent = createEvent;

        vm.activate();

        function activate() {
            common.activateController([], controllerId).then(function () {
                log('Activated Start View');
            });
        }

        function createEvent() {
            $location.path('/events/-1');
        }
    }
})();
