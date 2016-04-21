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
				NotificarServiciosAConductor: NotificarServiciosAConductor,
				GuardarValores: GuardarValores,
				Cerrar: Cerrar,
			    Facturar: Facturar
			}
			function http(method, urlMetodo, data) {
				var request = $http({
				    method: method,
				    url: ngAuthSettings.apiServiceBaseUri + 'api/' + urlMetodo,
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
			function Obtener(fechaInicial, fechaFinal, estado) {
			    var url = "servicio" + (estado ? "?estado=" + estado : "");
				fechaFinal = fechaFinal + " 23:00"
				var periodo = PeriodoSvc.FormatearParaApi(fechaInicial,fechaFinal);

				var obj = http("POST", url, periodo);
				return obj
			}

			function ObtenerParaSeguimiento(inicio) {
			    var inicio = PeriodoSvc.formatearFecha(inicio)
			    var url = "seguimientos/servicios/rango?startDate=" + inicio;
				var obj = http("GET", url)
				return obj
			}
			function ObtenerStrCSV(periodo) {
				var url = "servicio/ObtenerStrCSV/";
				var obj = http("POST", url, periodo);
				return obj
			}

			function NotificarServiciosAConductor(servicios, imprimir) {
			    var url = "servicio/NotificarServiciosAConductor/";
			    if (imprimir)
			        url = "servicio/ObtenerHtmlServiciosAConductor/";

				var obj = http("POST", url, servicios);
				return obj
			}

			function NotificarServiciosARuta(servicios, imprimir) {
			    var url = "servicio/NotificarServiciosARuta/";
			    if (imprimir)
			        url = "servicio/ObtenerHtmlServiciosARuta/";
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

			function Cerrar(servicioId, valores) {
			    var url = "servicio/" + servicioId + "/cerrar";
			    var obj = http('PUT', url, valores);
			    return obj;
			}

			function GuardarValores(servicioId, valores) {
			    var url = "servicio/" + servicioId + "/valores";
			    var obj = http('PUT', url, valores);

			    return obj;
			}

			function Facturar(servicioId, noFactura) {
			    var url = "servicio/" + servicioId + "/facturar/" + noFactura;
			    var obj = http('PUT', url);
			    return obj;
			}
		});
})();

