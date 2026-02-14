using System.ComponentModel.DataAnnotations;

namespace Submance.Application.DTOs.Usuario
{
    public class RegistroArtistaDto
    {
        [Required]
        public string NombreArtistico { get; set; } = string.Empty; // Para tabla Artistas

        [Required]
        public string NombreReal { get; set; } = string.Empty;      // Para tabla Usuarios y Artistas

        [Required, EmailAddress]
        public string Correo { get; set; } = string.Empty;          // El vínculo entre ambas tablas

        [Required]
        public string Password { get; set; } = string.Empty;        // Para tabla Usuarios

        public string Pais { get; set; } = string.Empty;            // Para tabla Artistas
    }
}