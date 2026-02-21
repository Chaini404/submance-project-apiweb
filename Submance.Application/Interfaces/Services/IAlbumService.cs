using Submance.Application.DTOs.Album;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Submance.Application.Interfaces.Services
{
    public interface IAlbumService
    {
        Task<IEnumerable<AlbumResponseDto>> GetAllAsync();
        Task<AlbumResponseDto?> GetByIdAsync(int id);
        Task<IEnumerable<AlbumResponseDto>> GetByArtistaAsync(int idArtista);
        Task CreateAsync(AlbumRequestDto request);
        Task UpdateAsync(int id, AlbumRequestDto request);
        Task DeleteAsync(int id);
    }
}