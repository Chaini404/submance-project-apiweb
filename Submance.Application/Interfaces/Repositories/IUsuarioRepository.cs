using System;
using System.Collections.Generic;
using System.Linq;
using Submance.Domain.Entities;
using System.Text;
using System.Threading.Tasks;

namespace Submance.Application.Interfaces.Repository
{
    public interface IUsuarioRepository
    {
        Task<IEnumerable<Usuario>> GetAllAsync();
        Task<Usuario?> GetByIdAsync(int id);
        Task<Usuario?> GetByCorreoAsync(string correo);

        Task Add(Usuario usuario);
        Task Update(Usuario usuario);
        Task Delete(int id);

        // --- 👇 👇 ---
        Task<Usuario?> GetByUsernameAsync(string nombre);
    }
}
