using Microsoft.AspNetCore.Mvc;
using Submance.Application.Interfaces.Repository;
using Submance.Domain.Entities;

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
            // 1. Verificar Sesión
            if (HttpContext.Session.GetString("UserRole") != "Artista")
                return RedirectToAction("Login", "Admin");

            string email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email)) return RedirectToAction("Login", "Admin");

            // 2. Obtener el Perfil del Artista
            var artista = await _artistaRepo.GetByEmailAsync(email);

            if (artista == null)
            {
                // Caso raro: Usuario existe pero no tiene perfil de Artista creado
                ViewBag.Error = "Perfil de artista no encontrado.";
                return View(new List<Cancion>());
            }

            // 3. Obtener sus Tracks (Demos)
            var misTracks = await _cancionRepo.GetByArtistaAsync(artista.IdArtista);

            // 4. Retornar a la Vista
            return View(misTracks);
        }
    }
}