(function () {
    'use strict';

    // Controller name is handy for logging
    var controllerId = 'beverage-type-details-controller';

    // Define the controller on the module.
    // Inject the dependencies. 
    // Point to the controller definition function.
    angular.module('beer-appreciation').controller(controllerId,
        ['$scope', "$q", "$location", '$routeParams', "$window", "bootstrap.dialog", "$modal", 'common', "config", "beverage-type-service", beverageTypeDetailsController]);

    function beverageTypeDetailsController($scope, $q, $location, $routeParams, $window, dialog, $modal, common, config, beverageTypeService) {
        // Using 'Controller As' syntax, so we assign this to the vm variable (for viewmodel).
        var vm = this;
        var logError = common.logger.getLogFn(controllerId, 'error');
        var logSuccess = common.logger.getLogFn(controllerId, 'success');
        var log = common.logger.getLogFn;

        // Bindable properties and functions are placed on vm.
        vm.title = 'BeverageType Details';
        vm.cancel = cancel;
        vm.activate = activate;
        vm.saveChanges = saveChanges;
        vm.delete = deleteBeverageType;
        vm.goBack = goBack;
        vm.goToBeverageType = goToBeverageType;
        vm.hasChanges = false;
        vm.isSaving = false;
        vm.beverageTypeInstance = null;
        vm.idParameter = $routeParams.id;
        $scope.predicate = "score";
        vm.canDelete = false;
        vm.canEdit = false;
        vm.canSave = canSave;

        activate();

        function canSave() {
            return $scope.beverageTypeContainerForm.$valid && !$scope.beverageTypeContainerForm.$pristine;
        }

        function activate() {

            var promises = [];
            common.activateController(promises, controllerId)
                .then(function () {
                    getRequestedBeverageType().then(function () {
                        log('Activated BeverageType Details View');
                    });
                });
        }

        function getRequestedBeverageType() {
            if (vm.idParameter === '-1') {
                vm.canEdit = true;
                vm.canDelete = false;
                return $q.when(vm.beverageTypeInstance = { id: 0 });
            }

            return beverageTypeService.getById(vm.idParameter)
                .then(function (data) {
                    vm.beverageTypeInstance = data;
                    //vm.canDelete = vm.beverageTypeInstance.beverages.count == 0; //vm.canEdit = (vm.manufacturerInstance.ownerId == common.appSettings.appreciator.id);
                    //$scope.manufacturerContainerForm.$setPristine();
                    //$scope.manufacturerContainerForm.$setValidity();

                }, function (error) {
                    logError('Unable to get beverageType ' + vm.idParameter);
                });
        }

        function saveChanges() {
            if (!$scope.beverageTypeContainerForm.$valid) {
                logError('Invalid Form');
            } else {
                beverageTypeService.save(vm.beverageTypeInstance).then(function (data) {
                    $scope.beverageTypeContainerForm.$setPristine();
                    $scope.beverageTypeContainerForm.$setValidity();
                    logSuccess("BeverageType saved!");
                }, function(error) {
                    logError("Failed to save!");
                });
            }
        };

        function cancel() {
            getRequestedBeverageType().then(function () {
                //$scope.beverageTypeContainerForm.$setPristine();
            });
        };

        function deleteBeverageType() {

            return dialog.deleteDialog(vm.beverageTypeInstance.name)
                .then(confirmDelete);

            function confirmDelete() {
                beverageTypeService.deleteBeverageType(vm.beverageTypeInstance).then(function () {
                    gotoBeverageTypes();
                }, function (error) {
                    logError('Error deleting beverageType: ' + error.statusText);
                });
            }
        }

        function gotoBeverageTypes() { $location.path('/beverageTypes'); }

        function goBack() {
            $window.history.back();
        };

        function goToBeverageType(beverageType) {
            $location.path('/beverageTypes/' + beverageType.id);
        }

    }

})();