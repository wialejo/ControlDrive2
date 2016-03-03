namespace ControlDrive.CORE.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Correo")]
    public partial class Correo
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string CORasunto { get; set; }

        [Required]
        public string CORdestinatarios { get; set; }

        public string CORmensajeHTML { get; set; }
    }
}
