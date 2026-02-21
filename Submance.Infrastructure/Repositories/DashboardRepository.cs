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
                    (SELECT COUNT(*) FROM ""Demos"")                                  AS totaldemos,
                    (SELECT COUNT(*) FROM ""Demos"" WHERE ""Estado"" = 'Pendiente')   AS pendientes,
                    (SELECT COUNT(*) FROM ""Demos"" WHERE ""Estado"" = 'Aprobado')    AS aprobados,
                    (SELECT COUNT(*) FROM ""Demos"" WHERE ""Estado"" = 'Rechazado')   AS rechazados,
                    (SELECT COUNT(*) FROM ""Artistas"")                               AS totalartistas";

            var sqlDemos = @"
                SELECT 
                    d.""IdDemo""           AS iddemo,
                    d.""Titulo""           AS titulo,
                    d.""Estado""           AS estado,
                    d.""FechaEnvio""       AS fechaenvio,
                    d.""UrlAudio""         AS urlaudio,
                    a.""NombreArtistico""  AS nombreartistico,
                    g.""NombreGenero""     AS nombregenero
                FROM ""Demos"" d
                LEFT JOIN ""Artistas"" a ON d.""IdArtista"" = a.""IdArtista""
                LEFT JOIN ""Generos"" g ON d.""IdGenero"" = g.""IdGenero""
                ORDER BY d.""FechaEnvio"" DESC";

            var sqlArtistas = @"
                SELECT 
                    ""IdArtista""        AS idartista,
                    ""NombreArtistico""  AS nombreartistico,
                    ""Pais""             AS pais,
                    ""Estado""           AS estado
                FROM ""Artistas""
                ORDER BY ""FechaRegistro"" DESC";

            var stats = await connection.QuerySingleAsync(sqlStats);
            var demos = await connection.QueryAsync(sqlDemos);
            var artistas = await connection.QueryAsync(sqlArtistas);

            return new
            {
                stats = new
                {
                    totalDemos = (int)stats.totaldemos,
                    pendientes = (int)stats.pendientes,
                    aprobados = (int)stats.aprobados,
                    rechazados = (int)stats.rechazados,
                    artistas = (int)stats.totalartistas
                },
                demos = demos.Select(d => new {
                    idDemo = (int)d.iddemo,
                    titulo = (string)d.titulo,
                    artistaNombre = (string)(d.nombreartistico ?? "Desconocido"),
                    genero = (string)(d.nombregenero ?? "General"),
                    estado = (string)d.estado,
                    fechaEnvio = (DateTime)d.fechaenvio,
                    urlAudio = (string)d.urlaudio
                }),
                artistas = artistas.Select(a => new {
                    id = (int)a.idartista,
                    nombreArtistico = (string)a.nombreartistico,
                    pais = (string)(a.pais ?? "N/A"),
                    estado = a.estado
                })
            };
        }
    }
}