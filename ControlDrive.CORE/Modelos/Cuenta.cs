namespace ControlDrive.CORE.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Cuenta")]
    public partial class Cuenta
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Descripcion { get; set; }

        [Required]
        [StringLength(100)]
        public string CorreoSalida { get; set; }

        [Required]
        [StringLength(150)]
        public string NombreMostrar { get; set; }

        [StringLength(100)]
        public string CorreoRespuesta { get; set; }

        [StringLength(100)]
        public string CorreoConfirmacionLectura { get; set; }

        [Required]
        [StringLength(50)]
        public string NombreServidor { get; set; }

        [StringLength(50)]
        public string NombreServidorIMAP { get; set; }

        [StringLength(50)]
        public string NombreServidorPOP { get; set; }

        [Required]
        [StringLength(50)]
        public string NombreServidorSMPT { get; set; }

        public int Puerto { get; set; }

        public bool Ssl { get; set; }

        [Required]
        [StringLength(50)]
        public string Usuario { get; set; }

        [Required]
        [StringLength(50)]
        public string Contrasena { get; set; }
    }
}
