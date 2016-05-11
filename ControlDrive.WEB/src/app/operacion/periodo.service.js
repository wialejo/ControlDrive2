(function() {
	'use strict';
		angular.module('controldriveApp')
			.service('PeriodoSvc', function ($filter, $http, ngAuthSettings) {
				return {
					FormatearParaApi: FormatearParaApi,
					ObtenerPeriodoActual: ObtenerPeriodoActual,
					formatearFecha: formatearFecha,
					FechaEnPeriodoActual: FechaEnPeriodoActual
				}
				function http(method, urlMetodo, data) {
				    var request = $http({
				        method: method,
				        url: ngAuthSettings.apiServiceBaseUri + 'api/' + urlMetodo,
				        data: data
				    });
				    return request;
				}

				function formatearFecha(fechaStr){
					var d = fechaStr.split("/").slice(0, 3).reverse();
					var fecha = new Date(d[1] + "/" + d[2] + "/" + d[0])
					
					var fechaFormateada =  $filter('date')(fecha, 'yyyy-MM-dd HH:mm:ss');
					return fechaFormateada;
				}

				function FormatearParaApi(fechaInicial, fechaFinal) {
					var Fecha = { 
						Inicio : formatearFecha(fechaInicial)
					}
					if(fechaFinal){
						Fecha.Fin = formatearFecha(fechaFinal);
					}

					return Fecha;
				}
				
				function ObtenerPeriodoActual() {
					var date = new Date();
					var horas = date.getHours();
                    //Si la hora actual es mejor a las 6am mantiene la fecha del dia anterior para conservar el periodo
					if (horas < 6 ) {
					  date.setDate(date.getDate() - 1);
					}

					var mes = (date.getMonth() + 1);
					mes = mes < 10 ? "0" + mes.toString() : mes;
					var dia = date.getDate();
					dia = dia < 10 ? "0" + dia.toString() : dia;

					var fechaSiguienteDia =  (dia + '/' + mes + '/' + date.getFullYear()); 
					return fechaSiguienteDia;
				}

				function FechaEnPeriodoActual(fecha) {
				    var url = "periodo/FechaEnPeriodoActual?fecha=" + fecha;
				    var obj = http("GET", url);
				    return obj
				}
			});
})();