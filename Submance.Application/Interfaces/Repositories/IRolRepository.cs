#nullable enable
using Submance.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Submance.Application.Interfaces.Repositories
{
    public interface IRolRepository
    {
        Task<IEnumerable<Rol>> GetAllAsync();
        Task<Rol?> GetByIdAsync(int id);
    }
}