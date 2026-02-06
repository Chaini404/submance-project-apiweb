using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submance.Domain.Entities
{
    public class Genero
    {
        [Key]
        public int IdGenero { get; set; }

        [Required]
        [MaxLength(50)]
        public string NombreGenero { get; set; }

        [MaxLength(150)]
        public string Descripcion { get; set; }

        public ICollection<Cancion> Canciones { get; set; }
    }
}
