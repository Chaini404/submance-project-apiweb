using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Submance.Application.Interfaces.Repositories;
using Submance.Domain.Entities;
using SubmanceProjectClient.Models;

namespace SubmanceProjectClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICancionRepository _cancionRepo;

        public HomeController(ILogger<HomeController> logger, ICancionRepository cancionRepo)
        {
            _logger = logger;
            _cancionRepo = cancionRepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> SubmitDemo([FromBody] PublicDemoRequest model)
        {
            if (string.IsNullOrWhiteSpace(model?.TrackTitle) || string.IsNullOrWhiteSpace(model?.DemoLink))
                return Json(new { success = false, message = "Faltan datos obligatorios." });

            try
            {
                var cancion = new Cancion
                {
                    Titulo = model.TrackTitle.Trim(),
                    UrlAudio = model.DemoLink.Trim(),
                    IdArtista = 1, // Demo enviado desde público, se asigna artista por defecto
                    IdGenero = 1,  // Género por defecto
                    Estado = "Pendiente",
                    FechaEnvio = DateTime.UtcNow
                };

                await _cancionRepo.AddAsync(cancion);
                return Json(new { success = true, message = "Demo recibido correctamente." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar demo público");
                return Json(new { success = false, message = "Error al procesar tu demo." });
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public class PublicDemoRequest
    {
        public string ArtistName { get; set; } = string.Empty;
        public string TrackTitle { get; set; } = string.Empty;
        public string DemoLink { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
