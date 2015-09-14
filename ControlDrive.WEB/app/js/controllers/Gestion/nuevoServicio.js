app.controller('nuevoServicioController', ['$scope', '$http', '$filter', '$state', 'toaster' , 
    function ($scope,  $http, $filter, $state, toaster) {
    var urlApiServicios = ApiUrl + "api/servicios";
    var urlApiConductores = ApiUrl + "api/conductores";
    $scope.servicio = {};
    $scope.esEdicion = false;
    $scope.conductores = [];
    $scope.Guardar = function (servicio) {
        var d = servicio.FechaD.split("/").slice(0,3).reverse();
        servicio.Fecha = new Date(d+" " + servicio.Hora);
        servicio.ConductorId = servicio.Conductor.Id; 
        if ($scope.esEdicion) {
            $http.put(urlApiServicios + "/" + servicio.Id, servicio).
            then(function (response) {
                toaster.pop('success', 'Servicios', 'Servicio actualizado correctamente.');
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
    $scope.ObtenerConductores = function () {
        $http.get(urlApiConductores).
            then(function (response) {
                $scope.conductores = response.data;
            }, function (response) {
                //alert(response.data.ExceptionMessage);
            });
    }()
    if ($state.params.servicio != null) {
        $scope.esEdicion = true;
        $scope.servicio = $state.params.servicio;
    }
}])
