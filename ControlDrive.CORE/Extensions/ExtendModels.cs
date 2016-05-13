using ControlDrive.CORE.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDrive.CORE.Extensions
{
    public static class ConductorExtension
    {
        public static string ToResumen(this Conductor conductor)
        {
            return (!string.IsNullOrEmpty(conductor.Nombre) ? conductor.Nombre : "") + " Tel:" + (!string.IsNullOrEmpty(conductor.Telefono1) ? " " + conductor.Telefono1 : "");
        }
    }

    public static class DireccionExtension
    {
        public static string ToResumen(this Direccion direccion)
        {
            return (!string.IsNullOrEmpty(direccion.Descripcion) ? direccion.Descripcion : "") 
                    + ", " + (!string.IsNullOrEmpty(direccion.Barrio) ? " " + direccion.Barrio : "")
                    + ", " + (!string.IsNullOrEmpty(direccion.Ciudad.Nombre) ? " " + direccion.Ciudad.Nombre : "");
        }
    }
}
