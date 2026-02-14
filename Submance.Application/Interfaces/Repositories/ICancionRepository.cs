#nullable enable
using Submance.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Submance.Application.Interfaces.Repository
{
    public interface ICancionRepository
    {
        // Métodos de Lectura
        Task<IEnumerable<Cancion>> GetAllAsync();
        Task<Cancion?> GetByIdAsync(int id);

        // Métodos Específicos (Staff y Artista)
        Task<IEnumerable<Cancion>> GetByArtistaAsync(int idArtista);
        Task<IEnumerable<Cancion>> GetPendientesRevisionAsync();

        // Métodos de Escritura
        Task AddAsync(Cancion cancion);
        Task UpdateAsync(Cancion cancion);
        Task DeleteAsync(int id);
    }
}