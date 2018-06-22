(function () {
    'use strict';

    // Factory name is handy for logging
    var serviceId = 'event-service';

    // Define the factory on the module.
    // Inject the dependencies. 
    // Point to the factory definition function.
    angular.module('beer-appreciation').factory(serviceId, ['$http', '$q', '$resource', 'common', eventService]);

    function eventService($http, $q, $resource, common) {

        var baseResource = '/api/events/';

        // Define the functions and properties to reveal.
        var service = {
            getPagedData: getPagedData,
            getUpcomingEvents: getUpcomingEvents,
            getById: getById,
            updateScores: updateScores,
            save: save,
            deleteEvent: deleteEvent
        };

        return service;

        function getPagedData(queryOptions) {
            var resourceString = baseResource;
            resourceString += buildODataFilter(queryOptions);
            var eventResource = $resource(resourceString);
            var events = eventResource.get();
            return events;
        }

        function buildODataFilter(queryOptions) {

            var oDataQueryString = "?";
            var hasFilter = false;

            if (queryOptions.searchText || queryOptions.drinkingClub || queryOptions.includePastEvents === false) {
                oDataQueryString += "$filter=";

                if (queryOptions.searchText) {
                    oDataQueryString += "indexof(Name,'" + queryOptions.searchText +
                        "') gt -1 or indexof(Description,'" + queryOptions.searchText + "') gt -1";

                    hasFilter = true;
                }

                if (queryOptions.drinkingClub) {
                    if (hasFilter) {
                        oDataQueryString += " and ";
                    }
                    oDataQueryString += "DrinkingClubId eq " + queryOptions.drinkingClub.id;
                    hasFilter = true;
                }
                if (queryOptions.includePastEvents === false) {
                    if (hasFilter) {
                        oDataQueryString += " and ";
                    }
                    var cutOffDate = moment().format('YYYY-MM-DD');
                    oDataQueryString += "Date gt DateTime'" + cutOffDate + "'";
                }
            }

            if (queryOptions.orderBy && queryOptions.orderByDirection) {
                oDataQueryString += "&$orderby=" + queryOptions.orderBy;
            }

            if (queryOptions.pageIndex || queryOptions.pageSize) {

                oDataQueryString += "&$skip=" + ((queryOptions.pageIndex - 1) * queryOptions.pageSize);
                oDataQueryString += "&$top=" + queryOptions.pageSize;
            }

            return oDataQueryString;
        }

        function getUpcomingEvents() {
            var cutOffDate = moment().format('YYYY-MM-DD');
            var eventsResourceString = baseResource + "?$filter=Date gt DateTime'" + cutOffDate + "'";
            var eventsResource = $resource(eventsResourceString);
            var events = eventsResource.get();
            return events;
        }

        function getById(id) {
            var eventResource = $resource(baseResource + '?id=:eventId', { eventId: id });
            return eventResource.get().$promise;
        }
         
        function save(event) {
            var eventResource = null;

            if (event.drinkingClub) {
                event.drinkingClubId = event.drinkingClub.id;
                event.drinkingClub = null;
            }

            if (event.id > 0) {

                // if this beverage is an existing beverage, ensure navigation
                // properties are nulled out to prevent saving.
                angular.forEach(event.beverages, function (eventBeverage) {
                    if (eventBeverage.beverage.id > 0) {
                        eventBeverage.beverage.manufacturer = null;
                        eventBeverage.beverage.style = null;
                        eventBeverage.beverage.type = null;
                    }});


                return $http.put(baseResource + event.id, event);
            } else {
                event.ownerId = common.appSettings.appreciator.id;
                eventResource = $resource(baseResource);
                eventResource.save(event);
            }
            return event;
        }

        function deleteEvent(event) {
            var eventResource = $resource(baseResource + event.id);
            eventResource.delete();
            return true;
        }

        function updateScores(id) {
            var eventResource = $resource(baseResource + id + "/updatescores");
            return eventResource.get().$promise;
        }
    }
})();