using Submance.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submance.Domain.Entities
{
    public class Revision
    {
        [Key]
        public int IdRevision { get; set; }

        public DateTime FechaRevision { get; set; } = DateTime.Now;

        [MaxLength(300)]
        public string Observacion { get; set; }

        [MaxLength(30)]
        public ResultadoRevision Resultado { get; set; }

        // FK
        public int IdCancion { get; set; }
        public int IdRevisor { get; set; }

        [ForeignKey(nameof(IdCancion))]
        public Cancion Cancion { get; set; }

        [ForeignKey(nameof(IdRevisor))]
        public Usuario Revisor { get; set; }
    }
}
