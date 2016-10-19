(function () {
    'use strict';
    angular.module('controldriveApp')
      .controller('CiudadesController', function (CiudadSvc, toastr, $scope) {
          $scope.$parent.$parent.app.viewName = "Ciudades";
          var vm = this;
          vm.esEdicion = false;
          vm.ciudades = [];
          vm.nuevaCiudad = {};

          (vm.Obtener = function () {
              CiudadSvc.Obtener()
                .then(function (response) {
                    vm.ciudades = response.data;
                })
                .catch(function (response) {
                    alert(response.data.ExceptionMessage);
                });
          })();

          vm.eliminar = function (ciudad) {
              CiudadSvc.Eliminar(ciudad.Id)
                .then(function () {
                    toastr.success('Ciudad eliminada correctamente.')
                    ciudad = {};
                    vm.Obtener();
                })
                .catch(function (response) {
                    toastr.error(response.data.ExceptionMessage);
                });
          }

          vm.editar = function (ciudad) {
              CiudadSvc.Guardar(ciudad)
               .then(function () {
                   toastr.success('Ciudad actualizada correctamente.')
                   vm.Obtener();
                   ciudad.enEdicion = false;
               })
               .catch(function (response) {
                   toastr.error(response.data.ExceptionMessage);
               });
          }

          vm.guardar = function (ciudad) {
              CiudadSvc.Guardar(ciudad)
                .then(function () {
                    toastr.success('Ciudad guardada correctamente.')
                    vm.nuevaCiudad = {};
                    vm.Obtener();
                })
                .catch(function (response) {
                    toastr.error(response.data.ExceptionMessage);
                });
          }
      })
})();