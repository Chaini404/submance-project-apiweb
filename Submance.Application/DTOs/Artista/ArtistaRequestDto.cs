using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submance.Application.DTOs.Artista
{
    public class ArtistaRequestDto
    {
        public string NombreArtistico { get; set; } = string.Empty; // 👈 ¡Agrega esto!
        public string NombreReal { get; set; } = string.Empty;      // 👈 ¡Agrega esto!
        public string Correo { get; set; } = string.Empty;          // 👈 ¡Agrega esto!
    }

}
