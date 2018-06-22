(function () {
    'use strict';

    // Factory name is handy for logging
    var serviceId = 'beer-service';

    // Define the factory on the module.
    // Inject the dependencies. 
    // Point to the factory definition function.
    angular.module('beer-appreciation').factory(serviceId, ['$http', '$resource', '$q', 'common', beerService]);

    function beerService($http, $resource, $q, common) {

        var baseResource = '/api/beverages';

        // Define the functions and properties to reveal.
        var service = {
            getPagedData: getPagedData,
            getById: getById,
            save: save,
            deleteBeverage: deleteBeverage
        };

        return service;

        function getPagedData(queryOptions) {
            var resourceString = baseResource;
            resourceString += buildODataFilter(queryOptions) + "&includes=BeverageType,Manufacturer";
            var beverageResource = $resource(resourceString);
            var beverages = beverageResource.get();
            return beverages;
        }

        function buildODataFilter(queryOptions) {

            var oDataQueryString = "?";
            var hasFilter = false;

            if (queryOptions.searchText || queryOptions.manufacturer || queryOptions.beverageType || queryOptions.beverageStyle) {
                oDataQueryString += "$filter=";

                if (queryOptions.searchText) {
                    oDataQueryString += "indexof(Name,'" + queryOptions.searchText +
                        "') gt -1 or indexof(Description,'" + queryOptions.searchText + "') gt -1";

                    hasFilter = true;
                }

                if (queryOptions.manufacturer) {
                    if (hasFilter) {
                        oDataQueryString += " and ";
                    }
                    oDataQueryString += "ManufacturerId eq " + queryOptions.manufacturer.id;
                    hasFilter = true;
                }
                if (queryOptions.beverageType) {
                    if (hasFilter) {
                        oDataQueryString += " and ";
                    }
                    oDataQueryString += "BeverageTypeId eq " + queryOptions.type.id;
                    hasFilter = true;
                }
                if (queryOptions.beverageStyle) {
                    if (hasFilter) {
                        oDataQueryString += " and ";
                    }
                    oDataQueryString += "BeverageStyleId eq " + queryOptions.style.id;
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

            if (id > 0) {
                var eventResource = $resource(baseResource + '/:id', { id: id, beverageStyle: { id: 0, beverageType: { id: 0 } }, beverageType: { id: 0 }, manufacturer: { id: 0 } });
                return eventResource.get().$promise;
            } else {
                var newBeverage = { id: 0, manufacturer: { id: 0 }, beverageStyle: { id: 0 }, beverageType: { id: 0 } }
                return $q.when(newBeverage);
            }
        }

        function save(beverage) {
            var beverageResource = null;

            if (beverage.beverageStyleId > 0) {
                beverage.beverageStyle = null;
            }

            if (beverage.beverageTypeId > 0) {
                beverage.beverageType = null;
            }

            if (beverage.manufacturerId > 0) {
                beverage.manufacturer = null;
            }

            if (beverage.id > 0) {
                return $http.put(baseResource + "/" + beverage.id, beverage);
            } else {

                var deferred = $q.defer();

                beverageResource = $resource(baseResource);
                beverageResource.save(beverage, function (value, responseHeaders) {
                // success
                var headers = responseHeaders();
                beverage.id = parseInt(headers.entityid);
                deferred.resolve(beverage);

                }, function(error) {
                    // failure
                    deferred.reject();
                });

                return deferred.promise;
            }
        }

        function deleteBeverage(beverage) {
            var beverageResource = $resource(baseResource + "/" + beverage.id);
            return beverageResource.delete().$promise;
        }
    }
})();