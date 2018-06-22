(function () {
    'use strict';

    // Controller name is handy for logging
    var controllerId = 'user-details-controller';

    // Define the controller on the module.
    // Inject the dependencies. 
    // Point to the controller definition function.
    angular.module('beer-appreciation').controller(controllerId,
        ['$scope', "$q", "$location", '$routeParams', "$window", "bootstrap.dialog", "$modal", 'common', "config", "user-service", "security-service", userDetailsController]);

    function userDetailsController($scope, $q, $location, $routeParams, $window, dialog, $modal, common, config, userService, securityService) {
        // Using 'Controller As' syntax, so we assign this to the vm variable (for viewmodel).
        var vm = this;
        var logError = common.logger.getLogFn(controllerId, 'error');
        var logSuccess = common.logger.getLogFn(controllerId, 'success');
        var log = common.logger.getLogFn;

        // Bindable properties and functions are placed on vm.
        vm.title = 'User Details';
        vm.cancel = cancel;
        vm.activate = activate;
        vm.saveChanges = saveChanges;
        vm.delete = deleteUser;
        vm.goBack = goBack;
        vm.goToUser = goToUser;
        vm.hasChanges = false;
        vm.isSaving = false;
        vm.userInstance = null;
        vm.idParameter = $routeParams.id;
        $scope.predicate = "score";
        vm.canDelete = false;
        vm.canEdit = false;
        vm.canSave = canSave;
        vm.getSecurityRoles = [];

        activate();

        function canSave() {
            return $scope.userContainerForm.$valid && !$scope.userContainerForm.$pristine;
        }

        function activate() {

            var promises = [getSecurityRoles()];
            common.activateController(promises, controllerId)
                .then(function () {
                    getRequestedUser().then(function () {
                        log('Activated User Details View');
                    });
                });
        }

        function getSecurityRoles() {
            securityService.getSecurityRoles().then(function(data) {
                vm.getSecurityRoles = data;
            });
        }

        function getRequestedUser() {
            if (vm.idParameter === '-1') {
                vm.canEdit = true;
                vm.canDelete = false;
                return $q.when(vm.userInstance = { id: 0 });
            }

            return userService.getById(vm.idParameter)
                .then(function (data) {
                    vm.userInstance = data;
                    //vm.canDelete = vm.userInstance.members.count == 0; //vm.canEdit = (vm.manufacturerInstance.ownerId == common.appSettings.appreciator.id);
                    //$scope.manufacturerContainerForm.$setPristine();
                    //$scope.manufacturerContainerForm.$setValidity();

                }, function (error) {
                    logError('Unable to get user ' + vm.idParameter);
                });
        }

        function saveChanges() {
            if (!$scope.userContainerForm.$valid) {
                logError('Invalid Form');
            } else {
                userService.save(vm.userInstance).then(function (data) {
                    $scope.userContainerForm.$setPristine();
                    $scope.userContainerForm.$setValidity();
                    logSuccess("User saved!");
                }, function (error) {
                    logError("Failed to save!");
                });
            }
        };

        function cancel() {
            getRequestedUser().then(function () {
                //$scope.userContainerForm.$setPristine();
            });
        };

        function deleteUser() {

            return dialog.deleteDialog(vm.userInstance.name)
                .then(confirmDelete);

            function confirmDelete() {
                userService.deleteUser(vm.userInstance).then(function () {
                    gotoUsers();
                }, function (error) {
                    logError('Error deleting user: ' + error.statusText);
                });
            }
        }

        function gotoUsers() { $location.path('/users'); }

        function goBack() {
            $window.history.back();
        };

        function goToUser(user) {
            $location.path('/users/' + user.id);
        }

    }

})();