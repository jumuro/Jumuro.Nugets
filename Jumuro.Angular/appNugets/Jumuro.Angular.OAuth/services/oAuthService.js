'use strict';

angular
    .module('jumuro.oAuth')
    .service('oAuthService', oAuthService);

oAuthService.$inject = ['$http', '$q', '$injector', 'ipCookie', 'oAuthConstants', 'oAuthAppConfigConstants', '$location'];

function oAuthService($http, $q, $injector, ipCookie, oAuthConstants, oAuthAppConfigConstants, $location) {
    //    var toaster = $injector.get('toaster');

    var service = {
        refreshToken: refreshToken,
        logOut: logOut,
        logIn: logIn,
        hasCookie: hasCookie,
        getUserInfo: getUserInfo,
        isAuthenticated: isAuthenticated
    };

    return service;

    function refreshToken() {
        var deferred = $q.defer();

        //get the cookie
        var authData = ipCookie(oAuthConstants.oAuthCookieName);

        if (authData) {
            var data = "grant_type=refresh_token&refresh_token=" + authData.refresh_token + "&client_id=" + authData.client_id;

            $http
                .post(oAuthAppConfigConstants.appConfig.oAuthURL, data, {
                    headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
                })
                .success(function (response) {
                    ipCookie(oAuthConstants.oAuthCookieName, response, { path: oAuthConstants.appPathName });
                    deferred.resolve(response);
                })
                .error(function (err, status) {
                    logOut();
                    deferred.reject(err);
                });
        }

        return deferred.promise;
    };

    function logIn(postData) {
        var deferred = $q.defer();

        var data = "grant_type=password&username=" + postData.username + "&password=" + postData.password + "&client_id=" + oAuthAppConfigConstants.appConfig.oAuthClientId;

        $http
            .post(oAuthAppConfigConstants.appConfig.oAuthURL, data, {
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
            })
            .success(function (data, status, headers, config) {
                // Create cookie. TODO: we have to return user info
                ipCookie(oAuthConstants.oAuthCookieName, data, { path: oAuthConstants.appPathName });
                //Provisionally store user in a local storage until we return the user info from the oAuth api.
                localStorage.setItem("login-info", JSON.stringify({ username: postData.username }));

                deferred.resolve(data);
            })
            .error(function (data, status, headers, config) {
                deferred.reject(data);
            });

        return deferred.promise;
    };

    function getUserInfo() {
        var userInfo = JSON.parse(localStorage.getItem("login-info"));

        return userInfo;
    }

    function hasCookie() {
        return ipCookie(oAuthConstants.oAuthCookieName);
    }

    function logOut() {
        // Delete current cookie if it already exsits
        ipCookie.remove(oAuthConstants.oAuthCookieName, { path: oAuthConstants.appPathName });

        localStorage.removeItem("login-info");

        $location.path('/login');
    };

    function isAuthenticated() {
        var ok = ipCookie(oAuthConstants.oAuthCookieName);
        //TODO: We may check some sort of route property like anonymous for static content routes.
        return ok;
    };
}
