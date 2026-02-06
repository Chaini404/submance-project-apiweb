using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submance.Application.DTOs.Cancion
{
    public class CancionRequestDto
    {
        public string Titulo { get; set; }
        public TimeSpan? Duracion { get; set; }
        public string Archivo { get; set; }
        public int? IdAlbum { get; set; }
        public int IdGenero { get; set; }
        public int IdArtista { get; set; }
    }

}
