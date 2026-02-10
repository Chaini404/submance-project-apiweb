using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submance.Application.DTOs.Cancion
{
    public class CancionResponseDto
    {
        public int IdCancion { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Genero { get; set; } = string.Empty;
        public string Artista { get; set; } = string.Empty;
        public string Album { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
    }

}
