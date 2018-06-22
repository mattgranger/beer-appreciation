(function () {
    'use strict';

    // Factory name is handy for logging
    var serviceId = 'signalR-hub-proxy';

    angular.module('beer-appreciation').factory(serviceId, ['$rootScope', 'signalRServer', signalr]);

    function signalr($rootScope, signalRServer) {
        function signalRHubProxyFactory(serverUrl, hubName, startOptions) {

            var connection = $.hubConnection();
            var proxy = connection.createHubProxy(hubName);
        
            return {
                on: function (eventName, callback) {
                    proxy.on(eventName, function (result) {
                        $rootScope.$apply(function () {
                            if (callback) {
                                callback(result);
                            }
                        });
                    });

                    return this;
                },
                off: function (eventName, callback) {
                    proxy.off(eventName, function (result) {
                        $rootScope.$apply(function () {
                            if (callback) {
                                callback(result);
                            }
                        });
                    });

                    return this;
                },
                invoke: function (methodName, callback) {
                    proxy.invoke(methodName)
                        .done(function (result) {
                            $rootScope.$apply(function () {
                                if (callback) {
                                    callback(result);
                                }
                            });
                        });

                    return this;
                },
                connection: connection,
                defaultServer: signalRServer,
                start: function() {
                    connection.start(startOptions).done(function () { });
                    return this;
                }
            };
        };

        return signalRHubProxyFactory;    
    }
})();