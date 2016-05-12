(function() {
  'use strict';

  angular.module('controldriveApp')
      .factory('FechaSvc', function () {
        
       return {
            ObtenerActual: ObtenerActual,
            AdicionarMes:AdicionarMes,
            Formatear:Formatear
        };

        function ObtenerActual () {
            var date = new Date();
            var mes = (date.getMonth() + 1);
            mes = mes < 10 ? "0" + mes.toString() : mes;
            var dia = date.getDate();
            dia = dia < 10 ? "0" + dia.toString() : dia;
            return (dia + '/' + mes + '/' + date.getFullYear());
        }
        function Formatear(date) {
            var mes = (date.getMonth() + 1);
            mes = mes < 10 ? "0" + mes.toString() : mes;
            var dia = date.getDate();
            dia = dia < 10 ? "0" + dia.toString() : dia;
            return (dia + '/' + mes + '/' + date.getFullYear());

        }

        function AdicionarMes(meses) {
            var date = new Date();
            var mes = (date.getMonth() + 1 ) + (meses);
            mes = mes < 10 ? "0" + mes.toString() : mes;
            var dia = date.getDate();
            dia = dia < 10 ? "0" + dia.toString() : dia;
            return (dia + '/' + mes + '/' + date.getFullYear());
        }
    });
})();