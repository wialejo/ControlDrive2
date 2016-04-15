(function() {
'use strict';
	angular.module('controldriveApp')
		.service('CiudadSvc',  function ($http, $q, ManejadorErrores, ngAuthSettings) {
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
				return (request.then(function (respuesta) { return respuesta }, ManejadorErrores.ResponseError));
            }
			function Guardar(ciudad) {
				var url = "Ciudades/Guardar/";
				var obj = http('POST', url, ciudad );
				return obj;
			}
			function ObtenerPorId(id) {
				var url = "Ciudades/ObtenerPorId/" + id;
				var obj = http("GET", url);
				return obj
			}
			function Obtener() {
				var url = "Ciudades/Obtener/";
				var obj = http("GET", url);
				return obj
			}
			function Eliminar(id) {
				var url = "Ciudades/Eliminar/" + id;
				var obj = http("DELETE", url);
				return obj
			}
		});
})();

