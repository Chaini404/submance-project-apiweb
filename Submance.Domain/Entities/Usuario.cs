using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submance.Domain.Entities
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty; // 👈 Agrega esto

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Correo { get; set; } = string.Empty; // 👈 Agrega esto

        [Required]
        [MaxLength(100)]
        public string Password { get; set; } = string.Empty; // 👈 Agrega esto

        public bool Activo { get; set; } = true;

        public int IdRol { get; set; }

        [ForeignKey(nameof(IdRol))]
        public Rol Rol { get; set; }

        public ICollection<Revision> Revisiones { get; set; }
    }
}