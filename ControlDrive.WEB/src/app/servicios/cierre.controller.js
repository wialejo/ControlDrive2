(function () {
    'use strict';
    angular.module('controldriveApp')
        .controller('CierreController', function ($scope, toastr, PeriodoSvc, $confirm, ServicioSvc, SeguimientoSvc, EstadoSvc, FechaSvc) {
            $scope.$parent.$parent.app.viewName = "Cierre de servicios";
            $scope.servicio = {};
            $scope.servicios = [];
            $scope.isSaving = false;

            $scope.servicios = [].concat($scope.servicios);
            $scope.periodo = PeriodoSvc.ObtenerPeriodoActual();

            $scope.Cerrar = function (servicio) {
                $confirm({text:"¿Esta seguro de registrar los valores asignados y cerrar el servicio?", title:"Cierre de servicio"}).then(function () {
                    $scope.isSaving = true;
                    ServicioSvc.Cerrar(servicio.Id, servicio.valores).then(function () {
                        toastr.success("Cerrado correctamente.")
                        $scope.ObtenerServicios();
                        $scope.isSaving = false;
                    }).catch(function (error) {
                        toastr.error(error.message)
                        $scope.isSaving = false;
                    })
                })
            }

            $scope.GuardarValores = function (servicio) {
                if (servicio.valores) {
                    ServicioSvc.GuardarValores(servicio.Id, servicio.valores).then(function () {
                    }).catch(function (error) {
                        toastr.error(error.message)
                    });
                }
            }

            $scope.ObtenerServicios = function () {
                ServicioSvc.ObtenerParaCierre($scope.periodo)
                    .then(function (response) {
                        $scope.servicios = response.data;
                    })
                    .catch(function (response) {
                        toastr.error(response.data.ExceptionMessage);
                    });
            }

            $scope.ObtenerServicios();
        });
})();