'use strict';

angular.module('jumuro.httpInterceptor')
    .service('httpInterceptorService', httpInterceptorService);

httpInterceptorService.$inject = ['$q', '$injector', 'toaster', 'httpInterceptorNotificationChannelService'];

function httpInterceptorService($q, $injector, toaster, httpInterceptorNotificationChannelService) {
    var $http;

    return {
        // optional method
        'request': function (config) {
            // do something on success

            // send notification requests are complete
            httpInterceptorNotificationChannelService.requestStarted(config);

            return config;
        },

        // optional method
        'requestError': function (rejection) {
            // do something on error

            // send notification requests are complete
            httpInterceptorNotificationChannelService.requestEnded(rejection);

            return $q.reject(rejection);
        },

        // optional method
        'response': function (response) {
            // do something on success

            // get $http via $injector because of circular dependency problem
            $http = $http || $injector.get('$http');

            // don't send notification until all requests are complete
            if ($http.pendingRequests.length < 1) {
                // send notification requests are complete
                httpInterceptorNotificationChannelService.requestEnded(response);
            }

            if (response != undefined)
            {
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
        },

        // optional method
        'responseError': function (rejection) {
            // get $http via $injector because of circular dependency problem
            $http = $http || $injector.get('$http');
            // don't send notification until all requests are complete
            if ($http.pendingRequests.length < 1) {
                // send notification requests are complete
                httpInterceptorNotificationChannelService.requestEnded(rejection);
            }

            return $q.reject(rejection);
        }
    };
}