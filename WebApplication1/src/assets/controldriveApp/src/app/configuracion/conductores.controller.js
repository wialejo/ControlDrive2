(function() {
'use strict';

  angular.module('controldriveApp')
    .controller('ConductoresController', ['$scope', '$modal', '$log', 'ConductorSvc',
        function ($scope, $modal, $log, ConductorSvc) {
        $scope.esEdicion = false;
        $scope.conductores = [];
        $scope.conductor = {};
        $scope.Obtener = function () {
           ConductorSvc.Obtener()
                .then(function (response) {
                    $scope.conductores = response.data;
                })
                .catch( function (response) {
                    alert(response.data.ExceptionMessage);
                });
        }()

        $scope.eliminar = function (conductor) {

        }
        $scope.editar = function (conductor) {
            $scope.esEdicion = true;
            $scope.conductor = conductor;
            $scope.abrirModalNuevo();
        }
        $scope.nuevo = function () {
            $scope.esEdicion = false;
            $scope.conductor = {};
            $scope.abrirModalNuevo();
        }
        $scope.abrirModalNuevo = function () {
            var modalInstance = $modal.open({
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
    }])



    angular.module('controldriveApp')
      .controller('ModalNuevoConductor', ['$scope', '$modalInstance', 
        function ($scope, $modalInstance) {
        $scope.ok = function () {
            $modalInstance.close();
        };
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
    }])
})();