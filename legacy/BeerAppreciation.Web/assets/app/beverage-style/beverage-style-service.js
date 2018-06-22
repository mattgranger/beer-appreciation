(function () {
    'use strict';

    // Factory name is handy for logging
    var serviceId = 'beverage-style-service';

    // Define the factory on the module.
    // Inject the dependencies. 
    // Point to the factory definition function.
    // TODO: replace app with your module name
    angular.module('beer-appreciation').factory(serviceId, ['$http', '$q', '$resource', 'common', beverageStyleService]);

    function beverageStyleService($http, $q, $resource, common) {

        var baseResource = '/api/beveragestyles/';

        var BeverageStyleSvc = $resource(baseResource + ':beverageStyleId?&includes=BeverageType,Parent,Beverages', { beverageStyleId: '@beverageStyleId' }, {
            post: { method: 'POST', url: baseResource + ':beverageStyleId' },
            update: { method: 'PUT', url: baseResource + ':beverageStyleId', params: { beverageStyleId: '@beverageStyleId' } },
            remove: { method: 'DELETE', url: baseResource + ':beverageStyleId' }
        });

        // Define the functions and properties to reveal.
        var service = {
            getPagedData: getPagedData,
            getById: getById,
            save: save,
            deleteBeverageStyle: deleteBeverageStyle
        };

        return service;

        function getPagedData(queryOptions) {
            var resourceString = baseResource;
            resourceString += buildODataFilter(queryOptions) + "&includes=BeverageType,Parent,Beverages";
            var beverageStyleResource = $resource(resourceString);
            var beverages = beverageStyleResource.get();
            return beverages;
        }

        function buildODataFilter(queryOptions) {

            if (!queryOptions) {
                queryOptions = { orderBy: "Name", orderByDirection: "Asc" };
            }

            var oDataQueryString = "?";

            if (queryOptions.searchText || queryOptions.beverageTypeId) {
                oDataQueryString += "$filter=";

                if (queryOptions.searchText) {
                    oDataQueryString += "indexof(Name,'" + queryOptions.searchText +
                        "') gt -1 or indexof(Description,'" + queryOptions.searchText + "') gt -1";
                }

                if (queryOptions.beverageTypeId) {
                    oDataQueryString += "BeverageTypeId eq " + queryOptions.beverageTypeId;
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

            return BeverageStyleSvc.get({ beverageStyleId: id }).$promise;
        }

        function save(beverageStyle) {
            if (beverageStyle.id > 0) {
                return BeverageStyleSvc.update({ beverageStyleId: beverageStyle.id }, beverageStyle).$promise;
            } else {
                return BeverageStyleSvc.save({ beverageStyleId: beverageStyle.id }, beverageStyle).$promise;
            }
        }

        function deleteBeverageStyle(beverageStyle) {
            return BeverageStyleSvc.delete({ beverageStyleId: beverageStyle.id }).$promise;
        }
    }
})();