(function () {
    'use strict';

    // Controller name is handy for logging
    var controllerId = 'beverage-style-editor-controller';

    // Define the controller on the module.
    // Inject the dependencies. 
    // Point to the controller definition function.
    angular.module('beer-appreciation').controller(controllerId,
        ['$scope', 'beverage-service', beverageStyleEditorController]);

    function beverageStyleEditorController($scope, beverageService) {
        // Bindable properties and functions are placed on vm.
        $scope.title = 'Beverage Style Editor';
        $scope.activate = activate;
        $scope.beverageTypes = [];
        $scope.beverageStyles = [];
        $scope.onBeverageTypeChanged = onBeverageTypeChanged;

        activate();

        function activate() {
            getBeverageTypes();
        }

        function getBeverageTypes() {
            return beverageService.beverageTypeService.getPagedData(null).$promise.then(function (data) {
                $scope.beverageTypes = data.items;
                onBeverageTypeChanged();
            });
        }

        function onBeverageTypeChanged() {

            var queryOptions = {};

            if ($scope.instance && $scope.instance.beverageTypeId) {
                queryOptions.beverageTypeId = $scope.instance.beverageTypeId;
            }

            return beverageService.beverageStyleService.getPagedData(queryOptions).$promise.then(function (data) {
                $scope.beverageStyles = data.items;
            });
        }
    }
})();
