using Microsoft.AspNetCore.Mvc;
using Submance.Application.Interfaces.Repositories;
using Submance.Application.Interfaces.Security;
using Submance.Application.Interfaces.Services;
using Submance.Application.ViewModels;
using System.Threading.Tasks;
using System;

namespace SubmanceProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IAuthService _authService;

        public AuthController(
            IUsuarioRepository usuarioRepository,
            IPasswordHasher passwordHasher,
            IAuthService authService)
        {
            _usuarioRepository = usuarioRepository;
            _passwordHasher = passwordHasher;
            _authService = authService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.NombreUsuario))
                return BadRequest(new { message = "Faltan datos." });

            var usuario = await _usuarioRepository.GetByUsernameAsync(request.NombreUsuario);

            if (usuario == null)
                return Unauthorized(new { message = "El usuario no existe." });

            bool esValido = _passwordHasher.Verify(usuario.Password, request.Password);

            if (!esValido)
                return Unauthorized(new { message = "Contraseña incorrecta." });

            return Ok(new { Message = "Login Exitoso", Usuario = request.NombreUsuario });
        }

        [HttpPost("register-dj")]
        public async Task<IActionResult> RegisterDJ([FromBody] RegistroViewModel dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Datos de entrada inválidos.", errors = ModelState });

            try
            {
                bool result = await _authService.RegistrarArtistaAsync(dto);

                if (!result)
                    return Conflict(new { success = false, message = "El correo ya está registrado en el sistema." });

                return Ok(new { success = true, message = "Cuenta de DJ y Perfil creados exitosamente." });
            }
            catch (Exception)
            {
                return StatusCode(500, new { success = false, message = "Fallo de integridad transaccional." });
            }
        }
    }

    public class LoginRequest
    {
        public string NombreUsuario { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}