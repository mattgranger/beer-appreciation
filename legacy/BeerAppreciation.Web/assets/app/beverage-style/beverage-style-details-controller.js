(function () {
    'use strict';

    // Controller name is handy for logging
    var controllerId = 'beverage-style-details-controller';

    // Define the controller on the module.
    // Inject the dependencies. 
    // Point to the controller definition function.
    angular.module('beer-appreciation').controller(controllerId,
        ['$scope', "$q", "$location", '$routeParams', "$window", "bootstrap.dialog", "$modal", 'common', "config", "beverage-style-service", beverageStyleDetailsController]);

    function beverageStyleDetailsController($scope, $q, $location, $routeParams, $window, dialog, $modal, common, config, beverageStyleService) {
        // Using 'Controller As' syntax, so we assign this to the vm variable (for viewmodel).
        var vm = this;
        var logError = common.logger.getLogFn(controllerId, 'error');
        var logSuccess = common.logger.getLogFn(controllerId, 'success');
        var log = common.logger.getLogFn;

        // Bindable properties and functions are placed on vm.
        vm.title = 'BeverageStyle Details';
        vm.cancel = cancel;
        vm.activate = activate;
        vm.saveChanges = saveChanges;
        vm.delete = deleteBeverageStyle;
        vm.goBack = goBack;
        vm.goToBeverageStyle = goToBeverageStyle;
        vm.hasChanges = false;
        vm.isSaving = false;
        vm.beverageTypes = [];
        vm.parentStyles = [];
        vm.beverageStyleInstance = null;
        vm.idParameter = $routeParams.id;
        $scope.predicate = "score";
        vm.canDelete = false;
        vm.canEdit = false;
        vm.canSave = canSave;

        activate();

        function canSave() {
            return $scope.beverageStyleContainerForm.$valid && !$scope.beverageStyleContainerForm.$pristine;
        }

        function activate() {

            var promises = [];
            common.activateController(promises, controllerId)
                .then(function () {
                    getRequestedBeverageStyle().then(function () {
                        log('Activated BeverageStyle Details View');
                    });
                });
        }

        function getRequestedBeverageStyle() {
            if (vm.idParameter === '-1') {
                vm.canEdit = true;
                vm.canDelete = false;
                return $q.when(vm.beverageStyleInstance = { id: 0 });
            }

            return beverageStyleService.getById(vm.idParameter)
                .then(function (data) {
                    vm.beverageStyleInstance = data;
                    //vm.canDelete = vm.beverageStyleInstance.beverages.count == 0; //vm.canEdit = (vm.manufacturerInstance.ownerId == common.appSettings.appreciator.id);
                    //$scope.manufacturerContainerForm.$setPristine();
                    //$scope.manufacturerContainerForm.$setValidity();

                }, function (error) {
                    logError('Unable to get beverageStyle ' + vm.idParameter);
                });
        }

        function saveChanges() {
            if (!$scope.beverageStyleContainerForm.$valid) {
                logError('Invalid Form');
            } else {
                beverageStyleService.save(vm.beverageStyleInstance).then(function (data) {
                    $scope.beverageStyleContainerForm.$setPristine();
                    $scope.beverageStyleContainerForm.$setValidity();
                    logSuccess("BeverageStyle saved!");
                });
            }
        };

        function cancel() {
            getRequestedBeverageStyle().then(function () {
                //$scope.beverageStyleContainerForm.$setPristine();
            });
        };

        function deleteBeverageStyle() {

            return dialog.deleteDialog(vm.beverageStyleInstance.name)
                .then(confirmDelete);

            function confirmDelete() {
                beverageStyleService.deleteBeverageStyle(vm.beverageStyleInstance).then(function () {
                    gotoBeverageStyles();
                }, function (error) {
                    logError('Error deleting beverageStyle: ' + error.statusText);
                });
            }
        }

        function gotoBeverageStyles() { $location.path('/beverageStyles'); }

        function goBack() {
            $window.history.back();
        };

        function goToBeverageStyle(beverageStyle) {
            $location.path('/beverageStyles/' + beverageStyle.id);
        }

    }

})();