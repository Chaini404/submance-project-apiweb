using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Submance.Domain.Entities
{
    [Table("Generos")]
    public class Genero
    {
        [Key]
        public int IdGenero { get; set; }

        [Required]
        public string Nombre { get; set; }

        public bool Estado { get; set; } = true;
    }
}