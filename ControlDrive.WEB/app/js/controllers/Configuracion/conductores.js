var urlApiConductores = "http://192.168.0.29/API/api/conductores";
app.controller('ConductoresController', ['$scope', '$modal', '$log', '$http',
    function ($scope, $modal, $log, $http) {
    $scope.esEdicion = false;
    $scope.conductores = [];
    $scope.conductor = {};
    $scope.Obtener = function () {
        $http.get(urlApiConductores).
            then(function (response) {
                $scope.conductores = response.data;
            }, function (response) {
                //alert(response.data.ExceptionMessage);
            });
    }
    $scope.Obtener();
    $scope.eliminar = function (conductor) {

    }
    $scope.editar = function (conductor) {
        $scope.esEdicion = true;
        $scope.conductor = conductor;
        $scope.abrirModalNuevo();
    },
    $scope.nuevo = function () {
        $scope.esEdicion = false;
        $scope.conductor = {};
        $scope.abrirModalNuevo();
    },
    $scope.abrirModalNuevo = function () {
        var modalInstance = $modal.open({
            templateUrl: 'NuevoConductor.html',
            size: 'md',
            controller: function ($scope, $modalInstance, $http, conductor, esEdicion, scope) {
                $scope.conductor = conductor;
                $scope.esEdicion = esEdicion;
                $scope.guardar = function (conductor) {
                    if ($scope.esEdicion) {
                        $http.put(urlApiConductores + "/" + conductor.Id, conductor).
                        then(function (response) {
                            alert("Actualizado");
                            $scope.conductor = {};
                            $scope.cancel();
                            scope.Obtener();

                        }, function (response) {
                          //  alert(response.data.ExceptionMessage);
                        });
                    }
                    else {
                        $http.post(urlApiConductores, conductor).
                            then(function (response) {
                                alert("Creado");
                                $scope.conductor = {};
                                $scope.cancel();
                                scope.Obtener();
                            }, function (response) {
                                //alert(response.data.ExceptionMessage);
                            });
                    }
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

app.controller('ModalNuevoConductor', ['$scope', '$modalInstance', '$http',
    function ($scope, $modalInstance, $http) {
    $scope.ok = function () {
        $modalInstance.close();
    };
    $scope.guardar = function (conductor) {
        if ($scope.esEdicion) {
            $http.put(urlApiConductores + "/" + conductor.Id, conductor).
            then(function (response) {
                alert("Actualizado");
            }, function (response) {
                //alert(response.data.ExceptionMessage);
            });
        }
        else {
            $http.post(urlApiConductores, conductor).
                then(function (response) {
                    alert("Creado");
                }, function (response) {
                   // alert(response.data.ExceptionMessage);
                });
        }
    }
    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };
}])