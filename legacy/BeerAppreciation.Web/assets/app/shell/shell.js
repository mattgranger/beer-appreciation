(function () {
    'use strict';

    var controllerId = 'shell';
    angular.module('beer-appreciation').controller(controllerId,
        ['$rootScope', '$scope', '$window', 'common', 'config', shell]);

    function shell($rootScope, $scope, $window, common, config) {
        var vs = this;
        var logSuccess = common.logger.getLogFn(controllerId, 'success');
        var events = config.events;
        vs.busyMessage = 'Please wait...';
        vs.isBusy = true;
        vs.showSplash = true;
        vs.progressType = "primary";
        vs.inprogress = true;
        vs.spinnerOptions = {
            radius: 32,
            lines: 13,
            length: 14,
            width: 7,
            speed: 1.3,
            corners: 1.0,
            trail: 78,
            color: '#FFF',
            shadow: true
        };

        activate();

        $rootScope.$on("$locationChangeStart", function (event, current, previous) {
            //var answer = $window.confirm("Leave?");
            //if (!answer) {
            //    event.preventDefault();
            //    return;
            //}

            vs.inprogress = true;

        });

        function activate() {

            logSuccess('Beer Appreciation loaded...', null, true);
            common.activateController([], controllerId).then(function () {
                //_.delay(function () {
                //    $rootScope.$apply(function() {
                vs.showSplash = false;
                //    });
                //}, 800);
            });
        }

        function toggleSpinner(on) {

            //_.defer(function() {
            //    $scope.$apply(function () {
            //        vs.isBusy = on;
            //    });
            //});
            //_.delay(function () {
            //$scope.$apply(function() {
            vs.isBusy = on;
            vs.inprogress = on;
            //});
            //}, 1000);
        }

        vs.toggleInProgress = function () {
            if (vs.inprogress) {
                common.$broadcast(config.events.progressBarDeactivate, {});
            } else {
                common.$broadcast(config.events.progressBarActivate, {});
            }
        };

        $rootScope.$on('$routeChangeStart',
            function (event, next, current) { toggleSpinner(true); }
        );

        $rootScope.$on(events.controllerActivateSuccess,
            function (data) {
                toggleSpinner(false);
            }
        );

        $rootScope.$on(events.spinnerToggle,
            function (data) { toggleSpinner(data.show); }
        );

        $rootScope.$on(events.progressBarActivate,
            function (data) {
                vs.inprogress = true;
            }
        );

        $rootScope.$on(events.progressBarDeactivate,
            function (data) {
                vs.inprogress = false;
            }
        );
    };
})();