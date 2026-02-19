using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Submance.Application.Interfaces.Repositories;
using System.Threading.Tasks;
using System;

namespace SubmanceProject.Web.Controllers
{
    public class StaffController : Controller
    {
        private readonly ICancionRepository _cancionRepo;

        public StaffController(ICancionRepository cancionRepo)
        {
            _cancionRepo = cancionRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Staff" && role != "Admin")
                return RedirectToAction("Login", "Admin");

            var pendientes = await _cancionRepo.GetPendientesRevisionAsync();
            return View(pendientes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // Se mantiene idCancion como parámetro asumiendo que el HTML Form name="idCancion"
        public async Task<IActionResult> Decidir(int idCancion, string decision)
        {
            try
            {
                var track = await _cancionRepo.GetByIdAsync(idCancion);

                if (track == null)
                {
                    TempData["Error"] = "La canción no existe o ya fue moderada.";
                    return RedirectToAction("Index");
                }

                if (decision == "Aprobado" || decision == "Rechazado")
                {
                    track.Estado = decision;
                    await _cancionRepo.UpdateAsync(track);
                    TempData["Success"] = $"El track '{track.Titulo}' ha sido {decision}.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al procesar: " + ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}