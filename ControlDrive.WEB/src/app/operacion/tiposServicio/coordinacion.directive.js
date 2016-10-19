(function() {
'use strict';
    angular.module('controldriveApp')
        .directive('coordinacion', function () {
            return {
                restrict: 'E ',
                templateUrl: 'app/operacion/tiposServicio/coordinacion.html'
            };
    });
})();

