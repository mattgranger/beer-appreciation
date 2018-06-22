(function () {
    'use strict';

    // Factory name is handy for logging
    var serviceId = 'user-service';

    // Define the factory on the module.
    // Inject the dependencies. 
    // Point to the factory definition function.
    // TODO: replace app with your module name
    angular.module('beer-appreciation').factory(serviceId, ['$http', '$q', '$resource', 'common', userService]);

    function userService($http, $q, $resource, common) {

        var baseResource = '/api/appreciators/';

        var UserSvc = $resource(baseResource + ':userId', { userId: '@userId' }, {
            post: { method: 'POST', url: baseResource + ':userId' },
            update: { method: 'PUT', url: baseResource + ':userId?&updateGraph=false', params: { userId: '@userId' } },
            remove: { method: 'DELETE', url: baseResource + ':userId' }
        });

        // Define the functions and properties to reveal.
        var service = {
            getPagedData: getPagedData,
            getById: getById,
            save: save,
            deleteUser: deleteUser
        };

        return service;

        function getPagedData(queryOptions) {
            var resourceString = baseResource;
            resourceString += buildODataFilter(queryOptions);// + "&includes=";
            var userResource = $resource(resourceString);
            var beverages = userResource.get();
            return beverages;
        }

        function buildODataFilter(queryOptions) {

            if (!queryOptions) {
                queryOptions = { orderBy: "UserName", orderByDirection: "Asc" };
            }

            var oDataQueryString = "?";

            if (queryOptions.searchText) {
                oDataQueryString += "$filter=";

                if (queryOptions.searchText) {
                    oDataQueryString += "indexof(UserName,'" + queryOptions.searchText +
                        "') gt -1 or indexof(DrinkingName,'" + queryOptions.searchText + "') gt -1";
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

            return UserSvc.get({ userId: id }).$promise;
        }

        function save(user) {
            if (user.id > 0) {
                return UserSvc.update({ userId: user.id }, {
                    id: user.id,
                    name: user.name,
                    description: user.description
                }).$promise;

                //return UserSvc.update({ userId: user.id }, user).$promise;

            } else {
                return UserSvc.post({ userId: user.id }, user).$promise;
            }
        }

        function deleteUser(user) {
            return UserSvc.delete({ userId: user.id }).$promise;
        }
    }
})();