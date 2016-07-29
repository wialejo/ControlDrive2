(function() {
'use strict';

  angular.module('controldriveApp')
    .controller('ConductoresController', function ($scope, $uibModal, $log, ConductorSvc, toastr) {
        $scope.$parent.$parent.app.viewName = "Conductores";
        $scope.esEdicion = false;
        $scope.conductores = [];
        $scope.conductor = {
        };
        $scope.Obtener = function () {
            
           ConductorSvc.Obtener()
                .then(function (response) {
                    $scope.conductores = response.data;
                })
                .catch( function (response) {
                    toastr.error(response.data.MessageDetail);
                });
        }
        $scope.Obtener();

        $scope.eliminar = function (conductor) {
            if (confirm("¿Esta seguro de elimnar el conductor '" + conductor.Nombre + "'.")) {
                ConductorSvc.Eliminar(conductor.Id)
                    .then(function () {
                        toastr.success("Eliminado");
                        $scope.Obtener();
                    })
                    .catch( function (response) {
                        toastr.error(response.data.ExceptionMessage);
                    });
            }
        }
        $scope.editar = function (conductor) {
            $scope.esEdicion = true;
            $scope.conductor = conductor;
            $scope.abrirModalNuevo();
        }
        $scope.nuevo = function () {
            $scope.esEdicion = false;
            $scope.conductor = {Activo:true };
            
            $scope.abrirModalNuevo();
        }
        $scope.abrirModalNuevo = function () {
            $uibModal.open({
                templateUrl: 'NuevoConductor.html',
                size: 'md',
                controller: function ($scope, $uibModalInstance, conductor, esEdicion, scope, toastr) {
                    $scope.conductor = conductor;
                    $scope.esEdicion = esEdicion;
                    $scope.guardar = function (conductor) {
                        ConductorSvc.Guardar(conductor)
                            .then(function(){
                                toastr.success("Conductor guardado correctamente");
                                $scope.conductor = {};
                                $scope.cancel();
                                scope.Obtener();

                            })
                            .catch(function(response){
                                toastr.error(response.data.ExceptionMessage);
                            })
                    }
                    $scope.cancel = function () {
                        $uibModalInstance.dismiss('cancel');
                    };
                },
                resolve: {
                    conductor: function () {
                        return $scope.conductor;
                    },
                    esEdicion: function () {
                        return $scope.esEdicion;
                    },
                    scope: function () {
                        return $scope;
                    }
                }
            });
        };
    })
})();