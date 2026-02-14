using Microsoft.AspNetCore.Mvc;
using Submance.Application.Interfaces.Repository;
using Submance.Domain.Entities;

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

        // 1. MOSTRAR FORMULARIO
        [HttpGet]
        public IActionResult Create()
        {
            // Seguridad: Solo Artistas
            if (HttpContext.Session.GetString("UserRole") != "Artista")
                return RedirectToAction("Login", "Admin");

            return View();
        }

        // 2. PROCESAR EL ENVÍO
        [HttpPost]
        public async Task<IActionResult> Create(Cancion model)
        {
            try
            {
                // A. Obtener el ID del Artista logueado
                string email = HttpContext.Session.GetString("UserEmail");
                var artista = await _artistaRepo.GetByEmailAsync(email);

                if (artista == null)
                {
                    ViewBag.Error = "Error de sesión. Vuelve a loguearte.";
                    return View(model);
                }

                // B. Completar datos automáticos
                model.IdArtista = artista.IdArtista;
                model.Estado = "Pendiente";
                // model.FechaEnvio se pone en el SQL con GETDATE(), así que no hace falta aquí

                // C. Guardar en Base de Datos
                await _cancionRepo.AddAsync(model);

                // D. Redirigir al Portal
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