using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submance.Application.DTOs.Album
{
    public class AlbumResponseDto
    {
        public int IdAlbum { get; set; }
        public string Titulo { get; set; }
        public DateTime? FechaLanzamiento { get; set; }
        public string Artista { get; set; }
    }

}
