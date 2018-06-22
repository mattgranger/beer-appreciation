(function () {
    'use strict';

    // Controller name is handy for logging
    var controllerId = 'drinking-club-list-controller';

    // Define the controller on the module.
    // Inject the dependencies. 
    // Point to the controller definition function.
    angular.module('beer-appreciation').controller(controllerId,
        ['$scope', "$location", 'common', 'config', 'drinking-club-service', drinkingClubListController]);

    function drinkingClubListController($scope, $location, common, config, drinkingClubService) {
        // setup shortcuts to common functions
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);
        // Using 'Controller As' syntax, so we assign this to the vm variable (for viewmodel).
        var vm = this;

        // Bindable properties and functions are placed on vm.
        vm.activate = activate;
        vm.dateFormat = common.appSettings.dateFormat;
        vm.title = 'Drinking Club List';
        vm.drinkingClubs = [];
        vm.totalItems = 0;
        vm.search = common._.debounce(searchDrinkingClubs, 300);
        vm.searchChange = searchChange;
        vm.addDrinkingClub = addDrinkingClub;
        vm.editDrinkingClub = editDrinkingClub;
        vm.isLoading = true;

        vm.queryOptions = {
            searchText: '',
            pageIndex: 1,
            pageSize: common.appSettings.pageSize,
            orderBy: 'Name',
            orderByDirection: 'Asc'
        };


        vm.activate();

        vm.pageChanged = function () {
            console.log('Page changed to: ' + vm.currentPage);
            getDrinkingClubsByPage(vm.currentPage);
        };

        function activate() {
            common.activateController([getDrinkingClubsByPage(1)], controllerId).then(function () {
                log('Activated DrinkingClub List View');
            });
        }

        function getDrinkingClubsByPage(pageIndex) {
            vm.loadingMessage = "Loading page " + pageIndex + "...";
            vm.isLoading = true;
            drinkingClubService.getPagedData(vm.queryOptions).$promise.then(function (data) {
                vm.drinkingClubs = data.items;
                vm.totalItems = data.count;
                vm.totalPages = Math.ceil(data.count / vm.queryOptions.pageSize);
                vm.isLoading = false;
            });
        }

        function searchDrinkingClubs($event) {
            if ($event) {
                if ($event.keyCode == config.keyCodes.esc) {
                    vm.queryOptions.searchText = '';
                }
            }
            getDrinkingClubsByPage(1);
        }

        function searchChange() {
            if (vm.queryOptions.searchText.length == 0) {
                getDrinkingClubsByPage(1);
            }
        }

        function addDrinkingClub() {
            $location.path('/drinkingClubs/-1');
        }

        function editDrinkingClub(id) {
            $location.path('/drinkingClubs/' + id);
        }
    }
})();
