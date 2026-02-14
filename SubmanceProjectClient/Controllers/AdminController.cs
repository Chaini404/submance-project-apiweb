using Microsoft.AspNetCore.Mvc;
using Submance.Application.DTOs.Usuario;
using Submance.Application.DTOs.Artista;
using Submance.Application.Interfaces.Services;

namespace SubmanceProject.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IArtistaService _artistaService;

        public AdminController(IUsuarioService usuarioService, IArtistaService artistaService)
        {
            _usuarioService = usuarioService;
            _artistaService = artistaService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            // Si ya tiene sesión, lo mandamos a su casa
            if (HttpContext.Session.GetString("UserRole") != null)
                return RedirigirPorRol(HttpContext.Session.GetString("UserRole")!);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto model)
        {
            try
            {
                // Usamos el Servicio corregido
                var usuario = await _usuarioService.LoginAsync(model.Correo, model.Password);

                if (usuario != null)
                {
                    // Guardar Sesión
                    HttpContext.Session.SetString("UserRole", usuario.Rol);
                    HttpContext.Session.SetString("UserEmail", usuario.Correo);
                    HttpContext.Session.SetInt32("UserId", usuario.IdUsuario);

                    return RedirigirPorRol(usuario.Rol);
                }

                ViewBag.Error = "Credenciales incorrectas.";
                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error: " + ex.Message;
                return View(model);
            }
        }

        // REGISTRO UNIFICADO (Usuario + Artista)
        [HttpPost]
        public async Task<IActionResult> Registro(RegistroArtistaDto model)
        {
            if (!ModelState.IsValid) return View("Login", model); // O vista de registro

            try
            {
                // 1. Crear Usuario (Rol 3 = Artista)
                await _usuarioService.RegisterAsync(new UsuarioRequestDto
                {
                    Nombre = model.NombreReal,
                    Correo = model.Correo,
                    Password = model.Password,
                    IdRol = 3 // ID fijo para Artistas
                });

                // 2. Crear Perfil Artista
                await _artistaService.CreateAsync(new ArtistaRequestDto
                {
                    NombreArtistico = model.NombreArtistico,
                    NombreReal = model.NombreReal,
                    Correo = model.Correo
                });

                ViewBag.Success = "Cuenta creada. Inicia sesión.";
                return View("Login");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al registrar: " + ex.Message;
                return View("Login");
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        private IActionResult RedirigirPorRol(string rol)
        {
            return rol switch
            {
                "Admin" => RedirectToAction("Index", "Artista"),
                "Staff" => RedirectToAction("Index", "Staff"),
                "Artista" => RedirectToAction("Index", "ArtistPortal"),
                _ => RedirectToAction("Index", "Home")
            };
        }
    }
}