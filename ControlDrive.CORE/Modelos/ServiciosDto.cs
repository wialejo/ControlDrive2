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
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ControlDrive.CORE.Modelos
{
    public class ServicioDto
    {
        public ServicioDto() {
            Movimientos = new List<MovimientoDto>();
        }
        public int Id { get; set; }

        public TipoServicioDto TipoServicio { get; set; }

        public string EstadoCodigo { get; set; }
        public virtual Estado Estado { get; set; }

        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public string Radicado { get; set; }
        public string AsignadoPor { get; set; }
        public int? VehiculoId { get; set; }
        public Vehiculo Vehiculo { get; set; }
        public int? AseguradoraId { get; set; }
        public Aseguradora Aseguradora { get; set; }
        public int? AseguradoId { get; set; }
        public Asegurado Asegurado { get; set; }

        public int? DireccionInicioId { get; set; }
        public Direccion DireccionInicio { get; set; }
        public string DireccionInicioStr { get; set; }

        public int? DireccionDestinoId { get; set; }
        public Direccion DireccionDestino { get; set; }
        public string DireccionDestinoStr { get; set; }

        public int? ConductorId { get; set; }
        public virtual Conductor Conductor { get; set; }
        public string ConductorResumen { get; set; }

        public int? RutaId { get; set; }
        public Conductor Ruta { get; set; }
        public string RutaResumen { get; set; }

        public string Observacion { get; set; }

        public DateTime FechaRegistro { get; internal set; }
        public DateTime FechaModificacion { get; internal set; }

        public int? UsuarioRegistroId { get; set; }
        public ApplicationUserDto UsuarioRegistro { get; set; }
        public int? UsuarioModificacionId { get; set; }
        public ApplicationUserDto UsuarioModificacion { get; set; }
        public ICollection<SeguimientoDto> Seguimientos { get; set; }
        public ICollection<MovimientoDto> Movimientos { get; set; }
        public bool Notificado { get; set; }

    }

    public class TipoServicioDto
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public bool RequiereSeguimiento { get; set; }
        public ICollection<ConceptoPago> ConceptosPagos { get; internal set; }
    }

    public class DocumentoDto
    {
        [Key]
        public int Id { get; set; }
        public string Tipo { get; set; }
        public string Numero { get; set; }
        public DateTime Fecha { get; set; }
        public string Concepto { get; set; }
        public decimal Valor { get; set; }
        public List<MovimientoDto> Movimientos { get; set; }
        public string UsuarioRegistroId { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int ClienteId { get; set; }
    }

    public class MovimientoDto
    {
        public int Id { get; set; }
        public ServicioDto Servicio { get; set; }
        public Conductor Proveedor { get; set; }
        public DocumentoDto Documento { get; set; }
        public int ServicioId { get; set; }
        public decimal Valor { get; set; }
        public string ConceptoCodigo { get; set; }
        public int? ProveedorId { get; set; }
        public int? ClienteId { get; set; }
        public int? DocumentoId { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public string UsuarioRegistroId { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacionId { get; set; }
        public bool Aprobado { get; set; }
        public ServicioConceptoDto Concepto { get; internal set; }
    }

    public class ServicioConceptoDto
    {
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
    }

    public class SeguimientoDto
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Observacion { get; set; }
        public virtual ApplicationUserDto UsuarioRegistro { get; set; }
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

    public class ApplicationUserDto {
        public string Nombre { get; set; }
    }
}