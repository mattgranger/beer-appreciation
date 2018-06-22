(function () {
    'use strict';

    // Factory name is handy for logging
    var serviceId = 'security-service';

    // Define the factory on the module.
    // Inject the dependencies. 
    // Point to the factory definition function.
    // TODO: replace app with your module name
    angular.module('beer-appreciation').factory(serviceId,
        ['$http', '$resource', '$q', 'common', securityService]);

    function securityService($http, $resource, $q, common) {
        var baseResource = "/api/security/";
        var isUserRolesInitialised = false;
        var isSecurityRolesInitialised = false;
        var securityRoles = [];
        var userRoles = [];
        // Define the functions and properties to reveal.
        var service = {
            primeRoleData: primeRoleData,
            getSecurityRoles: getSecurityRoles,
            getUserRoles: getUserRoles,
            isAdmin: isAdmin
        };

        return service;

        function primeRoleData() {
            getSecurityRoles();
            getUserRoles();
        }

        function getSecurityRoles() {
            if (!isSecurityRolesInitialised) {

                var deferred = $q.defer();

                var securityResource = $resource(baseResource + 'roles');
                securityResource.query().$promise.then(function (data) {
                    securityRoles = data;
                    deferred.resolve(securityRoles);
                    isSecurityRolesInitialised = true;
                });

                return deferred.promise;
            } else {
                return $q.when(securityRoles);
            }
        };

        function getUserRoles() {
            if (!isUserRolesInitialised) {

                var deferred = $q.defer();

                var securityResource = $resource(baseResource + ':userId/roles', { userId: common.appSettings.appreciator.id });
                securityResource.query().$promise.then(function(data) {
                    userRoles = data;
                    deferred.resolve(userRoles);
                    isUserRolesInitialised = true;
                });

                return deferred.promise;
            } else {
                return $q.when(userRoles);
            }
        };

        function isAdmin() {
            var deferred = $q.defer();
            if (!isUserRolesInitialised) {
                getUserRoles().then(function (data) {
                    deferred.resolve(data.indexOf("Admin") > -1);
                });
                return deferred.promise;
            } else {
                return $q.when(securityRoles.indexOf("Admin") > -1);
            }
        }
    }
})();