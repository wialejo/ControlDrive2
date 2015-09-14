

app.controller('SeguimientoController', ['$scope', '$http', function ($scope, $http) {
    var ApiUrlSeguimientos = ApiUrl + 'api/seguimientos/';
    $scope.servicio = {};
    $scope.Seguimientos = function (servicio) {
        $scope.servicio = servicio;
    },
    $scope.ActualizarServicio = function () {
          var urlApiServicios = ApiUrl + "api/servicios";

          $http.put(urlApi + "/" +  $scope.servicio.Id,  $scope.servicio).
          then(function (response) {
              alert("actualizado");
              //toaster.pop('success', '', 'Servicio actualizado correctamente.');
          }, function (response) {
              alert(response.data.ExceptionMessage);
          });
    }
    $scope.GuardarSeguimiento = function (seguimiento) {
        if(!seguimiento){
          seguimiento = {};
        }
        seguimiento.ServicioId = $scope.servicio.Id;
        seguimiento.NuevoEstado = $scope.servicio.EstadoCodigo;

        $http.post(ApiUrlSeguimientos, seguimiento).
                then(function (response) {
                  $scope.ObtenerServicios();
                  $scope.servicio.Seguimientos.push(response.data);
                    seguimiento = {};
                }, function (response) {
                    alert(response.data.ExceptionMessage);
                });
    }
    $scope.ObtenerSeguimientos = function () {
        $http.get(ApiUrlSeguimientos + $scope.servicio.Id).
              then(function (response) {
                  $scope.servicio.Seguimientos = response.data;
              }, function (response) {
                  alert(response.data.ExceptionMessage);
              });
    }
    $scope.ObtenerEstados = function () {
        $http.get(ApiUrl + "api/estados").
              then(function (response) {
                  $scope.estados = response.data;
              }, function (response) {
                  alert(response.data.ExceptionMessage);
              });
    }
    $scope.ObtenerServicios = function () {
        $http.get(ApiUrl + 'api/servicios/').
              then(function (response) {
                  $scope.servicios = response.data;
              }, function (response) {
                  alert(response.data.ExceptionMessage);
              });
    }
    $scope.ObtenerServicios();
    $scope.ObtenerEstados();
}]);
