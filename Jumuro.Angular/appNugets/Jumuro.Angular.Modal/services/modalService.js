﻿(function () {
    /// <reference path="../views/modalConfirm.html" />
    'use strict';

    angular.module('jumuro.modal')
        .factory('modalService', modalService);

    modalService.$inject = ['$modal'];

    function modalService($modal) {
        return {
            modal: modal
        }

        function modal(modalOptions) {
            var _templateUrl; // = './appNugets/Jumuro.Angular.Modal/templates/modalNotification.html';

            if (modalOptions.modalType == 'confirm') {
                _templateUrl = './appNugets/Jumuro.Angular.Modal/templates/modalConfirmation.html';
            }
            else if (modalOptions.modalType == 'error') {
                _templateUrl = './appNugets/Jumuro.Angular.Modal/templates/modalError.html?v1';
            }
            else { //if (modalOptions.modalType == 'notification') {
                _templateUrl = './appNugets/Jumuro.Angular.Modal/templates/modalNotification.html';
            }

            var modalInstance = $modal.open({
                templateUrl: _templateUrl,
                controller: 'ModalInstanceController',
                resolve: {
                    modalScope: function () {
                        return modalOptions;
                    }
                }
            });

            return modalInstance.result;
        }
    }
})();