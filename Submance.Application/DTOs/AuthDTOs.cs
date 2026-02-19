namespace Submance.Application.DTOs
{
    public class LoginViewModel { public string Correo { get; set; } public string Password { get; set; } }
    public class RegistroViewModel { public string NombreArtistico { get; set; } public string NombreReal { get; set; } public string Correo { get; set; } public string Password { get; set; } }
    public class ArtistaRequest
    {
        public string NombreArtistico { get; set; }
        public string NombreReal { get; set; }
        public string Correo { get; set; }
        public string Pais { get; set; }
    }
    public class DemoRequest
    {
        public string Titulo { get; set; }
        public string UrlAudio { get; set; }
        public int IdArtista { get; set; }
        public int IdGenero { get; set; }
    }
}