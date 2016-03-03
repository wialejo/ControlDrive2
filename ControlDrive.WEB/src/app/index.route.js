(function() {
'use strict';
  angular.module('controldriveApp')
    .run(function ($rootScope, $state, $stateParams) {
          $rootScope.$state = $state;
          $rootScope.$stateParams = $stateParams;
      })

    .config(function ($stateProvider,$urlRouterProvider) {
        // $routeProvider
        //   .when('/', {
        //     templateUrl: 'app/main/app.html',
        //     controller: 'MainController'
        //   })
        //   .when('/seguimiento', {
        //     templateUrl: 'app/seguimiento/seguimiento.html',
        //     controller: 'seguimientosController',
        //     controllerAs: 'seguimiento'
        //   })
        //   .when('/access/login', {
        //     templateUrl: 'app/main/login.html',
        //     controller: 'loginController'
        //   })
        //   .otherwise({
        //     redirectTo: '/'
        //   });

      $urlRouterProvider.otherwise('/app');
      $stateProvider
      .state('app', {
          //abstract: true,
          url: '/app',
          templateUrl: 'app/main/app.html'
      })
      .state('app.nuevo', {
          url: '/servicio',
          templateUrl: 'app/servicios/nuevoServicio.html'
      })

      .state('app.editar', {
          url: '/servicio/:id?',
          templateUrl: 'app/servicios/editarServicio.html'
      })
      .state('app.consultaServicios', {
          url: '/consultaServicios',
          templateUrl: 'app/servicios/consultaServicios.html'
      })
      .state('app.conductores', {
          url: '/conductores',
          templateUrl: 'app/configuracion/conductores.html'
      })
      .state('app.ciudades', {
          url: '/Ciudades',
          templateUrl: 'app/configuracion/ciudades.html'
      })
      .state('app.servicios', {
          url: '/servicios',
          template: '<div ui-view class="fade-in-up"></div>'
      })
      .state('app.seguimiento', {
            url: '/seguimiento',
            templateUrl: 'app/servicios/seguimiento.html'
      })
      .state('app.dashboard-v1', {
          url: '/dashboard-v1',
          templateUrl: 'tpl/app_dashboard_v1.html'
      })
      // others
      .state('lockme', {
          url: '/lockme',
          templateUrl: 'app/tpl/page_lockme.html'
      })
      .state('access', {
          url: '/access',
          template: '<div ui-view class="fade-in-right-big smooth"></div>'
      })
      .state('access.login', {
          url: '/login',
          templateUrl: 'app/account/login.html'
      })
      .state('access.register', {
          url: '/register',
          templateUrl: 'app/account/register.html'
      })
      .state('access.forgotpwd', {
          url: '/forgotpwd',
          templateUrl: 'app/tpl/page_forgotpwd.html'
      })
      .state('access.404', {
          url: '/404',
          templateUrl: 'app/tpl/page_404.html'
      });
    });
})();
