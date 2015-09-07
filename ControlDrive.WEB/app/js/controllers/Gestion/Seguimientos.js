
app.controller('SeguimientoController', ['$scope', '$http', function ($scope, $http) {
    var ApiUrlSeguimientos = ApiUrl + 'api/seguimientos/';
    $scope.servicio = {};
    $scope.Seguimientos = function (servicio) {
        $scope.servicio = servicio;
    },
    $scope.GuardarSeguimiento = function (seguimiento) {
        seguimiento.ServicioId = $scope.servicio.Id;

        $http.post(ApiUrlSeguimientos, seguimiento).
                then(function (response) {
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
    $scope.ObtenerServicios = function () {
        $http.get(ApiUrl + 'api/servicios/').
              then(function (response) {
                  $scope.servicios = response.data;
              }, function (response) {
                  alert(response.data.ExceptionMessage);
              });
    }
    $scope.ObtenerServicios();
}]);
