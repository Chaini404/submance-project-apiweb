using Submance.Application.DTOs.Cancion;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Submance.Application.Interfaces.Services
{
    public interface ICancionService
    {
        Task<IEnumerable<CancionResponseDto>> GetAllAsync();
        Task<CancionResponseDto?> GetByIdAsync(int id);
        Task<IEnumerable<CancionResponseDto>> GetByArtistaAsync(int idArtista);
        Task CreateAsync(CancionRequestDto request);
        Task UpdateAsync(int id, CancionRequestDto request);
        Task DeleteAsync(int id);
    }
}