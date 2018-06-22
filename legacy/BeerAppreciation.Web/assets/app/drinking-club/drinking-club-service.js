(function () {
    'use strict';

    // Factory name is handy for logging
    var serviceId = 'drinking-club-service';

    // Define the factory on the module.
    // Inject the dependencies. 
    // Point to the factory definition function.
    // TODO: replace app with your module name
    angular.module('beer-appreciation').factory(serviceId, ['$http', '$q', '$resource', 'common', drinkingClubService]);

    function drinkingClubService($http, $q, $resource, common) {

        var baseResource = '/api/drinkingclubs/';

        var DrinkingClubSvc = $resource(baseResource + ':drinkingClubId?&includes=Members,Events', { drinkingClubId: '@drinkingClubId' }, {
            post: { method: 'POST', url: baseResource + ':drinkingClubId' },
            update: { method: 'PUT', url: baseResource + ':drinkingClubId?&updateGraph=false', params: { drinkingClubId: '@drinkingClubId' } },
            remove: { method: 'DELETE', url: baseResource + ':drinkingClubId' }
        });

        // Define the functions and properties to reveal.
        var service = {
            getPagedData: getPagedData,
            getById: getById,
            save: save,
            deleteDrinkingClub: deleteDrinkingClub
        };

        return service;

        function getPagedData(queryOptions) {
            var resourceString = baseResource;
            resourceString += buildODataFilter(queryOptions);// + "&includes=";
            var drinkingClubResource = $resource(resourceString);
            var beverages = drinkingClubResource.get();
            return beverages;
        }

        function buildODataFilter(queryOptions) {

            if (!queryOptions) {
                queryOptions = { orderBy: "Name", orderByDirection: "Asc" };
            }

            var oDataQueryString = "?";

            if (queryOptions.searchText) {
                oDataQueryString += "$filter=";

                if (queryOptions.searchText) {
                    oDataQueryString += "indexof(Name,'" + queryOptions.searchText +
                        "') gt -1 or indexof(Description,'" + queryOptions.searchText + "') gt -1";
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
            //var manufacturerResource = $resource(baseResource + '?id=:manufacturerId', { manufacturerId: id });
            //return manufacturerResource.get().$promise;

            return DrinkingClubSvc.get({ drinkingClubId: id }).$promise;
        }

        function save(drinkingClub) {
            if (drinkingClub.id > 0) {
                return DrinkingClubSvc.update({ drinkingClubId: drinkingClub.id }, {
                    id: drinkingClub.id,
                    name: drinkingClub.name,
                    description: drinkingClub.description
                }).$promise;

                //return DrinkingClubSvc.update({ drinkingClubId: drinkingClub.id }, drinkingClub).$promise;

            } else {
                return DrinkingClubSvc.post({ drinkingClubId: drinkingClub.id }, drinkingClub).$promise;
            }
        }

        function deleteDrinkingClub(drinkingClub) {
            return DrinkingClubSvc.delete({ drinkingClubId: drinkingClub.id }).$promise;
        }
    }
})();