(function() {
  'use strict';

  angular.module('controldriveApp')
      .factory('ManejadorErrores',function ($q) {
       return {
            responseError: responseError
        };

        function responseError (rejection) {
            switch (rejection.status) {
                case 401: {
                    return ($q.reject("Error Presentado, Descripción: "));
                    break;
                }
                case 403: {
                    return ($q.reject("Error Presentado, Descripción: "));
                    break;
                }
                case 404: {
                    return ($q.reject("Error Presentado, Descripción: Servicio no encontrado."));
                    break;
                }
                default: {
                    return ($q.reject("Error Presentado, Descripción: " + rejection.data.ExceptionMessage));
                    break;
                }
            }
            return $q.reject(rejection);
        }
    });
})();