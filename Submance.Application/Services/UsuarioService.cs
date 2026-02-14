using Submance.Application.DTOs.Usuario;
using Submance.Application.Interfaces.Repository;
using Submance.Application.Interfaces.Services;
using Submance.Domain.Entities;

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

            // Validación simple
            if (usuario != null && usuario.Password == password && usuario.Activo)
            {
                return new UsuarioResponseDto
                {
                    IdUsuario = usuario.IdUsuario,
                    Nombre = usuario.Nombre,
                    Correo = usuario.Correo,
                    Rol = usuario.Rol, // Ahora es directo string -> string
                    Estado = usuario.Activo
                };
            }
            return null;
        }

        public async Task RegisterAsync(UsuarioRequestDto request)
        {
            // TRADUCCIÓN: Convertimos el ID del dropdown (Frontend) a Texto (Database)
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
                Rol = rolTexto, // Guardamos el texto
                Activo = true
            };

            await _usuarioRepository.Add(usuario);
        }

        // Métodos stub para cumplir interfaz
        public async Task<IEnumerable<UsuarioResponseDto>> GetAllAsync() => new List<UsuarioResponseDto>();
        public async Task<UsuarioResponseDto?> GetByIdAsync(int id) => null;
    }
}