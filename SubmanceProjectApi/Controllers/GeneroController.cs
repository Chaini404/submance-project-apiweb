using Microsoft.AspNetCore.Mvc;
using Submance.Application.Interfaces.Services;
using Submance.Application.DTOs.Genero;
using System.Threading.Tasks;

namespace SubmanceProjectApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GeneroController : ControllerBase
    {
        private readonly IGeneroService _generoService;

        public GeneroController(IGeneroService generoService)
        {
            _generoService = generoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var generos = await _generoService.GetAllAsync();
            return Ok(generos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var genero = await _generoService.GetByIdAsync(id);
            if (genero == null) return NotFound();
            return Ok(genero);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GeneroRequestDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _generoService.CreateAsync(request);
            return Ok(new { success = true, message = "Género creado." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] GeneroRequestDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _generoService.UpdateAsync(id, request);
            return Ok(new { success = true, message = "Género actualizado." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _generoService.DeleteAsync(id);
            return Ok(new { success = true, message = "Género eliminado." });
        }
    }
}
