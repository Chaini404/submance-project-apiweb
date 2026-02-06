using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submance.Application.DTOs.Revision
{
    public class RevisionRequestDto
    {
        public int IdCancion { get; set; }
        public int IdRevisor { get; set; }
        public string Observacion { get; set; }
        public string Resultado { get; set; } // Aprobada / Rechazada
    }

}
