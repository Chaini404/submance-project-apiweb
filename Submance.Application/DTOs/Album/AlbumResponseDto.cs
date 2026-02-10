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

        // 👇 AGREGAMOS EL '= string.Empty;' AQUÍ
        public string Titulo { get; set; } = string.Empty;

        public DateTime? FechaLanzamiento { get; set; } // Este está bien porque tiene el '?' (es opcional)

        // 👇 Y AQUÍ TAMBIÉN
        public string Artista { get; set; } = string.Empty;
    }
}
