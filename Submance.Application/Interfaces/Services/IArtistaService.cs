#nullable enable 
using System.Collections.Generic;
using System.Threading.Tasks;
using Submance.Application.DTOs.Artista;

namespace Submance.Application.Interfaces.Services
{
    public interface IArtistaService
    {
        Task<IEnumerable<ArtistaResponseDto>> GetAllAsync();
        Task<ArtistaResponseDto?> GetByIdAsync(int id);
        Task CreateAsync(ArtistaRequestDto request);
        Task UpdateAsync(int id, ArtistaRequestDto request);
        Task DeleteAsync(int id);
    }
}