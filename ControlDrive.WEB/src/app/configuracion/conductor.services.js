(function() {
'use strict';
	angular.module('controldriveApp')
		.service('ConductorSvc', function ($http, $q, ManejadorErrores, ngAuthSettings) {
			return {
				Guardar: Guardar,
				ObtenerPorId: ObtenerPorId,
				Obtener: Obtener,
				Eliminar: Eliminar
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
				return request;
			}
			function Guardar(conductor) {
				var url = "Conductor/Guardar/";
				var obj = http('POST', url, conductor );
				return obj;
			}
			function ObtenerPorId(id) {
				var url = "Conductor/ObtenerPorId/" + id;
				var obj = http("GET", url);
				return obj
			}
			function Obtener() {
				var url = "Conductor/Obtener/";
				var obj = http("GET", url);
				return obj
			}
			function Eliminar(id) {
				var url = "Conductor/Eliminar/" + id;
				var obj = http("DELETE", url);
				return obj
			}
		});
})();

