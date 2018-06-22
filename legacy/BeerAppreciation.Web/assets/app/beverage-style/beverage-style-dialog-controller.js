(function () {
    'use strict';

    // Controller name is handy for logging
    var controllerId = 'beverage-style-dialog-controller';

    // Define the controller on the module.
    // Inject the dependencies. 
    // Point to the controller definition function.
    angular.module('beer-appreciation').controller(controllerId,
        ["$scope", "$location", "$route", "$modalInstance", "common", "beverage-service", "instance", beverageStyleDialogController]);

    function beverageStyleDialogController($scope, $location, $route, $modalInstance, common, beverageService, instance) {
        // setup shortcuts to common functions
        var getLogFn = common.logger.getLogFn;
        var logError = getLogFn(controllerId, 'error');
        var logSuccess = getLogFn(controllerId, 'success');

        // Bindable properties and functions are placed on vm.
        $scope.title = 'Add Beverage Style';
        if (instance) {
            $scope.instance = instance;
        }

        $scope.ok = function () {
            beverageService.beverageStyleService.save($scope.instance).then(function (result) {
                //success
                $scope.instance = result;
                logSuccess(result.name + " created successfully!");
                $modalInstance.close($scope.instance);
            }, function (error) {
                // failure
                logError('Error: ' + error.data.exceptionMessage);
            });
        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };
    }
})();
