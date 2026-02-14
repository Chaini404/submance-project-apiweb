using Submance.Infrastructure.Data;
using Submance.Application.Interfaces.Repository;
using Submance.Infrastructure.Repositories;
using Submance.Application.Interfaces.Services;
using Submance.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Servicios Base
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(opts => {
    opts.IdleTimeout = TimeSpan.FromMinutes(60);
    opts.Cookie.HttpOnly = true;
    opts.Cookie.IsEssential = true;
});

// 2. Base de Datos
builder.Services.AddSingleton<DbConnectionFactory>();

// 3. Repositorios (DATA)
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IArtistaRepository, ArtistaRepository>();
builder.Services.AddScoped<ICancionRepository, CancionRepository>();

// 4. Servicios (NEGOCIO)
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IArtistaService, ArtistaService>();

var app = builder.Build();

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