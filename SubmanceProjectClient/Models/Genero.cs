using System.ComponentModel.DataAnnotations;

namespace SubmanceProject.Web.Models
{
    public class Genero
    {
        [Key]
        public int IdGenero { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Estado { get; set; } = "Activo";
    }
}