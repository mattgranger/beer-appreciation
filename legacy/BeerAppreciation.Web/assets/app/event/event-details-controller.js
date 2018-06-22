(function () {
    'use strict';

    // Controller name is handy for logging
    var controllerId = 'event-details-controller';

    // Define the controller on the module.
    // Inject the dependencies. 
    // Point to the controller definition function.
    angular.module('beer-appreciation').controller(controllerId,
        ['$scope', "$q", "$location", '$routeParams', "$window", "bootstrap.dialog", "$modal", 'common', "config", "drinking-club-service", "event-service", eventDetailsController]);

    function eventDetailsController($scope, $q, $location, $routeParams, $window, dialog, $modal, common, config, drinkingClubService, eventService) {
        // Using 'Controller As' syntax, so we assign this to the vm variable (for viewmodel).
        var vm = this;
        var logError = common.logger.getLogFn(controllerId, 'error');
        var logSuccess = common.logger.getLogFn(controllerId, 'success');
        var log = common.logger.getLogFn;

        // Bindable properties and functions are placed on vm.
        vm.title = 'Event Details';
        vm.cancel = cancel;
        vm.activate = activate;
        vm.saveChanges = saveChanges;
        vm.delete = deleteEvent;
        vm.goBack = goBack;
        vm.goToEvent = goToEvent;
        vm.hasChanges = false;
        vm.isSaving = false;
        vm.drinkingClubs = [];
        vm.eventInstance = null;
        vm.eventIdParameter = $routeParams.id;
        $scope.predicate = "score";
        vm.updateScores = updateScores;
        vm.canDelete = false;
        vm.canEdit = false;

        $scope.dateOptions = {
            formatYear: 'yy',
            startingDay: 1
        };
        $scope.format = 'dd-MMMM-yyyy';

        // vm.canSave Property
        Object.defineProperty(vm, 'canSave', {
            get: canSave
        });

        function canSave() { return vm.hasChanges && !vm.isSaving; }

        activate();

        function activate() {

            initLookups();
            onDestroy();
            onHasChanges();

            var promises = [getDrinkingClubs()];
            common.activateController(promises, controllerId)
                .then(function() {
                    getRequestedEvent().then(function () {
                        log('Activated Event Details View');
                        $scope.$broadcast("Event_Loaded", vm.eventInstance);
                    });
                });
        }

        function getDrinkingClubs() {
            drinkingClubService.getPagedData(null, 1, 50).$promise.then(function (data) {
                vm.drinkingClubs = data.items;
            });
        }

        function initLookups() {
            //var lookups = datacontext.lookup.lookupCachedData;
            //vm.menuTypes = lookups.enums["MenuType"];
        }

        function onDestroy() {
            $scope.$on('$destroy', function () {
                //datacontext.cancel();
            });
        }

        function onHasChanges() {
            $scope.$on(config.events.hasChangesChanged,
                function (event, data) {
                    vm.hasChanges = data.hasChanges;
                });
        }

        function getRequestedEvent() {
            if (vm.eventIdParameter === '-1') {
                vm.canEdit = vm.canDelete = true;
                return $q.when(vm.eventInstance = { id: 0});
            }

            return eventService.getById(vm.eventIdParameter)
                .then(function (data) {
                var localDate = moment.utc(data.date).local();
                data.date = localDate;
                vm.eventInstance = data;
                vm.canDelete = vm.canEdit = (vm.eventInstance.ownerId == common.appSettings.appreciator.id);

                    if (vm.eventInstance && vm.eventInstance.drinkingClub) {
                        angular.forEach(vm.drinkingClubs, function (item) {
                            if (item.id == vm.eventInstance.drinkingClub.id) {
                                vm.selectedClub = item;
                            }
                        });
                    }

                }, function (error) {
                    logError('Unable to get event ' + vm.eventIdParameter);
                });
        }

        function saveChanges() {
            if (!$scope.eventContainerForm.$valid) {
                logError('Invalid Form');
            } else {
                eventService.save(vm.eventInstance);
                $scope.eventContainerForm.$setPristine();
                logSuccess("Event saved!");
            }
        };

        vm.setDrinkingClub = function() {
            vm.eventInstance.drinkingClubId = vm.selectedClub.id;
        };

        function cancel() {
            getRequestedEvent().then(function () {
                $scope.eventDetailsForm.$setPristine();
            });
        };

        function deleteEvent() {

            return dialog.deleteDialog('Event')
                .then(confirmDelete);

            function confirmDelete() {
                eventService.deleteEvent(vm.eventInstance);
                gotoEvents();
            }
        }

        function gotoEvents() { $location.path('/events'); }

        function goBack() {
            $window.history.back();
        };

        function goToEvent(event) {
            $location.path('/events/' + event.id);
        }

        $scope.toggleSave = function () {
            $scope.canSave = !$scope.canSave;
        };

        vm.open = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();

            vm.eventDateOpened = true;
        };

        function updateScores() {
            eventService.updateScores(vm.eventIdParameter)
                .then(function (data) {
                vm.eventInstance.beverages = data.beverages;
            }, function (error) {
                    logError('Unable to get event ' + vm.eventIdParameter);
                });
        }
    }

})();