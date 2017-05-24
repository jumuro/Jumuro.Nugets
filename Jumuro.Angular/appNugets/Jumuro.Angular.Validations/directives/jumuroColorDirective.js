(function () {
    'use strict';
    angular.module('jumuro.validations')

    .directive('jumuroColor', jumuroColor);

    function jumuroColor() {
        return {
            require: 'ngModel',
            scope: {
                colorList: "&jumuroColorlist",
                colorObject: "@jumuroColorobject"
            },
            link: function (scope, elm, attrs, ctrl) {
                ctrl.$parsers.unshift(function (viewValue) {
                    var isValidColor = true;
                    var colorList = scope.colorList();
                    var colorObject = scope.colorObject;
                    for (var i = 0; i < colorList.length; i++) {
                        if (colorList[i].color) {
                            if (colorList[i].color == viewValue) {
                                isValidColor = false;
                                break;
                            }
                        }
                        else if (colorList[i][colorObject].color) {
                            if (colorList[i][colorObject].color == viewValue) {
                                isValidColor = false;
                                break;
                            }
                        }

                    }

                    ctrl.$setValidity('color', isValidColor);

                    return viewValue;
                });
            }
        };
    }
})();