(function () {
    'use strict';

    // Create the module and define its dependencies.
    var app = angular.module('beer-appreciation');

    app.config(['$routeProvider', function ($routeProvider) {

        var baseUrl = "/assets/app/";

        $routeProvider.when("/", {
            templateUrl: baseUrl + "dashboard/dashboard.html",
            title: 'Dashboard',
            settings: {
                nav: 1,
                content: '<i class="icon-dashboard"></i> Dashboard'
            }
        });

        $routeProvider.when("/events", {
            templateUrl: baseUrl + "event/event-list.html",
            title: 'Events',
            settings: {
                nav: 1,
                content: '<i class="icon-areas"></i> Events'
            }
        });

        $routeProvider.when("/events/:id", {
            templateUrl: baseUrl + "event/event-details.html",
            title: 'Event Details',
            settings: {
                nav: 1,
                content: '<i class="icon-areas"></i> Event Details'
            }
        });

        $routeProvider.when("/beers", {
            templateUrl: baseUrl + "beer/beer-list.html",
            title: 'Beers',
            settings: {
                nav: 1,
                content: '<i class="icon-areas"></i> Beers'
            }
        });

        $routeProvider.when("/beers/:id", {
            templateUrl: baseUrl + "beer/beer-details.html",
            title: 'Beer Details',
            settings: {
                nav: 1,
                content: '<i class="icon-areas"></i> Beer Details'
            }
        });

        $routeProvider.when("/register/:id", {
            templateUrl: baseUrl + "registration/register.html",
            title: 'Registration',
            settings: {
                nav: 1,
                content: '<i class="icon-areas"></i> Registration'
            }
        });

        $routeProvider.when("/rate/:id", {
            templateUrl: baseUrl + "rating/rate-event.html",
            title: 'Rate Event',
            settings: {
                nav: 1,
                content: '<i class="icon-events"></i> Rate Event'
            }
        });

        $routeProvider.when("/profile/events", {
            templateUrl: baseUrl + "profile/events.html",
            title: 'My Events',
            settings: {
                nav: 1,
                content: '<i class="icon-areas"></i> My Events'
            }
        });

        $routeProvider.when("/profile/registration/:id", {
            templateUrl: baseUrl + "profile/event-registration.html",
            title: 'Event Registration',
            settings: {
                nav: 1,
                content: '<i class="icon-areas"></i> Event Registration'
            }
        });

        $routeProvider.when("/manufacturers", {
            templateUrl: baseUrl + "manufacturer/manufacturer-list.html",
            title: 'Manufacturers',
            settings: {
                nav: 1,
                content: '<i class="icon-areas"></i> Manufacturers'
            }
        });

        $routeProvider.when("/manufacturers/:id", {
            templateUrl: baseUrl + "manufacturer/manufacturer-details.html",
            title: 'Manufacturer Details',
            settings: {
                nav: 1,
                content: '<i class="icon-areas"></i> Manufacturer Details'
            }
        });

        $routeProvider.when("/beverageStyles", {
            templateUrl: baseUrl + "beverage-style/beverage-style-list.html",
            title: 'Beverage Styles',
            settings: {
                nav: 1,
                content: '<i class="icon-areas"></i> Beverage Styles'
            }
        });

        $routeProvider.when("/beverageStyles/:id", {
            templateUrl: baseUrl + "beverage-style/beverage-style-details.html",
            title: 'Beverage Style Details',
            settings: {
                nav: 1,
                content: '<i class="icon-areas"></i> Beverage Style Details'
            }
        });

        $routeProvider.when("/beverageTypes", {
            templateUrl: baseUrl + "beverage-type/beverage-type-list.html",
            title: 'Beverage Types',
            settings: {
                nav: 1,
                content: '<i class="icon-areas"></i> Beverage Types'
            }
        });

        $routeProvider.when("/beverageTypes/:id", {
            templateUrl: baseUrl + "beverage-type/beverage-type-details.html",
            title: 'Beverage Type Details',
            settings: {
                nav: 1,
                content: '<i class="icon-areas"></i> Beverage Type Details'
            }
        });

        $routeProvider.when("/drinkingClubs", {
            templateUrl: baseUrl + "drinking-club/drinking-club-list.html",
            title: 'Drinking Clubs',
            settings: {
                nav: 1,
                content: '<i class="icon-areas"></i> Drinking Clubs'
            }
        });

        $routeProvider.when("/drinkingClubs/:id", {
            templateUrl: baseUrl + "drinking-club/drinking-club-details.html",
            title: 'Drinking Club Details',
            settings: {
                nav: 1,
                content: '<i class="icon-areas"></i> Drinking Club Details'
            }
        });

        $routeProvider.when("/users", {
            templateUrl: baseUrl + "user/user-list.html",
            title: 'Users',
            settings: {
                nav: 1,
                content: '<i class="icon-user"></i> Users'
            }
        });

        $routeProvider.when("/users/:id", {
            templateUrl: baseUrl + "user/user-details.html",
            title: 'User Details',
            settings: {
                nav: 1,
                content: '<i class="icon-user"></i> User Details'
            }
        });

        $routeProvider.when("/start/", {
            templateUrl: baseUrl + "start/start.html",
            title: 'Start Something',
            settings: {
                nav: 1,
                content: '<i class="icon-start"></i> Start'
            }
        });

        $routeProvider.otherwise({
            templateUrl: baseUrl + "dashboard/dashboard.html",
            settings: {
                nav: 1,
                content: '<i class="icon-dashboard"></i> Dashboard'
            }
        });
    }]);

})();