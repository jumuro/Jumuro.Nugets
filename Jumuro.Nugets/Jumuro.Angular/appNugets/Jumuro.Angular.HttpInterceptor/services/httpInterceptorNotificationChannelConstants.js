(function () {
    'use strict';

    angular.module('jumuro.httpInterceptor')
        .constant('httpInterceptorNotificationChannelConstants', {
            events: {
                _START_REQUEST_: '_START_REQUEST_',
                _END_REQUEST_: '_END_REQUEST_'
            }
        });
})();