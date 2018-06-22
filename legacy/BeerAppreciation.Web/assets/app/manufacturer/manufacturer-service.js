(function () {
    'use strict';

    // Factory name is handy for logging
    var serviceId = 'manufacturer-service';

    // Define the factory on the module.
    // Inject the dependencies. 
    // Point to the factory definition function.
    // TODO: replace app with your module name
    angular.module('beer-appreciation').factory(serviceId, ['$http', '$q', '$resource', 'common', manufacturerService]);

    function manufacturerService($http, $q, $resource, common) {

        var baseResource = '/api/manufacturers/';

        var ManufacturerSvc = $resource(baseResource + ':manufacturerId', { manufacturerId: '@manufacturerId' }, {
            post: {method:'POST'},
            update: { method: 'PUT', params: { manufacturerId: '@manufacturerId' } },
            remove: {method:'DELETE'}
        });

        // Define the functions and properties to reveal.
        var service = {
            getPagedData: getPagedData,
            getById: getById,
            save: save,
            deleteManufacturer: deleteManufacturer
        };

        return service;

        function getPagedData(queryOptions) {
            var resourceString = baseResource;
            resourceString += buildODataFilter(queryOptions);
            var beverageResource = $resource(resourceString);
            var beverages = beverageResource.get();
            return beverages;
        }

        function getById(id) {
            //var manufacturerResource = $resource(baseResource + '?id=:manufacturerId', { manufacturerId: id });
            //return manufacturerResource.get().$promise;

            return ManufacturerSvc.get({ manufacturerId: id }).$promise;
        }

        function save(manufacturer) {
            if (manufacturer.id > 0) {
                return ManufacturerSvc.update({ manufacturerId: manufacturer.id }, manufacturer).$promise;
            } else {
                return ManufacturerSvc.save({ manufacturerId: manufacturer.id }, manufacturer).$promise;
            }
        }

        function deleteManufacturer(manufacturer) {
            return ManufacturerSvc.delete({ manufacturerId: manufacturer.id }).$promise;
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
    }
})();