(function () {
    'use strict';

    // Controller name is handy for logging
    var controllerId = 'beverage-selector-controller';

    // Define the controller on the module.
    // Inject the dependencies. 
    // Point to the controller definition function.
    angular.module('beer-appreciation').controller(controllerId,
        ["$scope", "$location", "$route", "common", "beer-service", beverageSelectorController]);

    function beverageSelectorController($scope, $location, $route, common, beerService) {
        // setup shortcuts to common functions
        var getLogFn = common.logger.getLogFn;
        var logError = getLogFn(controllerId, 'error');
        var logSuccess = getLogFn(controllerId, 'success');

        // Bindable properties and functions are placed on vm.
        $scope.title = 'Beverage Selector';
        $scope.selectedBeverage = $scope.selectedBeverage || { id: 0, beverageType: {}, beverageStyle: {} };
        $scope.selectExistingBeverage = selectExistingBeverage;
        $scope.getBeverages = getBeverages;
        $scope.validateSave = validateSave;
        $scope.radioModel = 'Assign';
        $scope.isValidToSave = false;

        function validateSave() {
            var isValid = ($scope.radioModel == 'Create' && ($scope.beverageDialogForm && (!$scope.beverageDialogForm.$valid || $scope.beverageDialogForm.$pristine))) ||
                ($scope.radioModel == 'Assign' && ($scope.selectedBeverage != null && $scope.selectedBeverage.id > 0));
            return isValid;
        }

        function getBeverages(searchText) {
            var queryOptions = {
                searchText: searchText
            };

            return beerService.getPagedData(queryOptions).$promise.then(function (res) {
                var beers = [];
                angular.forEach(res.items, function (item) {
                    beers.push(item);
                });
                return beers;
            });
        }

        function selectExistingBeverage($item, $model, $label) {
            $scope.selectedBeverage = $model;
            if ($scope.$parent) {
                $scope.$parent.selectedBeverage = $model;
            }
            if ($scope.$parent.$parent) {
                $scope.$parent.$parent.selectedBeverage = $model;
            }
            $scope.isValidToSave = validateSave();
        }
    }
})();
