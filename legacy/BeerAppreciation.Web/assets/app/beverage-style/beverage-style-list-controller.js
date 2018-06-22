(function () {
    'use strict';

    // Controller name is handy for logging
    var controllerId = 'beverage-style-list-controller';

    // Define the controller on the module.
    // Inject the dependencies. 
    // Point to the controller definition function.
    angular.module('beer-appreciation').controller(controllerId,
        ['$scope', "$location", 'common', 'config', 'beverage-style-service', beverageStyleListController]);

    function beverageStyleListController($scope, $location, common, config, beverageStyleService) {
        // setup shortcuts to common functions
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);
        // Using 'Controller As' syntax, so we assign this to the vm variable (for viewmodel).
        var vm = this;

        // Bindable properties and functions are placed on vm.
        vm.activate = activate;
        vm.dateFormat = common.appSettings.dateFormat;
        vm.title = 'Beverage Style List';
        vm.beverageStyles = [];
        vm.totalItems = 0;
        vm.search = common._.debounce(searchBeverageStyles, 300);
        vm.searchChange = searchChange;
        vm.addBeverageStyle = addBeverageStyle;
        vm.editBeverageStyle = editBeverageStyle;
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
            getBeverageStylesByPage(vm.currentPage);
        };

        function activate() {
            common.activateController([getBeverageStylesByPage(1)], controllerId).then(function () {
                log('Activated BeverageStyle List View');
            });
        }

        function getBeverageStylesByPage(pageIndex) {
            vm.loadingMessage = "Loading page " + pageIndex + "...";
            vm.isLoading = true;
            beverageStyleService.getPagedData(vm.queryOptions).$promise.then(function (data) {
                vm.beverageStyles = data.items;
                vm.totalItems = data.count;
                vm.totalPages = Math.ceil(data.count / vm.queryOptions.pageSize);
                vm.isLoading = false;
            });
        }

        function searchBeverageStyles($event) {
            if ($event) {
                if ($event.keyCode == config.keyCodes.esc) {
                    vm.queryOptions.searchText = '';
                }
            }
            getBeverageStylesByPage(1);
        }

        function searchChange() {
            if (vm.queryOptions.searchText.length == 0) {
                getBeverageStylesByPage(1);
            }
        }

        function addBeverageStyle() {
            $location.path('/beverageStyles/-1');
        }

        function editBeverageStyle(id) {
            $location.path('/beverageStyles/' + id);
        }
    }
})();
