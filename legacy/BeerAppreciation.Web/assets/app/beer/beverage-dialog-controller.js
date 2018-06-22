(function () {
    'use strict';

    // Controller name is handy for logging
    var controllerId = 'beverage-dialog-controller';

    // Define the controller on the module.
    // Inject the dependencies. 
    // Point to the controller definition function.
    angular.module('beer-appreciation').controller(controllerId,
        ["$scope", "$location", "$route", "$modalInstance", "common", "instance", beverageDialogController]);

    function beverageDialogController($scope, $location, $route, $modalInstance, common, instance) {
        // setup shortcuts to common functions
        var getLogFn = common.logger.getLogFn;
        var logError = getLogFn(controllerId, 'error');
        var logSuccess = getLogFn(controllerId, 'success');

        // Bindable properties and functions are placed on vm.
        $scope.title = 'Beverage Details';

        if (instance) {
            $scope.instance = instance;
        }

        $scope.ok = function () {
            $modalInstance.close($scope.instance);
        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };
    }
})();
