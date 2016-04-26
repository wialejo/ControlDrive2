using ControlDrive.Core.Infraestructura;
using ControlDrive.Core.Modelos;
using ControlDrive.CORE.Modelos;
using ControlDrive.CORE.Repositorios;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDrive.CORE.Services
{
    public class PeriodoService
    {
        public Periodo Obtener(DateTime fecha)
        {
            Periodo periodo = new Periodo();
            TimeSpan ti = new TimeSpan(14, 0, 0);
            periodo.Inicio = fecha.Date + ti;

            TimeSpan tf = new TimeSpan(13, 59, 0);
            periodo.Fin = fecha.Date.AddDays(1) + tf;
            return periodo;
        }

        public bool FechaEnPeriodoActual(DateTime fecha)
        {
            var periodoActual = Obtener(DateTime.Now.Hour < 14 ? DateTime.Now.AddDays(-1) : DateTime.Now);
            if (fecha >= periodoActual.Inicio && fecha <= periodoActual.Fin)
                return true;
            else
                return false;
        }
    }
}   
