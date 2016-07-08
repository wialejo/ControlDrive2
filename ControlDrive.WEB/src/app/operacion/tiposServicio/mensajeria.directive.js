(function () {
    'use strict';
    angular.module('controldriveApp')
		.directive('mensajeria', function () {
		    return {
		        restrict: 'E ',
		        templateUrl: 'app/operacion/tiposServicio/mensajeria.html'
		    };
		});
})();

