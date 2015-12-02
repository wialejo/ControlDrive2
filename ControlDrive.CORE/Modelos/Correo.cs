using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDrive.CORE.Modelos
{
    public class Correo
    {
        [Key]
        public int Id { get; set; }

        //[Required]
        //[StringLength(2)]
        //public string CORestado { get; set; }

        [Required]
        [StringLength(200)]
        public string CORasunto { get; set; }

        //[Required]
        //[StringLength(2)]
        //public string CORprioridad { get; set; }

        [Required]
        public string CORdestinatarios { get; set; }

        //public string CORdestinatariosCopia { get; set; }

        //public string CORdestinatariosCopiaOculta { get; set; }

        public string CORmensajeHTML { get; set; }

        //public string CORmensajeTexto { get; set; }

        //public DateTime CORfechaProgramacion { get; set; }

        //public bool CORenProceso { get; set; }

    }
}

