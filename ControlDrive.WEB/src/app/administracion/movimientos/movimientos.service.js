(function () {
    'use strict';
    angular.module('controldriveApp')
        .service('MovimientosSvc', function ($http, $q, ManejadorErrores, ngAuthSettings, PeriodoSvc) {
            return {
                ObtenerMovimientosCliente: ObtenerMovimientosCliente,
                ObtenerMovimientosClienteAprobados: ObtenerMovimientosClienteAprobados,
                ObtenerPorId: ObtenerPorId,
                Guardar: Guardar,
                Actualizar: Actualizar
            }
            function http(method, urlMetodo, data) {
                var request = $http({
                    method: method,
                    url: ngAuthSettings.apiServiceBaseUri + 'api/' + urlMetodo,
                    data: data
                });
                return (request.then(function (respuesta) { return respuesta }, ManejadorErrores.ResponseError));
            }

            function ObtenerMovimientosCliente(inicioPeriodo, finPeriodo, clienteId) {
                var periodo = PeriodoSvc.FormatearParaApi(inicioPeriodo, finPeriodo);
                var url = "movimientos/cliente?inicio=" + periodo.Inicio + "&fin=" + periodo.Fin + "&clienteId=" + clienteId;
                var obj = http("GET", url);
                return obj
            }

            function ObtenerMovimientosClienteAprobados(inicioPeriodo, finPeriodo, clienteId) {
                var periodo = PeriodoSvc.FormatearParaApi(inicioPeriodo, finPeriodo);
                var url = "movimientos/cliente?aprobado=true&inicio=" + periodo.Inicio + "&fin=" + periodo.Fin + "&clienteId=" + clienteId ;
                var obj = http("GET", url);
                return obj
            }
            function ObtenerPorId(movimientoId) {
                var url = "movimientos/" + movimientoId;
                var obj = http("GET", url);
                return obj
            }
            function Actualizar(movimiento) {
                var url = "movimientos";
                var obj = http("PUT", url, movimiento);
                return obj
            }
            function Guardar(movimiento) {
                var url = "movimientos";
                var obj = http("POST", url, movimiento);
                return obj
            }
        });
})();