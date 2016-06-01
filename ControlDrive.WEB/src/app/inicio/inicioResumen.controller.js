(function () {
    'use strict';
    angular.module('controldriveApp')
        .controller('InicioController', function ($scope, toastr, ServicioSvc, FechaSvc) {

            var vm = this;
            vm.servicio = {};
            vm.servicios = [];
            $scope.$parent.$parent.app.viewName = "Resumen"

            vm.fechaInicial = "01/05/2016 14:00";
            vm.fechaFinal = FechaSvc.AdicionarMes(1);


            ServicioSvc.ObtenerResumenEstado(vm.fechaInicial, vm.fechaFinal)
                      .then(function (response) {
                          vm.resumen = response.data;

                      }, function (response) {
                          toastr.error(response.data.ExceptionMessage);

                      });

        });
})();