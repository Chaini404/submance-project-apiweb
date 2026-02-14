using Microsoft.AspNetCore.Mvc;
using Submance.Application.Interfaces.Repository;
using Submance.Domain.Entities;

namespace SubmanceProject.Web.Controllers
{
    public class StaffController : Controller
    {
        private readonly ICancionRepository _cancionRepo;

        public StaffController(ICancionRepository cancionRepo)
        {
            _cancionRepo = cancionRepo;
        }

        // 1. BANDEJA DE ENTRADA (Dashboard)
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Seguridad: Solo Staff o Admin
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Staff" && role != "Admin")
                return RedirectToAction("Login", "Admin");

            // Traer solo los pendientes
            var pendientes = await _cancionRepo.GetPendientesRevisionAsync();

            return View(pendientes);
        }

        // 2. MOTOR DE DECISIÓN (POST)
        [HttpPost]
        [ValidateAntiForgeryToken] // Seguridad extra contra ataques CSRF
        public async Task<IActionResult> Decidir(int idCancion, string decision)
        {
            try
            {
                // A. Buscar el track original
                var track = await _cancionRepo.GetByIdAsync(idCancion);

                if (track == null)
                {
                    TempData["Error"] = "La canción no existe o ya fue moderada.";
                    return RedirectToAction("Index");
                }

                // B. Cambiar el estado
                // Solo permitimos valores válidos para evitar inyecciones raras
                if (decision == "Aprobado" || decision == "Rechazado")
                {
                    track.Estado = decision;

                    // C. Guardar en BD
                    await _cancionRepo.UpdateAsync(track);

                    TempData["Success"] = $"El track '{track.Titulo}' ha sido {decision}.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al procesar: " + ex.Message;
            }

            // D. Recargar la página (La canción desaparecerá de la lista de pendientes)
            return RedirectToAction("Index");
        }
    }
}