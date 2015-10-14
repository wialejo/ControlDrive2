app.controller('seguimientoController', ['$scope', '$http', '$modal', function ($scope, $http, $modal) {
    var ApiUrlSeguimientos = ApiUrl + 'api/seguimientos/';
    $scope.servicio = {};
    $scope.Seguimientos = function (servicio) {
        $scope.servicio = servicio;
    },
    $scope.isSaving = false;
    $scope.ActualizarServicio = function () {
        var urlApiServicios = ApiUrl + "api/servicios";

        $http.put(urlApi + "/" + $scope.servicio.Id, $scope.servicio).
        then(function (response) {
            alert("actualizado");
            //toaster.pop('success', '', 'Servicio actualizado correctamente.');
        }, function (response) {
            alert(response.data.ExceptionMessage);
        });
    }
    $scope.GuardarSeguimiento = function (seguimiento) {
        $scope.isSaving = true;
        if (!seguimiento) {
            seguimiento = {};
        }
        seguimiento.ServicioId = $scope.servicio.Id;
        seguimiento.NuevoEstado = $scope.servicio.EstadoCodigo;

        $http.post(ApiUrlSeguimientos, seguimiento)
            .then(function (response) {
                seguimiento = {};
                $scope.ObtenerServicios();
                $scope.servicio.Seguimientos.push(response.data);
                $scope.isSaving = false;
            }, function (response) {
                $scope.isSaving = false;
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
    $scope.MostrarDetalles = function (servicio) {
        var modalInstance = $modal.open({
            templateUrl: 'detalleServicio.html',
            size: 'lg',
            controller: function ($scope, $modalInstance, $http, servicio) {
                $scope.servicio = servicio;
                $scope.cancel = function () {
                    $modalInstance.dismiss('cancel');
                };
            },
            resolve: {
                servicio: function () {
                    return servicio;
                }
            }
        });
    }
}]);
