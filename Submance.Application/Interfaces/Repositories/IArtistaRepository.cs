using Submance.Domain.Entities;

namespace Submance.Application.Interfaces.Repository
{
    public interface IArtistaRepository
    {
        Task<IEnumerable<Artista>> GetAllAsync();
        Task<Artista?> GetByIdAsync(int id);

        // 👇 ESTE ES EL NUEVO MÉTODO QUE NECESITAMOS
        Task<Artista?> GetByEmailAsync(string email);

        Task AddAsync(Artista artista);
        Task UpdateAsync(Artista artista);
        Task DeleteAsync(int id);
    }
}