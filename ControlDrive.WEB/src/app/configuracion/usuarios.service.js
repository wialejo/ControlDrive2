(function () {
    'use strict';
    angular.module('controldriveApp')
		.service('UsuarioSvc', function ($http, $q, ManejadorErrores, ngAuthSettings, localStorageService) {
		    return {
		        Guardar: Guardar,
		        ObtenerPorId: ObtenerPorId,
		        Obtener: Obtener,
		        Eliminar: Eliminar,
		        AsignarSucursal: AsignarSucursal,
		        DesAsignarSucursal: DesAsignarSucursal,
		        EstablecerTempUsuarioActual: EstablecerTempUsuarioActual,
		        ObtenerActual: ObtenerActual,
		        ObtenerSucursales: ObtenerSucursales,
		        ObtenerSucursalActual: ObtenerSucursalActual,
		        EstablecerSucursalActual: EstablecerSucursalActual,
		        RemoverUsuarioActual: RemoverUsuarioActual
		    }
		    function http(method, urlMetodo, data) {
		        var request = $http({
		            method: method,
		            url: ngAuthSettings.apiServiceBaseUri + 'api/' + urlMetodo,
		            headers: {
		                'Access-Control-Allow-Origin': true
		            },
		            data: data
		        });
		        return (request.then(function (respuesta) { return respuesta }, ManejadorErrores.ResponseError));
		    }
		    function Guardar(ciudad) {
		        var url = "Usuarios/Guardar/";
		        var obj = http('POST', url, ciudad);
		        return obj;
		    }
		    function ObtenerPorId(id) {
		        var url = "Usuarios/ObtenerPorId/" + id;
		        var obj = http("GET", url);
		        return obj
		    }
		    function Obtener() {
		        var url = "Usuarios/Obtener/";
		        var obj = http("GET", url);
		        return obj
		    }
		    function Eliminar(id) {
		        var url = "Usuarios/Eliminar/" + id;
		        var obj = http("DELETE", url);
		        return obj
		    }
		    function AsignarSucursal(idUsuario, idSucursal) {
		        var url = "Usuarios/" + idUsuario + "/AsignarSucursal/" + idSucursal;
		        var obj = http('GET', url);
		        return obj;
		    }
		    function DesAsignarSucursal(idUsuario, idSucursal) {
		        var url = "Usuarios/" + idUsuario + "/DesAsignarSucursal/" + idSucursal;
		        var obj = http('GET', url);
		        return obj;
		    }
		    //Temporales

		    function EstablecerTempUsuarioActual() {
		        var deferred = $q.defer();
		        var url = "Usuarios/ObtenerUsuarioActual";
		        http("GET", url).then(function (response) {
		            var usuario = response.data;
		            usuario.SucursalActual = usuario.Sucursales[0];
		            localStorageService.set('currentUser', usuario);
		            //function wait(ms) {
		            //    var start = new Date().getTime();
		            //    var end = start;
		            //    while (end < start + ms) {
		            //        end = new Date().getTime();
		            //    }
		            //} wait(10000);

		            deferred.resolve(response);
		        }).catch(function (error) {
		            deferred.reject(error);
		        });
		        return deferred.promise;
		    }
		    function ObtenerActual() {
		        return localStorageService.get('currentUser')
		    }
		    function ObtenerSucursales() {
		        return localStorageService.get('currentUser').Sucursales;
		    }
		    function ObtenerSucursalActual() {
		        return localStorageService.get('currentUser').SucursalActual
		    }
		    function EstablecerSucursalActual(sucursal) {
		        var usuarioActual = localStorageService.get('currentUser')
		        usuarioActual.SucursalActual = sucursal;
		        localStorageService.set('currentUser', usuarioActual);
		    }
		    function RemoverUsuarioActual() {
		        localStorageService.remove('currentUser');
		    }
		});
})();

