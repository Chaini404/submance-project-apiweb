using Microsoft.AspNetCore.Mvc;
using Submance.Application.Interfaces.Services;
using Submance.Application.DTOs.Artista;
using System.Threading.Tasks;

namespace SubmanceProjectApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArtistaController : ControllerBase
    {
        private readonly IArtistaService _artistaService;

        public ArtistaController(IArtistaService artistaService)
        {
            _artistaService = artistaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var artistas = await _artistaService.GetAllAsync();
            return Ok(artistas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var artista = await _artistaService.GetByIdAsync(id);
            if (artista == null) return NotFound();
            return Ok(artista);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ArtistaRequestDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _artistaService.CreateAsync(request);
            return Ok(new { success = true, message = "Artista creado." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ArtistaRequestDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _artistaService.UpdateAsync(id, request);
            return Ok(new { success = true, message = "Artista actualizado." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _artistaService.DeleteAsync(id);
            return Ok(new { success = true, message = "Artista eliminado." });
        }
    }
}
