(function () {
    'use strict';
    angular.module('controldriveApp')
        .controller('CarteraController', function ($scope, FechaSvc, $uibModal, $confirm, toastr, MovimientosSvc, DocumentosSvc, AseguradoraSvc) {
            $scope.$parent.$parent.app.viewName = "Cartera";
            $scope.isSaving = false;
            $scope.fechaInicial = FechaSvc.AdicionarMes(-1);
            $scope.fechaFinal = FechaSvc.ObtenerActual();

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
                angular.forEach($scope.documentos, function (documento) {
                    documento.Seleccionado = seleccion;
                });
            }

            $scope.ImprimirRelacion = function () {
                var documentosSeleccionados = [];
                angular.forEach($scope.documentos, function (documento) {
                    if (documento.Seleccionado) {
                        documentosSeleccionados.push(documento);
                    }
                });
                if (documentosSeleccionados.length == 0) {
                    toastr.warning("Debe seleccionar al menos un documento.");
                    return;
                }
                DocumentosSvc.ObtenerDocumentosRelacionServicios(documentosSeleccionados)
                  .then(function (response) {
                      toastr.success("Generado correctamente.")
                      MostrarRelacionServicios(response.data);
                  }, function (response) {
                      toastr.error(response.data.ExceptionMessage);
                  });
            }

            var MostrarRelacionServicios = function (documentosRelacionServicios) {
                $scope.documentosRelacionServicios = documentosRelacionServicios;

                setTimeout(function () {
                    var innerContents = document.getElementById('RelacionServicios').innerHTML;
                    var popupWinindow = window.open('', '_blank', 'width=600,height=500,scrollbars=yes,menubar=no,toolbar=no,location=no,status=no,titlebar=no');
                    popupWinindow.document.open();
                    popupWinindow.document.write('<html><body onload="window.print()">' + innerContents + '</html>');
                    popupWinindow.document.close();

                },500)
                
            }
        });
})();