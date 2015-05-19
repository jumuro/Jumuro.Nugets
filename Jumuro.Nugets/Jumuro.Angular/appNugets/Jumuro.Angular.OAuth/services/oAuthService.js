'use strict';

angular
    .module('jumuro.oAuth')
    .service('oAuthService', oAuthService);

oAuthService.$inject = ['$http', '$q', '$injector', 'ipCookie', 'oAuthConstants'];

function oAuthService($http, $q, $injector, ipCookie, oAuthConstants) {
    var refreshToken = function () {

        var urls = JSON.parse(localStorage.getItem('apiUrlList'));
        var oAuthURL = '';

        for (var i = 0; i < urls.length; i++) {
            if (urls[i].UrlName == 'OAuthURL') {
                oAuthURL = urls[i].UrlValue;
                break;
            }
        }

        var deferred = $q.defer();

        //get the cookie
        var authData = ipCookie(oAuthConstants.oAuthCookieName);

        if (authData) {
            var data = "grant_type=refresh_token&refresh_token=" + authData.refresh_token + "&client_id=" + authData.client_id;

            $http.post(oAuthURL, data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {
                //ipCookie.remove(oAuthConstants.oAuthCookieName);
                ipCookie(oAuthConstants.oAuthCookieName, response, { path: oAuthConstants.appPathName });
                deferred.resolve(response);

            }).error(function (err, status) {
                logOut();
                deferred.reject(err);
            });
        }

        return deferred.promise;
    };

    var logOut = function () {
        // Delete current cookie if it already exsits
        ipCookie.remove(oAuthConstants.oAuthCookieName, { path: oAuthConstants.appPathName });

        // Ahora mismo no se va a ver el toaster ya que al cambiar href se va a servidor.
        // Estudiar el ciclo de vida de la aplicación para que se haga todo en cliente

        var toaster = $injector.get('toaster');
        toaster.pop('error', "Cookie error", "Authorization info not found. Trying to authorize again...");

        window.location.href = oAuthConstants.appPathName;
    };

    return {
        refreshToken: function () {
            return refreshToken();
        },
        logOut: function () {
            return logOut();
        }
    };
}
