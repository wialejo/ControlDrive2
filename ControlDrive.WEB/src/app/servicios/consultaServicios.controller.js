(function() {
'use strict';
  angular
    .module('controldriveApp')
        .controller('ConsultaServiciosController', function (ServicioSvc, $window, $filter, $state, $scope, FechaSvc, toastr) {

        var vm = this;
        vm.servicio = {};
        vm.servicios = [];
         $scope.$parent.$parent.app.viewName = "Consultar de servicios"

        vm.fechaInicial = FechaSvc.AdicionarMes(-1);
        vm.fechaFinal = FechaSvc.ObtenerActual();

        vm.ObtenerServicios = function (tableState) {
         
          ServicioSvc.Obtener(vm.fechaInicial, vm.fechaFinal)
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

    });
})();