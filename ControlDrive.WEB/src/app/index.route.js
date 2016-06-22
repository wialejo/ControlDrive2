(function () {
    'use strict';
    angular.module('controldriveApp')
        .run(function ($rootScope, $state, $stateParams) {
            $rootScope.$state = $state;
            $rootScope.$stateParams = $stateParams;
        })
        .config(function ($stateProvider, $urlRouterProvider) {
            $urlRouterProvider.otherwise('/app/resumen');
            $stateProvider
            .state('app', {
                //abstract: true,
                url: '/app',
                templateUrl: 'app/main/app.html'
            })
            .state('app.resumen', {
                //abstract: true,
                url: '/resumen',
                templateUrl: 'app/inicio/inicioResumen.html'
            })
            .state('app.nuevo', {
                url: '/servicio',
                templateUrl: 'app/operacion/nuevoServicio.html'
            })
            .state('app.editar', {
                url: '/servicio/:id?',
                templateUrl: 'app/operacion/editarServicio.html'
            })
            .state('app.serviciosHistorico', {
                url: '/servicios/historico',
                templateUrl: 'app/operacion/consultaServicios.html'
            })
            //.state('app.servicios', {
            //    url: '/servicios',
            //    template: '<div ui-view class="fade-in-up"></div>'
            //})
            .state('app.seguimiento', {
                url: '/servicios/seguimiento',
                templateUrl: 'app/operacion/seguimiento.html'
            })
            .state('app.cierre', {
                url: '/servicios/cierre',
                templateUrl: 'app/operacion/cierre.html'
            })
            .state('app.facturacion', {
                url: '/administracion/facturacion',
                templateUrl: 'app/administracion/facturacion/facturacion.html'
            })
            .state('app.prefacturacion', {
                url: '/administracion/prefacturacion',
                templateUrl: 'app/administracion/facturacion/prefacturacion.html'
            })
            .state('app.cartera', {
                url: '/administracion/cartera',
                templateUrl: 'app/administracion/facturacion/cartera.html'
            })
            .state('app.pagoAProveedores', {
                url: '/pagos/proveedores',
                templateUrl: 'app/administracion/pagos/pagoAProveedores.html'
            })
            .state('app.cuentasPorPagar', {
                url: '/administracion/cuentasPorPagar',
                templateUrl: 'app/administracion/pagos/CuentasPorPagar.html'
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
            .state('access.passwordChange', {
                url: '/passwordChange',
                templateUrl: 'app/account/passwordChange.html'
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

