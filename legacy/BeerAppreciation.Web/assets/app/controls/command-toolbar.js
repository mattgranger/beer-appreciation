(function() {
    'use strict';

    // Define the directive on the module.
    // Inject the dependencies. 
    // Point to the directive definition function.
    angular.module('beer-appreciation').directive('commandToolbar', ['$window', commandToolbar]);
    
    function commandToolbar($window) {
        // Usage:
        // 
        // Creates:
        // 
        var directive = {
            link: link,
            restrict: 'AE',
            templateUrl: '/assets/app/controls/command-toolbar.html',
            scope: {
                goBack: "=",
                cancel: "=",
                save: "=",
                "delete": "=",
                hasChanges: "=",
                canSave: "=",
                canDelete: "=",
                form: "="
            }
        };
        return directive;

        function link(scope, element, attrs) {
        }
    }

})();