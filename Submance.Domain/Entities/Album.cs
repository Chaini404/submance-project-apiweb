using System;
using System.Collections.Generic;

namespace Submance.Domain.Entities
{
    public class Album
    {
        public int IdAlbum { get; set; }
        public string Titulo { get; set; }
        public DateTime? FechaLanzamiento { get; set; }

        public int IdArtista { get; set; }

        // Navegación (Requiere Dapper Multi-Mapping para llenarse)
        public Artista Artista { get; set; }
        public ICollection<Cancion> Canciones { get; set; } = new List<Cancion>();
    }
}