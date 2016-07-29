(function() {
'use strict';
	angular.module('controldriveApp')
		.service('SucursalSvc', function ($http, $q, ManejadorErrores, ngAuthSettings, localStorageService) {
			return {
				Guardar: Guardar,
				ObtenerPorId: ObtenerPorId,
				Obtener: Obtener,
				Eliminar: Eliminar,
				EstablecerSucursalActual: EstablecerSucursalActual,
				ObtenerSucursalActual: ObtenerSucursalActual,
				ObtenerPorUsuario: ObtenerPorUsuario
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
			function Guardar(sucursal) {
				var url = "Sucursales/Guardar/";
				var obj = http('POST', url, sucursal );
				return obj;
			}
			function ObtenerPorId(id) {
				var url = "Sucursales/ObtenerPorId/" + id;
				var obj = http("GET", url);
				return obj
			}
			function Obtener() {
				var url = "Sucursales/Obtener/";
				var obj = http("GET", url);
				return obj
			}
			function ObtenerPorUsuario() {
			    var url = "Sucursales/ObtenerPorUsuario/";
				var obj = http("GET", url);
				return obj
			}
			function Eliminar(id) {
				var url = "Sucursales/Eliminar/" + id;
				var obj = http("DELETE", url);
				return obj
			}
			function EstablecerSucursalActual(sucursal) {
			    localStorageService.set('sucursal', sucursal)
			}
			function ObtenerSucursalActual() {
			    return localStorageService.get('sucursal')
			}
		});
})();

