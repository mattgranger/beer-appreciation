(function () {
    'use strict';

    // Factory name is handy for logging
    var serviceId = 'registration-service';

    // Define the factory on the module.
    // Inject the dependencies. 
    // Point to the factory definition function.
    // TODO: replace app with your module name
    angular.module('beer-appreciation').factory(serviceId, ['$http', '$q', '$resource', 'common', 'event-service', 'beer-service', 'event-beverage-service', registrationService]);

    function registrationService($http, $q, $resource, common, eventService, beerService, eventBeverageService) {

        var baseResource = '/api/eventregistrations/:id';

        // Define the functions and properties to reveal.
        var service = {
            beerService: beerService,
            eventService: eventService,
            eventBeverageService: eventBeverageService,
            getUserRegistrations: getUserRegistrations,
            getPagedData: getPagedData,
            getById: getById,
            getByExisting: getByExisting,
            save: save,
            deleteRegistration: deleteRegistration
        };

        return service;

        function getPagedData(queryOptions) {
            var resourceString = baseResource;
            resourceString += buildODataFilter(queryOptions);

            resourceString += "&includes=Event";

            var registrationResource = $resource(resourceString);
            var registrations = registrationResource.get();
            return registrations;
        }

        function getUserRegistrations() {
            var cutOffDate = moment().format('YYYY-MM-DD');
            var eventsResourceString = baseResource + "?includes=Event&$filter=Event/Date gt DateTime'" + cutOffDate + "' and AppreciatorId eq '" + common.appSettings.appreciator.id + "'";

            var eventsResource = $resource(eventsResourceString);
            var events = eventsResource.get();
            return events;
        }

        function buildODataFilter(queryOptions) {

            var oDataQueryString = "?";
            var hasFilter = false;

            if (queryOptions.searchText || queryOptions.event || queryOptions.appreciatorId) {
                oDataQueryString += "$filter=";

                //if (queryOptions.searchText) {
                //    oDataQueryString += "indexof(Name,'" + queryOptions.searchText +
                //        "') gt -1 or indexof(Description,'" + queryOptions.searchText + "') gt -1";

                //    hasFilter = true;
                //}

                if (queryOptions.event) {
                    if (hasFilter) {
                        oDataQueryString += " and ";
                    }
                    oDataQueryString += "EventId eq " + queryOptions.event.id;
                    hasFilter = true;
                }
                if (queryOptions.appreciatorId) {
                    if (hasFilter) {
                        oDataQueryString += " and ";
                    }
                    oDataQueryString += "AppreciatorId eq '" + queryOptions.appreciatorId + "'";
                    hasFilter = true;
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

        function getById(id) {
            var eventResource = $resource(baseResource + '?includes=event,beverages,beverages.beverage', { id: id });
            return eventResource.get().$promise;
        }

        function getByExisting(eventId, appreciatorId) {
            var eventResource = $resource(baseResource + "?$filter=EventId eq :eventId and AppreciatorId eq ':appreciatorId'&includes=Beverages",
                { eventId: eventId, appreciatorId: appreciatorId });

            return eventResource.get().$promise;
        }

        function save(registration) {

            if (registration.event) {
                registration.eventId = registration.event.id;
                registration.event.state = 1;
            }
            if (registration.appreciator) {
                registration.appreciatorId = registration.appreciator.id;
                registration.appreciator.state = 1;
            }

            // if this beverage is an existing beverage, ensure navigation
            // properties are nulled out to prevent saving.
            angular.forEach(registration.beverages, function (eventBeverage) {
                eventBeverage.beverage.state = 1;
                if (eventBeverage.beverage.manufacturer) {
                    eventBeverage.beverage.manufacturer.state = 1;
                }
                if (eventBeverage.beverage.beverageStyle) {
                    eventBeverage.beverage.beverageStyle.state = 1;
                }
                if (eventBeverage.beverage.beverageType) {
                    eventBeverage.beverage.beverageType.state = 1;
                }
                //if (eventBeverage.beverage.manufacturerId > 0) {
                //    eventBeverage.beverage.manufacturer = null;
                //    //eventBeverage.beverage.manufacturer.state = 1;
                //}
                //if (eventBeverage.beverage.beverageStyleId > 0) {
                //    eventBeverage.beverage.beverageStyle = null;
                //    //eventBeverage.beverage.style.state = 1;
                //}
                //if (eventBeverage.beverage.beverageTypeId > 0) {
                //    //eventBeverage.beverage.type.state = 1;
                //    eventBeverage.beverage.beverageType = null;
                //}
            });

            var registrationUri = "/api/eventregistrations/";
            if (registration.id <= 0) {

                var deferred = $q.defer();

                var registrationResource = $resource(registrationUri);
                registrationResource.save(registration, function (value, responseHeaders) {
                    // success
                    var headers = responseHeaders();
                    registration.id = parseInt(headers.entityid);
                    deferred.resolve(registration);

                }, function (error) {
                    // failure
                    deferred.reject();
                });

                return deferred.promise;

            } else {
                return $http.put(registrationUri + registration.id + "?updateGraph=true", registration);
            }
        }

        function deleteRegistration(registration) {
            var registrationResource = $resource(baseResource, { id: registration.id });
            return registrationResource.delete().$promise;
        }
    }
})();