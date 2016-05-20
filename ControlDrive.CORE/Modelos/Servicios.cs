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
using ControlDrive.CORE.Enums;

namespace ControlDrive.CORE.Modelos
{
    public class Servicio
    {
        [Key]
        public int Id { get; set; }

        public string EstadoCodigo { get; set; }
        public virtual Estado Estado { get; set; }

        [Required]
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public string Radicado { get; set; }
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

        public int? RutaId { get; set; }
        public virtual Conductor Ruta { get; set; }

        public DateTime FechaRegistro { get; internal set; }
        public DateTime FechaModificacion { get; internal set; }

        public string UsuarioRegistroId { get; set; }
        public virtual ApplicationUser UsuarioRegistro { get; set; }

        public string UsuarioModificacionId { get; set; }
        public virtual ApplicationUser UsuarioModificacion { get; set; }

        public ICollection<Seguimiento> Seguimientos { get; set; }
        public string AsignadoPor { get; set; }

        public ICollection<Movimiento> Movimientos { get; set; }

        public bool Notificado { get; set; }
    }

    public class Movimiento
    {
        [Key]
        public int Id { get; set; }
        public int ServicioId { get; set; }
        public decimal Valor { get; set; }
        public string ConceptoCodigo { get; set; }
        //public int? ProveedorId { get; set; }
        //public int? ClienteId { get; set; }
        public int? DocumentoId { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public string UsuarioRegistroId { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacionId { get; set; }
        public bool Aprobado { get; set; }

        public Servicio Servicio { get; set; }
        public ServicioConcepto Concepto { get; set; }
        //public Conductor Proveedor { get; set; }
        public Documento Documento { get; set; }
        //public Aseguradora Cliente { get; set; }
        public ApplicationUser UsuarioRegistro { get; set; }
        public ApplicationUser UsuarioModificacion { get; set; }
    }

    public class Documento
    {
        [Key]
        public int Id { get; set; }
        public string Tipo { get; set; }
        public string Numero { get; set; }
        public DateTime Fecha { get; set; }
        public string Concepto { get; set; }
        public decimal Valor { get; set; }
        public List<Movimiento> Movimientos { get; set; }
        public string UsuarioRegistroId { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int? ClienteId { get; set; }
        public Aseguradora Cliente { get; set; }
        public int? ProveedorId { get; set; }
        public Conductor Proveedor { get; set; }
        public ApplicationUser UsuarioRegistro { get; set; }
    }

    public class Empresa {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Nit { get; set; }
        public string Ciudad { get; set; }
    }

    public class ServicioConcepto
    {
        [Key]
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public TipoConcepto TipoConcepto { get; set; }
    }

    

    public class Seguimiento
    {
        [Key]
        public int Id { get; set; }
        public int ServicioId { get; set; }
        public DateTime Fecha { get; set; }
        public string Observacion { get; set; }
        public string UsuarioRegistroId { get; set; }
        public virtual ApplicationUser UsuarioRegistro { get; set; }
        public string NuevoEstado { get; set; }
    }

    public class Aseguradora
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
    }

    public class Estado
    {
        public Estado()
        {
            EnOperacion = true;
        }
        [Key]
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public int Orden { get; set; }

        [DefaultValue(true)]
        public bool EnOperacion { get; set; }
    }

    public class Conductor
    {
        public Conductor()
        {
            Activo = true;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string TipoIdentificacion { get; set; }
        public int Identificacion { get; set; }

        [Required]
        public string Telefono1 { get; set; }
        public string Telefono2 { get; set; }
        public string Direccion { get; set; }

        public bool Activo { get; set; }
    }

    public class Periodo
    {
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }
    }

    public class Vehiculo
    {
        [Key]
        public int Id { get; set; }

        public string Placa { get; set; }

        public string Descripcion { get; set; }
    }

    public class Direccion
    {
        [Key]
        public int Id { get; set; }

        public string Descripcion { get; set; }

        public int CiudadId { get; set; }

        [ForeignKey("CiudadId")]
        public virtual Ciudad Ciudad { get; set; }

        public string Barrio { get; set; }
    }

    public class Ciudad
    {
        public Ciudad()
        {
            Principal = false;
        }
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public bool Principal { get; set; }
        public string Tarifa { get; set; }
    }

    public class Barrio
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
    }

    public class Asegurado
    {
        [Key]
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Telefono1 { get; set; }

        public string Telefono2 { get; set; }

    }
}