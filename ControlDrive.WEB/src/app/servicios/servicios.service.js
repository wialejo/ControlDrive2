(function() {
'use strict';
	angular.module('controldriveApp')
		.service('ServicioSvc',  function ($http, $q, ManejadorErrores, ngAuthSettings, PeriodoSvc) {
			return {
				Guardar: Guardar,
				ObtenerPorId: ObtenerPorId,
				Obtener: Obtener,
				ObtenerParaSeguimiento: ObtenerParaSeguimiento,
				ObtenerPorPeriodoCSV: ObtenerPorPeriodoCSV,
				NotificarServiciosARuta: NotificarServiciosARuta,
				NotificarServiciosAConductor : NotificarServiciosAConductor
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
			// return (request.then(function (respuesta) { return respuesta }, ManejadorErrores.ResponseError));
			return request;
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
			function Obtener(fechaInicial, fechaFinal) {
				var url = "Servicio/Obtener";
				fechaFinal = fechaFinal + " 23:00"
				var periodo = PeriodoSvc.FormatearParaApi(fechaInicial,fechaFinal);

				var obj = http("POST", url, periodo);
				return obj
			}
			function ObtenerParaSeguimiento(periodo) {
				var periodo = PeriodoSvc.FormatearParaApi(periodo)
				var url = "servicio/ObtenerParaSeguimiento/";
				var obj = http("POST", url, periodo);
				return obj
			}
			function ObtenerStrCSV(periodo) {
				var url = "servicio/ObtenerStrCSV/";
				var obj = http("POST", url, periodo);
				return obj
			}

			function NotificarServiciosAConductor(servicios) {
				var url = "servicio/NotificarServiciosAConductor/";
				var obj = http("POST", url, servicios);
				return obj
			}
			function NotificarServiciosARuta(servicios) {
				var url = "servicio/NotificarServiciosARuta/";
				var obj = http("POST", url, servicios);
				return obj
			}

			function open (verb, url, data, target) {
				var form = document.createElement("form");
				form.action = url;
				form.method = verb;
				form.target = target || "_self";
				if (data) {
					for (var key in data) {
						var input = document.createElement("textarea");
						input.name = key;
						input.value = typeof data[key] === "object" ? JSON.stringify(data[key]) : data[key];
						form.appendChild(input);
					}
				}
				form.style.display = 'none';
				document.body.appendChild(form);
				form.submit();
			};

			function ObtenerPorPeriodoCSV(periodo) {
				var url = ngAuthSettings.apiServiceBaseUri + 'api/servicio/ObtenerPorPeriodoCSV'
				open('POST', url, periodo, '_blank');
			}

		});
})();

