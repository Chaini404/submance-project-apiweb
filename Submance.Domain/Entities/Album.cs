using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submance.Domain.Entities
{
    public class Album
    {
        [Key]
        public int IdAlbum { get; set; }

        [Required]
        [MaxLength(100)]
        public string Titulo { get; set; }

        public DateTime? FechaLanzamiento { get; set; }

        // FK
        public int IdArtista { get; set; }

        [ForeignKey(nameof(IdArtista))]
        public Artista Artista { get; set; }

        public ICollection<Cancion> Canciones { get; set; }
    }
}
