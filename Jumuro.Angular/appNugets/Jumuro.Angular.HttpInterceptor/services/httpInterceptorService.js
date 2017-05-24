(function () {
    'use strict';

    angular.module('jumuro.httpInterceptor')
        .factory('httpInterceptorService', httpInterceptorService);

    httpInterceptorService.$inject = ['$q', '$injector', 'toaster', 'httpInterceptorNotificationChannelService'];

    function httpInterceptorService($q, $injector, toaster, httpInterceptorNotificationChannelFactory) {
        var $http;
        var service = {
            request: request,
            requestError: requestError,
            response: response,
            responseError: responseError
        };

        return service;
        ///////////////

        // optional method
        function request(config) {
            // do something on success

            // send notification requests are complete
            httpInterceptorNotificationChannelFactory.requestStarted(config);

            return config;
        }

        // optional method
        function requestError(rejection) {
            // do something on error

            // send notification requests are complete
            httpInterceptorNotificationChannelFactory.requestEnded(rejection);

            return $q.reject(rejection);
        }

        // optional method
        function response(response) {
            // do something on success

            // get $http via $injector because of circular dependency problem
            $http = $http || $injector.get('$http');

            // don't send notification until all requests are complete
            if ($http.pendingRequests.length < 1) {
                // send notification requests are complete
                httpInterceptorNotificationChannelFactory.requestEnded(response);
            }

            if (response != undefined) {
                if (response.status === 201) {
                    var header = response.headers();
                    if (header.message != undefined) {
                        toaster.pop('sucess', "Success", header.message);
                    }
                }
                else if (response.status === 200) {
                    var header = response.headers();
                    if (header.message != undefined) {
                        toaster.pop('sucess', "Success", header.message);
                    }
                }
            }

            return response;
        }

        // optional method
        function responseError(rejection) {
            // get $http via $injector because of circular dependency problem
            $http = $http || $injector.get('$http');
            // don't send notification until all requests are complete
            if ($http.pendingRequests.length < 1) {
                // send notification requests are complete
                httpInterceptorNotificationChannelFactory.requestEnded(rejection);
            }

            return $q.reject(rejection);
        }
    }
})();