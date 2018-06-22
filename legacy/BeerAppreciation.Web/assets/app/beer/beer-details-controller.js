(function () {
    'use strict';

    // Controller name is handy for logging
    var controllerId = 'beer-details-controller';

    // Define the controller on the module.
    // Inject the dependencies. 
    // Point to the controller definition function.
    angular.module('beer-appreciation').controller(controllerId,
        ['$scope', '$q', '$location', '$routeParams', "$window", "bootstrap.dialog", "$modal", 'common', 'config', 'beverage-service', beerDetailsController]);

    function beerDetailsController($scope, $q, $location, $routeParams, $window, dialog, $modal, common, config, beverageService) {

        // setup shortcuts to common functions
        var getLogFn = common.logger.getLogFn;
        var logError = getLogFn(controllerId, 'error');
        var logSuccess = getLogFn(controllerId, 'success');
        var logInfo = getLogFn(controllerId, 'info');

        // Using 'Controller As' syntax, so we assign this to the vm variable (for viewmodel).
        var defaultListItemsLoaded = false;
        var vm = this;
        vm.beverageInstance = null;
        vm.beverageIdParameter = $routeParams.id;

        // Bindable properties and functions are placed on vm.
        vm.activate = activate;
        vm.title = 'Beer Details';
        vm.manufacturers = [];
        vm.beverageTypes = [];
        vm.beverageStyles = [];
        vm.goBack = goBack;
        vm.cancel = cancel;
        vm.saveChanges = saveChanges;
        vm.hasChanges = hasChanges;
        vm.delete = deleteBeer;
        vm.onBeverageTypeChanged = onBeverageTypeChanged;
        vm.activate();
        vm.newManufacturer = null;
        vm.selectedBeverageType = null;
        vm.openBeverageStyleModel = openBeverageStyleModel;

        function activate() {
            common.activateController([getManufacturers(), getBeverageTypes()], controllerId)
                .then(function() {
                    getRequestedBeverage().then(function () {
                        if (vm.beverageInstance && vm.beverageInstance.id <= 0) {
                            vm.manufacturers.unshift({
                                id: 0,
                                name: "-- Add New --"
                            });
                        }

                        getLogFn('Activated Details View');
                });
            });
        }

        function getManufacturers() {
            return beverageService.manufacturerService.getPagedData().$promise.then(function (data) {
                vm.manufacturers = data.items;
            });
        }

        function hasChanges() {
            return $scope.beverageDetailsForm.$dirty;
        }

        function getEntityFromId(id, list) {
            var i = 0, len = list.length;
            for (; i < len; i++) {
                if (+list[i].id == +id) {
                    return list[i];
                }
            }
            return null;
        }

        function getBeverageTypes() {
            return beverageService.beverageTypeService.getPagedData(null, 1, 50).$promise.then(function (data) {
                vm.beverageTypes = data.items;
            });
        }

        function onBeverageTypeChanged() {

            var queryOptions = {
                beverageTypeId: vm.beverageInstance.beverageTypeId
            };

            return beverageService.beverageStyleService.getPagedData(queryOptions).$promise.then(function (data) {
                vm.beverageStyles = data.items;

                if (vm.beverageInstance && vm.beverageInstance.id <= 0) {
                    vm.beverageStyles.unshift({
                        id: 0,
                        beverageTypeId: vm.beverageInstance.beverageTypeId,
                        name: "-- Add New --"
                    });
                }
            });

        }

        function getRequestedBeverage() {

            return beverageService.beerService.getById(vm.beverageIdParameter)
                .then(function (data) {
                    vm.beverageInstance = data;
                    if (vm.beverageInstance.beverageStyleId) {
                        onBeverageTypeChanged();
                    }
                }, function (error) {
                    logError('Unable to get beverage ' + vm.beverageIdParameter);
                });
        }

        function cancel() {
            getRequestedBeverage().then(function() {
                $scope.beverageDetailsForm.$setPristine();
            });
        };

        function gotoBeverages() { $location.path('/beers'); }

        function goBack() {
            $window.history.back();
        };

        function saveChanges() {
            if (!$scope.beverageDetailsForm.$valid) {
                logError("Invalid Form!");
            } else {
                var isNew = vm.beverageInstance.id <= 0;
                beverageService.beerService.save(vm.beverageInstance).then(function (beverage) {
                    logSuccess("Beverage saved!");
                    if (isNew && beverage.id > 0) {
                        $location.path('/beers/' + beverage.id);
                    }
                }, function (error) {
                    logError('Error saving beverage ' + error.data.exceptionMessage);
                });
            }
        };

        function deleteBeer() {

            return dialog.deleteDialog(vm.beverageInstance.name + " " + vm.beverageInstance.beverageStyle.name + " " + vm.beverageInstance.beverageType.name)
                .then(confirmDelete);

            function confirmDelete() {
                var promise = beverageService.beerService.deleteBeverage(vm.beverageInstance);
                promise.then(function () {
                    logSuccess("Beverage deleted!");
                    gotoBeverages();
                }, function(error) {
                    logError('Unable to delete beverage ' + error.data.exceptionMessage);
                });
            }
        }

        var beverageStyleModalInstance = null;

        function openBeverageStyleModel() {

            beverageStyleModalInstance = $modal.open({
                templateUrl: "/assets/app/beverage-style/beverage-style-dialog.html",
                controller: "beverage-style-dialog-controller",
                windowClass: "eventDialog",
                resolve: {
                    instance: function () {
                        return {
                            id: 0,
                            beverageTypeId: 0,
                            name: null
                        };
                    }
                }
            });

            //modalInstance.opened.then(function(result, stuff) {
            //}, function () {
            //});

            beverageStyleModalInstance.result.then(function (newBeverageStyle) {
                vm.beverageStyles.push(newBeverageStyle);
                vm.beverageInstance.beverageStyleId = newBeverageStyle.id;
            }, function () {
                //logInfo('Modal dismissed at: ' + new Date());
            });

        }
    }
})();
