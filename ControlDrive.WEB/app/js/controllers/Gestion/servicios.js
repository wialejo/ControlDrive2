var urlApi = "http://192.168.0.29/API/api/servicios";
var urlApiConductores = "http://192.168.0.29/API/api/conductores";

app.controller('ServiciosController', ['$scope', '$http', '$filter', '$state', '$location', function ($scope, $http, $filter, $state, $location) {
    $scope.servicios = [];
    $scope.servicio = {};
    $scope.esEdicion = false;
    $scope.conductores = [];
    $scope.Guardar = function (servicio) {
        servicio.Fecha = servicio.Hora + ' ' + servicio.FechaD;
        if ($scope.esEdicion) {
            $http.put(urlApi + "/" + servicio.Id, servicio).
            then(function (response) {
                alert("Actualizado");
            }, function (response) {
                alert(response.data.ExceptionMessage);
            });
        }
        else {
            servicio.EstadoCodigo = "EJ";
            $http.post(urlApi, servicio).
                then(function (response) {
                    alert("Creado");
                }, function (response) {
                    alert(response.data.ExceptionMessage);
                });
        }
        
    }
    $scope.Editar = function (servicio) {
            $scope.servicio = servicio;
            $scope.servicio.Hora = $filter('date')(servicio.Fecha, 'HH:mm');
            $scope.servicio.FechaD = $filter('date')(servicio.Fecha, 'dd/MM/yyyy');
            $stateParams = servicio;
            $state.go('app.editar', { servicio: servicio })
    }
    $scope.ObtenerConductores = function () {
        $http.get(urlApiConductores).
            then(function (response) {
                $scope.conductores = response.data;
            }, function (response) {
                alert(response.data.ExceptionMessage);
            });
    }
    $scope.ObtenerConductores();
    if ($state.params.servicio != null) {
        $scope.esEdicion = true;
        $scope.servicio = $state.params.servicio;
    }
}])
app.controller('ServiciosResumenController', ['$scope', '$http', function ($scope, $http) {
    $scope.servicio = {};
    $scope.Seguimientos = function (servicio) {
        $scope.servicio = servicio;
    },
    $scope.Obtener = function () {
        $http.get(urlApi).
            then(function (response) {
                $scope.servicios = response.data;
                $scope.servicios[0].Seguimientos =
                    [{
                        Id: 1,
                        Fecha: new Date(),
                        Usuario: { Id: 1, Nombre: 'Julian' },
                        Observacion: 'Usuario en linea'
                    }, {
                        Id: 2,
                        Fecha: new Date(),
                        Usuario: { Id: 2, Nombre: 'David' },
                        Observacion: 'Usuario en linea'
                    }];
            }, function (response) {
                alert(response.data.ExceptionMessage);
            });
    }
    $scope.Obtener();
}]);
