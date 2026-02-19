using Microsoft.OpenApi.Models;
using Submance.Infrastructure.Data;
using Submance.Application.Interfaces.Repositories;
using Submance.Application.Interfaces.Services;
using Submance.Application.Services;
using Submance.Infrastructure.Repositories;
using Submance.Infrastructure.Security;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios con configuración de nombres (PropertyNamingPolicy = null)
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

builder.Services.AddEndpointsApiExplorer();

// ==========================================
// 2. BASE DE DATOS E INFRAESTRUCTURA (Dapper + PostgreSQL)
// ==========================================
builder.Services.AddScoped<DbConnectionFactory>();

// REGISTRO DEL SERVICIO DE SEGURIDAD (AÑADIDO)
builder.Services.AddSingleton<PasswordHasher>();

// Inyectamos los repositorios que usarán los controladores de la API
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IArtistaRepository, ArtistaRepository>();
builder.Services.AddScoped<ICancionRepository, CancionRepository>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();

// REGISTRO DEL SERVICIO DE AUTENTICACIÓN (CORREGIDO - faltaba)
builder.Services.AddScoped<IAuthService, AuthService>();

// ==========================================
// 3. Swagger
// ==========================================
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Submance API", Version = "v1" });
    c.CustomSchemaIds(type => type.FullName);
});

// ==========================================
// 4. CORS
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

// Configuración del Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 5. ACTIVAR LA POLÍTICA CORS
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();