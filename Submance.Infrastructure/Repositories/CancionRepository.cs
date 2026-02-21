#nullable enable
using Dapper;
using Submance.Application.Interfaces.Repositories;
using Submance.Domain.Entities;
using Submance.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Submance.Infrastructure.Repositories
{
    public class CancionRepository : ICancionRepository
    {
        private readonly DbConnectionFactory _db;
        public CancionRepository(DbConnectionFactory db) => _db = db;

        public async Task<IEnumerable<Cancion>> GetAllAsync()
        {
            await using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            return await conn.QueryAsync<Cancion>(@"SELECT * FROM ""Demos"" ORDER BY ""FechaEnvio"" DESC");
        }

        public async Task<Cancion?> GetByIdAsync(int id)
        {
            await using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            return await conn.QueryFirstOrDefaultAsync<Cancion>(
                @"SELECT * FROM ""Demos"" WHERE ""IdDemo"" = @Id", new { Id = id });
        }

        public async Task<IEnumerable<Cancion>> GetByArtistaAsync(int idArtista)
        {
            await using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            return await conn.QueryAsync<Cancion>(
                @"SELECT * FROM ""Demos"" WHERE ""IdArtista"" = @Id ORDER BY ""FechaEnvio"" DESC", new { Id = idArtista });
        }

        public async Task<IEnumerable<Cancion>> GetPendientesRevisionAsync()
        {
            await using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            return await conn.QueryAsync<Cancion>(
                @"SELECT * FROM ""Demos"" WHERE ""Estado"" = 'Pendiente' ORDER BY ""FechaEnvio"" ASC");
        }

        public async Task AddAsync(Cancion cancion)
        {
            await using var conn = _db.CreateConnection();
            await conn.OpenAsync();

            // Explícito: solo los campos que Postgres no autogenera
            var sql = @"
        INSERT INTO ""Demos"" (""Titulo"", ""UrlAudio"", ""IdArtista"", ""IdGenero"", ""Estado"", ""FechaEnvio"") 
        VALUES (@Titulo, @UrlAudio, @IdArtista, @IdGenero, @Estado, @FechaEnvio)
        RETURNING ""IdDemo""";

            var newId = await conn.ExecuteScalarAsync<int>(sql, new
            {
                cancion.Titulo,
                cancion.UrlAudio,
                cancion.IdArtista,
                cancion.IdGenero,
                cancion.Estado,
                cancion.FechaEnvio
            });

            cancion.IdDemo = newId;
        }

        public async Task UpdateAsync(Cancion cancion)
        {
            await using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            var sql = @"UPDATE ""Demos"" SET ""Titulo"" = @Titulo, ""UrlAudio"" = @UrlAudio, ""IdArtista"" = @IdArtista, ""IdGenero"" = @IdGenero, ""Estado"" = @Estado, ""FechaEnvio"" = @FechaEnvio WHERE ""IdDemo"" = @IdDemo";
            await conn.ExecuteAsync(sql, cancion);
        }

        public async Task DeleteAsync(int id)
        {
            await using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            await conn.ExecuteAsync(@"DELETE FROM ""Demos"" WHERE ""IdDemo"" = @Id", new { Id = id });
        }
    }
}