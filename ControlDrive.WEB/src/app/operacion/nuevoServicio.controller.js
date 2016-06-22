(function () {
    'use strict';

    angular
      .module('controldriveApp')
          .controller('NuevoServicioController', function ($scope, PeriodoSvc, AseguradoraSvc, $confirm, FechaSvc, localStorageService, $filter, $state, toastr, ServicioSvc, ConductorSvc, CiudadSvc) {

              $scope.$parent.$parent.app.viewName = "Registro de nuevo servicio";

              $scope.tiposServicio = [{
                  Id: 1,
                  Nombre: "Conductor elegido"
              }, {
                  Id: 2,
                  Nombre: "Coordinación"
              }, {
                  Id: 3,
                  Nombre: "Valet"
              }, {
                  Id: 4,
                  Nombre: "Mensajeria"
              }, {
                  Id: 5,
                  Nombre: "Transporte ejecutivo"
              }]

              $scope.esEdicion = false;
              $scope.conductores = [];
              $scope.ciudades = [];
              $scope.rutas = [];
              $scope.aseguradoras = [];

              $scope.Inicio = function () {
                  var servicioEnAlmacen = localStorageService.get('servicio');
                  $scope.servicio = servicioEnAlmacen ? servicioEnAlmacen : {
                      Tipo: {
                          Id: 4,
                          Nombre: "Valet"
                      }
                  };

                  if (!$scope.servicio.FechaD)
                      $scope.servicio.FechaD = FechaSvc.ObtenerActual();
              }

              $scope.$watch('servicio', function () {
                  localStorageService.add('servicio', $scope.servicio);
              }, true);

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
                        
                        $scope.servicio = { Tipo: servicio.Tipo };
                        localStorageService.remove('servicio');
                        toastr.success('Servicio guardado correctamente.');

                        $state.go('app.editar', { id: response.data.Id })
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
              $scope.ObtenerAseguradoras();

              $scope.ObtenerCiudades = function () {
                  CiudadSvc.Obtener()
                      .then(function (response) {
                          $scope.ciudades = response.data;
                      })
              }
              $scope.ObtenerCiudades();

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

              $scope.ObtenerFechaSiguienteDia = function (hora) {
                  var bits = hora.split(':');
                  var horas = bits[0];

                  var date = new Date();
                  var dateactual = new Date().getHours();
                  if (horas < 6 && dateactual > 6) {
                      $scope.CambioFecha = true;
                      date.setDate(date.getDate() + 1);
                  } else {
                      $scope.CambioFecha = false;
                  }
                  var mes = (date.getMonth() + 1);
                  mes = mes < 10 ? "0" + mes.toString() : mes;
                  var dia = date.getDate();
                  dia = dia < 10 ? "0" + dia.toString() : dia;

                  var fechaSiguienteDia = (dia + '/' + mes + '/' + date.getFullYear());

                  $scope.servicio.FechaD = fechaSiguienteDia;
              }

              $scope.Inicio();
          });
})();
