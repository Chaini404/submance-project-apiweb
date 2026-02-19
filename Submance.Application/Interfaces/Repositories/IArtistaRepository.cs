#nullable enable
using Submance.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Submance.Application.Interfaces.Repositories
{
    public interface IArtistaRepository
    {
        Task<IEnumerable<Artista>> GetAllAsync();
        Task<Artista?> GetByIdAsync(int id);
        Task<Artista?> GetByEmailAsync(string email);

        Task AddAsync(Artista artista);
        Task UpdateAsync(Artista artista);
        Task DeleteAsync(int id);
    }
}