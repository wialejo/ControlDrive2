(function() {
'use strict';

  angular.module('controldriveApp')
    .controller('SeguimientoController', ['$scope', '$filter', '$http', '$modal', 
            '$state','toastr', 'ServicioSvc', 'SeguimientoSvc', 
            function ($scope, $filter, $http, $modal, $state, toastr, ServicioSvc, SeguimientoSvc) {
            
            $scope.servicio = {};
            $scope.servicios = [];
            $scope.seguimiento = {};

            $scope.servicios = [].concat($scope.servicios);

            var date = new Date();
            var mes = (date.getMonth() + 1);
            mes = mes < 10 ? "0" + mes.toString() : mes;
            var dia = date.getDate();
            dia = dia < 10 ? "0" + dia.toString() : dia;
            var fechaActual = (dia + '/' + mes + '/' + date.getFullYear());
            $scope.periodo = fechaActual;
            //$scope.periodo = "23/11/2015";
            
            $scope.VerSeguimientos = function (servicio) {
                $scope.servicio = servicio;
                $scope.showEditRow(servicio)
            }

            $scope.isSaving = false;
            $scope.ActualizarServicio = function () {
                ServicioSvc.Guardar($scope.servicio)
                    .then(function (response) {
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
                    .then(function (response) {
                        $scope.seguimiento = {};
                        $scope.ObtenerSeguimientos();
                        $scope.isSaving = false;
                    }, function (response) {
                        $scope.isSaving = false;
                        toastr.error(response.data.ExceptionMessage);
                    });
            }

            $scope.ObtenerSeguimientos = function () {
                var idServicio = parseInt($scope.servicio.Id);
                SeguimientoSvc.ObtenerPorServicio(idServicio)
                    .then(function (response) {
                          $scope.servicio.Seguimientos = response.data;
                    }, function (response) {
                        toastr.error(response.data.ExceptionMessage);
                    });
            }

            $scope.ObtenerEstados = function () {
                $http.get(ApiUrl + "api/estados").
                      then(function (response) {
                          $scope.estados = response.data;
                      }, function (response) {
                          toastr.error(response.data.ExceptionMessage);
                      });
            }

            $scope.ObtenerServicios = function () {
                var d = $scope.periodo.split("/").slice(0, 3).reverse();
                var fecha = new Date(d[1] + "/" + d[2] + "/" + d[0])
                var Fecha = {
                    Inicio: $filter('date')(fecha, 'yyyy-MM-dd HH:mm:ss')
                }

                ServicioSvc.ObtenerPorPeriodo(Fecha)
                    .then(function(response){
                        $scope.servicios = response.data;
                    })
                    .catch(function (response){
                        toastr.error(response.data.ExceptionMessage);
                    });
            }();
            

            $scope.MostrarDetalles = function (servicio, event) {
                
                var modalInstance = $modal.open({
                    templateUrl: 'detalleServicio.html',
                    size: 'md',
                    controller: function ($scope, $modalInstance, $http, servicio) {
                        $scope.servicio = servicio;
                        $scope.cancel = function () {
                            $modalInstance.dismiss('cancel');
                        };
                        $scope.Editar = function (servicio) {
                            $scope.cancel();

                            $scope.servicio = servicio;
                            $scope.servicio.Hora = $filter('date')(servicio.Fecha, 'HH:mm');
                            $scope.servicio.FechaD = $filter('date')(servicio.Fecha, 'dd/MM/yyyy');
                            $stateParams = servicio;
                            $state.go('app.editar', { servicio: servicio })
                        }
                    },
                    resolve: {
                        servicio: function () {
                            return servicio;
                        }
                    }
                });
            }

            $scope.EnviarCorreoConductor = function () {
                var serviciosSeleccionados = [];
                angular.forEach($scope.serviciosActivos, function (servicio) {
                    if (servicio.Seleccionado && servicio.Conductor)
                        serviciosSeleccionados.push(servicio);
                });
                
                if (serviciosSeleccionados.length > 0) {
                    var urlApiServicios = ApiUrl + "/api/servicios/EnviarCorreoSeguimiento";
                    $http.post(urlApiServicios, serviciosSeleccionados)
                        .then(function (response) {
                            toastr.success('Correo enviado correctamente.');
                        }, function (response) {
                            toastr.error(response.data.ExceptionMessage);
                        });
                }
                else {
                    toastr.warning('El servicio seleccionado no tiene asignado un conductor.');
                }
            }

            $scope.EnviarCorreoRuta = function () {
                var serviciosSeleccionados = [];
                angular.forEach($scope.serviciosActivos, function (servicio) {
                    if (servicio.Seleccionado && servicio.Ruta)
                        serviciosSeleccionados.push(servicio);
                });
                if (serviciosSeleccionados.length > 0) {
                    var urlApiServicios = ApiUrl + "/api/servicios/EnviarCorreoRutaSeguimiento";
                    $http.post(urlApiServicios, serviciosSeleccionados).then(function (response) {
                        toastr.success('Correo enviado a ruta correctamente.');
                    }, function (response) {
                        toastr.error(response.data.ExceptionMessage);
                    });
                } else {
                    toastr.warning('El servicio seleccionado no tiene asignada una ruta.');
                }
            }

            $scope.Seleccionar = function (seleccion) {
                angular.forEach($scope.servicios, function (servicio) {
                    servicio.Seleccionado = seleccion;
                });
            }

            $scope.showEditRow = function (r) {
                if ($scope.active != r) {
                    $scope.active = r;
                }
                else {
                    $scope.active = null;
                }
            };
            
        }]);

})();