using Microsoft.AspNetCore.Mvc;
using Submance.Application.Interfaces.Repositories;
using System.Threading.Tasks;

namespace SubmanceProjectApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepo;

        public UsuarioController(IUsuarioRepository usuarioRepo)
        {
            _usuarioRepo = usuarioRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var usuarios = await _usuarioRepo.GetAllAsync();
            return Ok(usuarios);
        }
    }
}