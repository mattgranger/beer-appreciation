(function () {
    'use strict';

    // Controller name is handy for logging
    var controllerId = 'manufacturer-list-controller';

    // Define the controller on the module.
    // Inject the dependencies. 
    // Point to the controller definition function.
    angular.module('beer-appreciation').controller(controllerId,
        ['$scope', "$location", 'common', 'config', 'manufacturer-service', manufacturerListController]);

    function manufacturerListController($scope, $location, common, config, manufacturerService) {
        // setup shortcuts to common functions
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);
        // Using 'Controller As' syntax, so we assign this to the vm variable (for viewmodel).
        var vm = this;

        // Bindable properties and functions are placed on vm.
        vm.activate = activate;
        vm.dateFormat = common.appSettings.dateFormat;
        vm.title = 'Manufacturer List';
        vm.manufacturers = [];
        vm.totalItems = 0;
        vm.search = common._.debounce(searchManufacturers, 300);
        vm.searchChange = searchChange;
        vm.addManufacturer = addManufacturer;
        vm.editManufacturer = editManufacturer;
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
            getManufacturersByPage(vm.currentPage);
        };

        function activate() {
            common.activateController([getManufacturersByPage(1)], controllerId).then(function () {
                log('Activated Manufacturer List View');
            });
        }

        function getManufacturersByPage(pageIndex) {
            vm.loadingMessage = "Loading page " + pageIndex + "...";
            vm.isLoading = true;
            manufacturerService.getPagedData(vm.queryOptions).$promise.then(function (data) {
                vm.manufacturers = data.items;
                vm.totalItems = data.count;
                vm.totalPages = Math.ceil(data.count / vm.queryOptions.pageSize);
                vm.isLoading = false;
            });
        }

        function searchManufacturers($manufacturer) {
            if ($manufacturer) {
                if ($manufacturer.keyCode == config.keyCodes.esc) {
                    vm.queryOptions.searchText = '';
                }
            }
            getManufacturersByPage(1);
        }

        function searchChange() {
            if (vm.queryOptions.searchText.length == 0) {
                getManufacturersByPage(1);
            }
        }

        function addManufacturer() {
            $location.path('/manufacturers/-1');
        }

        function editManufacturer(id) {
            $location.path('/manufacturers/' + id);
        }
    }
})();
