(function () {
    'use strict';
    angular.module('controldriveApp')
        .controller('CierreController', function ($scope, toastr, PeriodoSvc, $confirm, ServicioSvc, MovimientosSvc, SeguimientoSvc, EstadoSvc, FechaSvc) {
            $scope.$parent.$parent.app.viewName = "Cierre de servicios";
            $scope.servicio = {};
            $scope.servicios = [];
            $scope.isSaving = false;
            $scope.movimientos = [
                {}
            ]

            function ConstruirMovimientosFijos(servicio) {
                servicio.Movimientos.map(function (movimiento) {
                    if (movimiento.ConceptoCodigo == "CLIENTE_COND_ELE")
                        servicio.valorCliente = movimiento.Valor;
                    if (movimiento.ConceptoCodigo == "PROVE_COND_ELE")
                        servicio.valorProveedorConductor = movimiento.Valor;
                    if (movimiento.ConceptoCodigo == "PROVE_RUTA_COND_ELE")
                        servicio.valorProveedorRuta = movimiento.Valor;
                })
                return servicio;
            }

            function ObtenerMovimiento(servicio, concepto) {
                var movimientoEncontrado = {};

                servicio.Movimientos.filter(function (m) {
                    if (m.ConceptoCodigo == concepto) {
                        movimientoEncontrado = m;
                        return;
                    }
                })

                if (movimientoEncontrado.Id) {
                    return movimientoEncontrado;
                } else {
                    movimientoEncontrado = {
                        Id: 0,
                        ConceptoCodigo: concepto,
                        ServicioId: servicio.Id,
                        ProveedorId: concepto == 'PROVE_COND_ELE' ? servicio.ConductorId : (concepto == 'PROVE_RUTA_COND_ELE' ? servicio.RutaId : null),
                        ClienteId: concepto == 'CLIENTE_COND_ELE' ? servicio.AseguradoraId : null
                    }
                }
                return movimientoEncontrado;
            }

            $scope.GuardarMovimientoCliente = function (servicio) {
                if (!servicio.valorCliente)
                    return;
                var movimientoCliente = ObtenerMovimiento(servicio, 'CLIENTE_COND_ELE');
                movimientoCliente.Valor = parseFloat(servicio.valorCliente);

                MovimientosSvc.Guardar(movimientoCliente)
                    .then(function response() {

                    }).catch(function (error) {
                        toastr.error(error.data.ExceptionMessage);
                    })
            }
            $scope.GuardarMovimientoConductor = function (servicio) {
                if (!servicio.valorProveedorConductor)
                    return;
                var movimientoConductor = ObtenerMovimiento(servicio, 'PROVE_COND_ELE');
                movimientoConductor.Valor = servicio.valorProveedorConductor;

                MovimientosSvc.Guardar(movimientoConductor)
                    .then(function response() {

                    }).catch(function (error) {
                        toastr.error(error.message);
                    })
            }
            $scope.GuardarMovimientoRuta = function (servicio) {
                if (!servicio.valorProveedorRuta)
                    return;
                var movimientoRuta = ObtenerMovimiento(servicio, 'PROVE_RUTA_COND_ELE');
                movimientoRuta.Valor = servicio.valorProveedorRuta;

                MovimientosSvc.Guardar(movimientoRuta)
                    .then(function response() {

                    }).catch(function (error) {
                        toastr.error(error.message);
                    })
            }

            $scope.servicios = [].concat($scope.servicios);
            $scope.periodo = PeriodoSvc.ObtenerPeriodoActual();

            $scope.Cerrar = function () {
                var serviciosACerrar = [];
                var valido = true;
                angular.forEach($scope.servicios, function (servicio) {
                    if (servicio.Seleccionado && valido) {
                        if (!servicio.valorCliente)
                            valido = false;
                        if (!servicio.valorProveedorConductor)
                            valido = false;
                        if (!servicio.valorProveedorRuta)
                            valido = false;

                        if (!valido) {
                            toastr.error("Se deben completar los valores del servicio: " + servicio.Id);
                            return;
                        } else {
                            var nuevoServicio = {
                                Id: servicio.Id,
                                EstadoCodigo: servicio.EstadoCodigo
                            }
                            serviciosACerrar.push(nuevoServicio)
                        }
                    }

                });
                
                if (!valido) 
                    return;

                if (serviciosACerrar.length == 0) {
                    toastr.warning("Seleccione los servicios que desea cerrar");
                    return;
                }
                $confirm({ text: "¿Esta seguro de cerrar los servicios seleccionados?", title: "Cierre de servicio" }).then(function () {

                    $scope.isSaving = true;
                    ServicioSvc.Cerrar(serviciosACerrar).then(function () {
                        toastr.success("Cerrado correctamente.")
                        $scope.ObtenerServicios();
                        $scope.isSaving = false;
                    }).catch(function (error) {
                        toastr.error(error.message)
                        $scope.isSaving = false;
                    })
                })
            }

            $scope.GuardarValores = function (servicio) {
                if (servicio.valores) {
                    ServicioSvc.GuardarValores(servicio.Id, servicio.valores).then(function () {
                    }).catch(function (error) {
                        toastr.error(error.message)
                    });
                }
            }

            $scope.ObtenerServicios = function () {
                ServicioSvc.ObtenerTerminados($scope.periodo)
                    .then(function (response) {
                        $scope.servicios = response.data.map(function (servicio) {
                            return ConstruirMovimientosFijos(servicio);
                        });
                    })
                    .catch(function (response) {
                        toastr.error(response.data.ExceptionMessage);
                    });
            }

            $scope.ObtenerServicios();

            $scope.Seleccionar = function (seleccion) {
                angular.forEach($scope.servicios, function (servicio) {
                    servicio.Seleccionado = seleccion;
                });
            }
        });
})();