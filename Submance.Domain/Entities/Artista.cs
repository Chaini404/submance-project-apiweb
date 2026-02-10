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
        public string NombreArtistico { get; set; }
        public string NombreReal { get; set; }
        public string Pais { get; set; }
        public string Correo { get; set; }

        public DateTime FechaRegistro { get; set; }

        // Esta es la columna REAL de la base de datos (bit/bool)
        public bool Estado { get; set; }

      
        [NotMapped]
        public bool Activo
        {
            get { return Estado; }
            set { Estado = value; }
        }
    }
}