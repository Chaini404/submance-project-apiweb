using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Submance.Domain.Entities
{
    [Table("Demos")]
    public class Demo
    {
        [Key]
        public int IdDemo { get; set; }

        
        public string? TituloDemo { get; set; }

        public string? NombreArtistico { get; set; }

        public string? Estilo { get; set; }

        public string? LinkDemo { get; set; }

        public string? Email { get; set; }

        public string? Estado { get; set; }
        public DateTime? FechaLanzamiento { get; set; }

        public DateTime? FechaEnvio { get; set; }
    }
}