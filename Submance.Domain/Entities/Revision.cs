using Submance.Domain.Enums;
using System;

namespace Submance.Domain.Entities
{
    public class Revision
    {
        public int IdRevision { get; set; }
        public DateTime FechaRevision { get; set; } = DateTime.Now;
        public string Observacion { get; set; }
        public ResultadoRevision Resultado { get; set; }

        public int IdCancion { get; set; }
        public int IdRevisor { get; set; }

        // Navegación
        public Cancion Cancion { get; set; }
        public Usuario Revisor { get; set; }
    }
}