var urlApiServicios = ApiUrl + "api/servicios";
var urlApiConductores = ApiUrl + "api/conductores";

app.controller('ServiciosController', ['$scope', '$http', '$filter', '$state', '$location',
    function ($scope,  $http, $filter, $state, $location) {
    $scope.servicios = [];
    $scope.servicio = {};
    $scope.esEdicion = false;
    $scope.conductores = [];
    $scope.Guardar = function (servicio) {
        servicio.Fecha = new Date(servicio.Hora + ' ' + servicio.FechaD);
        servicio.FechaRegistro = new Date($filter('date')(servicio.FechaRegistro , 'HH:mm dd/MM/yyyy'));
        servicio.ConductorId = servicio.Conductor.Id; 
        if ($scope.esEdicion) {
            $http.put(urlApiServicios + "/" + servicio.Id, servicio).
            then(function (response) {
                //toaster.pop('success', '', 'Servicio actualizado correctamente.');
            }, function (response) {
                alert(response.data.ExceptionMessage);
            });
        }
        else {
            servicio.EstadoCodigo = "EJ";
            $http.post(urlApiServicios, servicio).
                then(function (response) {
                    alert("Creado");
                    $scope.servicio = {};
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
                //alert(response.data.ExceptionMessage);
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
    $scope.Obtener = function () {
        $http.get(urlApiServicios).
            then(function (response) {
                $scope.servicios = response.data;
            }, function (response) {
               //alert(response.data.ExceptionMessage);
            });
    }
    $scope.Obtener();
}]);
