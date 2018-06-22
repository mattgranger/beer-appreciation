(function () {
    'use strict';

    // Controller name is handy for logging
    var controllerId = 'rate-event-controller';

    // Define the controller on the module.
    // Inject the dependencies. 
    // Point to the controller definition function.
    angular.module('beer-appreciation').controller(controllerId,
        ['$scope', "$location", '$routeParams', 'common', 'registration-service', 'rating-service', rateEventController]);

    function rateEventController($scope, $location, $routeParams, common, registrationService, ratingService) {
        // setup shortcuts to common functions
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);
        var logError = common.logger.getLogFn(controllerId, 'error');
        var logSuccess = common.logger.getLogFn(controllerId, 'success');
        // Using 'Controller As' syntax, so we assign this to the vm variable (for viewmodel).
        var vm = this;

        // Bindable properties and functions are placed on vm.
        vm.activate = activate;
        vm.title = 'Rate Event';
        vm.dateFormat = common.appSettings.dateFormat;
        vm.registrationInstance = null;
        vm.registrationIdParameter = $routeParams.id;
        vm.resetScore = resetScore;
        vm.rate = 7;
        vm.max = 10;
        vm.isReadonly = false;
        vm.hoveringOver = hoveringOver;
        vm.setRating = setRating;
        vm.isEnabled = false;
        vm.goToRegistration = goToRegistration;

        function hoveringOver(beverageRating, value) {
            beverageRating.overStar = value;
        };

        vm.activate();

        function activate() {
            common.activateController([getEventRegistration()], controllerId).then(function () {
                log('Activated Rating View');
                $scope.$broadcast("Event_Loaded", vm.registrationInstance.event);
            });
        }

        function goToRegistration() {
            $location.path('/profile/registration/' + vm.registrationInstance.id);
        }

        function getEventRegistration() {

            return registrationService.getById(vm.registrationIdParameter)
                .then(function (data) {
                    vm.registrationInstance = data;
                    vm.isEnabled = moment(data.event.date).add(2, 'd').isAfter(moment());

            }, function (error) {
                    logError('Unable to get registration ' + vm.registrationIdParameter);
                });
        }

        function resetScore(beverageRating) {
            beverageRating.score = 0;
            setRating(beverageRating);
        }

        function setRating(beverageRating) {

            if (vm.isEnabled) {
                beverageRating.submittedDate = moment().format('YYYY-MM-DD');

                beverageRating.percent = 100 * (beverageRating.score / vm.max);
                beverageRating.total = beverageRating.score + "/" + vm.max;

                ratingService.rateBeverage(beverageRating).then(function() {
                    ratingService.notificationService.rateBeverage({
                        beverage: beverageRating.eventBeverage.beverage.name,
                        drinkingName: common.appSettings.appreciator.drinkingName,
                        rating: beverageRating.score
                    });

                }, function(error, data) {
                    logError('Unable to save rating ' + error);
                });
            }
        }
    }
})();
