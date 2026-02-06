using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submance.Application.DTOs.Revision
{
    public class RevisionResponseDto
    {
        public int IdRevision { get; set; }
        public string Cancion { get; set; }
        public string Revisor { get; set; }
        public string Resultado { get; set; }
        public string Observacion { get; set; }
        public DateTime FechaRevision { get; set; }
    }

}
