/// <reference path="../views/modalConfirm.html" />
'use strict';

angular.module('jumuro.modal')
    .service('modalService', modalService);

modalService.$inject = ['$modal'];

function modalService($modal) {
    var modal = function (modalOptions) {

        var _templateUrl = './appNugets/Jumuro.Angular.Modal/templates/modalNotification.html';

        if (modalOptions.modalType == 'confirm') {
            _templateUrl = './appNugets/Jumuro.Angular.Modal/templates/modalConfirm.html';
        }
        else if (modalOptions.modalType == 'error') {
            _templateUrl = './appNugets/Jumuro.Angular.Modal/templates/modalError.html?v1';
        }
        else if (modalOptions.modalType == 'notification') {
            _templateUrl = './appNugets/Jumuro.Angular.Modal/templates/modalNotification.html';
        }

        var modalInstance = $modal.open({
            templateUrl: _templateUrl,
            controller: 'modalInstanceCtrl',
            resolve: {
                modalScope: function () {
                    return modalOptions;
                }
            }
        });

        return modalInstance.result;
    };

    return {
        modal: function (modalOptions) {
            return modal(modalOptions);
        }
    }
}