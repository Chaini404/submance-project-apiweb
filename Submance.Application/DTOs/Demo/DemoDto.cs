using System;

namespace Submance.Application.DTOs.Demo
{
    public class DemoDto
    {
        public int IdDemo { get; set; }

        // Nombres OFICIALES
        public string NombreArtistico { get; set; } = string.Empty; // Antes: NombreArtista
        public string NombreTrack { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;           // Antes: EmailContacto
        public string LinkAudio { get; set; } = string.Empty;       // Antes: UrlAudio

        public string Estado { get; set; } = "Pendiente";
        public DateTime FechaEnvio { get; set; } = DateTime.Now;
    }
}