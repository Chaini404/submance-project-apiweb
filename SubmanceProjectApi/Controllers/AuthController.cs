using Microsoft.AspNetCore.Mvc;
using Submance.Infrastructure.Repositories;
using Submance.Infrastructure.Security;
// NOTA: Borré la línea de Microsoft.AspNetCore.Identity para evitar conflictos

namespace SubmanceProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UsuarioRepository _usuarioRepository;
        private readonly PasswordHasher _passwordHasher;

        public AuthController(UsuarioRepository usuarioRepository, PasswordHasher passwordHasher)
        {
            _usuarioRepository = usuarioRepository;
            _passwordHasher = passwordHasher;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // 1. Validar datos
            if (request == null || string.IsNullOrEmpty(request.NombreUsuario))
                return BadRequest("Faltan datos.");

            // 2. Buscar usuario (ajusta GetByUsernameAsync según como se llame en tu repo real)
            // Si te da error aquí, verifica si tu método en UsuarioRepository es asíncrono (Task)
            // Si no es asíncrono, quita el 'await' y el 'Async' del nombre.
            var usuario = await _usuarioRepository.GetByUsernameAsync(request.NombreUsuario);

            if (usuario == null)
            {
                return Unauthorized(new { message = "El usuario no existe." });
            }

            // 3. Verificar contraseña
            bool esValido = _passwordHasher.Verify(request.Password, usuario.Password); // Asegúrate que tu entidad Usuario tenga 'PasswordHash'

            if (!esValido)
            {
                return Unauthorized(new { message = "Contraseña incorrecta." });
            }

            return Ok(new { Message = "Login Exitoso", Usuario = request.NombreUsuario });
        }
    }

    public class LoginRequest
    {
        public string NombreUsuario { get; set; }
        public string Password { get; set; }
    }
}