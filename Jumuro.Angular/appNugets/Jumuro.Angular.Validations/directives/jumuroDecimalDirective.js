(function () {
    'use strict';
    angular.module('jumuro.validations')

    .directive('jumuroDecimal', jumuroDecimal);

    function jumuroDecimal() {
        var DECIMAL_REGEXP = /^\d+(\.\d{1,5})?$/;
        return {
            require: 'ngModel',
            link: function (scope, elm, attrs, ctrl) {
                ctrl.$parsers.unshift(function (viewValue) {
                    if (viewValue == '' || (DECIMAL_REGEXP.test(viewValue) && viewValue >= 0 && viewValue < 999999999)) {
                        // it is valid
                        ctrl.$setValidity('decimal', true);

                        return viewValue;
                    } else {
                        // it is invalid, return undefined (no model update)
                        ctrl.$setValidity('decimal', false);

                        return viewValue;
                    }
                });
            }
        };
    }
})();