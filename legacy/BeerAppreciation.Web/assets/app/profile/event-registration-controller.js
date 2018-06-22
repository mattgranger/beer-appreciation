(function () {
    'use strict';

    // Controller name is handy for logging
    var controllerId = 'event-registration-controller';

    // Define the controller on the module.
    // Inject the dependencies. 
    // Point to the controller definition function.
    angular.module('beer-appreciation').controller(controllerId,
        ['$scope', '$window', '$routeParams', '$location', "$modal", "bootstrap.dialog", 'registration-service', 'common', eventRegistrationController]);

    function eventRegistrationController($scope, $window, $routeParams, $location, $modal, dialog, registrationService, common) {
        // setup shortcuts to common functions
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);
        var logError = getLogFn(controllerId, 'error');
        var logSuccess = getLogFn(controllerId, 'success');
        var logInfo = getLogFn(controllerId, 'info');
        // Using 'Controller As' syntax, so we assign this to the vm variable (for viewmodel).
        var vm = this;
        vm.dateFormat = common.appSettings.dateFormat;
        vm.registrationId = -1;
        // Bindable properties and functions are placed on vm.
        vm.common = common;
        vm.activate = activate;
        vm.title = 'Event Registration';
        vm.eventInstance = null;
        vm.registrationInstance = null;
        vm.showBeerSelector = false;
        vm.registeredBeverages = [];
        vm.getBeverages = getBeverages;
        vm.selectExistingBeverage = selectExistingBeverage;
        vm.assignBeverage = assignBeverage;
        vm.canAssignBeverage = false;
        vm.selectedBeverage = null;
        vm.removeBeverageRegistration = removeBeverageRegistration;
        vm.openNewBeverageDialog = openNewBeverageDialog;
        vm.updateRegistration = updateRegistration;
        vm.registrationButtonText = "Update";
        vm.unRegisterEvent = unRegisterEvent;
        vm.beginRating = beginRating;
        vm.onShowBeerSelector = onShowBeerSelector;
        vm.openBeverageSelectorDialog = openBeverageSelectorDialog;
        vm.status = {
            isopen: false
        };
        vm.toggleOption = function(option) {
            alert(open);
        };

        $scope.status = {
            isopen: false
        };

        $scope.disabled = true;

        $scope.toggled = function (open) {
            log('Dropdown is now: ' + open);
        };

        $scope.toggleDropdown = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();
            //$scope.status.isopen = !$scope.status.isopen;
            vm.status.isopen = !vm.status.isopen;
        };

        vm.activate();

        function activate() {

            vm.registrationId = $routeParams.id;
            if (vm.registrationId === '-1') {
                $location.path('/events/' + vm.eventId);
            }

            common.activateController([getEventRegistration()], controllerId).then(function () {
                log('Activated Registration Details View');
            });
        }

        function getEventRegistration() {
            return registrationService.getById(vm.registrationId)
                .then(function (data) {
                    vm.registrationInstance = data;
                }, function (error) {
                    logError('Unable to get event ' + vm.eventIdParameter);
                });
        }

        function getBeverages(searchText) {
            var queryOptions = {
                searchText: searchText
            };

            return registrationService.beerService.getPagedData(queryOptions).$promise.then(function (res) {
                var beers = [];
                angular.forEach(res.items, function (item) {
                    beers.push(item);
                });
                return beers;
            });
        }

        function selectExistingBeverage($item, $model, $label) {
            vm.selectedBeverage = $model;
        }

        function assignBeverage() {
            vm.registrationInstance.beverages.push({
                eventId: vm.registrationInstance.eventId,
                eventRegistrationId: vm.registrationInstance.id,
                beverageId: vm.selectedBeverage.id,
                beverage: vm.selectedBeverage
            });
            vm.selectedBeverage = null;
            vm.asyncSelected = "";
        }

        function removeBeverageRegistration(eventBeverage) {

            return dialog.deleteDialog(eventBeverage.beverage.name + " from your registration")
                .then(confirmDelete);

            function confirmDelete() {
                registrationService.eventBeverageService.deleteEventBeverage(eventBeverage.id).then(function() {
                    vm.registrationInstance.beverages.splice(vm.registrationInstance.beverages.indexOf(eventBeverage), 1);
                });
            }
        }

        var beverageModalInstance = null;
        function openNewBeverageDialog() {
            var newBeverage = {
                id: 0,
                manufacturer: {
                    id: 0
                },
                beverageType: {
                    id: 0
                },
                beverageStyle: {
                    id: 0
                }
            };

            beverageModalInstance = $modal.open({
                templateUrl: "/assets/app/beer/beverage-dialog.html",
                controller: "beverage-dialog-controller",
                resolve: {
                    instance: function () {
                        return newBeverage;
                    }
                }
            });

            beverageModalInstance.result.then(function (beverage) {
                vm.registrationInstance.beverages.push({
                    eventId: vm.registrationInstance.eventId,
                    eventRegistrationId: vm.registrationInstance.id,
                    beverageId: beverage.id,
                    beverage: beverage
                });
            }, function () {
                //logInfo('Modal dismissed at: ' + new Date());
            });
        }

        function updateRegistration() {

            angular.forEach(vm.registrationInstance.beverages, function (eventBeverage) {
                eventBeverage.beverage.state = 1;
                if (eventBeverage.beverage.manufacturer) {
                    eventBeverage.beverage.manufacturer.state = 1;
                }
                if (eventBeverage.beverage.beverageStyle) {
                    eventBeverage.beverage.beverageStyle.state = 1;
                }
                if (eventBeverage.beverage.beverageType) {
                    eventBeverage.beverage.beverageType.state = 1;
                }
            });


            var registrationToUpdate = {
                id: vm.registrationInstance.id,
                appreciatorId: vm.registrationInstance.appreciatorId,
                eventId: vm.registrationInstance.eventId,
                comments: vm.registrationInstance.comments,
                registrationDate: vm.registrationInstance.registrationDate,
                beverages: vm.registrationInstance.beverages,
                freeloader: vm.registrationInstance.freeloader
            };

            var result = registrationService.save(registrationToUpdate, false).then(function (data) {
                if (data) {
                    logSuccess('Registration Updated!');
                    if (vm.registrationInstance.beverages.length > 0) {
                        // reload registration to update beverage list
                        getEventRegistration();
                    }
                }
            }, function(error) {
                logError('Save failed! - ' + error.data.exceptionMessage);
            });
        }

        function beginRating() {
            $location.path('/rate/' + vm.registrationInstance.id);
        }

        function unRegisterEvent() {
            return dialog.deleteDialog(" event registration?")
                .then(confirmDelete);

            function confirmDelete() {
                registrationService.deleteRegistration(vm.registrationInstance).then(function () {
                    $location.path('/events');
                });
            }
        }

        function onShowBeerSelector() {
            vm.showBeerSelector = !vm.showBeerSelector;

            if (vm.showBeerSelector) {
                
            }
        }

        var beverageSelectorModalInstance = null;
        function openBeverageSelectorDialog() {

            var selectedBeverage = {
                id: 0,
                manufacturer: {
                    id: 0
                },
                beverageType: {
                    id: 0
                },
                beverageStyle: {
                    id: 0
                }
            };

            beverageSelectorModalInstance = $modal.open({
                templateUrl: "/assets/app/beverage/beverage-selector-dialog.html",
                controller: "beverage-selector-dialog-controller",
                resolve: {
                    selectedBeverage: function () {
                        return selectedBeverage;
                    }
                }
            });

            beverageSelectorModalInstance.result.then(function (beverage) {
                if (beverage.id == 0) {
                    registrationService.beerService.save(beverage).then(function (data) {
                        beverage.id = data.id;
                        vm.registrationInstance.beverages.push({
                            eventId: vm.registrationInstance.eventId,
                            eventRegistrationId: vm.registrationInstance.id,
                            beverageId: data.id,
                            beverage: data
                        });

                        // update registration (need it within the then
                        // function to ensure gets called after beverage
                        // gets added to registration
                        updateRegistration();
                    });
                } else {
                    vm.registrationInstance.beverages.push({
                        eventId: vm.registrationInstance.eventId,
                        eventRegistrationId: vm.registrationInstance.id,
                        beverageId: beverage.id,
                        beverage: beverage
                    });

                    // update registration
                    updateRegistration();
                }

            }, function () {
                //logInfo('Modal dismissed at: ' + new Date());
            });
        }
    }
})();
