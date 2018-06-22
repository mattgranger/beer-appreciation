(function () {
    'use strict';

    // Controller name is handy for logging
    var controllerId = 'beverage-selector-dialog-controller';

    // Define the controller on the module.
    // Inject the dependencies. 
    // Point to the controller definition function.
    angular.module('beer-appreciation').controller(controllerId,
        ["$scope", "$location", "$route", "$modalInstance", "common", "beer-service", "selectedBeverage", beverageDialogController]);

    function beverageDialogController($scope, $location, $route, $modalInstance, common, beerService, selectedBeverage) {
        // setup shortcuts to common functions
        var getLogFn = common.logger.getLogFn;
        var logError = getLogFn(controllerId, 'error');
        var logSuccess = getLogFn(controllerId, 'success');

        $scope.selectedBeverage = selectedBeverage;

        // Bindable properties and functions are placed on vm.
        $scope.title = 'Beverage Selector';

        $scope.ok = function () {
            //if (validateSave()) {
                $modalInstance.close($scope.selectedBeverage);
            //}
        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };

        function validateSave() {
            if ($scope.selectedBeverage.id > 0 || ($scope.beverageDialogForm.$valid && !$scope.beverageDialogForm.$pristine)) {
                return true;
            }
            return false;
        }
    }
})();
