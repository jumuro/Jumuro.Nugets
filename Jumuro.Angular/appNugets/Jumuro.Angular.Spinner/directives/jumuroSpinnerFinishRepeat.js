angular.module('jumuro.spinner')
.directive('jumuroSpinnerFinishRepeat', jumuroSpinnerFinishRepeatDirective);

jumuroSpinnerFinishRepeatDirective.$inject = ['$timeout', 'spinnerNotificationChannelService'];

function jumuroSpinnerFinishRepeatDirective($timeout, spinnerNotificationChannelService) {
    return {
        restrict: 'A',
        link: function (scope, element, attr) {
            spinnerNotificationChannelService.repeatStarted(attr.jumuroSpinnerFinishRepeat);
            if (scope.$last === true) {
                spinnerNotificationChannelService.repeatEnded(attr.jumuroSpinnerFinishRepeat);
            }
        }
    }
}