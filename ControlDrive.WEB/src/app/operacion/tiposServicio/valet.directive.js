(function() {
'use strict';
	angular.module('controldriveApp')
		.directive('valet', function () {
		    return {
		        restrict: 'E ',
		        templateUrl: 'app/operacion/tiposServicio/valet.html'
		    };
		});
})();

