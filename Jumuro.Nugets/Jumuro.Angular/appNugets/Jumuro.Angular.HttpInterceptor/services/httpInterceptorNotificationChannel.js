'use strict';

angular
    .module('jumuro.httpInterceptor')
    //Service Constants
    .constant('httpInterceptorNotificationChannelConstants', {
        events: {
            _START_REPEAT_: '_START_REQUEST_',
            _END_REPEAT_: '_END_REQUEST_'

        }
    })
    .service('httpInterceptorNotificationChannelService', httpInterceptorNotificationChannelService);

httpInterceptorNotificationChannelService.$inject = ['httpInterceptorNotificationChannelConstants', '$rootScope'] ;

function httpInterceptorNotificationChannelService(httpInterceptorNotificationChannelConstants, $rootScope) {
    //broadcast event
    var requestStarted = function (request) {
        $rootScope.$broadcast(httpInterceptorNotificationChannelConstants.events._START_REQUEST_, request);
    };
    //broadcast event
    var requestEnded = function (response) {
        $rootScope.$broadcast(httpInterceptorNotificationChannelConstants.events._END_REQUEST_, response);
    };

    //map event to your handler
    var onRequestStarted = function ($scope, handler) {
        $scope.$on(httpInterceptorNotificationChannelConstants.events._START_REQUEST_, function (event, request) {
            handler(request);
        });
    };
    //map event to your handler
    var onRequestEnded = function ($scope, handler) {
        $scope.$on(httpInterceptorNotificationChannelConstants.events._END_REQUEST_, function (event, response) {
            handler(response);
        });
    };

    return {
        requestStarted: requestStarted,
        requestEnded: requestEnded,
        onRequestStarted: onRequestStarted,
        onRequestEnded: onRequestEnded
    };
}]);