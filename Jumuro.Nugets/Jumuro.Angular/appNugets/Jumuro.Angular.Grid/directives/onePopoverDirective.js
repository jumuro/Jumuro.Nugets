(function () {
    'use strict';

    angular.module('jumuro.grid')
        .directive('jumuroOnepopover', jumuroOnepopover);

    jumuroOnepopover.$inject = ['$document', '$rootScope', '$timeout', '$popover'];

    function jumuroOnepopover($document, $rootScope, $timeout, $popover) {
        var directive = {
            restrict: 'EA',
            link: link
        };

        return directive;

        function link(scope, element, attrs) {
            var $element = $(element);
            $element.click(function () {
                $('.popover').each(function () {
                    var $this = $(this);
                    if ($this.parent().find('button').attr('id') != $element.parent().attr('id')) {
                        $this.scope().$hide();
                    }
                });
            });
        }
    }
})();