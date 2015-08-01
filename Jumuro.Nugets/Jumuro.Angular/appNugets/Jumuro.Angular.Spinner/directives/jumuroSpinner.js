'use strict';

angular.module('jumuro.spinner')
.directive('jumuroSpinner', ['httpInterceptorNotificationChannelService', '$rootScope', '$timeout', '$http',
    function (httpInterceptorNotificationChannelService, $rootScope, $timeout, $http) {
    return {
        restrict: "AE",
        templateUrl: 'appNugets/Jumuro.Angular.Spinner/templates/spinnerTemplate.html?=v1',
        link: function (scope, element) {
            //hide the element initially taking into account the pending requests.
            if ($http.pendingRequests.length < 1 ) {
                element.hide();
            }

            var startRequestHandler = function (request) {
                //console.log('startRequestHandler');
                //check headers in order to know if we must show the spinner.
                if (request.ignoreSpinner == undefined || !request.ignoreSpinner)
                {
                    //console.log('Show spinner');
                    // got the request start notification, show the element
                    element.show();
                }
            };

            var endRequestHandler = function () {
                //console.log('endRequestHandler');
                $rootScope.$evalAsync(function () {
                    //console.log('hide endRequestHandler');
                    // got the request end notification and hide the element taking into account the angular processing.
                    element.hide();
                });
                
            };

            httpInterceptorNotificationChannelService.onRequestStarted(scope, startRequestHandler);

            httpInterceptorNotificationChannelService.onRequestEnded(scope, endRequestHandler);
        }
    };
}]);