(function () {
    'use strict';

    // Module name is handy for logging
    var id = 'beer-appreciation';

    // Create the module and define its dependencies.
    var app = angular.module(id, [
        // Angular modules 
        'ngAnimate',        // animations
        'ngRoute',           // routing
        'ngResource',        // rest api

        // Custom modules 
        'common',           // common functions, logger, spinner
        'common.bootstrap', // bootstrap dialog wrapper functions

        // 3rd Party Modules
        'underscore',
        'ui.bootstrap'      // angular ui

    ]);

    // Specify SignalR server URL here for supporting CORS
    app.value('signalRServer', '');
    //app.value('signalRServer', 'http://beer-appreciation:7778/');

    // Execute bootstrapping code and any dependencies.
    app.run(['$q', '$rootScope', 'routemediator', 'security-service',
        function ($q, $rootScope, routemediator, securityService) {

            routemediator.initialiseRoutingHandlers();
            securityService.primeRoleData();
        }]);
})();