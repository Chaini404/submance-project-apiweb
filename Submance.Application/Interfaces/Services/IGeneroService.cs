using Submance.Application.DTOs.Genero;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Submance.Application.Interfaces.Services
{
    public interface IGeneroService
    {
        Task<IEnumerable<GeneroResponseDto>> GetAllAsync();
        Task<GeneroResponseDto?> GetByIdAsync(int id);
        Task CreateAsync(GeneroRequestDto request);
        Task UpdateAsync(int id, GeneroRequestDto request);
        Task DeleteAsync(int id);
    }
}
