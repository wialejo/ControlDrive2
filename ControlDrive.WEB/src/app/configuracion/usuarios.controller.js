(function () {
    'use strict';
    angular.module('controldriveApp')
      .controller('UsuariosController', function (SucursalSvc, UsuarioSvc, toastr, $uibModal, $scope) {
          $scope.$parent.$parent.app.viewName = "Usuarios";
          var vm = this;
          vm.sucursales = [];
          vm.usuarios = [];

          $scope.ObtenerUsuarios = function () {
              UsuarioSvc.Obtener().then(function (response) {
                  vm.usuarios = response.data;
              })
          }
          $scope.ObtenerUsuarios();

          SucursalSvc.Obtener().then(function (response) {
              vm.sucursales = response.data;
          });

          vm.abrirModalSucursales = function (usuario) {
              var modalInstance = $uibModal.open({
                  templateUrl: 'UsuarioSucursales.html',
                  size: 'sm',
                  controller: function ($scope, $uibModalInstance, usuario, sucursales, scope, UsuarioSvc) {
                      $scope.usuario = usuario;
                      $scope.sucursales = CargarSucursalesAsignadas(sucursales, $scope.usuario.Sucursales);

                      $scope.asignar = function (sucursal) {
                          if (sucursal.asignada) {
                              UsuarioSvc.AsignarSucursal($scope.usuario.Id, sucursal.Id).then(function () {
                                  UsuarioSvc.ObtenerPorId(usuario.Id).then(function (response) {
                                      $scope.usuario = response.data;
                                      CargarSucursalesAsignadas(sucursales, $scope.usuario.Sucursales);
                                  });
                              });
                          } else {
                              UsuarioSvc.DesAsignarSucursal($scope.usuario.Id, sucursal.Id).then(function () {
                                  UsuarioSvc.ObtenerPorId(usuario.Id).then(function (response) {
                                      $scope.usuario = response.data;
                                      CargarSucursalesAsignadas(sucursales, $scope.usuario.Sucursales);
                                  });
                              })
                          }
                      }

                      function CargarSucursalesAsignadas(sucursales, sucursalesUsuario) {
                          sucursales.forEach(function (sucursal) {
                              if (sucursalesUsuario.filter(function (su) { return su.Id == sucursal.Id; }).length > 0) {
                                  sucursal.asignada = true;
                              } else {
                                  sucursal.asignada = false;
                              }
                          })
                          return sucursales;
                      }

                      $scope.cancel = function () {
                          scope.ObtenerUsuarios();
                          $uibModalInstance.dismiss('cancel');
                      };

                  },
                  resolve: {
                      usuario: function () {
                          return usuario;
                      },
                      sucursales: function () {
                          return vm.sucursales;
                      },
                      scope: function () {
                          return $scope;
                      }
                  }
              });

              modalInstance.closed.then(function () {
                  $scope.ObtenerUsuarios();
              })
          };
      })
})();