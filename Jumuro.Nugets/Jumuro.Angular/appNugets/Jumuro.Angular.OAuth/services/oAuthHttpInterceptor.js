'use strict';

angular
    .module('jumuro.oAuth')
    .factory('oAuthHttpInterceptor', oAuthHttpInterceptor);

oAuthHttpInterceptor.$inject = ['$q', '$injector', 'ipCookie', 'oAuthConstants', 'toaster', '$location'];

function oAuthHttpInterceptor($q, $injector, ipCookie, oAuthConstants, toaster, $location) {
    var service = {
        request: request,
        responseError: responseError
    };

    return service;

    function request(config) {
        if ($location.path() !== '/login') {
            config.headers = config.headers || {};

            //get the cookie
            var authData = ipCookie(oAuthConstants.oAuthCookieName);

            if (authData) {
                config.headers.Authorization = 'Bearer ' + authData.access_token;
            }
            else {
                var authService = $injector.get('oAuthService');
                authService.logOut();
            }
        }

        return config;
    }

    function responseError(rejection) {
        if (rejection.status === 400) {
            if (rejection.data && rejection.data.message) {
                toaster.pop('error', "Error", rejection.data.message);
            }
            else if (rejection.data && rejection.data.error) {
                if (rejection.data.error === 'invalid_grant') {
                    toaster.pop('error', "Error", rejection.data.error_description);
                }
            }
        }
        else if (rejection.status === 401) {
            var authService = $injector.get('oAuthService');
            var authData = ipCookie(oAuthConstants.oAuthCookieName);
            var $http = $http || $injector.get('$http');
            var deferred = $q.defer();

            if (authData) {
                authService.refreshToken().then(function () {
                    //this repeats the request with the original parameters
                    return deferred.resolve($http(rejection.config));
                });
            }
            return deferred.promise;
        }
        else if (rejection.status === 403) {
            var toaster = $injector.get('toaster');
            toaster.pop('error', "Access Denied", "You are not authorized to do this request.");

            //window.location.path = oAuthConstants.appPathName;
        }
        else if (rejection.status === 0 || rejection.status === 500) {
            if (rejection.data && rejection.data.message) {
                toaster.pop('error', "Error", rejection.data.message);
            } else {
                toaster.pop('error', "Error", rejection.data);
            }
        }
        else {
            return $q.reject(rejection);
        }
    }
}