(function() {
'use strict';
	angular.module('controldriveApp')
		.directive('conductorElegido', function () {
		    return {
		        restrict: 'E ',
		        templateUrl: 'app/operacion/tiposServicio/conductorElegido.html'
		    };
		});
})();

