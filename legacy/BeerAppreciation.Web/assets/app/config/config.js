(function () {
    'use strict';

    var app = angular.module('beer-appreciation');

    // Configure Toastr
    toastr.options.timeOut = 8000;
    toastr.options.positionClass = 'toast-bottom-right';

    // keyCodes
    var keyCodes = {
        backspace: 8,
        tab: 9,
        enter: 13,
        esc: 27,
        space: 32,
        pageup: 33,
        pagedown: 34,
        end: 35,
        home: 36,
        left: 37,
        up: 38,
        right: 39,
        down: 40,
        insert: 45,
        del: 46
    };

    var imageSettings = {
        imageBasePath: '/content/images/photos/',
        unknownPersonImageSource: 'unknown_person.jpg'
    };

    var events = {
        controllerActivateSuccess: 'controller.activateSuccess',
        progressBarActivate: 'progress.activate',
        progressBarDeactivate: 'progress.deactivate',
        spinnerToggle: 'spinner.toggle'
    };

    var config = {
        appErrorPrefix: '[BA Error] ', //Configure the exceptionHandler decorator
        docTitle: 'BA : ',
        events: events,
        version: '2.0.0',
        imageSettings: imageSettings,
        keyCodes: keyCodes
    };

    app.value('config', config);


    app.config(['$logProvider', function ($logProvider) {
        // turn debugging off/on (no info or warn)
        if ($logProvider.debugEnabled) {
            $logProvider.debugEnabled(true);
        }
    }]);

    //#region Configure the common services via commonConfig
    app.config(['commonConfigProvider', function (cfg) {
        cfg.config.controllerActivateSuccessEvent = config.events.controllerActivateSuccess;
        cfg.config.spinnerToggleEvent = config.events.spinnerToggle;
        cfg.config.progressBarActivate = config.events.progressBarActivate;
        cfg.config.progressBarDeactivate = config.events.progressBarDeactivate;
        cfg.config.translateLanguage = config.events.translateLanguage;
    }]);
    //#endregion

})();