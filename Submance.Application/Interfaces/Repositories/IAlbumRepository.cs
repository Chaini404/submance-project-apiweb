#nullable enable
using Submance.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Submance.Application.Interfaces.Repositories
{
    public interface IAlbumRepository
    {
        Task<IEnumerable<Album>> GetAllAsync();
        Task<Album?> GetByIdAsync(int id);
        Task<IEnumerable<Album>> GetByArtistaAsync(int idArtista);

        Task AddAsync(Album album);
        Task UpdateAsync(Album album);
        Task DeleteAsync(int id);
    }
}