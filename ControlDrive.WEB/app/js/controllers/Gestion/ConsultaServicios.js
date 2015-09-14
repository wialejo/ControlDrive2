app.controller('consultaServiciosController', ['$scope', '$http', '$filter', '$state', function ($scope, $http, $filter, $state) {
	var urlApiServicios = ApiUrl + "api/servicios";
    $scope.servicio = {};
    $scope.servicios = [];
    $scope.ObtenerServicios = function () {
        $http.get(urlApiServicios).
            then(function (response) {
                $scope.servicios = response.data;
            }, function (response) {
               //alert(response.data.ExceptionMessage);
            });
    }()
    $scope.Editar = function (servicio) {
            $scope.servicio = servicio;
            $scope.servicio.Hora = $filter('date')(servicio.Fecha, 'HH:mm');
            $scope.servicio.FechaD = $filter('date')(servicio.Fecha, 'dd/MM/yyyy');
            $stateParams = servicio;
            $state.go('app.editar', { servicio: servicio })
    }
}]);