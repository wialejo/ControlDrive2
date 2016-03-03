(function() {
	'use strict';
		angular.module('controldriveApp')
			.service('PeriodoSvc', function ($filter) {
				return {
					FormatearParaApi: FormatearParaApi,
					ObtenerPeriodoActual : ObtenerPeriodoActual
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

					if(horas < 6){
					  date.setDate(date.getDate() + 1);
					}

					var mes = (date.getMonth() + 1);
					mes = mes < 10 ? "0" + mes.toString() : mes;
					var dia = date.getDate();
					dia = dia < 10 ? "0" + dia.toString() : dia;

					var fechaSiguienteDia =  (dia + '/' + mes + '/' + date.getFullYear()); 
					return fechaSiguienteDia;
				}

			});
})();