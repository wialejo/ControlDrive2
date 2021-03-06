(function () {
    'use strict';
    angular.module('controldriveApp')
        .controller('SeguimientoController', function ($scope, $filter, $window, $uibModal, $state, toastr, TiposServicioSvc, PeriodoSvc, ServicioSvc, SeguimientoSvc, EstadoSvc, FechaSvc) {
            $scope.$parent.$parent.app.viewName = "Seguimiénto";
            $scope.servicio = {};
            $scope.servicios = [];
            $scope.seguimiento = {};
            $scope.search = {
            }
            $scope.isSaving = false;
            $scope.verHerramientas = true;

            $scope.MostrarHerramientas = function () {
                if ($scope.verHerramientas) {
                    $scope.verHerramientas = false;
                } else {
                    $scope.verHerramientas = true;
                }
            }
            $scope.servicios = [].concat($scope.servicios);
            $scope.periodo = PeriodoSvc.ObtenerPeriodoActual();
            $scope.VerSeguimientos = function (servicio) {
                $scope.servicio = servicio;
                if ($scope.active != servicio) {
                    $scope.ObtenerSeguimientos(servicio);
                    $scope.active = servicio;
                }
                else {
                    angular.forEach($scope.servicios, function (servicio) {
                        servicio.Seguimientos = null;
                    });
                    $scope.active = null;
                }
            }
            $scope.ActualizarServicio = function () {
                ServicioSvc.Guardar($scope.servicio)
                    .then(function () {
                        toastr.success('Servicio actualizado correctamente.');
                    }, function (response) {
                        toastr.error(response.data.ExceptionMessage);
                    });
            }
            $scope.GuardarSeguimiento = function () {
                $scope.isSaving = true;
                if (!$scope.seguimiento) {
                    $scope.seguimiento = {};
                }
                $scope.seguimiento.ServicioId = $scope.servicio.Id;
                $scope.seguimiento.NuevoEstado = $scope.servicio.EstadoCodigo;

                SeguimientoSvc.Guardar($scope.seguimiento)
                    .then(function () {
                        $scope.seguimiento = {};
                        toastr.success('Seguimiento guardado correctamente.');
                        $scope.ObtenerServicios();
                        $scope.isSaving = false;
                    }, function (response) {
                        $scope.isSaving = false;
                        toastr.error(response.data.ExceptionMessage);
                    });
            }

            $scope.ObtenerSeguimientos = function (servicio) {
                //var idServicio = parseInt($scope.servicio.Id);
                SeguimientoSvc.ObtenerPorServicio(servicio.Id)
                    .then(function (response) {
                        servicio.Seguimientos = response.data;
                    }, function (response) {
                        toastr.error(response.data.ExceptionMessage);
                    });
            }

            $scope.ObtenerTiposServicio = function () {
                TiposServicioSvc.Obtener()
                    .then(function (response) {
                        var tiposServicios = response.data;
                        angular.forEach(tiposServicios, function (tipoServicio) {
                            tipoServicio.Mostrar = true;
                        });

                        $scope.tiposServicio = tiposServicios;
                    })
                    .catch(function (response) {
                        toastr.error(response.data.ExceptionMessage);
                    });
            }
            $scope.ObtenerTiposServicio();

            $scope.TiposServicioVisiblesFilter = function (servicio) {
                var existe = 0;
                $scope.tiposServicio.filter(function (tipoServicio) {
                    if (tipoServicio.Mostrar) {
                        if (tipoServicio.Id == servicio.TipoServicio.Id) {
                            existe++;
                            return;
                        }
                    }
                });
                if (existe == 1)
                    return servicio;
            }


            $scope.ObtenerEstados = function () {
                EstadoSvc.Obtener()
                    .then(function (response) {

                        $scope.estados = response.data.filter(function (estado) {
                            estado.Mostrar = true;
                            if (estado.EnOperacion == true) {
                                return estado;
                            }

                        });
                        $scope.estadosParaRegistro = response.data.filter(function (estado) {
                            if (estado.Codigo != "CR" && estado.Codigo != "CF" && estado.Codigo != "FA") {
                                return estado;
                            }
                        });
                    })
                    .catch(function (response) {
                        toastr.error(response.data.ExceptionMessage);
                    });
            }
            $scope.ObtenerEstados();

            $scope.EstadosVisibles = function (servicio) {
                var existe = 0;
                $scope.estados.filter(function (estado) {
                    if (estado.Mostrar) {
                        if (estado.Codigo == servicio.Estado.Codigo) {
                            existe++;
                            return;
                        }
                    }
                })
                if (existe == 1)
                    return servicio;
            }

            $scope.ObtenerServicios = function () {
                ServicioSvc.ObtenerParaSeguimiento($scope.periodo)
                    .then(function (response) {
                        var estadoServicios = {};
                        $scope.servicios = response.data.filter(function (servicio) {
                            if (servicio.Estado.EnOperacion == true) {
                                estadoServicios[servicio.Estado.Descripcion] = estadoServicios[servicio.Estado.Descripcion] ? estadoServicios[servicio.Estado.Descripcion] + 1 : 1
                                return servicio.EstadoCodigo;
                            }
                        });
                        $scope.estadoServiciosActivos = estadoServicios;

                        ////$scope.servicios = ;
                        ////$scope.filtrarServicios(response.data);

                        //var estadoServicios = {};
                        //response.data.filter(function (servicio) {
                        //    if (servicio.Estado.EnOperacion == true) {
                        //        estadoServicios[servicio.Estado.Descripcion] = estadoServicios[servicio.Estado.Descripcion] ? estadoServicios[servicio.Estado.Descripcion] + 1 : 1
                        //        return servicio.EstadoCodigo;
                        //    }
                        //});
                        //$scope.estadoServiciosActivos = estadoServicios;
                    })
                    .catch(function (response) {
                        toastr.error(response.data.ExceptionMessage);
                    });
            }
            $scope.ObtenerServicios();

            $scope.Editar = function (servicio) {
                //$scope.cancel();
                //var url = $state.href('app.editar', { id: servicio.Id })
                //$window.open(url, '_blank');
                $state.go('app.editar', { id: servicio.Id })
            }

            $scope.ExportarCSV = function () {
                ServicioSvc.ObtenerPorPeriodoCSV($scope.periodo);
            }

            $scope.MostrarDetalles = function (servicio) {
                $uibModal.open({
                    templateUrl: 'detalleServicio.html',
                    size: 'md',
                    controller: function ($scope, $uibModalInstance, servicio) {
                        $scope.servicio = servicio;
                        $scope.cancel = function () {
                            $uibModalInstance.dismiss('cancel');
                        };
                        $scope.Editar = function (servicio) {
                            $scope.cancel();
                            //var url = $state.href('app.editar', { id: servicio.Id })
                            //$window.open(url,'_blank');
                            $state.go('app.editar', { id: servicio.Id })
                        }
                    },
                    resolve: {
                        servicio: function () {
                            return servicio;
                        }
                    }
                });
            }

            $scope.EnviarCorreoConductor = function (impresion) {
                $scope.isSaving = true;
                var serviciosSeleccionados = [];
                angular.forEach($scope.serviciosActivos, function (servicio) {
                    if (servicio.Seleccionado && servicio.ConductorResumen)
                        serviciosSeleccionados.push(servicio);
                });
                if (serviciosSeleccionados.length > 0) {
                    ServicioSvc.NotificarServiciosAConductor(serviciosSeleccionados, impresion)
                        .then(function (response) {
                            if (impresion) {
                                popUp(response.data)
                            } else {
                                toastr.success(response.data);
                                //$scope.ObtenerServicios();
                            }
                            $scope.isSaving = false;
                        })
                        .catch(function (response) {
                            toastr.error(response.data.ExceptionMessage);
                            $scope.isSaving = false;
                        })
                }
                else {
                    toastr.warning('El servicio seleccionado no tiene asignado un conductor.');
                    $scope.isSaving = false;
                }
            }

            $scope.EnviarCorreoRuta = function (impresion) {
                $scope.isSaving = true;
                var serviciosSeleccionados = [];
                angular.forEach($scope.serviciosActivos, function (servicio) {
                    if (servicio.Seleccionado && servicio.RutaResumen)
                        serviciosSeleccionados.push(servicio);
                });
                if (serviciosSeleccionados.length > 0) {
                    ServicioSvc.NotificarServiciosARuta(serviciosSeleccionados, impresion)
                        .then(function (response) {
                            if (impresion) {
                                popUp(response.data)
                            } else {
                                toastr.success(response.data);
                                //$scope.ObtenerServicios();
                            }
                            $scope.isSaving = false;
                        })
                        .catch(function (response) {
                            toastr.error(response.data.ExceptionMessage);
                            $scope.isSaving = false;
                        })
                } else {
                    toastr.warning('El servicio seleccionado no tiene asignada una ruta.');
                    $scope.isSaving = false;
                }
            }

            $scope.Seleccionar = function (seleccion) {
                angular.forEach($scope.servicios, function (servicio) {
                    servicio.Seleccionado = seleccion;
                });
            }

            function popUp(html) {

                var frog = window.open("", "wildebeast", "width=800,height=600,scrollbars=1,resizable=1")

                frog.document.open()
                frog.document.write(html)
                frog.document.close()
            }
        });
})();