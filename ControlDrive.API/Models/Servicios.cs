using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ControlDrive.API.Models
{
    public class Servicio    {
        
        public int Id { get; set; }

        [Required]
        public string EstadoCodigo { get; set; }

        public virtual Estado Estado { get; set; }

        [Required]
        public DateTime Fecha { get; set; }
        
        public string Radicado { get; set; }

        public int? VehiculoId { get; set; }

        public virtual Vehiculo Vehiculo { get; set; }

        public int AseguradoraId { get; set; }

        public virtual Aseguradora Aseguradora { get; set; }

        
        public int? AseguradoId { get; set; }

        public virtual Asegurado Asegurado { get; set; }


        public virtual Direccion DireccionInicio { get; set; }

        public virtual Direccion DireccionDestino { get; set; }


        public int? ConductorId { get; set; }

        public virtual Conductor Conductor { get; set; }
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
    }

    public class Conductor
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        public int  Identificacion { get; set; }    

        public ICollection<int> Telefonos { get; set; }
    }

    

    public class Vehiculo
    {
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

        public string Barrio { get; set; }
    }

    public class Barrio
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }

    public class Asegurado
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

       // public ICollection<int> Telefonos { get; set; }

    }
}