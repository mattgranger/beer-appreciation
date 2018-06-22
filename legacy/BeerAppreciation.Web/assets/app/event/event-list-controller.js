(function () {
    'use strict';

    // Controller name is handy for logging
    var controllerId = 'event-list-controller';

    // Define the controller on the module.
    // Inject the dependencies. 
    // Point to the controller definition function.
    angular.module('beer-appreciation').controller(controllerId,
        ['$scope', "$location", 'common', 'config', 'event-service', eventListController]);

    function eventListController($scope, $location, common, config, eventService) {
        // setup shortcuts to common functions
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);
        // Using 'Controller As' syntax, so we assign this to the vm variable (for viewmodel).
        var vm = this;

        // Bindable properties and functions are placed on vm.
        vm.activate = activate;
        vm.dateFormat = common.appSettings.dateFormat;
        vm.title = 'Event List';
        vm.events = [];
        vm.totalItems = 0;
        vm.search = common._.debounce(searchEvents, 300);
        vm.searchChange = searchChange;
        vm.addEvent = addEvent;
        vm.editEvent = editEvent;
        vm.register = register;
        vm.checkEvent = checkEvent;
        vm.isLoadingEvents = true;

        vm.queryOptions = {
            searchText: '',
            drinkingClub: null,
            includePastEvents: false,
            pageIndex: 1,
            pageSize: common.appSettings.pageSize,
            orderBy: 'Name',
            orderByDirection: 'Asc'
        };


        vm.activate();

        vm.pageChanged = function () {
            console.log('Page changed to: ' + vm.currentPage);
            getEventsByPage(vm.currentPage);
        };

        function activate() {
            common.activateController([getEventsByPage(1)], controllerId).then(function () {
                log('Activated Event List View');
            });
        }

        function getEventsByPage(pageIndex) {
            vm.loadingMessage = "Loading page " + pageIndex + "...";
            vm.isLoadingEvents = true;
            eventService.getPagedData(vm.queryOptions).$promise.then(function (data) {
                vm.events = data.items;
                vm.totalItems = data.count;
                vm.totalPages = Math.ceil(data.count / vm.queryOptions.pageSize);
                vm.isLoadingEvents = false;
            });
        }

        function searchEvents($event) {
            if ($event) {
                if ($event.keyCode == config.keyCodes.esc) {
                    vm.queryOptions.searchText = '';
                }
            }
            getEventsByPage(1);
        }

        function searchChange() {
            if (vm.queryOptions.searchText.length == 0) {
                getEventsByPage(1);
            }
        }

        function addEvent() {
            $location.path('/events/-1');
        }

        function editEvent(id) {
            $location.path('/events/' + id);
        }

        function checkEvent(event) {
            var isValid = moment(event.date).isAfter(moment());
            //log(event.name + ' is ' + (isValid ? 'valid' : 'invalid'));
            return isValid;
        }

        function register(event) {
            if (checkEvent) {
                $location.path('/register/' + event.id);
            }
        }
    }
})();
