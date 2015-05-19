'use strict';

angular
    .module('jumuro.oAuth')
    .factory('oAuthHttpInterceptor', oAuthHttpInterceptor);

oAuthHttpInterceptor.$inject = ['$q', '$injector', 'ipCookie', 'oAuthConstants'];

function oAuthHttpInterceptor($q, $injector, ipCookie, oAuthConstants) {
    var authInterceptorServiceFactory = {};

    var _request = function (config) {
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

        return config;
    };

    var _responseError = function (rejection) {
        if (rejection.status === 401) {
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
        else {
            return $q.reject(rejection);
        }
    };

    authInterceptorServiceFactory.request = _request;
    authInterceptorServiceFactory.responseError = _responseError;

    return authInterceptorServiceFactory;
}