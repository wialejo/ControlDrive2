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
            return conductor != null ? ((!string.IsNullOrEmpty(conductor.Nombre) ? conductor.Nombre : "") 
                + " Tel:" + (!string.IsNullOrEmpty(conductor.Telefono1) ? " " + conductor.Telefono1 : "")) : "";
        }
    }

    public static class DireccionExtension
    {
        public static string ToResumen(this Direccion direccion)
        {
            return direccion != null ? ((!string.IsNullOrEmpty(direccion.Descripcion) ? direccion.Descripcion : "")
                    + ", " + (!string.IsNullOrEmpty(direccion.Barrio) ? " " + direccion.Barrio : "")
                    + ", " + (!string.IsNullOrEmpty(direccion.Ciudad.Nombre) ? " " + direccion.Ciudad.Nombre : "")) : "";
        }
    }

    public static class VehiculoExtension
    {
        public static string ToResumen(this Vehiculo vehiculo)
        {
            return vehiculo != null ? ((!string.IsNullOrEmpty(vehiculo.Placa) ? vehiculo.Placa : "")
                    + (!string.IsNullOrEmpty(vehiculo.Descripcion) ? " " + vehiculo.Descripcion : "")) : "";
        }
    }
}
