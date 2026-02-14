#nullable enable // 👈 ESTO ELIMINA LAS ADVERTENCIAS AMARILLAS
using System.Collections.Generic;
using System.Threading.Tasks;
using Submance.Application.DTOs.Artista;

namespace Submance.Application.Interfaces.Services
{
    public interface IArtistaService
    {
        Task<IEnumerable<ArtistaResponseDto>> GetAllAsync();
        Task<ArtistaResponseDto?> GetByIdAsync(int id);

        // 👇 ESTO SOLUCIONA EL ERROR "'IArtistaService' no contiene..."
        // Renombramos a CreateAsync para seguir el estándar
        Task CreateAsync(ArtistaRequestDto request);

        Task UpdateAsync(int id, ArtistaRequestDto request);
        Task DeleteAsync(int id);
    }
}