(function () {
    'use strict';
    angular.module('controldriveApp')
        .controller('CarteraController', function ($scope, FechaSvc, $uibModal, $confirm, toastr, MovimientosSvc, DocumentosSvc, AseguradoraSvc) {
            $scope.$parent.$parent.app.viewName = "Cartera";
            $scope.isSaving = false;
            $scope.fechaInicial = FechaSvc.AdicionarMes(-1);
            $scope.fechaFinal = FechaSvc.ObtenerActual();
            $scope.valorFactura = 0;

            $scope.ObtenerDocumentos = function () {
                DocumentosSvc.ObtenerDocumentosCliente($scope.fechaInicial, $scope.fechaFinal, $scope.cliente.Id)
                    .then(function (response) {
                        $scope.documentos = response.data;
                    }, function (response) {
                        toastr.error(response.data.ExceptionMessage);
                    });
            }

            $scope.ObtenerAseguradoras = function () {
                AseguradoraSvc.Obtener()
                    .then(function (response) {
                        $scope.aseguradoras = response.data;
                    })
            };
            $scope.ObtenerAseguradoras();


            $scope.Seleccionar = function (seleccion) {
                angular.forEach($scope.movimientos, function (movimiento) {
                    movimiento.Seleccionado = seleccion;
                });
            }
        });
})();