﻿(function() {
'use strict';

  angular
    .module('controldriveApp')
        .controller('EditarServicioController', function ($scope, $filter, $state, $stateParams, toastr, ServicioSvc, AseguradoraSvc, ConductorSvc, CiudadSvc) {

            $scope.servicio = {};
            $scope.conductores = [];
            $scope.ciudades = [];
            $scope.rutas = [];
            $scope.aseguradoras = [];

            $scope.Inicio = function () {
                var idServicio = $stateParams.id;
                if (idServicio) {
                    ServicioSvc.ObtenerPorId(idServicio)
                        .then(function(response){
                            var servicio = response.data;
                            $scope.$parent.$parent.app.viewName = "Edición de servicio: " + servicio.Id
                            CargarServicio(servicio);
                        })
                        .catch(function(err){
                            toastr.error(err, 'Edición de servicio');    
                        })
                }
            }

            function CargarServicio(servicio){
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

                ServicioSvc.Guardar(servicio)
                    .then(function(response){
                        $scope.isSaving = false;
                        toastr.success('Servicio actualizado correctamente.');
                        CargarServicio(response.data);
                    })
                    .catch(function(response){
                        $scope.isSaving = false;
                        toastr.error(response.data.ExceptionMessage);
                    });
            }

            $scope.ObtenerAseguradoras = function(){
                AseguradoraSvc.Obtener()
                    .then(function(response){
                        $scope.aseguradoras = response.data;
                    })
            }();

            $scope.ObtenerCiudades = function () {
                CiudadSvc.Obtener()
                    .then(function (response) {
                        $scope.ciudades = response.data;
                    })
            }()

            $scope.ObtenerConductores = function () {
                ConductorSvc.Obtener()
                    .then(function (response) {
                        $scope.conductores = response.data;
                    })
            }()

            $scope.Inicio()
        });
})();