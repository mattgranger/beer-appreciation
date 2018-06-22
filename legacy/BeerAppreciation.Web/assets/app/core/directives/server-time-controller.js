(function () {
    'use strict';

    // Controller name is handy for logging
    var controllerId = 'server-time-controller';

    // Define the controller on the module.
    // Inject the dependencies. 
    // Point to the controller definition function.
    angular.module('beer-appreciation').controller(controllerId,
        ['$scope', '$rootScope', 'signalR-hub-proxy', serverTimeController]);

    function serverTimeController($scope, $rootScope, signalRHubProxy) {

        var clientPushHubProxy = signalRHubProxy('', 'clientPushHub', { logging: true });
        var serverTimeHubProxy = signalRHubProxy('', 'serverTimeHub').start();

        clientPushHubProxy.on('serverTime', function (data) {

            var serverTime = moment(data);
            serverTime.local();
            $scope.currentServerTime = serverTime.format("HH:mm:ss A");

            var x = clientPushHubProxy.connection.id;
        }).start();

        $scope.getServerTime = function () {
            serverTimeHubProxy.invoke('getServerTime', function (data) {
                $scope.currentServerTime = data;
            });
        };

        //var serverTimeHub = $.connection.serverTimeHub;
        //var clientPushHub = $.connection.clientPushHub;

        //clientPushHub.client.serverTime = function(currentTime) {
        //    $scope.$apply(function () {
        //        $scope.currentServerTime = currentTime;
        //    });
        //}

        //serverTimeHub.client.displayCurrentServerTime = function (currentTime) {
        //    $rootScope.$apply(function () {
        //        $scope.currentServerTime = currentTime;
        //    });
        //};
        
        // Start the hub.
        //$.connection.hub.start().done(function () {
            
        //});

    }
})();
