app.controller('seguimientoController', ['$scope','$filter', '$http', '$modal', '$state', function ($scope, $filter, $http, $modal, $state) {
    var ApiUrlSeguimientos = ApiUrl + 'api/seguimientos/';
    $scope.servicio = {};
    $scope.servicios = [];
    $scope.serviciosVisibles = [].concat($scope.servicios);

    var date = new Date();
    var mes = (date.getMonth() + 1);
    mes = mes < 10 ? "0" + mes.toString() : mes;
    var dia = date.getDate();
    dia = dia < 10 ? "0" + dia.toString() : dia;
    var fechaActual = (dia + '/' + mes + '/' + date.getFullYear());
    $scope.periodo = fechaActual;
    
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
        var d = $scope.periodo.split("/").slice(0, 3).reverse();
        var fecha = new Date(d[1] + "/" + d[2] + "/" + d[0])
        var Fecha = {
            Inicio: $filter('date')(fecha, 'yyyy-MM-dd HH:mm:ss')
        }

        $http.post(ApiUrl + 'api/servicios/ServiciosByPeriodo/', Fecha).
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
            size: 'md',
            controller: function ($scope, $modalInstance, $http, servicio) {
                $scope.servicio = servicio;
                $scope.cancel = function () {
                    $modalInstance.dismiss('cancel');
                };
                $scope.Editar = function (servicio) {
                    $scope.cancel();

                    $scope.servicio = servicio;
                    $scope.servicio.Hora = $filter('date')(servicio.Fecha, 'HH:mm');
                    $scope.servicio.FechaD = $filter('date')(servicio.Fecha, 'dd/MM/yyyy');
                    $stateParams = servicio;
                    $state.go('app.editar', { servicio: servicio })
                }
            },
            resolve: {
                servicio: function () {
                    return servicio;
                }
            }
        });
    }
    $scope.EnviarCorreo = function () {
        var serviciosSeleccionados = [];
        angular.forEach($scope.serviciosVisibles, function (servicio) {
            if (servicio.isSelected)
                serviciosSeleccionados.push(servicio);
        });
        var urlApiServicios = ApiUrl + "/api/servicios/EnviarCorreoSeguimiento";

        //http://localhost/API2/api/servicios/EnviarCorreoSeguimiento
        $http.post(urlApiServicios, serviciosSeleccionados)
            .then(function (response) {
                alert("Enviado");
            }, function (response) {
                alert(response.data.ExceptionMessage);
            });
    }

    $scope.Seleccionar = function (seleccion) {
        angular.forEach($scope.serviciosVisibles, function (servicio) {
            servicio.isSelected = seleccion;
        });
    }
    
}]);

