﻿(function () {
    'use strict';
    angular
      .module('controldriveApp')
          .factory('authInterceptorService', function ($q, $injector, $location, localStorageService) {

              var authInterceptorServiceFactory = {};

              var _request = function (config) {

                  config.headers = config.headers || {};

                  var authData = localStorageService.get('authorizationData');
                  if (authData) {
                      config.headers.Authorization = 'Bearer ' + authData.token;
                  }

                  var usuarioActual = localStorageService.get('currentUser')
                  if (usuarioActual) {
                      if (usuarioActual.SucursalActual) {
                          config.headers.idSucursal = usuarioActual.SucursalActual.Id;
                      }
                      //else {
                      //    alert("Debe seleccionar una sucursal.");
                      //}

                  }


                  return config;
              }

              var _responseError = function (rejection) {
                  if (rejection.status === 401) {
                      var authService = $injector.get('authService');
                      var authData = localStorageService.get('authorizationData');

                      if (authData) {
                          if (authData.useRefreshTokens) {
                              $location.path('/refresh');
                              return $q.reject(rejection);
                          }
                      }
                      authService.logOut();
                      $location.path('/access/login');
                  }
                  return $q.reject(rejection);
              }

              authInterceptorServiceFactory.request = _request;
              authInterceptorServiceFactory.responseError = _responseError;

              return authInterceptorServiceFactory;
          });
})();