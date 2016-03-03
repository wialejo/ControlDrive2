(function() {
'use strict';

  angular.module('controldriveApp')
    .controller('ConductoresController', function ($scope, $modal, $log, ConductorSvc) {
        $scope.$parent.$parent.app.viewName = "Conductores";
        $scope.esEdicion = false;
        $scope.conductores = [];
        $scope.conductor = { };
        $scope.Obtener = function () {
           ConductorSvc.Obtener()
                .then(function (response) {
                    $scope.conductores = response.data;
                })
                .catch( function (response) {
                    alert(response.data.MessageDetail);
                });
        }()

        $scope.eliminar = function (conductor) {
            if(confirm("Se elimnara el conductor.")){
                ConductorSvc.Eliminar(conductor.Id)
                    .then(function () {
                        alert("Eliminado");
                        $scope.Obtener();
                    })
                    .catch( function (response) {
                        alert(response.data.ExceptionMessage);
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
            $modal.open({
                templateUrl: 'NuevoConductor.html',
                size: 'md',
                controller: function ($scope, $modalInstance, conductor, esEdicion, scope) {
                    $scope.conductor = conductor;
                    $scope.esEdicion = esEdicion;
                    $scope.guardar = function (conductor) {
                        ConductorSvc.Guardar(conductor)
                            .then(function(){
                                alert("Guardado");
                                $scope.conductor = {};
                                $scope.cancel();
                                scope.Obtener();

                            })
                            .catch(function(response){
                                alert(response.data.ExceptionMessage);
                            })
                    }
                    $scope.cancel = function () {
                        $modalInstance.dismiss('cancel');
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