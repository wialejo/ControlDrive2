(function () {
    'use strict';
    angular.module('controldriveApp')
        .controller('GeneracionCuentasPorPagarController', function ($scope, FechaSvc, toastr, DocumentosSvc, ConductorSvc, MovimientosSvc) {
            $scope.$parent.$parent.app.viewName = "Pago a proveedores";
            $scope.servicio = {};
            $scope.movimientos = [];
            $scope.isSaving = false;
            $scope.proveedor = {};
            $scope.fechaInicial = FechaSvc.AdicionarMes(-1);
            $scope.fechaFinal = FechaSvc.ObtenerActual();

            $scope.ObtenerMovimientos = function () {
                MovimientosSvc.ObtenerMovimientosProveedor($scope.fechaInicial, $scope.fechaFinal, $scope.servicio.proveedor.Id)
                    .then(function (response) {
                        $scope.movimientos = response.data;
                    }, function (response) {
                        toastr.error(response.data.ExceptionMessage);
                    });
            }

            $scope.$watch('movimientos', function () {
                $scope.Calcular();
            }, true);
            $scope.GenerarCuentaPorPagar = function () {
                toastr.success("Se generará cuenta por pagar");
            }

            $scope.GenerarCuentaPorPagar = function () {
                var movimientosSeleccionados = [];
                var valorDocumento = 0;
                angular.forEach($scope.movimientos, function (movimiento) {
                    if (movimiento.Seleccionado) {
                        valorDocumento = valorDocumento + parseFloat(movimiento.Valor)
                        movimientosSeleccionados.push(movimiento);
                    };
                });

                var documento = {};
                if (movimientosSeleccionados.length == 0) {
                    toastr.warning("Debe seleccionar al menos un concepto.");
                    return;
                }

                documento.Tipo = "CP";
                documento.Numero = "";
                documento.ProveedorId = $scope.servicio.proveedor.Id;
                documento.movimientos = movimientosSeleccionados;

                DocumentosSvc.Guardar(documento)
                    .then(function () {
                        toastr.success("Generado correctamente.")
                        $scope.ObtenerMovimientos();
                    })
                    .catch(function (error) {
                        toastr.error(error.data.ExceptionMessage)
                    });
            }

            $scope.Calcular = function () {
                var valorTotalAPagar = 0;
                angular.forEach($scope.movimientos, function (servicio) {
                    if (servicio.Seleccionado) {
                        valorTotalAPagar = valorTotalAPagar + parseFloat(servicio.Valor);
                    }
                });
                $scope.valorTotalAPagar = valorTotalAPagar;
            }

            $scope.Seleccionar = function (seleccion) {
                angular.forEach($scope.movimientos, function (servicio) {
                    servicio.Seleccionado = seleccion;
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

        });
})();