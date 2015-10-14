
var urlApiCiudades = ApiUrl + "api/Ciudades";
app.controller('CiudadesController', ['$scope', '$http',
    function ($scope, $http) {
        $scope.esEdicion = false;
        $scope.ciudades = [];
        $scope.nuevaCiudad = {};
        $scope.Obtener = function () {
            $http.get(urlApiCiudades).
                then(function (response) {
                    $scope.ciudades = response.data;
                }, function (response) {
                    //alert(response.data.ExceptionMessage);
                });
        }
        $scope.Obtener();
        $scope.eliminar = function (ciudad) {
            $http.delete(urlApiCiudades + "/" + ciudad.Id).
               then(function (response) {
                   alert("Elmininado");
                   ciudad = {};
                   $scope.Obtener();
               }, function (response) {
                   alert(response.data.ExceptionMessage);
               });
        }
        $scope.guardar = function (ciudad) {
            if ($scope.esEdicion) {
                $http.put(urlApiCiudades + "/" + ciudad.Id, ciudad).
                then(function (response) {
                    alert("Actualizado");
                    $scope.nuevaCiudad = {};
                    $scope.Obtener();
                }, function (response) {
                      alert(response.data.ExceptionMessage);
                });
            }
            else {
                $http.post(urlApiCiudades, ciudad).
                    then(function (response) {
                        alert("Creado");
                        $scope.nuevaCiudad = {};
                        $scope.Obtener();
                    }, function (response) {
                       alert(response.data.ExceptionMessage);
                    });
            }
        };
    }])
    