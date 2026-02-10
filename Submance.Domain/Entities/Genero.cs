using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Submance.Domain.Entities
{
    // C# busca la tabla "Genero", pero en tu SQL se llama "Generos". Esto lo arregla:
    [Table("Generos")]
    public class Genero
    {
        [Key]
        public int IdGenero { get; set; }

     
        [Column("Nombre")]
        public string NombreGenero { get; set; }

        public string Descripcion { get; set; }
        public bool Estado { get; set; }
    }
}