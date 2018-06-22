(function () {
    'use strict';

    // Controller name is handy for logging
    var controllerId = 'profile-events-controller';

    // Define the controller on the module.
    // Inject the dependencies. 
    // Point to the controller definition function.
    angular.module('beer-appreciation').controller(controllerId,
        ['$scope', "$location", 'common', 'config', 'registration-service', profileEventListController]);

    function profileEventListController($scope, $location, common, config, registrationService) {
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
        vm.editRegistration = editRegistration;
        vm.register = register;
        vm.rate = rate;
        vm.checkEvent = checkEvent;
        vm.isLoadingBeverages = true;

        vm.queryOptions = {
            appreciatorId: common.appSettings.appreciator.id,
            searchText: '',
            drinkingClub: null,
            includePastEvents: false,
            pageIndex: 1,
            pageSize: common.appSettings.pageSize,
            orderBy: 'Event/Name',
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
            registrationService.getPagedData(vm.queryOptions).$promise.then(function (data) {
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

        function editRegistration(registration) {
            $location.path('/profile/registration/' + registration.id);
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

        function rate(event) {
            $location.path('/rate/' + event.id);
        }
    }
})();
