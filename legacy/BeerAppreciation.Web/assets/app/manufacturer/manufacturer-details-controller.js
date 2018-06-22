(function () {
    'use strict';

    // Controller name is handy for logging
    var controllerId = 'manufacturer-details-controller';

    // Define the controller on the module.
    // Inject the dependencies. 
    // Point to the controller definition function.
    angular.module('beer-appreciation').controller(controllerId,
        ['$scope', "$q", "$location", '$routeParams', "$window", "bootstrap.dialog", "$modal", 'common', "config", "manufacturer-service", manufacturerDetailsController]);

    function manufacturerDetailsController($scope, $q, $location, $routeParams, $window, dialog, $modal, common, config, manufacturerService) {
        // Using 'Controller As' syntax, so we assign this to the vm variable (for viewmodel).
        var vm = this;
        var logError = common.logger.getLogFn(controllerId, 'error');
        var logSuccess = common.logger.getLogFn(controllerId, 'success');
        var log = common.logger.getLogFn;

        // Bindable properties and functions are placed on vm.
        vm.title = 'Manufacturer Details';
        vm.cancel = cancel;
        vm.activate = activate;
        vm.saveChanges = saveChanges;
        vm.delete = deleteManufacturer;
        vm.goBack = goBack;
        vm.goToManufacturer = goToManufacturer;
        vm.hasChanges = false;
        vm.isSaving = false;
        vm.drinkingClubs = [];
        vm.manufacturerInstance = null;
        vm.idParameter = $routeParams.id;
        $scope.predicate = "score";
        vm.canDelete = false;
        vm.canEdit = false;
        vm.canSave = canSave;

        activate();

        function canSave() {
            return $scope.manufacturerContainerForm.$valid && !$scope.manufacturerContainerForm.$pristine;
        }

        function activate() {

            var promises = [];
            common.activateController(promises, controllerId)
                .then(function () {
                    getRequestedManufacturer().then(function () {
                        log('Activated Manufacturer Details View');
                    });
                });
        }

        function getRequestedManufacturer() {
            if (vm.idParameter === '-1') {
                vm.canEdit = true;
                vm.canDelete = false;
                return $q.when(vm.manufacturerInstance = { id: 0 });
            }

            return manufacturerService.getById(vm.idParameter)
                .then(function (data) {
                    vm.manufacturerInstance = data;
                vm.canDelete = vm.manufacturerInstance.beverages.count == 0; //vm.canEdit = (vm.manufacturerInstance.ownerId == common.appSettings.appreciator.id);
                //$scope.manufacturerContainerForm.$setPristine();
                //$scope.manufacturerContainerForm.$setValidity();

            }, function (error) {
                    logError('Unable to get manufacturer ' + vm.idParameter);
                });
        }

        function saveChanges() {
            if (!$scope.manufacturerContainerForm.$valid) {
                logError('Invalid Form');
            } else {
                manufacturerService.save(vm.manufacturerInstance).then(function(data) {
                    //$scope.manufacturerContainerForm.$setPristine();
                    logSuccess("Manufacturer saved!");
                });
            }
        };

        function cancel() {
            getRequestedManufacturer().then(function () {
                //$scope.manufacturerContainerForm.$setPristine();
            });
        };

        function deleteManufacturer() {

            return dialog.deleteDialog(vm.manufacturerInstance.name)
                .then(confirmDelete);

            function confirmDelete() {
                manufacturerService.deleteManufacturer(vm.manufacturerInstance).then(function() {
                    gotoManufacturers();
                }, function(error) {
                    logError('Error deleting manufacturer: ' + error.statusText);
                });
            }
        }

        function gotoManufacturers() { $location.path('/manufacturers'); }

        function goBack() {
            $window.history.back();
        };

        function goToManufacturer(manufacturer) {
            $location.path('/manufacturers/' + manufacturer.id);
        }

    }

})();