using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Submance.Domain.Entities
{
    [Table("Usuarios")]
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Correo { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Rol { get; set; }

        public string Estado { get; set; } = "Activo";
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        // Navegación 1:1
        public virtual Artista Artista { get; set; }
    }
}