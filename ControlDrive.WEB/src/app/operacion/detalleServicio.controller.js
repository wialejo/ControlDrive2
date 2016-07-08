(function () {
    'use strict';
    angular
      .module('controldriveApp')
          .controller('DetalleServicioController', function ($scope, ServicioSvc, $uibModalInstance, $state, servicio) {
              ServicioSvc.ObtenerPorId(servicio.Id).then(function (response) {
                  $scope.servicio = response.data;
              })

              
              $scope.cancel = function () {
                  $uibModalInstance.dismiss('cancel');
              };
              $scope.Editar = function (servicio) {
                  $scope.cancel();
                  //var url = $state.href('app.editar', { id: servicio.Id })
                  //$window.open(url,'_blank');
                  $state.go('app.editar', { id: servicio.Id })
              }
          })
})();