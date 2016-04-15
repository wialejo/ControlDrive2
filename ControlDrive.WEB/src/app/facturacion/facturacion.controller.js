(function () {
    'use strict';
    angular.module('controldriveApp')
        .controller('FacturacionController', function ($scope, FechaSvc, toastr, ServicioSvc) {
            $scope.$parent.$parent.app.viewName = "Facturación de servicios";
            $scope.servicio = {};
            $scope.servicios = [];
            $scope.isSaving = false;

            $scope.fechaInicial = FechaSvc.AdicionarMes(-1);
            $scope.fechaFinal = FechaSvc.ObtenerActual();

            $scope.servicios = [].concat($scope.servicios);

            $scope.Guardar = function (servicio) {
                if (confirm("¿Esta seguro de facturar el servicio?")) {
                    $scope.isSaving = true;
                    ServicioSvc.Facturar(servicio.Id, servicio.NoFactura).then(function () {
                        toastr.success("Cerrado correctamente.")
                        $scope.ObtenerServicios();
                    }).catch(function (error) {
                        toastr.error(error.message)
                    }).finally(function () {
                        $scope.isSaving = false;
                    });
                }
            }

            $scope.ObtenerServicios = function () {
                ServicioSvc.Obtener($scope.fechaInicial, $scope.fechaFinal, "CR")
                    .then(function (response) {
                        $scope.servicios = response.data;
                    }, function (response) {
                        toastr.error(response.data.ExceptionMessage);
                    });
            }

            $scope.ObtenerServicios();
        });
})();