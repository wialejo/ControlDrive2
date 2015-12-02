app.controller('nuevoServicioController', ['$scope', '$http', '$filter', '$state', 'toaster' , 
    function ($scope,  $http, $filter, $state, toaster) {
    var urlApiServicios = ApiUrl + "api/servicios";
    var urlApiConductores = ApiUrl + "api/conductores";
    var urlApiCiudades = ApiUrl + "api/Ciudades";
    var urlApiRutas = ApiUrl + "api/rutas";
    
    $scope.servicio = {};

    $scope.esEdicion = false;
    $scope.conductores = [];
    $scope.ciudades = [];
    $scope.rutas = [];

    var date = new Date();
    var mes = (date.getMonth() + 1);
    mes = mes < 10 ? "0" + mes.toString() : mes;
    var dia = date.getDate();
    dia = dia < 10 ? "0" + dia.toString() : dia;
    var fechaActual = (dia + '/' + mes + '/' + date.getFullYear());
        
    $scope.isSaving = false;
    $scope.servicio.FechaD = fechaActual;
    $scope.Guardar = function (servicio) {

        $scope.isSaving = true;

        var d = servicio.FechaD.split("/").slice(0, 3).reverse();
        var fecha = new Date(d[1] + "/" + d[2] + "/" + d[0] + " " + servicio.Hora)
        if (fecha == "Invalid Date") {
            alert("Fecha no valida")
            return; 
        }

        servicio.Fecha = $filter('date')(fecha, 'yyyy-MM-dd HH:mm:ss');

        if ($scope.esEdicion) {
            $http.put(urlApiServicios + "/" + servicio.Id, servicio).
            then(function (response) {
                $scope.isSaving = false;
                alert('Servicio actualizado correctamente.');
                toaster.pop('success', 'Servicios', 'Servicio actualizado correctamente.');
                $scope.servicio = {};
            }, function (response) {
                $scope.isSaving = false;
                alert(response.data.ExceptionMessage);
            });
        }
        else {
            servicio.EstadoCodigo = "RG";
            $http.post(urlApiServicios, servicio).
                then(function (response) {
                    if (response.status == 201) {
                        alert("Servicio registrado correctamente.");
                        $scope.servicio = response.data;
                    } else {
                        alert(response.statusText);
                    }
                    $scope.isSaving = false;
                }, function (response) {
                    alert(response.data.ExceptionMessage);
                    $scope.isSaving = false;
                });
        }
    }
    $scope.ObtenerCiudades = function () {
        $http.get(urlApiCiudades).
            then(function (response) {
                $scope.ciudades = response.data;
            }, function (response) {
                //alert(response.data.ExceptionMessage);
            });
    }()
    $scope.ObtenerConductores = function () {
        $http.get(urlApiConductores).
            then(function (response) {
                $scope.conductores = response.data;
            }, function (response) {
                //alert(response.data.ExceptionMessage);
            });
    }()
    $scope.ObtenerRutas = function () {
        $http.get(urlApiRutas).
            then(function (response) {
                $scope.rutas = response.data;
            }, function (response) {
                //alert(response.data.ExceptionMessage);
            });
    }();
    if ($state.params.servicio != null) {
        var servicio = $state.params.servicio;
        servicio.Hora = $filter('date')(servicio.Fecha, 'HH:mm');
        servicio.FechaD = $filter('date')(servicio.Fecha, 'dd/MM/yyyy');
        $scope.servicio = servicio;
        $scope.esEdicion = true;
    }
    
}])
