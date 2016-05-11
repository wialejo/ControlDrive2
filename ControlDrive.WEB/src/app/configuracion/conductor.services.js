(function() {
'use strict';
	angular.module('controldriveApp')
		.service('ConductorSvc', function ($http, $q, ManejadorErrores, ngAuthSettings, PeriodoSvc) {
			return {
			    Guardar: Guardar,
			    ObtenerPorId: ObtenerPorId,
			    ObtenerPorDescripcion: ObtenerPorDescripcion,
			    ObtenerServiciosTerminados: ObtenerServiciosTerminados,
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
			function Guardar(conductor) {
				var url = "Conductores/Guardar/";
				var obj = http('POST', url, conductor );
				return obj;
			}
			function ObtenerPorId(id) {
			    var url = "Conductores/ObtenerPorId/" + id;
				var obj = http("GET", url);
				return obj
			}
			function ObtenerPorDescripcion(filtro) {
			    var url = "Conductores/ObtenerPorDescripcion/" + filtro;
				var obj = http("GET", url);
				return obj
			}
			function ObtenerServiciosTerminados(inicioPeriodo, finPeriodo, proveedorId) {
			    var periodo = PeriodoSvc.FormatearParaApi(inicioPeriodo, finPeriodo);
			    var url = "Conductores/servicios/terminados?inicio=" + periodo.Inicio + "&fin=" + periodo.Fin + "&proveedorId=" + proveedorId;
			    var obj = http("GET", url)
			    return obj
			}

			function Obtener() {
			    var url = "Conductores/Obtener/";
				var obj = http("GET", url);
				return obj
			}
			function Eliminar(id) {
			    var url = "Conductores/Eliminar/" + id;
				var obj = http("DELETE", url);
				return obj
			}
		});
})();

