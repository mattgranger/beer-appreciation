(function () {
    'use strict';

    // Controller name is handy for logging
    var controllerId = 'dashboard-controller';

    // Define the controller on the module.
    // Inject the dependencies. 
    // Point to the controller definition function.
    angular.module('beer-appreciation').controller(controllerId,
        ['$scope', "$location", 'common', 'drinking-club-service', 'registration-service', dashboardController]);

    function dashboardController($scope, $location, common, drinkingClubService, registrationService) {
        // setup shortcuts to common functions
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);
        // Using 'Controller As' syntax, so we assign this to the vm variable (for viewmodel).
        var vm = this;

        // Bindable properties and functions are placed on vm.
        vm.activate = activate;
        vm.title = 'Home Dashboard';
        vm.drinkingClub = null;
        vm.dateFormat = common.appSettings.dateFormat;
        vm.upcomingEvents = [];
        vm.event = null;
        vm.drinkingClubs = [];
        vm.isLoadingUpcomingEvents = true;
        vm.events = [];

        vm.getDrinkingClubs = getDrinkingClubs;
        vm.getDrinkingClubEvents = getDrinkingClubEvents;
        vm.goToEvent = goToEvent;
        vm.goToEventRegistration = goToEventRegistration;
        vm.register = register;

        $scope.getLocalDate = function(date) {
            return moment.utc(date).local().format('d-MMM-YYYY hh:mm a');
        }

        vm.activate();

        function activate() {
            common.activateController([getDrinkingClubs(), getUpcomingRegisteredEvents()], controllerId).then(function () {
                log('Activated Dashboard View');
            });
        }

        //#region Internal Methods      

        function getUpcomingRegisteredEvents() {
            registrationService.getUserRegistrations().$promise.then(function (data) {
                vm.upcomingEvents = data.items;
                vm.isLoadingUpcomingEvents = false;
            });
        }

        function getDrinkingClubs() {
            drinkingClubService.getPagedData(null, 1, 50).$promise.then(function (data) {
                vm.drinkingClubs = data.items;
            });
        }

        function getDrinkingClubEvents() {
            if (vm.drinkingClub) {
                var queryOptions = {
                    drinkingClub: vm.drinkingClub,
                    includePastEvents: false,
                    pageIndex: 1,
                    pageSize: common.appSettings.pageSize,
                    orderBy: 'Name',
                    orderByDirection: 'Asc'
                };
                registrationService.eventService.getPagedData(queryOptions).$promise.then(function (data) {
                    vm.events = data.items;
                });
            } else {
                vm.events = [];
            }
        }

        function goToEvent(event) {
            $location.path('/events/' + event.id);
        }

        function goToEventRegistration(eventRegistration) {
            $location.path('/profile/registration/' + eventRegistration.id);
        }

        function register() {
            $location.path('/register/' + vm.event.id);
        }

        //#endregion
    }
})();
