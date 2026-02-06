using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submance.Application.Interfaces.Services
{
    public interface IArtistaService
    {
        IEnumerable<ArtistaDto> GetAll();
        ArtistaDto GetById(int id);

        void Create(CreateArtistaRequest request);
        void Update(int id, UpdateArtistaRequest request);
        void Delete(int id);
    }

}
