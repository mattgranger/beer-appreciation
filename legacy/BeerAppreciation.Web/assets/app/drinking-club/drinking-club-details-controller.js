(function () {
    'use strict';

    // Controller name is handy for logging
    var controllerId = 'drinking-club-details-controller';

    // Define the controller on the module.
    // Inject the dependencies. 
    // Point to the controller definition function.
    angular.module('beer-appreciation').controller(controllerId,
        ['$scope', "$q", "$location", '$routeParams', "$window", "bootstrap.dialog", "$modal", 'common', "config", "drinking-club-service", drinkingClubDetailsController]);

    function drinkingClubDetailsController($scope, $q, $location, $routeParams, $window, dialog, $modal, common, config, drinkingClubService) {
        // Using 'Controller As' syntax, so we assign this to the vm variable (for viewmodel).
        var vm = this;
        var logError = common.logger.getLogFn(controllerId, 'error');
        var logSuccess = common.logger.getLogFn(controllerId, 'success');
        var log = common.logger.getLogFn;

        // Bindable properties and functions are placed on vm.
        vm.title = 'DrinkingClub Details';
        vm.cancel = cancel;
        vm.activate = activate;
        vm.saveChanges = saveChanges;
        vm.delete = deleteDrinkingClub;
        vm.goBack = goBack;
        vm.goToDrinkingClub = goToDrinkingClub;
        vm.hasChanges = false;
        vm.isSaving = false;
        vm.drinkingClubInstance = null;
        vm.idParameter = $routeParams.id;
        $scope.predicate = "score";
        vm.canDelete = false;
        vm.canEdit = false;
        vm.canSave = canSave;

        activate();

        function canSave() {
            return $scope.drinkingClubContainerForm.$valid && !$scope.drinkingClubContainerForm.$pristine;
        }

        function activate() {

            var promises = [];
            common.activateController(promises, controllerId)
                .then(function () {
                    getRequestedDrinkingClub().then(function () {
                        log('Activated DrinkingClub Details View');
                    });
                });
        }

        function getRequestedDrinkingClub() {
            if (vm.idParameter === '-1') {
                vm.canEdit = true;
                vm.canDelete = false;
                return $q.when(vm.drinkingClubInstance = { id: 0 });
            }

            return drinkingClubService.getById(vm.idParameter)
                .then(function (data) {
                    vm.drinkingClubInstance = data;
                    //vm.canDelete = vm.drinkingClubInstance.members.count == 0; //vm.canEdit = (vm.manufacturerInstance.ownerId == common.appSettings.appreciator.id);
                    //$scope.manufacturerContainerForm.$setPristine();
                    //$scope.manufacturerContainerForm.$setValidity();

                }, function (error) {
                    logError('Unable to get drinkingClub ' + vm.idParameter);
                });
        }

        function saveChanges() {
            if (!$scope.drinkingClubContainerForm.$valid) {
                logError('Invalid Form');
            } else {
                drinkingClubService.save(vm.drinkingClubInstance).then(function (data) {
                    $scope.drinkingClubContainerForm.$setPristine();
                    $scope.drinkingClubContainerForm.$setValidity();
                    logSuccess("DrinkingClub saved!");
                }, function (error) {
                    logError("Failed to save!");
                });
            }
        };

        function cancel() {
            getRequestedDrinkingClub().then(function () {
                //$scope.drinkingClubContainerForm.$setPristine();
            });
        };

        function deleteDrinkingClub() {

            return dialog.deleteDialog(vm.drinkingClubInstance.name)
                .then(confirmDelete);

            function confirmDelete() {
                drinkingClubService.deleteDrinkingClub(vm.drinkingClubInstance).then(function () {
                    gotoDrinkingClubs();
                }, function (error) {
                    logError('Error deleting drinkingClub: ' + error.statusText);
                });
            }
        }

        function gotoDrinkingClubs() { $location.path('/drinkingClubs'); }

        function goBack() {
            $window.history.back();
        };

        function goToDrinkingClub(drinkingClub) {
            $location.path('/drinkingClubs/' + drinkingClub.id);
        }

    }

})();