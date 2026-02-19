using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SubmanceProjectClient.Models;

namespace SubmanceProjectClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // ESTE ES TU INDEX PRINCIPAL (La p�gina p�blica de env�o de demos)
        public IActionResult Index()
        {
            return View();
        }

        // AGREGAMOS ESTO: Es la acci�n que recibir� el formulario cuando el artista le de a "SEND"
        [HttpPost]
        public IActionResult SubmitDemo(string ArtistName, string TrackTitle, string DemoLink, string Email)
        {
            // Aqu� m�s adelante pondremos la l�gica para guardar en la Base de Datos.
            // Por ahora, simplemente nos devuelve al inicio.
            return RedirectToAction("Index");
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
}
