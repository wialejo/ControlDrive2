using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Security.Principal;
using ControlDrive.Core.Modelos;

namespace ControlDrive.CORE.Modelos
{
    public class ServicioDto
    {
        public int Id { get; set; }

        public string EstadoCodigo { get; set; }
        public virtual Estado Estado { get; set; }

        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public string Radicado { get; set; }
        public string AsignadoPor { get; set; }
        public int? VehiculoId { get; set; }
        public virtual Vehiculo Vehiculo { get; set; }
        public int AseguradoraId { get; set; }
        public virtual Aseguradora Aseguradora { get; set; }
        public int? AseguradoId { get; set; }
        public virtual Asegurado Asegurado { get; set; }

        public int? DireccionInicioId { get; set; }
        public virtual Direccion DireccionInicio { get; set; }

        public int? DireccionDestinoId { get; set; }
        public virtual Direccion DireccionDestino { get; set; }

        public int? ConductorId { get; set; }
        public virtual Conductor Conductor { get; set; }
        public string ConductorResumen { get; set; }

        public int? RutaId { get; set; }
        public virtual Conductor Ruta { get; set; }
        public string RutaResumen { get; set; }

        public DateTime FechaRegistro { get; internal set; }
        public DateTime FechaModificacion { get; internal set; }

        public int? UsuarioRegistroId { get; set; }
        public virtual ApplicationUser UsuarioRegistro { get; set; }

        public int? UsuarioModificacionId { get; set; }
        public virtual ApplicationUser UsuarioModificacion { get; set; }

        public ICollection<Seguimiento> Seguimientos { get; set; }

        public Valor valores { get; set; }

        public string NoFactura { get; set; }

    }

    public class ValorDto
    {
        public int ServicioId { get; set; }
        public decimal cierre { get; set; }
        public decimal ruta { get; set; }
        public decimal conductor { get; set; }
    }


    public class SeguimientoDto
    {
        public int Id { get; set; }
        public int ServicioId { get; set; }
        public DateTime Fecha { get; set; }
        public string Observacion { get; set; }
        public string UsuarioRegistroId { get; set; }
        public virtual ApplicationUser UsuarioRegistro { get; set; }
        public string NuevoEstado { get; set; }
    }


    public class AseguradoraDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }

    public class EstadoDto
    {
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public int Orden { get; set; }
    }

    public class ConductorDto
    {
        public ConductorDto() {
            Activo = true;
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string TipoIdentificacion { get; set; }
        public int  Identificacion { get; set; }
        public string Telefono1 { get; set; }
        public string Telefono2 { get; set; }
        public string Direccion { get; set; }
        
        public bool Activo { get; set; }

    }

    public class VehiculoDto
    {
        public int Id { get; set; }
        public string Placa { get; set; }
        public string Marca { get; set; }
        public string Referencia { get; set; }
        public string Observaciones { get; set; }
    }

    public class DireccionDto
    {
        public int Id { get; set; }

        public string Descripcion { get; set; }
       
        public int CiudadId { get; set; }

        public virtual Ciudad Ciudad { get; set; }

        public string Barrio { get; set; }
    }

    public class CiudadDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Tarifa { get; set; }
    }

    public class BarrioDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }

    public class AseguradoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Telefono1 { get; set; }
        public string Telefono2 { get; set; }

    }
}