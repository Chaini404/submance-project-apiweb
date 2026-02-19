using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Submance.Domain.Entities
{
    [Table("Artistas")]
    public class Artista
    {
        [Key]
        public int IdArtista { get; set; }

        [Required]
        [ForeignKey("Usuario")]
        public int IdUsuario { get; set; }

        [Required]
        public string NombreArtistico { get; set; }

        public string NombreReal { get; set; }
        public string Pais { get; set; }
        public bool Estado { get; set; } = true;
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        // Navegación 1:1
        public virtual Usuario Usuario { get; set; }
    }
}