(function () {
    'use strict';
    angular.module('controldriveApp')
        .controller('GeneracionCuentasPorPagarController', function ($scope, FechaSvc, toastr, ConductorSvc) {
            $scope.$parent.$parent.app.viewName = "Pago a proveedores";
            $scope.servicio = {};
            $scope.servicios = [];
            $scope.isSaving = false;
            $scope.proveedor = {};
            $scope.fechaInicial = FechaSvc.AdicionarMes(-1);
            $scope.fechaFinal = FechaSvc.ObtenerActual();

            $scope.Guardar = function (servicio) {
                if (confirm("¿Esta seguro de facturar el servicio?")) {
                    $scope.isSaving = true;
                    ServicioSvc.Facturar(servicio.Id, servicio.NoFactura).then(function () {
                        toastr.success("Cerrado correctamente.")
                        $scope.ObtenerServicios();
                    }).catch(function (error) {
                        toastr.error(error.message)
                    }).finally(function () {
                        $scope.isSaving = false;
                    });
                }
            }

            $scope.ObtenerServicios = function () {
                ConductorSvc.ObtenerServiciosTerminados($scope.fechaInicial, $scope.fechaFinal, $scope.servicio.proveedor.Id)
                    .then(function (response) {
                        $scope.servicios = response.data;
                    }, function (response) {
                        toastr.error(response.data.ExceptionMessage);
                    });
            }

            $scope.$watch('servicios', function () {
                $scope.Calcular();
            }, true);
            $scope.GenerarCuentaPorPagar = function () {
                toastr.success("Se generará cuenta por pagar");
            }

            $scope.Calcular = function () {
                var valorTotalAPagar = 0;
                angular.forEach($scope.servicios, function (servicio) {
                    if (servicio.Seleccionado) {
                        valorTotalAPagar = valorTotalAPagar + parseFloat(servicio.Valor);
                    }
                });
                $scope.valorTotalAPagar = valorTotalAPagar;
            }

            $scope.Seleccionar = function (seleccion) {
                angular.forEach($scope.servicios, function (servicio) {
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