'use strict';

/**
 * Config for the   er
 */
angular.module('app')
  .run(
    ['$rootScope', '$state', '$stateParams',
      function ($rootScope, $state, $stateParams) {
          $rootScope.$state = $state;
          $rootScope.$stateParams = $stateParams;
      }
    ]
  )
  .config(
    ['$stateProvider', '$urlRouterProvider', 'JQ_CONFIG', 'MODULE_CONFIG',
      function ($stateProvider, $urlRouterProvider, JQ_CONFIG, MODULE_CONFIG) {

          $urlRouterProvider.otherwise('/app');
          $stateProvider
              .state('app', {
                  //abstract: true,
                  url: '/app',
                  templateUrl: "app/tpl/app.html",
                  resolve: load(['app/js/controllers/signup.js'])

              })
              .state('app.nuevo', {
                  url: '/nuevo',
                  templateUrl: 'app/views/servicios/nuevoServicio.html',
                  resolve: load(['toaster', 'ui.select','app/js/controllers/Gestion/nuevoServicio.js'])
              })
              .state('app.editar', {
                  url: '/editar',
                  templateUrl: 'app/views/servicios/nuevoServicio.html',
                  params: { servicio: null },
                  resolve: load(['toaster','ui.select','app/js/controllers/Gestion/nuevoServicio.js'])
              })
              .state('app.consultaServicios', {
                  url: '/consultaServicios',
                  templateUrl: 'app/views/servicios/consultaServicios.html',
                  resolve: load(['app/js/controllers/Gestion/consultaServicios.js'])
              })
              .state('app.conductores', {
                  url: '/conductores',
                  controller: 'ConductoresController',
                  templateUrl: 'app/views/Configuracion/Conductores.html',
                  resolve: load(['app/js/controllers/Configuracion/conductores.js'])
              })
              .state('app.ciudades', {
                  url: '/Ciudades',
                  controller: 'CiudadesController',
                  templateUrl: 'app/views/Configuracion/Ciudades.html',
                  resolve: load(['app/js/controllers/Configuracion/Ciudades.js'])
              })
              .state('app.rutasConsulta', {
                  url: '/rutasConsulta',
                  templateUrl: 'app/views/rutas/consultaRutas.html'
              })
              .state('app.servicios', {
                  url: '/servicios',
                  template: '<div ui-view class="fade-in-up"></div>'
              })
              .state('app.seguimiento', {
                  url: '/servicios/seguimiento',
                  templateUrl: 'app/views/servicios/seguimientoServicios.html',
                  resolve: load(['app/js/controllers/Gestion/seguimientos.js'])
              })
              .state('app.dashboard-v1', {
                  url: '/dashboard-v1',
                  templateUrl: 'tpl/app_dashboard_v1.html',
                  resolve: load(['app/js/controllers/chart.js'])
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
              .state('access.signin', {
                  url: '/signin',
                  templateUrl: 'app/tpl/page_signin.html',
                  resolve: load(['app/js/controllers/signin.js'])
              })
              .state('access.signup', {
                  url: '/signup',
                  templateUrl: 'app/tpl/page_signup.html',
                  resolve: load(['app/js/controllers/signup.js'])
              })
              .state('access.forgotpwd', {
                  url: '/forgotpwd',
                  templateUrl: 'app/tpl/page_forgotpwd.html'
              })
              .state('access.404', {
                  url: '/404',
                  templateUrl: 'app/tpl/page_404.html'
              })
          ;


          function load(srcs, callback) {
              return {
                  deps: ['$ocLazyLoad', '$q',
                    function ($ocLazyLoad, $q) {
                        var deferred = $q.defer();
                        var promise = false;
                        srcs = angular.isArray(srcs) ? srcs : srcs.split(/\s+/);
                        if (!promise) {
                            promise = deferred.promise;
                        }
                        angular.forEach(srcs, function (src) {
                            console.log(src);
                            promise = promise.then(function () {
                                if (JQ_CONFIG[src]) {
                                    return $ocLazyLoad.load(JQ_CONFIG[src]);
                                }
                                angular.forEach(MODULE_CONFIG, function (module) {
                                    if (module.name == src) {
                                        name = module.name;
                                    } else {
                                        name = src;
                                    }
                                });
                                return $ocLazyLoad.load(name);
                            });
                        });
                        deferred.resolve();
                        return callback ? promise.then(function () { return callback(); }) : promise;
                    }]
              }
          }
      }
    ]
  );