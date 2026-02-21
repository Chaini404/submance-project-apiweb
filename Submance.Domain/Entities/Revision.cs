using Submance.Domain.Enums;
using System;

namespace Submance.Domain.Entities
{
    public class Revision
    {
        public int IdRevision { get; set; }
        public DateTime FechaRevision { get; set; } = DateTime.UtcNow;
        public string Observacion { get; set; } = string.Empty;
        public ResultadoRevision Resultado { get; set; } = ResultadoRevision.Pendiente;

        public int IdDemo { get; set; }
        public int IdRevisor { get; set; }

        // Navegación
        public Cancion Demo { get; set; }
        public Usuario Revisor { get; set; }
    }
}