(function () {
    'use strict';

    // Factory name is handy for logging
    var serviceId = 'rating-notification-service';

    // Define the factory on the module.
    // Inject the dependencies. 
    // Point to the factory definition function.
    // TODO: replace app with your module name
    angular.module('beer-appreciation').factory(serviceId,
        ['$rootScope', 'common', '_', ratingNotificationService]);

    function ratingNotificationService($rootScope, common, _) {

        // set up logging functions
        var logError = common.logger.getLogFn(serviceId, 'error');
        var logSuccess = common.logger.getLogFn(serviceId, 'success');
        var logInfo = common.logger.getLogFn(serviceId, 'info');
        var log = common.logger.getLogFn;

        var ratings = [
            "is not dissimilar to cats piss",
            "that they need to eat a turd sandwich to improve the flavour in their mouth",
            "that whoever brewed this shite should be beaten heavily then stabbed",
            "is absolutely fucking disgraceful",
            "is fucking terrible",
            "is a disgusting example of what beer should NOT taste like",
            "is pretty fucking shit",
            "makes me want to drink tooheys", // end of terrible 8
            "is pretty fucking average",
            "is middle of the road",
            "is palatable enough to drink",
            "is meh",
            "is mediocre",
            "is tolerable... just", // end of average 14
            "is better than average",
            "is good not great",
            "is a decent fucking brew",
            "is preferrable to a kick in the nuts", // end of better than average 18
            "is pretty fucking drinkable",
            "is very fucking drinkable",
            "is da shiz",
            "is the bees' knees of beer", // end of top 22
            "is a king amongst beer",
            "is the cat's meow",
            "the brewer deserves a fucking medal",
            "the cream of the crop" // end of excellent 26
        ];

        function getRandomisedRating(start, end) {
            return ratings[_.random(start, end)];
        }

        // create signalr proxy
        var ratingNotificationHubProxy = $.connection.ratingNotificationHub;
        ratingNotificationHubProxy.client.beverageRated = beverageRated;
        $.connection.hub.start().done(function () {
            // Wire up call to the server.
        });

        // Define the functions and properties to reveal.
        var service = {
            rateBeverage: rateBeverage
        };

        function beverageRated(ratingData) {

            if (ratingData.Rating == 0) {
                logInfo(ratingData.DrinkingName + " has reset his score for " + ratingData.Beverage);
            } else {
                var ratingTranslation = "meh";
                switch (ratingData.Rating) {
                    case 1:
                        ratingTranslation = getRandomisedRating(0,4);
                        break;
                    case 2:
                        ratingTranslation = getRandomisedRating(4, 8);
                        break;
                    case 3:
                        ratingTranslation = getRandomisedRating(4, 8);
                        break;
                    case 4:
                        ratingTranslation = getRandomisedRating(8, 14);
                        break;
                    case 5:
                        ratingTranslation = getRandomisedRating(8, 14);
                        break;
                    case 6:
                        ratingTranslation = getRandomisedRating(14, 18);
                        break;
                    case 7:
                        ratingTranslation = getRandomisedRating(14, 18);
                        break;
                    case 8:
                        ratingTranslation = getRandomisedRating(18, 22);
                        break;
                    case 9:
                        ratingTranslation = getRandomisedRating(18, 22);
                        break;
                    case 10:
                        ratingTranslation = getRandomisedRating(22, 26);
                        break;
                }

                var message = "<strong>" + ratingData.DrinkingName + "</strong> thinks <strong>" + ratingData.Beverage + "</strong> " + ratingTranslation + " with a <span class='badge'>" + ratingData.Rating + "</span>";
                logSuccess(message);
            }

        }

        function rateBeverage(ratingData) {

            if (ratingNotificationHubProxy.connection.state === $.signalR.connectionState.connected) {
                ratingNotificationHubProxy.server.rateBeverage(ratingData);
            } else if (ratingNotificationHubProxy.connection.state === $.signalR.connectionState.disconnected) {
                logError("SignalR has been disconnected. Try refreshing your browser");
            }

        }

        return service;
    }
})();