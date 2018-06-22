(function () {
    'use strict';

    // Factory name is handy for logging
    var serviceId = 'notification-service';

    // Define the factory on the module.
    // Inject the dependencies. 
    // Point to the factory definition function.
    // TODO: replace app with your module name
    angular.module('beer-appreciation').factory(serviceId,
        ['$scope', '$rootScope', 'signalR-hub-proxy', 'common', notiicationService]);

    function notiicationService($scope, $rootScope, signalRHubProxy, common) {
        // Define the functions and properties to reveal.
        var service = {
        };

        return service;
    }
})();