(function () {
    'use strict';
    angular.module('controldriveApp')
        .controller('FacturacionController', function ($scope, FechaSvc, $uibModal, $confirm, toastr, MovimientosSvc, DocumentosSvc, AseguradoraSvc) {
            $scope.$parent.$parent.app.viewName = "Facturación";
            $scope.isSaving = false;
            $scope.fechaInicial = FechaSvc.AdicionarMes(-1);
            $scope.fechaFinal = FechaSvc.ObtenerActual();
            $scope.valorFactura = 0;

            $scope.ObtenerMovimientos = function () {
                MovimientosSvc.ObtenerMovimientosClienteAprobados($scope.fechaInicial, $scope.fechaFinal, $scope.cliente.Id)
                    .then(function (response) {
                        $scope.movimientos = response.data;
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

            $scope.$watch('movimientos', function () {
                $scope.Calcular();
            }, true);

            $scope.Calcular = function () {
                var valorFactura = 0;
                angular.forEach($scope.movimientos, function (movimiento) {
                    if (movimiento.Seleccionado) {
                        valorFactura = valorFactura + parseFloat(movimiento.Valor)
                    };
                });
                $scope.valorFactura = valorFactura;
            }

            $scope.MostrarFacturacionMasiva = function () {
                if ($scope.valorFactura == 0) {
                    toastr.warning("Debe seleccionar algún item.")
                    return;
                }

                $uibModal.open({
                    templateUrl: 'MostrarFacturacionMasiva.html',
                    size: 'md',
                    controller: function ($scope, $uibModalInstance, scope) {
                        $scope.valorFactura = scope.valorFactura;
                        $scope.movimientos = scope.movimientos;

                        $scope.cancel = function () {
                            $uibModalInstance.dismiss('cancel');
                        };
                        $scope.FacturarMasivo = function () {
                            scope.FacturarMasivo($scope.numeroFactura);
                            $scope.cancel();
                        };
                    },
                    resolve: {
                        scope: function () {
                            return $scope;
                        }
                    }
                });
            }

            $scope.Facturar = function (movimiento) {
                var documento = {};
                if (!movimiento.NumeroDocumento) {
                    toastr.warning("Debe asignar un número de factura");
                    return;
                }

                documento.Numero = movimiento.NumeroDocumento;
                documento.ClienteId = $scope.cliente.Id;
                documento.movimientos = [];
                documento.movimientos.push(movimiento);
                DocumentosSvc.Guardar(documento)
                    .then(function () {
                        toastr.success("Facturado correctamente.")
                        $scope.ObtenerMovimientos();
                    })
                    .catch(function (error) {
                        toastr.error(error.data.ExceptionMessage)
                    });
            }

            $scope.FacturarMasivo = function (numeroDocumento) {
                var movimientosSeleccionados = [];
                var valorFactura = 0;
                angular.forEach($scope.movimientos, function (movimiento) {
                    if (movimiento.Seleccionado) {
                        valorFactura = valorFactura + parseFloat(movimiento.Valor)
                        movimientosSeleccionados.push(movimiento);
                    };
                });

                var documento = {};
                if (!numeroDocumento) {
                    toastr.warning("Debe asignar un número de factura");
                    return;
                }

                documento.Numero = numeroDocumento;
                documento.ClienteId = $scope.cliente.Id;
                documento.movimientos = movimientosSeleccionados;

                DocumentosSvc.Guardar(documento)
                    .then(function () {
                        toastr.success("Facturado correctamente.")
                        $scope.ObtenerMovimientos();
                    })
                    .catch(function (error) {
                        toastr.error(error.data.ExceptionMessage)
                    });
            }

            $scope.Seleccionar = function (seleccion) {
                angular.forEach($scope.movimientos, function (movimiento) {
                    movimiento.Seleccionado = seleccion;
                });
            }
        });
})();