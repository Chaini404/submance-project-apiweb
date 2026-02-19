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
                ViewBag.Error = "Error: No se encontró un perfil de Artista vinculado a este correo.";
                return View(new List<Cancion>());
            }

            var tracks = await _cancionRepo.GetByArtistaAsync(artista.IdArtista);
            ViewBag.ArtistId = artista.IdArtista;
            ViewBag.ArtistName = artista.NombreArtistico;

            return View(tracks);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload([FromBody] Cancion model)
        {
            try
            {
                await _cancionRepo.AddAsync(model);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}