(function () {
    'use strict';

    // Controller name is handy for logging
    var controllerId = 'user-list-controller';

    // Define the controller on the module.
    // Inject the dependencies. 
    // Point to the controller definition function.
    angular.module('beer-appreciation').controller(controllerId,
        ['$scope', "$location", 'common', 'config', 'user-service', userListController]);

    function userListController($scope, $location, common, config, userService) {
        // setup shortcuts to common functions
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);
        // Using 'Controller As' syntax, so we assign this to the vm variable (for viewmodel).
        var vm = this;

        // Bindable properties and functions are placed on vm.
        vm.activate = activate;
        vm.dateFormat = common.appSettings.dateFormat;
        vm.title = 'Drinking Club List';
        vm.users = [];
        vm.totalItems = 0;
        vm.search = common._.debounce(searchUsers, 300);
        vm.searchChange = searchChange;
        vm.addUser = addUser;
        vm.editUser = editUser;
        vm.isLoading = true;

        vm.queryOptions = {
            searchText: '',
            pageIndex: 1,
            pageSize: common.appSettings.pageSize,
            orderBy: 'UserName',
            orderByDirection: 'Asc'
        };


        vm.activate();

        vm.pageChanged = function () {
            console.log('Page changed to: ' + vm.currentPage);
            getUsersByPage(vm.currentPage);
        };

        function activate() {
            common.activateController([getUsersByPage(1)], controllerId).then(function () {
                log('Activated User List View');
            });
        }

        function getUsersByPage(pageIndex) {
            vm.loadingMessage = "Loading page " + pageIndex + "...";
            vm.isLoading = true;
            userService.getPagedData(vm.queryOptions).$promise.then(function (data) {
                vm.users = data.items;
                vm.totalItems = data.count;
                vm.totalPages = Math.ceil(data.count / vm.queryOptions.pageSize);
                vm.isLoading = false;
            });
        }

        function searchUsers($event) {
            if ($event) {
                if ($event.keyCode == config.keyCodes.esc) {
                    vm.queryOptions.searchText = '';
                }
            }
            getUsersByPage(1);
        }

        function searchChange() {
            if (vm.queryOptions.searchText.length == 0) {
                getUsersByPage(1);
            }
        }

        function addUser() {
            $location.path('/users/-1');
        }

        function editUser(id) {
            $location.path('/users/' + id);
        }
    }
})();
