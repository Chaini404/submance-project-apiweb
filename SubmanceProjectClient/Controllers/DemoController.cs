using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Submance.Application.Interfaces.Repositories;
using Submance.Domain.Entities;
using System.Threading.Tasks;
using System;

namespace SubmanceProject.Web.Controllers
{
    public class DemoController : Controller
    {
        private readonly ICancionRepository _cancionRepo;
        private readonly IArtistaRepository _artistaRepo;

        public DemoController(ICancionRepository cancionRepo, IArtistaRepository artistaRepo)
        {
            _cancionRepo = cancionRepo;
            _artistaRepo = artistaRepo;
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("UserRole") != "Artista")
                return RedirectToAction("Login", "Admin");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cancion model)
        {
            try
            {
                string email = HttpContext.Session.GetString("UserEmail");
                var artista = await _artistaRepo.GetByEmailAsync(email);

                if (artista == null)
                {
                    ViewBag.Error = "Error de sesión. Vuelve a loguearte.";
                    return View(model);
                }

                model.IdArtista = artista.IdArtista;
                model.Estado = "Pendiente";

                await _cancionRepo.AddAsync(model);

                return RedirectToAction("Index", "ArtistPortal");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error al guardar: " + ex.Message;
                return View(model);
            }
        }
    }
}