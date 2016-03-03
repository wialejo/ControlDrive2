﻿using Microsoft.AspNet.Identity.EntityFramework;
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

        public int? UsuarioRegistroId { get; set; }
        public virtual ApplicationUser UsuarioRegistro { get; set; }

        public int? UsuarioModificacionId { get; set; }
        public virtual ApplicationUser UsuarioModificacion { get; set; }

        public ICollection<Seguimiento> Seguimientos { get; set; }
        public string AsignadoPor { get; set; }
    }

    [Table("Seguimientos")]
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
        [Key]
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public int Orden { get; set; }
    }

    public class Conductor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string TipoIdentificacion { get; set; }
        public int  Identificacion { get; set; }

        [Required]
        public string Telefono1 { get; set; }
        public string Telefono2 { get; set; }
        public string Direccion { get; set; }

        [DefaultValue("true")]
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

        public string Marca { get; set; }

        public string Referencia { get; set; }

        public string Observaciones { get; set; }
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
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
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