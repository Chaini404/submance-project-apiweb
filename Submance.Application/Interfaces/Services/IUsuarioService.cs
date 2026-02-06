using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submance.Application.Interfaces.Services
{
    public interface IUsuarioService
    {
        IEnumerable<UsuarioDto> GetAll();
        UsuarioDto GetById(int id);

        void Create(CreateUsuarioRequest request);
        void Update(int id, UpdateUsuarioRequest request);
        void ChangeStatus(int id, bool enabled);
    }

}
