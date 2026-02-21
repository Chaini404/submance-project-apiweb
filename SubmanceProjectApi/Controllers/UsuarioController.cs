using Microsoft.AspNetCore.Mvc;
using Submance.Application.Interfaces.Repositories;
using Submance.Application.Interfaces.Services;
using Submance.Application.DTOs.Usuario;
using System.Threading.Tasks;

namespace SubmanceProjectApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var usuarios = await _usuarioService.GetAllAsync();
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var usuario = await _usuarioService.GetByIdAsync(id);
            if (usuario == null) return NotFound();
            return Ok(usuario);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UsuarioRequestDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                await _usuarioService.RegisterAsync(request);
                return Ok(new { success = true, message = "Usuario registrado." });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginApiRequest request)
        {
            var result = await _usuarioService.LoginAsync(request.Correo, request.Password);
            if (result == null) return Unauthorized(new { message = "Credenciales incorrectas." });
            return Ok(result);
        }
    }

    public class LoginApiRequest
    {
        public string Correo { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}