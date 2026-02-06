using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submance.Domain.Entities
{
    public class Artista
    {
        [Key]
        public int IdArtista { get; set; }

        [Required]
        [MaxLength(100)]
        public string NombreArtistico { get; set; }

        [MaxLength(100)]
        public string NombreReal { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Correo { get; set; }

        public bool Activo { get; set; } = true;

        public ICollection<Album> Albums { get; set; }
        public ICollection<Cancion> Canciones { get; set; }
    }
}
