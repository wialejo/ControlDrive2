(function () {
    'use strict';

    angular
      .module('controldriveApp')
          .controller('EditarServicioController', function ($scope, $filter, $state, $confirm, PeriodoSvc,EstadoSvc, $stateParams, toastr, ServicioSvc, AseguradoraSvc, ConductorSvc, CiudadSvc) {

              $scope.servicio = {};
              $scope.conductores = [];
              $scope.ciudades = [];
              $scope.rutas = [];
              $scope.aseguradoras = [];

              $scope.Inicio = function () {
                  var idServicio = $stateParams.id;
                  if (idServicio) {
                      ServicioSvc.ObtenerPorId(idServicio)
                          .then(function (response) {
                              var servicio = response.data;
                              $scope.$parent.$parent.app.viewName = "Edición de servicio: " + servicio.Id
                              CargarServicio(servicio);
                          })
                          .catch(function (err) {
                              toastr.error(err, 'Edición de servicio');
                          })
                  }
              }

              function CargarServicio(servicio) {
                  servicio.Hora = $filter('date')(servicio.Fecha, 'HH:mm');
                  servicio.FechaD = $filter('date')(servicio.Fecha, 'dd/MM/yyyy');
                  $scope.servicio = servicio;
              }

              $scope.isSaving = false;
              $scope.Guardar = function (servicio) {
                  $scope.isSaving = true;

                  var d = servicio.FechaD.split("/").slice(0, 3).reverse();
                  var fecha = new Date(d[1] + "/" + d[2] + "/" + d[0] + " " + servicio.Hora)
                  if (fecha == "Invalid Date") {
                      toastr.warning('Fecha no valida.');
                      return;
                  }
                  servicio.Fecha = $filter('date')(fecha, 'yyyy-MM-dd HH:mm:ss');
                  PeriodoSvc.FechaEnPeriodoActual(servicio.Fecha)
                       .then(function (response) {
                           if (!response.data) {
                               $confirm({ text: "El servicio se guardará en un periodo diferente, ¿esta seguro de continuar?" }).then(function () {
                                   Guardar(servicio);
                               }).catch(function () {
                                   $scope.isSaving = false;
                               });
                           } else {
                               Guardar(servicio)
                           }
                       })
                       .catch(function (response) {
                           $scope.isSaving = false;
                           toastr.error(response.data.ExceptionMessage);
                       });
              }

              function Guardar(servicio) {
                  ServicioSvc.Guardar(servicio)
                      .then(function (response) {
                          $scope.isSaving = false;
                          toastr.success('Servicio actualizado correctamente.');
                          $state.go('app.editar', { id: response.data.Id }, { reload: true })
                      })
                      .catch(function (response) {
                          $scope.isSaving = false;
                          toastr.error(response.data.ExceptionMessage);
                      });
              }

              $scope.ObtenerAseguradoras = function () {
                  AseguradoraSvc.Obtener()
                      .then(function (response) {
                          $scope.aseguradoras = response.data;
                      })
              };
              $scope.ObtenerCiudades = function () {
                  CiudadSvc.Obtener()
                      .then(function (response) {
                          $scope.ciudades = response.data;
                      })
              }
              $scope.ObtenerConductores = function (filtro) {
                  if (filtro) {
                      ConductorSvc.ObtenerPorDescripcion(filtro)
                          .then(function (response) {
                              $scope.conductores = response.data;
                          })
                  } else {
                      $scope.conductores = [];
                  }
              }
              $scope.ObtenerEstados = function () {
                  EstadoSvc.Obtener()
                      .then(function (response) {
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

              $scope.ObtenerAseguradoras();
              $scope.ObtenerCiudades();
              $scope.ObtenerEstados();
              $scope.Inicio()
          });
})();
