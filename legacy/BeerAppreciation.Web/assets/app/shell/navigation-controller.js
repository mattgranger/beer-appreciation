(function () {
    'use strict';

    var controllerId = 'navigation-controller';
    angular.module('beer-appreciation').controller(controllerId,
        ['$rootScope', '$scope', 'common', 'security-service', navigationController]);

    function navigationController($rootScope, $scope, common, securityService) {
        $scope.isAdmin = false;
        activate();

        function activate() {
            securityService.isAdmin().then(function(data) {
                $scope.isAdmin = data;
            });
        }
    };
})();