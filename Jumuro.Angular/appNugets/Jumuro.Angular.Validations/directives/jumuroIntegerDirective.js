(function () {
    'use strict';

    angular.module('jumuro.validations')

    .directive('jumuroInteger', jumuroInteger);

    function jumuroInteger() {
        var INTEGER_REGEXP = /^\-?\d+$/;
        return {
            require: 'ngModel',
            scope: {
                zero: '@jumuroIntegerZero'
            },
            link: function (scope, elm, attrs, ctrl) {
                var zero = scope.zero;
                //if (zero === undefined)
                //{
                //    zero = 'false';
                //}

                ctrl.$parsers.unshift(function (viewValue) {
                    if (viewValue == '' || (INTEGER_REGEXP.test(viewValue) && viewValue < 999999999)
                        && (zero == 'true' ? (viewValue >= 0) : (viewValue > 0))) {
                        // it is valid
                        ctrl.$setValidity('integer', true);

                        return viewValue;
                    } else {
                        // it is invalid, return undefined (no model update)
                        ctrl.$setValidity('integer', false);

                        return viewValue;
                    }
                });
            }
        };
    }
})();