
var urlApiRutas = ApiUrl + "api/rutas";
var urlApiConductores = ApiUrl + "api/conductores";
app.controller('RutasController', ['$scope', '$modal', '$log', '$http',
    function ($scope, $modal, $log, $http) {
        $scope.esEdicion = false;
        $scope.conductores = [];
        $scope.rutas = [];
        $scope.ruta = {};
        $scope.Obtener = function () {
            $http.get(urlApiRutas).
                then(function (response) {
                    $scope.rutas = response.data;
                }, function (response) {
                    //alert(response.data.ExceptionMessage);
                });
        }();
        $scope.eliminar = function (ruta) {

        }
        $scope.editar = function (ruta) {
            $scope.esEdicion = true;
            $scope.ruta = ruta;
            $scope.abrirModalNuevo();
        },
        $scope.nuevo = function () {
            $scope.esEdicion = false;
            $scope.ruta  = {};
            $scope.abrirModalNuevo();
        },
        $scope.abrirModalNuevo = function () {
            var modalInstance = $modal.open({
                templateUrl: 'NuevaRuta.html',
                size: 'md',
                controller: function ($scope, $modalInstance, $http, ruta, esEdicion, scope) {
                    $scope.ObtenerConductores = function () {
                        $http.get(urlApiConductores).
                            then(function (response) {
                                $scope.conductores = response.data;
                            }, function (response) {
                                //alert(response.data.ExceptionMessage);
                            });
                    }()
                    $scope.ruta = ruta;
                    $scope.esEdicion = esEdicion;
                    $scope.guardar = function (ruta) {
                        if ($scope.esEdicion) {
                            $http.put(urlApiRutas + "/" + ruta.Id, ruta).
                            then(function (response) {
                                alert("Actualizado");
                                $scope.ruta = {};
                                $scope.cancel();
                                scope.Obtener();

                            }, function (response) {
                                //  alert(response.data.ExceptionMessage);
                            });
                        }
                        else {
                            $http.post(urlApiRutas, ruta).
                                then(function (response) {
                                    alert("Creado");
                                    $scope.ruta = {};
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
                    ruta: function () {
                        return $scope.ruta;
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

app.controller('ModalNuevaRuta', ['$scope', '$modalInstance', '$http',
    function ($scope, $modalInstance, $http) {
        $scope.ok = function () {
            $modalInstance.close();
        };
        $scope.guardar = function (ruta) {
            if ($scope.esEdicion) {
                $http.put(urlApiRutas + "/" + ruta.Id, ruta).
                then(function (response) {
                    alert("Actualizado");
                }, function (response) {
                    //alert(response.data.ExceptionMessage);
                });
            }
            else {
                $http.post(urlApiRutas, ruta).
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

