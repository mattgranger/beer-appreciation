(function () {
    'use strict';

    // Factory name is handy for logging
    var serviceId = 'beverage-service';

    // Define the factory on the module.
    // Inject the dependencies. 
    // Point to the factory definition function.
    // TODO: replace app with your module name
    angular.module('beer-appreciation').factory(serviceId,
        ['beer-service', 'manufacturer-service', 'beverage-type-service', 'beverage-style-service', beverageService]);

    function beverageService(beerService, manufacturerService, beverageTypeService, beverageStyleService) {
        // Define the functions and properties to reveal.
        var service = {
            beerService: beerService,
            manufacturerService: manufacturerService,
            beverageTypeService: beverageTypeService,
            beverageStyleService: beverageStyleService
        };

        return service;
    }
})();