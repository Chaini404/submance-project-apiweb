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
        public string Titulo { get; set; }
        public string Genero { get; set; }
        public string Artista { get; set; }
        public string Album { get; set; }
        public string Estado { get; set; }
    }

}
