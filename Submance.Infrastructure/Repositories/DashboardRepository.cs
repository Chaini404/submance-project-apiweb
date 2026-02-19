using Dapper;
using Submance.Application.Interfaces.Repositories;
using Submance.Infrastructure.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Submance.Infrastructure.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly DbConnectionFactory _db;
        public DashboardRepository(DbConnectionFactory db) => _db = db;

        public async Task<object> GetDashboardDataAsync()
        {
            await using var connection = _db.CreateConnection();
            await connection.OpenAsync();

            var sqlStats = @"
                SELECT 
                    (SELECT COUNT(*) FROM ""Demos"") as totaldemos,
                    (SELECT COUNT(*) FROM ""Demos"" WHERE ""Estado"" = 'Pendiente') as pendientes,
                    (SELECT COUNT(*) FROM ""Demos"" WHERE ""Estado"" = 'Aprobado') as aprobados,
                    (SELECT COUNT(*) FROM ""Artistas"") as totalartistas";

            var sqlDemos = @"
                SELECT d.""IdDemo"" as iddemo, d.""Titulo"" as titulo, d.""Estado"" as estado, d.""FechaEnvio"" as fechaenvio, d.""UrlAudio"" as urlaudio, a.""NombreArtistico"" as nombreartistico
                FROM ""Demos"" d
                LEFT JOIN ""Artistas"" a ON d.""IdArtista"" = a.""IdArtista""
                ORDER BY d.""FechaEnvio"" DESC
                LIMIT 10";

            var sqlArtistas = @"
                SELECT ""IdArtista"" as idartista, ""NombreArtistico"" as nombreartistico, ""Pais"" as pais, ""Estado"" as estado 
                FROM ""Artistas"" 
                ORDER BY ""FechaRegistro"" DESC 
                LIMIT 5";

            var stats = await connection.QuerySingleAsync(sqlStats);
            var demos = await connection.QueryAsync(sqlDemos);
            var artistas = await connection.QueryAsync(sqlArtistas);

            return new
            {
                stats = new
                {
                    totalDemos = stats.totaldemos,
                    pendientes = stats.pendientes,
                    aprobados = stats.aprobados,
                    artistas = stats.totalartistas
                },
                demos = demos.Select(d => new {
                    id = d.iddemo,
                    titulo = d.titulo,
                    artistaNombre = d.nombreartistico ?? "Desconocido",
                    genero = "General",
                    estado = d.estado,
                    fechaEnvio = d.fechaenvio,
                    urlAudio = d.urlaudio
                }),
                artistas = artistas.Select(a => new {
                    id = a.idartista,
                    nombreArtistico = a.nombreartistico,
                    pais = a.pais,
                    estado = a.estado
                })
            };
        }
    }
}