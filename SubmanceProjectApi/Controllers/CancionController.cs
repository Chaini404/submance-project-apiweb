using Microsoft.AspNetCore.Mvc;
using Submance.Application.Interfaces.Services;
using Submance.Application.DTOs.Cancion;
using System.Threading.Tasks;

namespace SubmanceProjectApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CancionController : ControllerBase
    {
        private readonly ICancionService _cancionService;

        public CancionController(ICancionService cancionService)
        {
            _cancionService = cancionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var canciones = await _cancionService.GetAllAsync();
            return Ok(canciones);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cancion = await _cancionService.GetByIdAsync(id);
            if (cancion == null) return NotFound();
            return Ok(cancion);
        }

        [HttpGet("artista/{idArtista}")]
        public async Task<IActionResult> GetByArtista(int idArtista)
        {
            var canciones = await _cancionService.GetByArtistaAsync(idArtista);
            return Ok(canciones);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CancionRequestDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _cancionService.CreateAsync(request);
            return Ok(new { success = true, message = "Demo creado." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CancionRequestDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _cancionService.UpdateAsync(id, request);
            return Ok(new { success = true, message = "Demo actualizado." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _cancionService.DeleteAsync(id);
            return Ok(new { success = true, message = "Demo eliminado." });
        }
    }
}
