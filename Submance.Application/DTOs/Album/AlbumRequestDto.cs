using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submance.Application.DTOs.Album
{
    public class AlbumRequestDto
    {
        public string Titulo { get; set; }
        public DateTime? FechaLanzamiento { get; set; }
        public int IdArtista { get; set; }
    }

}
