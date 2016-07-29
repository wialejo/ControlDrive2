(function () {
    'use strict';

    angular
      .module('controldriveApp')
          .controller('NuevoServicioController', function ($scope, $filter, $state, $confirm, PeriodoSvc, TiposServicioSvc, EstadoSvc, $stateParams, FechaSvc, toastr, ServicioSvc, AseguradoraSvc, localStorageService, ConductorSvc, CiudadSvc) {

              $scope.servicio = {};
              $scope.tiposServicio = []
              $scope.esEdicion = false;
              $scope.conductores = [];
              $scope.ciudades = [];
              $scope.rutas = [];
              $scope.aseguradoras = [];

              $scope.Inicio = function () {
                  $scope.servicio.id = $stateParams.id;
                  if ($scope.servicio.id) {
                      $scope.esEdicion = true;
                      EdicionServicio();
                  } else {
                      $scope.esEdicion = false;
                      NuevoServicio();
                  }
              }
              var watchServicio;

              function EdicionServicio() {

                  $scope.ObtenerEstados();

                  ServicioSvc.ObtenerPorId($scope.servicio.id)
                      .then(function (response) {
                          var servicio = response.data;
                          $scope.$parent.$parent.app.viewName = "Edición de servicio: " + servicio.Id
                          CargarServicio(servicio);
                      })
                      .catch(function (err) {
                          toastr.error(err, 'Edición de servicio');
                      })
                  if (watchServicio) {
                      watchServicio();
                      watchServicio = undefined;
                  }
              }

              function NuevoServicio() {
                  $scope.$parent.$parent.app.viewName = "Registro de nuevo servicio";

                  var servicioEnAlmacen = localStorageService.get('servicio');

                  $scope.servicio = servicioEnAlmacen ? servicioEnAlmacen : {
                      TipoServicio: $scope.tiposServicio[0]
                  };

                  if (!$scope.servicio.FechaD)
                      $scope.servicio.FechaD = FechaSvc.ObtenerActual();

                  watchServicio = $scope.$watch('servicio', function () {
                      localStorageService.add('servicio', $scope.servicio);
                  }, true);
              }

              function CargarServicio(servicio) {
                  servicio.Hora = $filter('date')(servicio.Fecha, 'HH:mm');
                  servicio.FechaD = $filter('date')(servicio.Fecha, 'dd/MM/yyyy');
                  $scope.servicio = servicio;
              }

              $scope.isSaving = false;
              $scope.Guardar = function (servicio) {
                  $scope.isSaving = true;

                  if (servicio.TipoServicio.Id != 2) {
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

                  } else {
                      var d = servicio.FechaD.split("/").slice(0, 3).reverse();
                      var fecha = new Date(d[1] + "/" + d[2] + "/" + d[0])
                      servicio.Fecha = $filter('date')(fecha, 'yyyy-MM-dd HH:mm:ss');
                      Guardar(servicio);
                  }
              }

              function Guardar(servicio) {
                  ServicioSvc.Guardar(servicio)
                      .then(function (response) {
                          $scope.isSaving = false;
                          if ($scope.esEdicion) {
                              toastr.success('Servicio actualizado correctamente.');
                              $state.go($state.current, { id: response.data.Id }, { notify: false })
                              $stateParams.id = response.data.Id;
                              $scope.Inicio();
                          } else {
                              toastr.success('Servicio guardado correctamente.');
                              $state.go($state.current, { id: response.data.Id }, { notify: false })
                              $stateParams.id = response.data.Id;
                              localStorageService.remove('servicio');
                              $scope.Inicio();
                          }
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
              $scope.ObtenerFechaSiguienteDia = function (hora) {

                  if (!$scope.esEdicion && hora) {
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
              $scope.ObtenerTiposServicio = function () {
                  TiposServicioSvc.Obtener()
                      .then(function (response) {
                          $scope.tiposServicio = response.data;
                          $scope.Inicio();
                      })
                      .catch(function (response) {
                          toastr.error(response.data.ExceptionMessage);
                      });
              }


              $scope.ObtenerTiposServicio();
              $scope.ObtenerAseguradoras();
              $scope.ObtenerCiudades();
             
          });
})();
