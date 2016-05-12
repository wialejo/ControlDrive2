(function () {
    'use strict';
    angular.module('controldriveApp')
        .service('DocumentosSvc', function ($http, $q, ManejadorErrores, ngAuthSettings, PeriodoSvc) {
            return {
                ObtenerDocumentosCliente: ObtenerDocumentosCliente,
                ObtenerDocumentosRelacionServicios: ObtenerDocumentosRelacionServicios,
                Guardar: Guardar
            }
            function http(method, urlMetodo, data) {
                var request = $http({
                    method: method,
                    url: ngAuthSettings.apiServiceBaseUri + 'api/' + urlMetodo,
                    data: data
                });
                return (request.then(function (respuesta) { return respuesta }, ManejadorErrores.ResponseError));
            }

            function ObtenerDocumentosCliente(inicioPeriodo, finPeriodo, clienteId) {
                var periodo = PeriodoSvc.FormatearParaApi(inicioPeriodo, finPeriodo);
                var url = "documentos/cliente?inicio=" + periodo.Inicio + "&fin=" + periodo.Fin + "&clienteId=" + clienteId;
                var obj = http("GET", url);
                return obj
            }
            function ObtenerDocumentosRelacionServicios(documentosSeleccionados) {
                var url = "documentos/relacionServicios";
                var obj = http("POST", url, documentosSeleccionados);
                return obj
            }
            function Guardar(documento) {
                var url = "documentos";
                var obj = http("POST", url, documento);
                return obj
            }
        });
})();