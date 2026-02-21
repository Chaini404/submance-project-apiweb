using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Submance.Application.Interfaces.Repositories;
using Submance.Application.Interfaces.Services;
using Submance.Application.Interfaces.Security;
using Submance.Application.ViewModels;
using Submance.Domain.Entities;
using System.Threading.Tasks;
using System;

namespace SubmanceProject.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUsuarioRepository _usuarioRepo;
        private readonly IArtistaRepository _artistaRepo;
        private readonly ICancionRepository _cancionRepo;
        private readonly IDashboardRepository _dashboardRepo;
        private readonly IAuthService _authService;
        private readonly IPasswordHasher _passwordHasher;

        public AdminController(
            IUsuarioRepository usuarioRepo,
            IArtistaRepository artistaRepo,
            ICancionRepository cancionRepo,
            IDashboardRepository dashboardRepo,
            IAuthService authService,
            IPasswordHasher passwordHasher)
        {
            _usuarioRepo = usuarioRepo;
            _artistaRepo = artistaRepo;
            _cancionRepo = cancionRepo;
            _dashboardRepo = dashboardRepo;
            _authService = authService;
            _passwordHasher = passwordHasher;
        }

        [HttpGet]
        public IActionResult Login()
        {
            var currentRole = HttpContext.Session.GetString("UserRole");
            if (!string.IsNullOrEmpty(currentRole))
                return RedirigirPorRol(currentRole);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var usuario = await _usuarioRepo.GetByCorreoAsync(model.Correo);

            if (usuario != null && _passwordHasher.Verify(usuario.Password, model.Password))
            {
                HttpContext.Session.SetString("UserRole", usuario.Rol);
                HttpContext.Session.SetString("UserEmail", usuario.Correo);
                HttpContext.Session.SetInt32("UserId", usuario.IdUsuario);
                return RedirigirPorRol(usuario.Rol);
            }

            ViewBag.Error = "Credenciales incorrectas.";
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
                return RedirectToAction("Login");
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboardData()
        {
            var data = await _dashboardRepo.GetDashboardDataAsync();
            return Json(data);
        }

        // AJAX endpoint — no requiere AntiForgeryToken para peticiones JS con query params
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> UpdateDemoStatus(int id, string status)
        {
            var cancion = await _cancionRepo.GetByIdAsync(id);
            if (cancion == null)
                return Json(new { success = false, message = "Demo no encontrada." });

            cancion.Estado = status;
            await _cancionRepo.UpdateAsync(cancion);

            return Json(new { success = true });
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> CreateArtist([FromBody] ArtistaRequest model)
        {
            if (!ModelState.IsValid) return Json(new { success = false, message = "Datos inválidos." });

            try
            {
                var artista = new Artista
                {
                    NombreArtistico = model.NombreArtistico,
                    NombreReal = model.NombreReal,
                    Pais = model.Pais,
                    Estado = true,
                    FechaRegistro = DateTime.UtcNow
                };

                await _artistaRepo.AddAsync(artista);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> CreateDemo([FromBody] DemoRequest model)
        {
            if (!ModelState.IsValid) return Json(new { success = false, message = "Datos inválidos." });

            try
            {
                var cancion = new Cancion
                {
                    Titulo = model.Titulo,
                    UrlAudio = model.UrlAudio,
                    IdArtista = model.IdArtista,
                    IdGenero = model.IdGenero,
                    Estado = "Pendiente",
                    FechaEnvio = DateTime.UtcNow
                };

                await _cancionRepo.AddAsync(cancion);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(RegistroViewModel model)
        {
            if (!ModelState.IsValid) return View("Login", model);

            try
            {
                bool creado = await _authService.RegistrarArtistaAsync(model);
                if (creado)
                {
                    ViewBag.Success = "Cuenta creada correctamente. Ahora puedes iniciar sesión.";
                }
                else
                {
                    ViewBag.Error = "El correo ya existe en el sistema.";
                }
                return View("Login");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Excepción BD: " + ex.Message;
                return View("Login");
            }
        }

        private IActionResult RedirigirPorRol(string rol)
        {
            return rol switch
            {
                "Admin" => RedirectToAction("Dashboard", "Admin"),
                "Staff" => RedirectToAction("Index", "Staff"),
                "Artista" => RedirectToAction("Index", "ArtistPortal"),
                _ => RedirectToAction("Index", "Home")
            };
        }
    }

    // Request models for Admin endpoints
    public class ArtistaRequest
    {
        public string NombreArtistico { get; set; } = string.Empty;
        public string NombreReal { get; set; } = string.Empty;
        public string Pais { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
    }

    public class DemoRequest
    {
        public string Titulo { get; set; } = string.Empty;
        public string UrlAudio { get; set; } = string.Empty;
        public int IdArtista { get; set; }
        public int IdGenero { get; set; }
    }

    public class LoginViewModel
    {
        public string Correo { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}