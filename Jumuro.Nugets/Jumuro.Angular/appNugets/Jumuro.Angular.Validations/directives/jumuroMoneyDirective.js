(function () {
    'use strict';

    angular.module('jumuro.validations')

    .directive('jumuroMoney', function () {

        var NUMBER_REGEXP = /^\s*(\-|\+)?(\d+|(\d*(\.\d*)))\s*$/;

        function link(scope, el, attrs, ngModelCtrl) {

            var min = parseFloat(scope.$eval(attrs.min) || 0);
            var max = parseFloat(scope.$eval(attrs.min) || 0);
            var scale = parseFloat(scope.$eval(attrs.jumuroMoneyScale) || 10);
            var precision = parseFloat(scope.$eval(attrs.jumuroMoneyPrecision) || 2);
            var lastValidValue;

            function round(num) {
                var d = Math.pow(10, precision);
                return Math.round(num * d) / d;
            }

            function formatPrecision(value) {
                if (value.toString().split(".")[0].length > scale) {
                    var decimalpart = "";
                    for (i = 0; i < precision; i++) {
                        decimalpart += "9";
                    }
                    value = parseFloat((value - 1).toString() + "." + decimalpart);
                }
                return parseFloat(value).toFixed(precision);
            }

            function formatViewValue(value) {
                return ngModelCtrl.$isEmpty(value) ? '' : '' + value;
            }


            ngModelCtrl.$parsers.push(function (value) {
                if (angular.isUndefined(value)) {
                    value = '';
                }

                // Handle leading decimal point, like ".5"
                if (value.indexOf('.') === 0) {
                    value = '0' + value;
                }

                // Allow "-" inputs only when min < 0
                if (value.indexOf('-') === 0) {
                    if (min >= 0) {
                        value = null;
                        ngModelCtrl.$setViewValue('');
                        ngModelCtrl.$render();
                    } else if (value === '-') {
                        value = '';
                    }
                }

                var empty = ngModelCtrl.$isEmpty(value);
                var isValidValue = true;
                if (empty || NUMBER_REGEXP.test(value)) {
                    if (value.split(".")[0].length <= scale) {
                        if (value.split(".").length > 1 && value.split(".")[1].length > precision) {
                            isValidValue = false;
                        }
                    }
                    else {
                        isValidValue = false;
                    }
                }
                else {
                    isValidValue = false;
                }

                if (isValidValue) {
                    lastValidValue = (value === '') ? null : (empty ? value : parseFloat(value));
                }
                else {
                    // Render the last valid input in the field
                    ngModelCtrl.$setViewValue(formatViewValue(lastValidValue));
                    ngModelCtrl.$render();
                }

                ngModelCtrl.$setValidity('money', true);
                if (lastValidValue != null) {
                    if (lastValidValue.toString().split(".").length > 1 && lastValidValue.toString().split(".")[1].length > precision) {
                        lastValidValue = parseFloat(lastValidValue.toString().split(".")[0] + "." + lastValidValue.toString().split(".")[1].substring(0, precision));
                    }
                    return formatPrecision(lastValidValue);
                } else {
                    return null;
                }
            });
            ngModelCtrl.$formatters.push(formatViewValue);

            var minValidator = function (value) {
                if (!ngModelCtrl.$isEmpty(value) && value < min) {
                    ngModelCtrl.$setValidity('moneyMinValue', false);
                    return undefined;
                } else {
                    ngModelCtrl.$setValidity('moneyMinValue', true);
                    return value;
                }
            };
            ngModelCtrl.$parsers.push(minValidator);
            ngModelCtrl.$formatters.push(minValidator);

            if (max) {
                var max = parseFloat(max);
                var maxValidator = function (value) {
                    if (!ngModelCtrl.$isEmpty(value) && value > max) {
                        ngModelCtrl.$setValidity('moneyMaxValue', false);
                        return undefined;
                    } else {
                        ngModelCtrl.$setValidity('moneyMaxValue', true);
                        return value;
                    }
                };

                ngModelCtrl.$parsers.push(maxValidator);
                ngModelCtrl.$formatters.push(maxValidator);
            }

            // Round off
            if (precision > -1) {
                ngModelCtrl.$parsers.push(function (value) {
                    return value ? round(value) : value;
                });
                ngModelCtrl.$formatters.push(function (value) {
                    return (!angular.isUndefined(value) && value != null) ? formatPrecision(value) : value;
                });
            }

            el.bind('blur', function () {
                var value = ngModelCtrl.$modelValue;
                if (!angular.isUndefined(value) && value != null) {
                    ngModelCtrl.$viewValue = formatPrecision(value);
                    ngModelCtrl.$render();
                }
            });
        }

        return {
            restrict: 'A',
            require: 'ngModel',
            link: link
        };
    });
})();