using System.ComponentModel.DataAnnotations;

namespace SubmanceProject.Web.Models
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Rol { get; set; } = "Admin";
        public string Estado { get; set; } = "Activo";
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}