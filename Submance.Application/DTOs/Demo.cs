using System;

namespace Submance.Application.DTOs
{
    public class DemoDto
    {
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