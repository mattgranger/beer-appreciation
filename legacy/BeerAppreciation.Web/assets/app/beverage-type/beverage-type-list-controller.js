(function () {
    'use strict';

    // Controller name is handy for logging
    var controllerId = 'beverage-type-list-controller';

    // Define the controller on the module.
    // Inject the dependencies. 
    // Point to the controller definition function.
    angular.module('beer-appreciation').controller(controllerId,
        ['$scope', "$location", 'common', 'config', 'beverage-type-service', beverageTypeListController]);

    function beverageTypeListController($scope, $location, common, config, beverageTypeService) {
        // setup shortcuts to common functions
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);
        // Using 'Controller As' syntax, so we assign this to the vm variable (for viewmodel).
        var vm = this;

        // Bindable properties and functions are placed on vm.
        vm.activate = activate;
        vm.dateFormat = common.appSettings.dateFormat;
        vm.title = 'Beverage Type List';
        vm.beverageTypes = [];
        vm.totalItems = 0;
        vm.search = common._.debounce(searchBeverageTypes, 300);
        vm.searchChange = searchChange;
        vm.addBeverageType = addBeverageType;
        vm.editBeverageType = editBeverageType;
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
            getBeverageTypesByPage(vm.currentPage);
        };

        function activate() {
            common.activateController([getBeverageTypesByPage(1)], controllerId).then(function () {
                log('Activated BeverageType List View');
            });
        }

        function getBeverageTypesByPage(pageIndex) {
            vm.loadingMessage = "Loading page " + pageIndex + "...";
            vm.isLoading = true;
            beverageTypeService.getPagedData(vm.queryOptions).$promise.then(function (data) {
                vm.beverageTypes = data.items;
                vm.totalItems = data.count;
                vm.totalPages = Math.ceil(data.count / vm.queryOptions.pageSize);
                vm.isLoading = false;
            });
        }

        function searchBeverageTypes($event) {
            if ($event) {
                if ($event.keyCode == config.keyCodes.esc) {
                    vm.queryOptions.searchText = '';
                }
            }
            getBeverageTypesByPage(1);
        }

        function searchChange() {
            if (vm.queryOptions.searchText.length == 0) {
                getBeverageTypesByPage(1);
            }
        }

        function addBeverageType() {
            $location.path('/beverageTypes/-1');
        }

        function editBeverageType(id) {
            $location.path('/beverageTypes/' + id);
        }
    }
})();
