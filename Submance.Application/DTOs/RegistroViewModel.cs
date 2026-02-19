using System.ComponentModel.DataAnnotations;

namespace Submance.Application.ViewModels
{
    public class RegistroViewModel
    {
        [Required]
        public string Correo { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string NombreArtistico { get; set; }

        [Required]
        public string NombreReal { get; set; }

        public string Pais { get; set; }
    }
}