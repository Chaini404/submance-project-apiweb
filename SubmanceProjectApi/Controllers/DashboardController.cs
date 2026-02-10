using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubmanceProject.Api.Data;
using Submance.Domain.Entities;
using System.Net;
using System.Net.Mail;

namespace SubmanceProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly ApiContext _context;

        public DashboardController(ApiContext context)
        {
            _context = context;
        }

        [HttpGet("GetStats")]
        public async Task<IActionResult> GetStats()
        {
            var totalDemos = await _context.Demos.CountAsync();
            var pendientes = await _context.Demos.CountAsync(d => d.Estado == "Pendiente");
            var aprobados = await _context.Demos.CountAsync(d => d.Estado == "Aprobada");
            var artistas = await _context.Demos.Select(d => d.NombreArtistico).Distinct().CountAsync();

            return Ok(new { totalDemos, pendientes, aprobados, artistas });
        }

        [HttpGet("GetDemos")]
        public async Task<IActionResult> GetDemos()
        {
            try
            {
                // NOTA: Usamos AsNoTracking para mejorar rendimiento en lecturas
                var lista = await _context.Demos
                    .AsNoTracking()
                    .Select(d => new {
                        IdDemo = d.IdDemo,
                        TituloDemo = d.TituloDemo ?? "Sin Título",
                        NombreArtistico = d.NombreArtistico ?? "Anónimo",
                        Estado = d.Estado ?? "Pendiente",
                        LinkDemo = d.LinkDemo ?? "#",
                        Email = d.Email ?? "N/A",
                        FechaEnvio = d.FechaEnvio,
                        FechaLanzamiento = d.FechaLanzamiento // <--- IMPORTANTE: Incluimos la fecha de lanzamiento
                    })
                    .ToListAsync();

                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error de mapeo: {ex.Message}");
            }
        }

        [HttpPost("RecibirDemoPublico")]
        public async Task<IActionResult> RecibirDemoPublico([FromBody] DemoPublicoDto info)
        {
            try
            {
                if (info == null || string.IsNullOrEmpty(info.TrackTitle)) return BadRequest("Faltan datos");

                var nuevoDemo = new Demo
                {
                    TituloDemo = info.TrackTitle,
                    NombreArtistico = info.ArtistName,
                    LinkDemo = info.Link,
                    Email = info.Email,
                    Estado = "Pendiente",
                    FechaEnvio = DateTime.Now
                };

                _context.Demos.Add(nuevoDemo);
                await _context.SaveChangesAsync();
                return Ok(new { message = "¡Demo recibido!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("CambiarEstado")]
        public async Task<IActionResult> CambiarEstado([FromBody] EstadoDemoDto datos)
        {
            try
            {
                var demo = await _context.Demos.FindAsync(datos.IdDemo);
                if (demo == null) return NotFound(new { message = "El demo no existe." });

                demo.Estado = datos.NuevoEstado;
                // Si tuvieras feedback: demo.Comentario = datos.Comentario; 

                await _context.SaveChangesAsync();
                return Ok(new { message = $"Demo actualizado a: {datos.NuevoEstado}" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al procesar: {ex.Message}" });
            }
        }

        // ==========================================
        //        MÉTODOS PARA ARTISTAS (Editar/Eliminar)
        // ==========================================

        [HttpPost("ActualizarArtista")]
        public async Task<IActionResult> ActualizarArtista([FromBody] EditarArtistaDto datos)
        {
            try
            {
                // Buscamos todos los demos que coincidan con el nombre antiguo
                var tracks = await _context.Demos
                    .Where(d => d.NombreArtistico == datos.NombreActual)
                    .ToListAsync();

                if (!tracks.Any()) return NotFound(new { message = "Artista no encontrado" });

                // Actualizamos nombre y correo en todos los registros
                foreach (var track in tracks)
                {
                    track.NombreArtistico = datos.NuevoNombre;
                    track.Email = datos.NuevoEmail;
                }

                await _context.SaveChangesAsync();
                return Ok(new { message = "Artista actualizado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("EliminarArtista")]
        public async Task<IActionResult> EliminarArtista([FromBody] EliminarArtistaDto datos)
        {
            try
            {
                var tracks = await _context.Demos
                    .Where(d => d.NombreArtistico == datos.NombreArtistico)
                    .ToListAsync();

                if (tracks.Any())
                {
                    _context.Demos.RemoveRange(tracks);
                    await _context.SaveChangesAsync();
                }

                return Ok(new { message = "Artista y sus tracks eliminados" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // ==========================================
        //        NUEVO MÉTODO PARA LANZAMIENTOS
        // ==========================================

        [HttpPost("AgendarLanzamiento")]
        public async Task<IActionResult> AgendarLanzamiento([FromBody] AgendarLanzamientoDto datos)
        {
            try
            {
                var demo = await _context.Demos.FindAsync(datos.IdDemo);
                if (demo == null) return NotFound(new { message = "Demo no encontrado" });

                demo.FechaLanzamiento = datos.Fecha;

                await _context.SaveChangesAsync();
                return Ok(new { message = "Fecha de lanzamiento guardada" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }

    // ================= DTOs =================

    public class DemoPublicoDto
    {
        public string ArtistName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string TrackTitle { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
    }

    public class EstadoDemoDto
    {
        public int IdDemo { get; set; }
        public string NuevoEstado { get; set; } = string.Empty;
        public string Comentario { get; set; } = string.Empty;
    }

    public class EditarArtistaDto
    {
        public string NombreActual { get; set; }
        public string NuevoNombre { get; set; }
        public string NuevoEmail { get; set; }
    }

    public class EliminarArtistaDto
    {
        public string NombreArtistico { get; set; }
    }

    // Nuevo DTO para agendar fecha
    public class AgendarLanzamientoDto
    {
        public int IdDemo { get; set; }
        public DateTime Fecha { get; set; }
    }
}