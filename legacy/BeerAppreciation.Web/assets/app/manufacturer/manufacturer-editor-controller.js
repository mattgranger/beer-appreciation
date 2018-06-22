(function () {
    'use strict';

    // Controller name is handy for logging
    var controllerId = 'manufacturer-editor-controller';

    // Define the controller on the module.
    // Inject the dependencies. 
    // Point to the controller definition function.
    angular.module('beer-appreciation').controller(controllerId,
        ['$scope', manufacturerEditorController]);

    function manufacturerEditorController($scope) {
        // Using 'Controller As' syntax, so we assign this to the vm variable (for viewmodel).
        var mvm = this;

        mvm.instance = $scope.instance || {};
        mvm.isDisabled = $scope.isDisabled || false;

        // Bindable properties and functions are placed on vm.
        mvm.activate = activate;
        mvm.title = 'Manufacturer Editor Controller';

        function activate() {

        }

        //#region Internal Methods        

        //#endregion
    }
})();
