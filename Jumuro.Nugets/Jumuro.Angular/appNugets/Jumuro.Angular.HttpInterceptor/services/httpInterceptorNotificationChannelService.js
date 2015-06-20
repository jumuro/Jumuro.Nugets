(function () {
    'use strict';

    angular.module('jumuro.httpInterceptor')
        .factory('httpInterceptorNotificationChannelService', httpInterceptorNotificationChannelService);

    httpInterceptorNotificationChannelService.$inject = ['httpInterceptorNotificationChannelConstants', '$rootScope'];

    function httpInterceptorNotificationChannelService(httpInterceptorNotificationChannelConstants, $rootScope) {
        var service = {
            requestStarted: requestStarted,
            requestEnded: requestEnded,
            onRequestStarted: onRequestStarted,
            onRequestEnded: onRequestEnded
        }

        return service;
        ///////////////

        //broadcast event
        function requestStarted(request) {
            $rootScope.$broadcast(httpInterceptorNotificationChannelConstants.events._START_REQUEST_, request);
        }

        //broadcast event
        function requestEnded(response) {
            $rootScope.$broadcast(httpInterceptorNotificationChannelConstants.events._END_REQUEST_, response);
        }

        //map event to your handler
        function onRequestStarted($scope, handler) {
            $scope.$on(httpInterceptorNotificationChannelConstants.events._START_REQUEST_, function (event, request) {
                handler(request);
            });
        }

        //map event to your handler
        function onRequestEnded($scope, handler) {
            $scope.$on(httpInterceptorNotificationChannelConstants.events._END_REQUEST_, function (event, response) {
                handler(response);
            });
        }
    }
})();