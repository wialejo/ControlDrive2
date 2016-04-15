(function() {
'use strict';
	angular.module('controldriveApp')
		.service('AseguradoraSvc', function ($http, $q, ManejadorErrores, ngAuthSettings) {
			return {
				Guardar: Guardar,
				ObtenerPorId: ObtenerPorId,
				Obtener: Obtener
			}
			function http(method, urlMetodo, data) {
				var request = $http({
					method: method,
					url: ngAuthSettings.apiServiceBaseUri + 'api/' + urlMetodo,
					headers: { 
						'Access-Control-Allow-Origin':true	
					},
					data: data
				});
				return (request.then(function (respuesta) { return respuesta }, ManejadorErrores.ResponseError));
            }

			function Guardar(servicio) {
				var url = "Servicio/Guardar/";
				var obj = http('POST', url, servicio );
				return obj;
			}
			function ObtenerPorId(id) {
				var url = "Servicio/ObtenerPorId/" + id;
				var obj = http("GET", url);
				return obj
			}
			function Obtener() {
				var url = "Aseguradora/Obtener/";
				var obj = http("GET", url);
				return obj
			}
		});
})();

