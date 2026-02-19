#nullable enable
using System.ComponentModel.DataAnnotations;

namespace SubmanceProject.Web.Models
{
    public class Artista
    {
        [Key]
        public int IdArtista { get; set; }
        [Required]
        public string NombreArtistico { get; set; } = string.Empty;
        [Required]
        public string NombreReal { get; set; } = string.Empty;
        public string? Pais { get; set; }
        [Required]
        public string Correo { get; set; } = string.Empty;
        public string Estado { get; set; } = "Activo";
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}