using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submance.Application.Interfaces.Services
{
    public interface IGeneroService
    {
        IEnumerable<GeneroDto> GetAll();
        GeneroDto GetById(int id);

        void Create(CreateGeneroRequest request);
        void Update(int id, UpdateGeneroRequest request);
        void Delete(int id);
    }

}
