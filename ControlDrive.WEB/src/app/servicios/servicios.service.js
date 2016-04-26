(function () {
    'use strict';
    angular.module('controldriveApp')
		.service('ServicioSvc', function ($http, $q, ManejadorErrores, ngAuthSettings, PeriodoSvc) {
		    return {
		        Guardar: Guardar,
		        ObtenerPorId: ObtenerPorId,
		        Obtener: Obtener,
		        ObtenerParaSeguimiento: ObtenerParaSeguimiento,
		        ObtenerParaCierre: ObtenerParaCierre,
		        ObtenerParaFacturacion: ObtenerParaFacturacion,
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
		        var obj = http('POST', url, servicio);
		        return obj;
		    }

		    function ObtenerPorId(id) {
		        var url = "Servicio/ObtenerPorId/" + id;
		        var obj = http("GET", url);
		        return obj
		    }

		    function Obtener(inicioPeriodo, finPeriodo) {
		        var periodo = PeriodoSvc.FormatearParaApi(inicioPeriodo, finPeriodo);
		        var url = "servicios/rango?inicio=" + periodo.Inicio + "&fin=" + periodo.Fin;
		        var obj = http("GET", url);
		        return obj
		    }

		    function ObtenerParaSeguimiento(inicio) {
		        var inicio = PeriodoSvc.formatearFecha(inicio)
		        var url = "servicios/seguimiento?inicioPeriodo=" + inicio;
		        var obj = http("GET", url)
		        return obj
		    }

		    function ObtenerParaCierre(inicioPeriodo) {
		        var inicio = PeriodoSvc.formatearFecha(inicioPeriodo)
		        var url = "servicios/cierre?inicioPeriodo=" + inicio;
		        var obj = http("GET", url)
		        return obj
		    }

		    function ObtenerParaFacturacion(inicioPeriodo, finPeriodo) {
		        var periodo = PeriodoSvc.FormatearParaApi(inicioPeriodo, finPeriodo);
		        var url = "servicios/facturacion/rango?inicio=" + periodo.Inicio + "&fin=" + periodo.Fin;
		        var obj = http("GET", url)
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

		    function open(verb, url, data, target) {
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

		    function ObtenerPorPeriodoCSV(periodoIncial) {
		        var periodo = PeriodoSvc.FormatearParaApi(periodoIncial)
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

