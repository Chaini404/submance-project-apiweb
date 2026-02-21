#nullable enable
using Dapper;
using Submance.Application.Interfaces.Repositories;
using Submance.Domain.Entities;
using Submance.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Submance.Infrastructure.Repositories
{
    public class AlbumRepository : IAlbumRepository
    {
        private readonly DbConnectionFactory _db;

        public AlbumRepository(DbConnectionFactory db) => _db = db;

        public async Task<IEnumerable<Album>> GetAllAsync()
        {
            await using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            var sql = @"SELECT ""IdAlbum"", ""Titulo"", ""FechaLanzamiento"", ""IdArtista"" FROM ""Albumes""";
            return await conn.QueryAsync<Album>(sql);
        }

        public async Task<Album?> GetByIdAsync(int id)
        {
            await using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            var sql = @"SELECT ""IdAlbum"", ""Titulo"", ""FechaLanzamiento"", ""IdArtista"" FROM ""Albumes"" WHERE ""IdAlbum"" = @Id";
            return await conn.QueryFirstOrDefaultAsync<Album>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Album>> GetByArtistaAsync(int idArtista)
        {
            await using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            var sql = @"SELECT ""IdAlbum"", ""Titulo"", ""FechaLanzamiento"", ""IdArtista"" FROM ""Albumes"" WHERE ""IdArtista"" = @IdArtista";
            return await conn.QueryAsync<Album>(sql, new { IdArtista = idArtista });
        }

        public async Task AddAsync(Album album)
        {
            await using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            var sql = @"INSERT INTO ""Albumes"" (""Titulo"", ""FechaLanzamiento"", ""IdArtista"") 
                        VALUES (@Titulo, @FechaLanzamiento, @IdArtista)";
            await conn.ExecuteAsync(sql, album);
        }

        public async Task UpdateAsync(Album album)
        {
            await using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            var sql = @"UPDATE ""Albumes"" SET ""Titulo"" = @Titulo, ""FechaLanzamiento"" = @FechaLanzamiento, ""IdArtista"" = @IdArtista 
                        WHERE ""IdAlbum"" = @IdAlbum";
            await conn.ExecuteAsync(sql, album);
        }

        public async Task DeleteAsync(int id)
        {
            await using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            var sql = @"DELETE FROM ""Albumes"" WHERE ""IdAlbum"" = @Id";
            await conn.ExecuteAsync(sql, new { Id = id });
        }
    }
}