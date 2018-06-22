(function () {
    'use strict';

    // Controller name is handy for logging
    var controllerId = 'rate-beverage-controller';

    // Define the controller on the module.
    // Inject the dependencies. 
    // Point to the controller definition function.
    angular.module('beer-appreciation').controller(controllerId,
        ['$scope', "rating-notification-service", 'common', rateBeverageController]);

    function rateBeverageController($scope, ratingNoticiationService, common) {
        
        $scope.$watch('score', function() {
            alert("score changed!");
        }, true);

    }
})();
