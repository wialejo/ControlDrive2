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
        public Periodo Obtener(DateTime Fecha)
        {
            Periodo periodo = new Periodo();
            TimeSpan ti = new TimeSpan(18, 0, 0);
            periodo.Inicio = Fecha.Date + ti;

            TimeSpan tf = new TimeSpan(17, 59, 0);
            periodo.Fin = Fecha.Date.AddDays(1) + tf;
            return periodo;
        }
    }
}   
