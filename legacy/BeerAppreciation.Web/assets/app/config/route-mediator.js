(function () {
    'use strict';

    // Factory name is handy for logging
    var serviceId = 'routemediator';
    angular.module('beer-appreciation').factory(serviceId, ['$http', '$location', '$rootScope', 'config', 'logger', routemediator]);

    function routemediator($http, $location, $rootScope, config, logger) {
        // Define the functions and properties to reveal.
        var handleRouteChangeError = false;

        var service = {
            initialiseRoutingHandlers: initialiseRoutingHandlers
        };

        return service;

        function initialiseRoutingHandlers() {
            handleRoutingSuccess();
            handleRoutingErrors();
        }

        function handleRoutingSuccess() {
            $rootScope.$on("$routeChangeSuccess", function (event, current, previous) {
                handleRouteChangeError = false;
                if (current) {
                    updateDocTitle(event, current, previous);
                }
            });
        }

        function updateDocTitle(event, current, previous) {
            var title = config.docTitle + ' ' + (current.title || '');
            $rootScope.title = title;
        }

        function handleRoutingErrors() {
            $rootScope.$on("$routeChangeError", function (event, current, previous, rejection) {

                if (handleRouteChangeError) {
                    return;
                }

                handleRouteChangeError = true;
                var msg = 'Error routing: ' + (current && current.originalPath) + ". " + rejection.msg;
                logger.logWarning(msg, current, serviceId, true);
                $location.path("/");
            });
        }
    }
})();