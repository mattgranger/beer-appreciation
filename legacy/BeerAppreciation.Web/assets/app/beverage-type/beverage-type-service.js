(function () {
    'use strict';

    // Factory name is handy for logging
    var serviceId = 'beverage-type-service';

    // Define the factory on the module.
    // Inject the dependencies. 
    // Point to the factory definition function.
    // TODO: replace app with your module name
    angular.module('beer-appreciation').factory(serviceId, ['$http', '$q', '$resource', 'common', beverageTypeService]);

    function beverageTypeService($http, $q, $resource, common) {

        var baseResource = '/api/beveragetypes/';

        var BeverageTypeSvc = $resource(baseResource + ':beverageTypeId?&includes=Styles,Styles.Parent', { beverageTypeId: '@beverageTypeId' }, {
            post: { method: 'POST', url: baseResource + ':beverageTypeId' },
            update: { method: 'PUT', url: baseResource + ':beverageTypeId?&updateGraph=false', params: { beverageTypeId: '@beverageTypeId' } },
            remove: { method: 'DELETE', url: baseResource + ':beverageTypeId' }
        });

        // Define the functions and properties to reveal.
        var service = {
            getPagedData: getPagedData,
            getById: getById,
            save: save,
            deleteBeverageType: deleteBeverageType
        };

        return service;

        function getPagedData(queryOptions) {
            var resourceString = baseResource;
            resourceString += buildODataFilter(queryOptions) + "&includes=";
            var beverageTypeResource = $resource(resourceString);
            var beverages = beverageTypeResource.get();
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

            return BeverageTypeSvc.get({ beverageTypeId: id }).$promise;
        }

        function save(beverageType) {
            if (beverageType.id > 0) {
                return BeverageTypeSvc.update({ beverageTypeId: beverageType.id }, {
                    id: beverageType.id,
                    name: beverageType.name,
                    description: beverageType.description
                }).$promise;

                //return BeverageTypeSvc.update({ beverageTypeId: beverageType.id }, beverageType).$promise;

            } else {
                return BeverageTypeSvc.post({ beverageTypeId: beverageType.id }, beverageType).$promise;
            }
        }

        function deleteBeverageType(beverageType) {
            return BeverageTypeSvc.delete({ beverageTypeId: beverageType.id }).$promise;
        }
    }
})();