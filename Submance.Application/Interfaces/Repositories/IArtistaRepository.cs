using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Submance.Domain.Entities;
using System.Threading.Tasks;

namespace Submance.Application.Interfaces.Repository
{
    public interface IArtistaRepository
    {
        Task<IEnumerable<Artista>> GetAllAsync();
        Task<Artista?> GetByIdAsync(int id);

        Task AddAsync(Artista artista);
        Task UpdateAsync(Artista artista);
        Task DeleteAsync(int id);
    }
}
