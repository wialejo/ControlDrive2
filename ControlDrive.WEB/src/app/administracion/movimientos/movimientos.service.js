(function () {
    'use strict';
    angular.module('controldriveApp')
        .service('MovimientosSvc', function ($http, $q, ManejadorErrores, ngAuthSettings, PeriodoSvc) {
            return {
                ObtenerMovimientosCliente: ObtenerMovimientosCliente,
                ObtenerMovimientosProveedor: ObtenerMovimientosProveedor,
                ObtenerMovimientosClienteAprobados: ObtenerMovimientosClienteAprobados,
                ObtenerMovimientosClienteCsv: ObtenerMovimientosClienteCsv,
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


            function ObtenerMovimientosClienteCsv(inicioPeriodo, finPeriodo, clienteId) {
                var periodo = PeriodoSvc.FormatearParaApi(inicioPeriodo, finPeriodo);

                var data = {
                    inicio: periodo.Inicio,
                    fin: periodo.Fin,
                    clienteId: clienteId
                }
                var url = ngAuthSettings.apiServiceBaseUri + "api/movimientos/clienteCsv";
                open('POST', url, data, '_blank');
            }

            function ObtenerMovimientosCliente(inicioPeriodo, finPeriodo, clienteId) {
                var periodo = PeriodoSvc.FormatearParaApi(inicioPeriodo, finPeriodo);
                var url = "movimientos/cliente?inicio=" + periodo.Inicio + "&fin=" + periodo.Fin + "&clienteId=" + clienteId;
                var obj = http("GET", url);
                return obj
            }

            function ObtenerMovimientosProveedor(inicioPeriodo, finPeriodo, proveedorId) {
                var periodo = PeriodoSvc.FormatearParaApi(inicioPeriodo, finPeriodo);
                var url = "movimientos/proveedor?inicio=" + periodo.Inicio + "&fin=" + periodo.Fin + "&proveedorId=" + proveedorId;
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