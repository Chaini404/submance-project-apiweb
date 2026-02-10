using Microsoft.AspNetCore.Mvc;
using Submance.Application.DTOs.Usuario; // 👈 Importante para reconocer el DTO
using System.Text.Json;
using System.Text;

namespace SubmanceProject.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly string _apiBaseUrl = "https://localhost:7064/api";
        private readonly HttpClient _httpClient;

        public AdminController()
        {
            _httpClient = new HttpClient();
        }

        // 1. GET: Muestra el formulario
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // 2. POST: Recibe los datos del formulario
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto model)
        {
            // --- PASO 1: PUERTA TRASERA (Para que entres YA al Dashboard) ---
            // Si usas estos datos, entra directo sin preguntar a la API.
            if (model.Correo == "admin@submance.com" && model.Password == "admin123")
            {
                return RedirectToAction("Dashboard");
            }
            // ---------------------------------------------------------------

            // --- PASO 2: INTENTO DE CONEXIÓN A TU API ---
            try
            {
                // Preparamos los datos para la API
                // Nota: Asegúrate que tu API espere "NombreUsuario" o "Correo"
                var loginData = new { NombreUsuario = model.Correo, Password = model.Password };

                var content = new StringContent(JsonSerializer.Serialize(loginData), Encoding.UTF8, "application/json");

                // Llamada a la API
                var response = await _httpClient.PostAsync($"{_apiBaseUrl}/Auth/Login", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Dashboard");
                }
                else
                {
                    // Si la API dice que no (401/400)
                    ViewBag.Error = "⛔ Usuario o contraseña incorrectos (API).";
                    return View(model);
                }
            }
            catch (Exception)
            {
                // Si la API está apagada o falla la conexión
                ViewBag.Error = "⚠️ La API no responde. Usa el usuario: admin@submance.com / admin123";
                return View(model);
            }
        }

        // 3. LA VISTA DASHBOARD (A donde llegas al entrar)
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}