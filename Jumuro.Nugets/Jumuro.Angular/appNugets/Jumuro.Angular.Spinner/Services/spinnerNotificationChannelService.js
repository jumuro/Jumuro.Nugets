'use strict';

angular.module('jumuro.spinner')
//Service Constants
.constant('spinnerNotificationChannelConstants', {
    events: {
        _START_REPEAT_: '_START_REPEAT_',
        _END_REPEAT_: '_END_REPEAT_'

    }
})
.service('spinnerNotificationChannelService', spinnerNotificationChannelService);

spinnerNotificationChannelService.$inject = ['spinnerNotificationChannelConstants', '$rootScope'];

function notificationChannelSpinnerService(spinnerNotificationChannelConstants, $rootScope) {
    //broadcast event
    var repeatStarted = function (repeatID) {
        $rootScope.$broadcast(spinnerNotificationChannelConstants.events._START_REPEAT_, repeatID);
    };

    //broadcast event
    var repeatEnded = function (repeatID) {
        $rootScope.$broadcast(spinnerNotificationChannelConstants.events._END_REPEAT_, repeatID);
    };

    //map event to your handler
    var onRepeatStarted = function ($scope, handler) {
        $scope.$on(spinnerNotificationChannelConstants.events._START_REPEAT_, function (event, repeatID) {
            handler(repeatID);
        });
    };

    //map event to your handler
    var onRepeatEnded = function ($scope, handler) {
        $scope.$on(spinnerNotificationChannelConstants.events._END_REPEAT_, function (event, repeatID) {
            handler(repeatID);
        });
    };

    return {
        repeatStarted: repeatStarted,
        repeatEnded: repeatEnded,
        onRepeatStarted: onRepeatStarted,
        onRepeatEnded: onRepeatEnded
    };
};