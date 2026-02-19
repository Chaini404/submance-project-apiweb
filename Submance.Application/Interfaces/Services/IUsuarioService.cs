using Submance.Application.DTOs.Usuario;
using Submance.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

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