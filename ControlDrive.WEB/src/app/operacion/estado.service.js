(function() {
	'use strict';
		angular.module('controldriveApp')
			.service('EstadoSvc', function ($http, $q, ManejadorErrores, ngAuthSettings) {
				return {
					Obtener: Obtener
				}
				function http(method, urlMetodo, data) {
					var request = $http({
						method: method,
						url: ngAuthSettings.apiServiceBaseUri + 'api/' + urlMetodo,
						data: data
					});
					return (request.then(function (respuesta) { return respuesta }, ManejadorErrores.ResponseError));
				}
				function Obtener() {
					var url = "estados/Obtener/";
					var obj = http("GET", url);
					return obj
				}
			});
})();