using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Submance.Domain.Entities;
using System.Threading.Tasks;

namespace Submance.Application.Interfaces.Repositories
{
    public interface IRevisionRepository
    {
        Task<IEnumerable<Revision>> GetAllAsync();
        Task<Revision> GetByIdAsync(int id);

        Task<IEnumerable<Revision>> GetByCancionAsync(int idCancion);

        Task Add(Revision revision);
    }
}
