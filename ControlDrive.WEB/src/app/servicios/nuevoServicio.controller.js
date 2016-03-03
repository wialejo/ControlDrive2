(function() {
'use strict';

  angular
    .module('controldriveApp')
        .controller('NuevoServicioController', function ($scope, AseguradoraSvc, FechaSvc, localStorageService, $filter, $state, toastr, ServicioSvc, ConductorSvc, CiudadSvc) {

            $scope.$parent.$parent.app.viewName = "Registro de nuevo servicio";
            $scope.servicio = {};
            $scope.esEdicion = false;
            $scope.conductores = [];
            $scope.ciudades = [];
            $scope.rutas = [];
            $scope.aseguradoras = [];

            $scope.Inicio = function () {
                var servicioEnAlmacen = localStorageService.get('servicio');
                $scope.servicio = servicioEnAlmacen ? servicioEnAlmacen: {};
                
                if(!$scope.servicio.FechaD)
                    $scope.servicio.FechaD = FechaSvc.ObtenerActual();

            }

            $scope.$watch('servicio', function() {
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
                ServicioSvc.Guardar(servicio)
                    .then(function(response){
                        $scope.isSaving = false;
                        $scope.servicio = {};
                        localStorageService.remove('servicio');
                        toastr.success('Servicio guardado correctamente.');
                        
                        $state.go('app.editar', { id: response.data.Id })
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

            $scope.ObtenerFechaSiguienteDia = function (hora) {
                var bits = hora.split(':');
                var horas = bits[0];

                var date = new Date();
                if(horas < 6){
                    $scope.CambioFecha = true ;
                    date.setDate(date.getDate() + 1);
                }else{
                    $scope.CambioFecha = false ;
                }
                var mes = (date.getMonth() + 1);
                mes = mes < 10 ? "0" + mes.toString() : mes;
                var dia = date.getDate();
                dia = dia < 10 ? "0" + dia.toString() : dia;

                var fechaSiguienteDia =  (dia + '/' + mes + '/' + date.getFullYear()); 
                
                $scope.servicio.FechaD = fechaSiguienteDia;
            }

            $scope.Inicio();
            });
})();
