#nullable enable
using Submance.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Submance.Application.Interfaces.Repositories
{
    public interface IRevisionRepository
    {
        Task<IEnumerable<Revision>> GetAllAsync();
        Task<Revision?> GetByIdAsync(int id);
        Task<IEnumerable<Revision>> GetByCancionAsync(int idCancion);

        Task AddAsync(Revision revision);
    }
}