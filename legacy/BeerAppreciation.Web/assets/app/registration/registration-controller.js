(function () {
    'use strict';

    // Controller name is handy for logging
    var controllerId = 'registration-controller';

    // Define the controller on the module.
    // Inject the dependencies. 
    // Point to the controller definition function.
    angular.module('beer-appreciation').controller(controllerId,
        ['$scope', '$window', '$routeParams', '$location', "$modal", "bootstrap.dialog", 'registration-service', 'common', registrationController]);

    function registrationController($scope, $window, $routeParams, $location, $modal, dialog, registrationService, common) {

        // setup shortcuts to common functions
        var getLogFn = common.logger.getLogFn;
        var logError = getLogFn(controllerId, 'error');
        var logSuccess = getLogFn(controllerId, 'success');
        var log = getLogFn(controllerId);

        // Using 'Controller As' syntax, so we assign this to the vm variable (for viewmodel).
        var vm = this;
        vm.dateFormat = common.appSettings.dateFormat;
        vm.eventId = -1;
        // Bindable properties and functions are placed on vm.
        vm.common = common;
        vm.activate = activate;
        vm.title = 'Event Registration';
        vm.eventInstance = null;
        vm.registrationInstance = null;
        vm.showBeerSelector = false;
        vm.registerEvent = registerEvent;
        vm.registrationButtonText = "Confirm Registration";

        vm.activate();

        function activate() {

            vm.eventId = $routeParams.id;
            if (vm.eventId === '-1') {
                $location.path('/events/' + vm.eventId);
            }

            common.activateController([getRequestedEvent(), getEventRegistration()], controllerId).then(function () {
                log('Activated Registration View');
            });
        }

        function getRequestedEvent() {

            return registrationService.eventService.getById(vm.eventId)
                .then(function (data) {
                    vm.eventInstance = data;
            }, function (error) {
                    logError('Unable to get event ' + vm.eventIdParameter);
                });
        }

        function getEventRegistration() {

            return registrationService.getByExisting(vm.eventId, common.appSettings.appreciator.id)
                .then(function (data) {
                    if (data.items.length > 0) {

                        var existingRegistration = data.items[0];
                        $location.path('/profile/registration/' + existingRegistration.id);

                    } else {
                        vm.registrationInstance = {
                            id: 0,
                            appreciatorId: common.appSettings.appreciator.id,
                            eventId: vm.eventId,
                            beverages: []
                        };
                    }
                }, function (error) {
                    logError('Unable to get event ' + vm.eventIdParameter);
                });
        }

        function registerEvent() {

            vm.registrationInstance.registrationDate = moment().utc();

            var result = registrationService.save(vm.registrationInstance).then(function (data) {
                if (data) {
                    logSuccess('Registration Created!');
                    $location.path('/profile/registration/' + data.id);
                }
            }, function(error) {
                logError('Registration Error. ' + error.data.exceptionMessage);
            });

        }
    }
})();
