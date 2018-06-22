(function () {
    'use strict';

    // Factory name is handy for logging
    var serviceId = 'event-beverage-service';

    angular.module('beer-appreciation').factory(serviceId, ['$http', '$resource', eventBeverageService]);

    function eventBeverageService($http, $resource) {
        // Define the functions and properties to reveal.
        var EventBeverageService = $resource(
            "/api/eventbeverages/:id", // uri
            {}, // param default}
            { // actions
                'get': { method: 'GET' },
                'create': { method: 'POST' },
                'update': { method: 'PUT' },
                'query': { method: 'GET', isArray: true },
                'remove': { method: 'DELETE' },
                'delete': {method: 'DELETE'}
            });

        var service = {
            getById: getById,
            deleteEventBeverage: deleteEventBeverage
        };

        return service;

        function getById(eventBeverageId) {
            var eventBeverage = EventBeverageService.get({ id: eventBeverageId }, function(data) {
                return data;
            });
        }

        function deleteEventBeverage(eventBeverageId) {
            var deletePromise = EventBeverageService.delete({ id: eventBeverageId });
            return deletePromise.$promise;
        }

        //#region Internal Methods        

        //#endregion
    }
})();