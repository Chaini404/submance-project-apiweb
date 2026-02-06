using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submance.Application.DTOs.Artista
{
    public class ArtistaResponseDto
    {
        public int IdArtista { get; set; }
        public string NombreArtistico { get; set; }
        public string Correo { get; set; }
        public bool Estado { get; set; }
    }

}
