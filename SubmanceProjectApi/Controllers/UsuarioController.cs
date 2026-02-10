using Microsoft.AspNetCore.Mvc;
using Submance.Application.Interfaces.Services;
using Submance.Application.DTOs.Usuario;

namespace SubmanceProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        // ESTE ES EL BOTÓN QUE TE FALTA EN SWAGGER 👇
        [HttpPost]
        public IActionResult Create([FromBody] UsuarioRequestDto request)
        {
            try
            {
                _usuarioService.Create(request);
                return Ok(new { message = "Usuario creado exitosamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_usuarioService.GetAll());
        }
    }
}
