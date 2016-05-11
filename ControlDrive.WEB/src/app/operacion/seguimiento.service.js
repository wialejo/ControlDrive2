(function() {
	'use strict';
		angular.module('controldriveApp')
			.service('SeguimientoSvc', function ($http, $q, ManejadorErrores, ngAuthSettings) {
				return {
					Guardar: Guardar,
					ObtenerPorId: ObtenerPorId,
					ObtenerPorServicio: ObtenerPorServicio
				}
				function http(method, urlMetodo, data) {
					var request = $http({
						method: method,
						url: ngAuthSettings.apiServiceBaseUri + 'api/' + urlMetodo,
						data: data
					});
					return (request.then(function (respuesta) { return respuesta }, ManejadorErrores.ResponseError));
				}
				function Guardar(seguimiento) {
					var url = "seguimientos/Guardar/";
					var obj = http('POST', url, seguimiento );
					return obj;
				}
				function ObtenerPorId(id) {
					var url = "seguimientos/ObtenerPorId/" + id;
					var obj = http("GET", url);
					return obj
				}
				function ObtenerPorServicio(idServicio) {
					var url = "seguimientos/ObtenerPorServicio/" + idServicio;
					var obj = http("GET", url);
					return obj
				}
			});
})();