using Submance.Application.DTOs.Usuario;
using Submance.Application.Interfaces.Repositories;
using Submance.Application.Interfaces.Services;
using Submance.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Submance.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<UsuarioResponseDto> LoginAsync(string correo, string password)
        {
            var usuario = await _usuarioRepository.GetByCorreoAsync(correo);

            // Corregido: El estado en BD es TEXT ('Activo', 'Inactivo')
            if (usuario != null && usuario.Password == password && usuario.Estado == "Activo")
            {
                return new UsuarioResponseDto
                {
                    IdUsuario = usuario.IdUsuario,
                    Nombre = usuario.Nombre,
                    Correo = usuario.Correo,
                    Rol = usuario.Rol,
                    Estado = true // Asumiendo que el DTO espera bool
                };
            }
            return null;
        }

        public async Task RegisterAsync(UsuarioRequestDto request)
        {
            string rolTexto = "Usuario";
            switch (request.IdRol)
            {
                case 1: rolTexto = "Admin"; break;
                case 2: rolTexto = "Staff"; break;
                case 3: rolTexto = "Artista"; break;
                default: rolTexto = "Usuario"; break;
            }

            var usuario = new Usuario
            {
                Nombre = request.Nombre,
                Correo = request.Correo,
                Password = request.Password,
                Rol = rolTexto,
                Estado = "Activo" // Corregido de Activo = true
            };

            await _usuarioRepository.AddAsync(usuario);
        }

        public async Task<IEnumerable<UsuarioResponseDto>> GetAllAsync() => new List<UsuarioResponseDto>();
        public async Task<UsuarioResponseDto?> GetByIdAsync(int id) => null;
    }
}