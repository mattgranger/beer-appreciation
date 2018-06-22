(function () {
    'use strict';

    // Controller name is handy for logging
    var controllerId = 'beverage-details-controller';

    // Define the controller on the module.
    // Inject the dependencies. 
    // Point to the controller definition function.
    angular.module('beer-appreciation').controller(controllerId,
        ['$scope', 'common', 'config', 'beverage-service', beverageDetailsController]);

    function beverageDetailsController($scope, common, config, beverageService) {
        // setup shortcuts to common functions
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);
        // Using 'Controller As' syntax, so we assign this to the vm variable (for viewmodel).
        var vm = this;

        // get instance from scope
        vm.instance = $scope.instance || {};

        // Bindable properties and functions are placed on vm.
        vm.title = 'Beverage Details';
        vm.activate = activate;
        vm.beverageTypes = [];
        vm.beverageStyles = [];
        vm.manufacturers = [];
        vm.onBeverageTypeChanged = onBeverageTypeChanged;

        activate();

        function activate() {
            getManufacturers();
            getBeverageTypes();
        }

        function getDefaultList() {
            return [
                getDefaultListItem()
            ];
        }

        function getDefaultListItem() {
            return {
                id: 0,
                name: '-- Add New --'
            };
        }

        function insertDefaultListItem(list) {
            list.unshift(getDefaultListItem());
        }

        function getManufacturers() {
            return beverageService.manufacturerService.getPagedData(null, 1, 50).$promise.then(function (data) {
                vm.manufacturers = data.items;
                insertDefaultListItem(vm.manufacturers);
            });
        }

        function getBeverageTypes() {
            return beverageService.beverageTypeService.getPagedData(null, 1, 50).$promise.then(function (data) {
                vm.beverageTypes = data.items;
            });
        }

        function onBeverageTypeChanged() {

            $scope.$parent.canSave = $scope.beverageDetailsForm.$valid;

            if ($scope.instance.beverageTypeId && $scope.instance.beverageTypeId > 0) {

                var queryOptions = {
                    beverageTypeId: $scope.instance.beverageTypeId
                };

                return beverageService.beverageStyleService.getPagedData(queryOptions).$promise.then(function (data) {
                    vm.beverageStyles = data.items;
                    insertDefaultListItem(vm.beverageStyles);
                });
            } else {
                return vm.beverageStyles = [];
            }
        }
    }
})();
