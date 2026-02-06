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
    public class Cancion
    {
        [Key]
        public int IdCancion { get; set; }

        [Required]
        [MaxLength(100)]
        public string Titulo { get; set; }

        public TimeSpan? Duracion { get; set; }

        [MaxLength(200)]
        public string Archivo { get; set; }

        public EstadoCancion Estado { get; set; } = EstadoCancion.Pendiente;

        // FK
        public int? IdAlbum { get; set; }
        public int IdGenero { get; set; }
        public int IdArtista { get; set; }

        [ForeignKey(nameof(IdAlbum))]
        public Album Album { get; set; }

        [ForeignKey(nameof(IdGenero))]
        public Genero Genero { get; set; }

        [ForeignKey(nameof(IdArtista))]
        public Artista Artista { get; set; }

        public ICollection<Revision> Revisiones { get; set; }
    }
}
