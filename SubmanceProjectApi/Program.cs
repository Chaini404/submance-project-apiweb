using Microsoft.OpenApi.Models;
using Submance.Infrastructure.Data;
using Submance.Application.Interfaces.Repositories;
using Submance.Application.Interfaces.Services;
using Submance.Application.Interfaces.Security;
using Submance.Application.Services;
using Submance.Infrastructure.Repositories;
using Submance.Infrastructure.Security;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// JSON con nombres de propiedad tal cual (sin camelCase automático)
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

builder.Services.AddEndpointsApiExplorer();

// ==========================================
// 2. BASE DE DATOS E INFRAESTRUCTURA
// ==========================================
builder.Services.AddScoped<DbConnectionFactory>();
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();

// ==========================================
// 3. REPOSITORIOS
// ==========================================
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IArtistaRepository, ArtistaRepository>();
builder.Services.AddScoped<ICancionRepository, CancionRepository>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<IAlbumRepository, AlbumRepository>();
builder.Services.AddScoped<IGeneroRepository, GeneroRepository>();
builder.Services.AddScoped<IRevisionRepository, RevisionRepository>();
builder.Services.AddScoped<IRolRepository, RolRepository>();

// ==========================================
// 4. SERVICIOS DE APLICACIÓN
// ==========================================
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IArtistaService, ArtistaService>();
builder.Services.AddScoped<ICancionService, CancionService>();
builder.Services.AddScoped<IAlbumService, AlbumService>();
builder.Services.AddScoped<IGeneroService, GeneroService>();

// ==========================================
// 5. Swagger
// ==========================================
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Submance API", Version = "v1" });
    c.CustomSchemaIds(type => type.FullName);
});

// ==========================================
// 6. CORS
// ==========================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
app.Run();