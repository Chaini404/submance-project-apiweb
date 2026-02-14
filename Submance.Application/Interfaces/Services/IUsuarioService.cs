using Submance.Application.DTOs.Usuario;
using Submance.Domain.Entities; // Para devolver Usuario en Login (o usa un DTO si prefieres)

namespace Submance.Application.Interfaces.Services
{
    public interface IUsuarioService
    {
        Task<UsuarioResponseDto> LoginAsync(string correo, string password);
        Task RegisterAsync(UsuarioRequestDto request);
        Task<IEnumerable<UsuarioResponseDto>> GetAllAsync();
        Task<UsuarioResponseDto?> GetByIdAsync(int id);
    }
}