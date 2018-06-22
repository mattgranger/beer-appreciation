(function () {
    'use strict';

    // Filter name is handy for logging
    var filterId = 'localDate';

    angular.module('beer-appreciation').filter(filterId,
    ['common', localDate]);

    function localDate(common) {

        return function(input) {
            return moment.utc(input).local().format('DD-MMM-YYYY hh:mm a');
        }

    };
})();