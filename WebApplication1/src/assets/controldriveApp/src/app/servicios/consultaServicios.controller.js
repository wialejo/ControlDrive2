(function() {
'use strict';

  angular
    .module('controldriveApp')
        .controller('ConsultaServiciosController', ['$scope', 'ServicioSvc', '$filter', '$state', function ($scope, ServicioSvc, $filter, $state) {
        $scope.servicio = {};
        $scope.servicios = [];
        $scope.ObtenerServicios = function () {
            ServicioSvc.Obtener()
                .then(function (response) {
                    $scope.servicios = response.data;
                }, function (response) {
                   //alert(response.data.ExceptionMessage);
                });
        }()
        $scope.Editar = function (servicio) {
                $scope.servicio = servicio;
                $scope.servicio.Hora = $filter('date')(servicio.Fecha, 'HH:mm');
                $scope.servicio.FechaD = $filter('date')(servicio.Fecha, 'dd/MM/yyyy');
                $stateParams = servicio;
                $state.go('app.editar', { servicio: servicio })
        }
    }]);
})();