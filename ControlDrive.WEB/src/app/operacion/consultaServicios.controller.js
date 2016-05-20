(function () {
    'use strict';
    angular
      .module('controldriveApp')
          .controller('ConsultaServiciosController', function (ServicioSvc, $window, $uibModal, $filter, $state, $scope, FechaSvc, toastr) {

              var vm = this;
              vm.servicio = {};
              vm.servicios = [];
              $scope.$parent.$parent.app.viewName = "Consultar de servicios"

              vm.fechaInicial = new Date();// FechaSvc.AdicionarMes(-1);
              vm.fechaFinal = new Date();  //FechaSvc.ObtenerActual();

              vm.ObtenerServicios = function (tableState) {

                  ServicioSvc.Obtener(FechaSvc.Formatear(vm.fechaInicial), FechaSvc.Formatear(vm.fechaFinal))
                      .then(function (response) {
                          vm.servicios = response.data;

                      }, function (response) {
                          toastr.error(response.data.ExceptionMessage);

                      });
              }

              vm.Editar = function (servicio) {
                  //var url = $state.href('app.editar', { id: servicio.Id })
                  //$window.open(url,'_blank');
                  $state.go('app.editar', { id: servicio.Id })
              }

              vm.MostrarDetalles = function (servicio) {
                  $uibModal.open({
                      templateUrl: 'app/operacion/detalleServicio.html',
                      size: 'lg',
                      controller: 'DetalleServicioController',
                      resolve: {
                          servicio: function () {
                              return servicio;
                          }
                      }
                  });
              }


          });

    angular
   .module('controldriveApp')
        .directive('stSummary', function () {
        return {
            restrict: 'E',
            require: '^stTable',
            template: '{{size}}',
            scope: {},
            link: function (scope, element, attr, ctrl) {
                scope.$watch(ctrl.getFilteredCollection, function (val) {
                    scope.size = (val || []).length;
                })
            }
        }
    });

})();