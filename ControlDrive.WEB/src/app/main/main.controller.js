(function () {
    'use strict';

    /* Controllers */

    angular.module('controldriveApp')
      .controller('MainController', function ($location, $localStorage, $rootScope, $window, $state, authService, $scope, UsuarioSvc, SucursalSvc) {
          $scope.authentication = authService.authentication;
          $scope.logOut = function () {
              authService.logOut();
              $location.path('/access/login');
          }

          if (!$scope.authentication.isAuth) {
              $location.path('/access/login');
          } else {
              $scope.sucursales = UsuarioSvc.ObtenerSucursales();
              $scope.sucursal = UsuarioSvc.ObtenerSucursalActual();
          }

          $scope.seleccionarSucursal = function (sucursal) {
              UsuarioSvc.EstablecerSucursalActual(sucursal);
              $scope.sucursal = sucursal;
              $state.go($state.current, {}, { reload: true })
          }

          // add 'ie' classes to html
          var isIE = !!navigator.userAgent.match(/MSIE/i);
          isIE && angular.element($window.document.body).addClass('ie');
          isSmartDevice($window) && angular.element($window.document.body).addClass('smart');

          // config
          $scope.app = {
              name: 'Control drive',
              version: '0.0.1',
              // for chart colors
              color: {
                  primary: '#7266ba',
                  info: '#23b7e5',
                  success: '#27c24c',
                  warning: '#fad733',
                  danger: '#f05050',
                  light: '#e8eff0',
                  dark: '#3a3f51',
                  black: '#1c2b36'
              },
              settings: {
                  themeID: 1,
                  navbarHeaderColor: 'bg-black',
                  navbarCollapseColor: 'bg-white-only',
                  asideColor: 'bg-black',
                  headerFixed: true,
                  asideFixed: true,
                  asideFolded: false,
                  asideDock: false,
                  container: false
              }
          }

          // save settings to local storage
          if (angular.isDefined($localStorage.settings)) {
              $scope.app.settings = $localStorage.settings;
          } else {
              $localStorage.settings = $scope.app.settings;
          }
          //$scope.$watch('app.settings', function () {
          //    if ($scope.app.settings.asideDock && $scope.app.settings.asideFixed) {
          //        // aside dock and fixed must set the header fixed.
          //        $scope.app.settings.headerFixed = true;
          //    }
          //    // for box layout, add background image
          //    $scope.app.settings.container ? angular.element('html').addClass('bg') : angular.element('html').removeClass('bg');
          //    // save to local storage
          //    $localStorage.settings = $scope.app.settings;
          //}, true);


          

          function isSmartDevice($window) {
              // Adapted from http://www.detectmobilebrowsers.com
              var ua = $window['navigator']['userAgent'] || $window['navigator']['vendor'] || $window['opera'];
              // Checks for iOs, Android, Blackberry, Opera Mini, and Windows mobile devices
              return (/iPhone|iPod|iPad|Silk|Android|BlackBerry|Opera Mini|IEMobile/).test(ua);
          }

      });
})();
