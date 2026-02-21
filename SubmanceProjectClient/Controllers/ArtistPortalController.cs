using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Submance.Application.Interfaces.Repositories;
using Submance.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace SubmanceProject.Web.Controllers
{
    public class ArtistPortalController : Controller
    {
        private readonly IArtistaRepository _artistaRepo;
        private readonly ICancionRepository _cancionRepo;

        public ArtistPortalController(IArtistaRepository artistaRepo, ICancionRepository cancionRepo)
        {
            _artistaRepo = artistaRepo;
            _cancionRepo = cancionRepo;
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("UserRole") != "Artista")
                return RedirectToAction("Login", "Admin");

            string email = HttpContext.Session.GetString("UserEmail");
            var artista = await _artistaRepo.GetByEmailAsync(email);

            if (artista == null)
            {
                ViewBag.Error = "Error: Perfil de artista no encontrado.";
                return View(new List<Cancion>());
            }

            var tracks = await _cancionRepo.GetByArtistaAsync(artista.IdArtista);
            ViewBag.ArtistId = artista.IdArtista;
            ViewBag.ArtistName = artista.NombreArtistico;

            return View(tracks);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload([FromForm] Cancion model)
        {
            // ModelState falla por IdDemo=0 y otras props autogeneradas.
            // Removemos esos campos de la validación.
            ModelState.Remove(nameof(Cancion.IdDemo));
            ModelState.Remove(nameof(Cancion.Estado));
            ModelState.Remove(nameof(Cancion.FechaEnvio));

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);
                return Json(new { success = false, message = string.Join(" | ", errors) });
            }

            // Nunca confiar en el IdArtista del form — tomarlo de la sesión
            var email = HttpContext.Session.GetString("UserEmail");
            var artista = await _artistaRepo.GetByEmailAsync(email);
            if (artista == null)
                return Json(new { success = false, message = "Sesión inválida." });

            try
            {
                var cancion = new Cancion
                {
                    Titulo = model.Titulo.Trim(),
                    UrlAudio = model.UrlAudio.Trim(),
                    IdArtista = artista.IdArtista,  // ← desde sesión, nunca del form
                    IdGenero = model.IdGenero > 0 ? model.IdGenero : 1,
                    Estado = "Pendiente",
                    FechaEnvio = DateTime.UtcNow
                };

                await _cancionRepo.AddAsync(cancion);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Log del error REAL de Postgres/Supabase
                Console.Error.WriteLine($"[Upload Error] {ex}");
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}