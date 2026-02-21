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
        public string NombreGenero { get; set; } = string.Empty;

        public string Descripcion { get; set; } = string.Empty;
    }
}