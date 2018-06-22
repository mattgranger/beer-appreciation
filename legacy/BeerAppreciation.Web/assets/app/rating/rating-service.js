(function () {
    'use strict';

    // Factory name is handy for logging
    var serviceId = 'rating-service';

    // Define the factory on the module.
    // Inject the dependencies. 
    // Point to the factory definition function.
    // TODO: replace app with your module name
    angular.module('beer-appreciation').factory(serviceId, ['$http', '$resource', 'rating-notification-service', ratingService]);

    function ratingService($http, $resource, ratingNotificationService) {

        var RatingService = $resource(
            "/api/ratings/:id", // uri
            {}, // param default}
            { // actions
                'get': { method: 'GET' },
                'create': { method: 'POST' },
                'update': { method: 'PUT' },
                'query': { method: 'GET', isArray: true },
                'remove': { method: 'DELETE' },
                'delete': { method: 'DELETE' }
            });


        // Define the functions and properties to reveal.
        var service = {
            getData: getData,
            rateBeverage: rateBeverage,
            notificationService: ratingNotificationService
        };

        return service;

        function getData() {

        }

        function rateBeverage(beverageRating) {
            var savePromise;
            if (beverageRating.id === 0) {
                savePromise = RatingService.create(beverageRating, function (value, responseHeaders)
            {
                    var headers = responseHeaders();
                    beverageRating.id = parseInt(headers.entityid);
            });
            } else {
                // RatingService.update(beverageRating);
                return $http.put("/api/ratings/" + beverageRating.id, beverageRating);
            }
            return savePromise.$promise;
        }
    }
})();