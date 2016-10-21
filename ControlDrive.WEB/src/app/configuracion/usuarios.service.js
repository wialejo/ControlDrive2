(function() {
'use strict';
	angular.module('controldriveApp')
		.service('UsuarioSvc',  function ($http, $q, ManejadorErrores, ngAuthSettings) {
			return {
				Guardar: Guardar,
				ObtenerPorId: ObtenerPorId,
				ObtenerUsuarioActual: ObtenerUsuarioActual,
				Obtener: Obtener,
				Eliminar: Eliminar,
				AsignarSucursal: AsignarSucursal,
				AsignarRol: AsignarRol,
				DesAsignarSucursal: DesAsignarSucursal
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
			function Guardar(ciudad) {
				var url = "Usuarios/Guardar/";
				var obj = http('POST', url, ciudad );
				return obj;
			}
			function AsignarSucursal(idUsuario, idSucursal) {
			    var url = "Usuarios/" + idUsuario + "/AsignarSucursal/" + idSucursal;
			    var obj = http('GET', url);
				return obj;
			}
			function AsignarRol(idUsuario,nombreRol) {
			    var rol = { usuario: idUsuario, Rol: nombreRol };
			    var url = 'account/setRol';
			    var obj = http('POST', url, rol);
				return obj;
			}
			function AsignarRol2(idUsuario, nombreRol) {
			    var url = "Usuarios/" + idUsuario + "/AsignarRol/" + nombreRol;

			    //var rol = { usuario: idUsuario, Rol: nombreRol };
			    //var url = 'account/setRol';
			    var obj = http('GET', url);
			    return obj;
			}
			function DesAsignarSucursal(idUsuario, idSucursal) {
			    var url = "Usuarios/" + idUsuario + "/DesAsignarSucursal/" + idSucursal;
			    var obj = http('GET', url);
				return obj;
			}
			function ObtenerPorId(id) {
				var url = "Usuarios/ObtenerPorId/" + id;
				var obj = http("GET", url);
				return obj
			}
			function ObtenerUsuarioActual() {
			    var url = "Usuarios/ObtenerUsuarioActual";
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
		});
})();

