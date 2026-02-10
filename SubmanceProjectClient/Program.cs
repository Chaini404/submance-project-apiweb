using Microsoft.EntityFrameworkCore;
using SubmanceProject.Web.Data; // O el namespace donde tengas tu Context en el Web

var builder = WebApplication.CreateBuilder(args);

// 1. Servicios para Vistas (HTML)
builder.Services.AddControllersWithViews();

// 2. Base de Datos (Se queda por si tienes Login MVC normal)
// Asegúrate de que la conexión "DefaultConnection" esté en el appsettings.json del Web
builder.Services.AddDbContext<SubmanceContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configuración básica
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Importante para cargar tus CSS y JS

app.UseRouting();

app.UseAuthorization();

// Rutas para que cargue tu Dashboard.cshtml
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();