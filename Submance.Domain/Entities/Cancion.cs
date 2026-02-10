#nullable disable // <--- ESTO MATA LAS ADVERTENCIAS AMARILLAS
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Submance.Domain.Entities
{
    [Table("Cancion")]
    public class Cancion
    {
        [Key]
        public int IdCancion { get; set; }

        public string Titulo { get; set; }

        public string Archivo { get; set; } // Link del demo

        // Relaciones
        public int IdArtista { get; set; }
        public int IdGenero { get; set; }

        // --- AQUÍ FALTABA ESTO ---
        // El error decía que no existía "Duracion". Aquí la agregamos.
        // En SQL es TIME, en C# usamos TimeSpan? (el ? permite nulos)
        public TimeSpan? Duracion { get; set; }

        public string Version { get; set; } = "Original Mix";
        public int Tempo { get; set; } = 138;
        public int ClaveMusical { get; set; } = 1;

        public string Estado { get; set; }
        public DateTime FechaEnvio { get; set; }

        public bool Activo { get; set; }

        [NotMapped]
        public int? IdAlbum { get; set; }
    }
}