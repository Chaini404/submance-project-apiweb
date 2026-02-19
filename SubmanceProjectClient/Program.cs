using Submance.Infrastructure.Data;
using Submance.Application.Interfaces.Repositories;
using Submance.Infrastructure.Repositories;
using Submance.Application.Interfaces.Services;
using Submance.Application.Services;
using Submance.Infrastructure.Security; // Necesario para PasswordHasher

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. SERVICIOS BASE
// ==========================================
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(opts => {
    opts.IdleTimeout = TimeSpan.FromMinutes(60);
    opts.Cookie.HttpOnly = true;
    opts.Cookie.IsEssential = true;
});

// ==========================================
// 2. BASE DE DATOS E INFRAESTRUCTURA
// ==========================================
builder.Services.AddScoped<DbConnectionFactory>();

// REGISTRO DE SEGURIDAD (Soluciona error de imagen 12339b.png)
builder.Services.AddScoped<PasswordHasher>();

// ==========================================
// 3. REPOSITORIOS (ACCESO A DATOS)
// ==========================================
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IArtistaRepository, ArtistaRepository>();
builder.Services.AddScoped<ICancionRepository, CancionRepository>();

// Registros de repositorios adicionales
builder.Services.AddScoped<IAlbumRepository, AlbumRepository>();
builder.Services.AddScoped<IGeneroRepository, GeneroRepository>();
builder.Services.AddScoped<IRevisionRepository, RevisionRepository>();
builder.Services.AddScoped<IRolRepository, RolRepository>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();

// ==========================================   
// 4. SERVICIOS (LÓGICA DE NEGOCIO / APLICACIÓN)
// ==========================================
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IArtistaService, ArtistaService>();
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

// ==========================================
// 5. PIPELINE HTTP
// ==========================================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();