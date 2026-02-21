using Submance.Application.DTOs.Usuario;
using Submance.Application.Interfaces.Repositories;
using Submance.Application.Interfaces.Security;
using Submance.Application.Interfaces.Services;
using Submance.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Submance.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPasswordHasher _passwordHasher;

        public UsuarioService(IUsuarioRepository usuarioRepository, IPasswordHasher passwordHasher)
        {
            _usuarioRepository = usuarioRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<UsuarioResponseDto> LoginAsync(string correo, string password)
        {
            var usuario = await _usuarioRepository.GetByCorreoAsync(correo);

            if (usuario == null || usuario.Estado != "Activo")
                return null;

            // Verificar password con hash
            if (!_passwordHasher.Verify(usuario.Password, password))
                return null;

            return new UsuarioResponseDto
            {
                IdUsuario = usuario.IdUsuario,
                Nombre = usuario.Nombre,
                Correo = usuario.Correo,
                Rol = usuario.Rol,
                Estado = true
            };
        }

        public async Task RegisterAsync(UsuarioRequestDto request)
        {
            string rolTexto = request.IdRol switch
            {
                1 => "Admin",
                2 => "Staff",
                3 => "Artista",
                _ => "Artista"
            };

            var usuario = new Usuario
            {
                Nombre = request.Nombre,
                Correo = request.Correo,
                Password = _passwordHasher.Hash(request.Password),
                Rol = rolTexto,
                Estado = "Activo"
            };

            await _usuarioRepository.AddAsync(usuario);
        }

        public async Task<IEnumerable<UsuarioResponseDto>> GetAllAsync()
        {
            var usuarios = await _usuarioRepository.GetAllAsync();
            return usuarios.Select(u => new UsuarioResponseDto
            {
                IdUsuario = u.IdUsuario,
                Nombre = u.Nombre,
                Correo = u.Correo,
                Rol = u.Rol,
                Estado = u.Estado == "Activo"
            });
        }

        public async Task<UsuarioResponseDto?> GetByIdAsync(int id)
        {
            var u = await _usuarioRepository.GetByIdAsync(id);
            if (u == null) return null;

            return new UsuarioResponseDto
            {
                IdUsuario = u.IdUsuario,
                Nombre = u.Nombre,
                Correo = u.Correo,
                Rol = u.Rol,
                Estado = u.Estado == "Activo"
            };
        }
    }
}