(function () {
    'use strict';
    angular.module('controldriveApp')
        .controller('PrefacturacionController', function ($scope, FechaSvc, $confirm, toastr, MovimientosSvc, AseguradoraSvc) {
            $scope.$parent.$parent.app.viewName = "Prefacturación";
            $scope.isSaving = false;
            $scope.fechaInicial = FechaSvc.AdicionarMes(-1);
            $scope.fechaFinal = FechaSvc.ObtenerActual();
            $scope.ObtenerMovimientos = function () {
                MovimientosSvc.ObtenerMovimientosCliente($scope.fechaInicial, $scope.fechaFinal, $scope.cliente.Id)
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
            $scope.ActualizarMovimiento = function ($index) {
                var movimiento = {};
                angular.copy($scope.movimientos[$index], movimiento);
                
                if (movimiento.Valor) {
                    //$confirm({ text: "¿Esta seguro de actualizar Consecutivo: " + movimiento.Servicio.Radicado + " Valor: " + movimiento.Valor + "?" })
                    //    .then(function () {
                    $scope.isSaving = true;
                    MovimientosSvc.Actualizar(movimiento).then(function () {
                        toastr.success("Actualizado correctamente.")
                        MovimientosSvc.ObtenerPorId(movimiento.Id)
                            .then(function (response) {
                                var movimientoActualizado = response.data;
                                angular.copy(movimientoActualizado, $scope.movimientos[$index])

                                //movimiento = response.data;
                            }).catch(function (error) {
                                toastr.error(error.message)
                            })
                    }).catch(function (error) {
                        toastr.error(error.message)
                    }).finally(function () {
                        $scope.isSaving = false;
                    });
                    //})
                } else {
                    toastr.warning("Debe ingresar un valor para aprobar.")
                }
            }
        });
})();