(function () {
    'use strict';
    angular.module('controldriveApp')
        .run(function ($rootScope, $state, $stateParams) {
            $rootScope.$state = $state;
            $rootScope.$stateParams = $stateParams;
        })
        .config(function ($stateProvider, $urlRouterProvider) {
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
            .state('app.servicios', {
                url: '/servicios',
                templateUrl: 'app/servicios/consultaServicios.html'
            })
            //.state('app.servicios', {
            //    url: '/servicios',
            //    template: '<div ui-view class="fade-in-up"></div>'
            //})
            .state('app.seguimiento', {
                url: '/servicios/seguimiento',
                templateUrl: 'app/servicios/seguimiento.html'
            })
            .state('app.cierre', {
                url: '/servicios/cierre',
                templateUrl: 'app/servicios/cierre.html'
            })
            .state('app.facturacion', {
                url: '/servicios/facturacion',
                templateUrl: 'app/facturacion/facturacion.html'
            })
            .state('app.conductores', {
                url: '/configuracion/conductores',
                templateUrl: 'app/configuracion/conductores.html'
            })
            .state('app.ciudades', {
                url: '/configuracion/ciudades',
                templateUrl: 'app/configuracion/ciudades.html'
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

