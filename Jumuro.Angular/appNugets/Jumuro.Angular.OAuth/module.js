angular
    .module('jumuro.oAuth', ['ipCookie', 'toaster'])
    .run(oAuthRun);

oAuthRun.$inject = ['$rootScope', 'oAuthService', '$location', 'toaster'];

function oAuthRun($rootScope, oAuthService, $location, toaster) {
    $rootScope.$on('$routeChangeStart', function (event, next, current) {
        if (!oAuthService.isAuthenticated()) {
            if ($location.path() !== '/login') {
                toaster.pop('error', 'Access Denied', 'Access denied. Plese enter your user and password.')
            }

            $location.path('/login');
        }
    });
}

angular
    .module('jumuro.oAuth')
    .config(config);

config.$inject = ['$routeProvider', '$httpProvider', 'oAuthConstants'];

function config($routeProvider, $httpProvider, oAuthConstants) {
    $httpProvider.interceptors.push('oAuthHttpInterceptor');
    // Set appPathName deleting page file name if it exists in current window.location.pathname
    oAuthConstants.appPathName = window.location.pathname.substr(0, window.location.pathname.lastIndexOf("/") + 1);
    oAuthConstants.oAuthCookieName = ('AppInfo_' + window.location.host + oAuthConstants.appPathName).replace(/[:\/]/g, '_');
}