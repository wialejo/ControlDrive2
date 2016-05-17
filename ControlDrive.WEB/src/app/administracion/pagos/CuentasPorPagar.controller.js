(function () {
    'use strict';
    angular.module('controldriveApp')
        .controller('CuentasPorPagarController', function ($scope, FechaSvc, $uibModal, $confirm, toastr, ConductorSvc, MovimientosSvc, DocumentosSvc) {
            $scope.$parent.$parent.app.viewName = "Cuentas por pagar"
            $scope.isSaving = false;
            $scope.servicio = {};
            $scope.fechaInicial = FechaSvc.AdicionarMes(-1);
            $scope.fechaFinal = FechaSvc.ObtenerActual();

            $scope.ObtenerDocumentos = function () {
                DocumentosSvc.ObtenerDocumentosProveedor($scope.fechaInicial, $scope.fechaFinal, $scope.servicio.proveedor.Id)
                    .then(function (response) {
                        $scope.documentos = response.data;
                    }, function (response) {
                        toastr.error(response.data.ExceptionMessage);
                    });
            }

            $scope.ObtenerProveedores = function (filtro) {
                if (filtro) {
                    ConductorSvc.ObtenerPorDescripcion(filtro)
                        .then(function (response) {
                            $scope.conductores = response.data;
                        })
                } else {
                    $scope.conductores = [];
                }
            }

            $scope.Seleccionar = function (seleccion) {
                angular.forEach($scope.documentos, function (documento) {
                    documento.Seleccionado = seleccion;
                });
            }

            $scope.MostrarServicios = function (documento) {
                var documentos = [];
                documentos.push(documento);

                DocumentosSvc.ObtenerDocumentosRelacionServicios(documentos)
                    .then(function (response) {
                        toastr.success("Generado correctamente.")
                        $scope.documentoServicios = response.data;
                        AbrirNuevaVentanaHtmlElemento('ResumenServicios');
                    }, function (response) {
                        toastr.error(response.data.ExceptionMessage);
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
                      $scope.documentosRelacionServicios = response.data;
                      AbrirNuevaVentanaHtmlElemento('RelacionServicios');
                  }, function (response) {
                      toastr.error(response.data.ExceptionMessage);
                  });
            }

            var AbrirNuevaVentanaHtmlElemento = function (elemento) {
                setTimeout(function () {
                    var innerContents = document.getElementById(elemento).innerHTML;
                    var popupWinindow = window.open('Relacion', '_blank', 'width=800,height=500,scrollbars=yes,menubar=no,toolbar=no,location=no,status=no,titlebar=no');
                    popupWinindow.document.open();
                    popupWinindow.document.write('<html><body onload="window.print()">' + innerContents + '</html>');
                    popupWinindow.document.close();

                }, 500)

            }
        });
})();