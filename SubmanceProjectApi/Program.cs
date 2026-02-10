using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SubmanceProject.Api.Data;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// 1. Agregar servicios con configuración de nombres (PropertyNamingPolicy = null)
// ESTO ES LO QUE ARREGLA EL BUZÓN VACÍO
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

builder.Services.AddEndpointsApiExplorer();

// 2. Base de Datos
builder.Services.AddDbContext<ApiContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3. Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Submance API", Version = "v1" });
    c.CustomSchemaIds(type => type.FullName);
});

// 4. CORS
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