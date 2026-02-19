namespace Submance.Domain.Entities
{
    public class Cancion
    {
        public int IdDemo { get; set; } // NO IdCancion
        public string Titulo { get; set; } = string.Empty;
        public string UrlAudio { get; set; } = string.Empty; // NO Archivo
        public int IdArtista { get; set; }
        public int IdGenero { get; set; }
        public string Estado { get; set; } = "Pendiente";
        public DateTime FechaEnvio { get; set; } = DateTime.UtcNow;
    }
}