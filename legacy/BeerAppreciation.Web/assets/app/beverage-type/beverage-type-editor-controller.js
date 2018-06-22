(function () {
    'use strict';

    // Controller name is handy for logging
    var controllerId = 'beverage-type-editor-controller';

    // Define the controller on the module.
    // Inject the dependencies. 
    // Point to the controller definition function.
    angular.module('beer-appreciation').controller(controllerId,
        ['$scope', 'beverage-service', beverageTypeEditorController]);

    function beverageTypeEditorController($scope, beverageService) {
        // Bindable properties and functions are placed on vm.
        $scope.title = 'Beverage Type Editor';
        $scope.activate = activate;

        activate();

        function activate() {

        }
    }
})();
