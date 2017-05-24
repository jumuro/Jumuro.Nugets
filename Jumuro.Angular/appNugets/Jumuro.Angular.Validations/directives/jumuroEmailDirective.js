(function () {
    'use strict';
    angular.module('jumuro.validations')

    .directive('jumuroEmail', jumuroEmail);

    function jumuroEmail() {
        var EMAIL_REGEXP = /[-0-9a-zA-Z.+_]+@[-0-9a-zA-Z.+_]+\.[a-zA-Z]{2,4}/;
        return {
            require: 'ngModel',
            link: function (scope, elm, attrs, ctrl) {
                ctrl.$parsers.unshift(function (viewValue) {
                    if (viewValue == '' || EMAIL_REGEXP.test(viewValue)) {
                        // it is valid
                        ctrl.$setValidity('email', true);

                        return viewValue;
                    } else {
                        // it is invalid, return undefined (no model update)
                        ctrl.$setValidity('email', false);

                        return viewValue;
                    }
                });
            }
        };
    }
})();