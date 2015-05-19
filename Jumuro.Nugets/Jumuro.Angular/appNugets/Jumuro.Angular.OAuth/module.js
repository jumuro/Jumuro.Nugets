angular
    .module('jumuro.oAuth', ['jumuro.notificationChannel', 'ipCookie', 'toaster'])
    .config(config);

config.$inject = ['$routeProvider', '$httpProvider', 'oAuthConstants'];

function config($routeProvider, $httpProvider, oAuthConstants) {
    $httpProvider.interceptors.push('oAuthHttpInterceptor');
    // Set appPathName deleting page file name if it exists in current window.location.pathname
    oAuthConstants.appPathName = window.location.pathname.substr(0, window.location.pathname.lastIndexOf("/") + 1);
    oAuthConstants.oAuthCookieName = ('AppInfo_' + window.location.host + oAuthConstants.appPathName).replace(/[:\/]/g, '_');
}