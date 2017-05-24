(function () {
    'use strict';

    // Declares how the application should be bootstrapped. See: http://docs.angularjs.org/guide/module
    //'mgcrea.ngStrap'
    angular.module('jumuro.modal')
        .controller('ModalInstanceController', ModalInstanceController);

    ModalInstanceController.$inject = ['$scope', '$modalInstance', 'modalScope'];

    function ModalInstanceController($scope, $modalInstance, modalScope) {
        $scope.buttons = modalScope.buttons;
        $scope.message = modalScope.message;
        $scope.headerText = modalScope.headerText;
        $scope.details = modalScope.details;

        $scope.ok = function () {
            $modalInstance.close(true);
        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };
    }
})();