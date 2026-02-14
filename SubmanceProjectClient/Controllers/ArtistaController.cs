using Microsoft.AspNetCore.Mvc;
using Submance.Application.DTOs.Artista;
using Submance.Application.Interfaces.Services;

namespace SubmanceProject.Web.Controllers
{
    public class ArtistaController : Controller
    {
        private readonly IArtistaService _artistaService;

        public ArtistaController(IArtistaService artistaService)
        {
            _artistaService = artistaService;
        }

        // GET: Artista (Solo lista)
        public async Task<IActionResult> Index()
        {
            var artistas = await _artistaService.GetAllAsync();
            return View(artistas);
        }

        // GET: Artista/Create (La página en blanco que pidió ella)
        public IActionResult Create()
        {
            return View();
        }

        // POST: Artista/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ArtistaRequestDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                await _artistaService.CreateAsync(model);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al guardar: " + ex.Message);
                return View(model);
            }
        }

        // ... (Implementar Edit/Delete similarmente si es necesario)
    }
}