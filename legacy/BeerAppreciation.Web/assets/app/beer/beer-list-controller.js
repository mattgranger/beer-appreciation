(function () {
    'use strict';

    // Controller name is handy for logging
    var controllerId = 'beer-list-controller';

    // Define the controller on the module.
    // Inject the dependencies. 
    // Point to the controller definition function.
    angular.module('beer-appreciation').controller(controllerId,
        ['$scope', '$window', '$location', 'common', 'config', 'beverage-service', beerListController]);

    function beerListController($scope, $window, $location, common, config, beverageService) {
        // setup shortcuts to common functions
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);
        // Using 'Controller As' syntax, so we assign this to the vm variable (for viewmodel).
        var vm = this;

        // Bindable properties and functions are placed on vm.
        vm.activate = activate;
        vm.title = 'Beer List';
        vm.beverages = [];
        vm.totalItems = 0;
        vm.showAdvancedFilters = false;
        vm.isReducedSize = false;
        vm.search = common._.debounce(searchBeers, 300);
        vm.searchChange = searchChange;
        vm.addBeer = addBeverage;
        vm.goToBeverage = goToBeverage;
        vm.isLoadingBeverages = true;
        vm.beverageStyles = [];
        vm.manufacturers = [];
        vm.queryOptions = {
            searchText: '',
            beverageStyle: null,
            manufacturer: null,
            pageIndex: 1,
            pageSize: common.appSettings.pageSize,
            orderBy: 'Name',
            orderByDirection: 'Asc'
        };

        vm.currentWidth = $window.innerWidth;
        vm.changePageSize = changePageSize;

        vm.activate();

        $(window).resize(function () {

            var applyScope = false;
            var newWidth = $window.innerWidth;
            if (newWidth != vm.currentWidth) {
                vm.currentWidth = newWidth;
                if (window.innerWidth < 640) {

                    vm.showAdvancedFilters = true;
                    vm.isReducedSize = true;
                    applyScope = true;

                } else if (window.innerWidth >= 640) {
                    vm.showAdvancedFilters = false;
                    vm.isReducedSize = false;
                    applyScope = true;
                }

                if (applyScope) {
                    $scope.$apply(function() {
                        //do something to update current scope based on the new innerWidth and let angular update the view.
                    });
                }
            }
        });

        vm.pageChanged = function () {
            console.log('Page changed to: ' + vm.queryOptions.pageIndex);
            getBeersByPage(vm.queryOptions.pageIndex);
        };

        function activate() {

            if ($window.innerWidth < 640) {
                vm.isReducedSize = true;
                vm.showAdvancedFilters = true;
            }

            common.$broadcast(config.events.progressBarActivate, {});
            common.activateController([getBeersByPage(1), getBeverageStyles(), getManufacturers()], controllerId).then(function () {
                log('Activated Beer List View');
            });
        }

        function getBeverageStyles() {
            return beverageService.beverageStyleService.getPagedData(null, null, 1, 50, true).$promise.then(function (data) {
                vm.beverageStyles = data.items;
            });
        }

        function getBeersByPage(pageIndex) {
            common.$broadcast(config.events.progressBarActivate, {});
            vm.loadingMessage = "Loading page " + pageIndex + "...";
            vm.isLoadingBeverages = true;

            beverageService.beerService.getPagedData(vm.queryOptions).$promise.then(function (data) {
                vm.beverages = data.items;
                vm.totalItems = data.count;
                vm.totalPages = Math.ceil(data.count / vm.queryOptions.pageSize);
                common.$broadcast(config.events.progressBarDeactivate, {});
                vm.isLoadingBeverages = false;
            });

        }

        function getManufacturers() {
            return beverageService.manufacturerService.getPagedData().$promise.then(function (data) {
                vm.manufacturers = data.items;
            });
        }

        vm.toggleAdvancedSearch = function() {
            vm.showAdvancedFilters = !vm.showAdvancedFilters;
        };

        function searchBeers($event) {
            if ($event.keyCode == config.keyCodes.esc) {
                vm.queryOptions.searchText = '';
            }
            getBeersByPage(1);
        }

        function searchChange() {
            if (vm.queryOptions.searchText.length == 0) {
                getBeersByPage(1);
            }
        }

        function addBeverage() {
            $location.path('/beers/' + -1);
        }

        function goToBeverage(beverage) {
            $location.path('/beers/' + beverage.id);
        }

        function changePageSize() {
            getBeersByPage(1);
        }
    }
})();
