#nullable enable
using Dapper;
using Submance.Application.Interfaces.Repositories;
using Submance.Domain.Entities;
using Submance.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Submance.Infrastructure.Repositories
{
    public class ArtistaRepository : IArtistaRepository
    {
        private readonly DbConnectionFactory _db;
        public ArtistaRepository(DbConnectionFactory db) => _db = db;

        public async Task<IEnumerable<Artista>> GetAllAsync()
        {
            await using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            return await conn.QueryAsync<Artista>(@"SELECT * FROM ""Artistas"" ORDER BY ""FechaRegistro"" DESC");
        }

        public async Task<Artista?> GetByIdAsync(int id)
        {
            await using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            return await conn.QueryFirstOrDefaultAsync<Artista>(
                @"SELECT * FROM ""Artistas"" WHERE ""IdArtista"" = @Id", new { Id = id });
        }

        public async Task<Artista?> GetByEmailAsync(string email)
        {
            await using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            var sql = @"
                SELECT a.* FROM ""Artistas"" a
                INNER JOIN ""Usuarios"" u ON a.""IdUsuario"" = u.""IdUsuario""
                WHERE u.""Correo"" = @Email";
            return await conn.QueryFirstOrDefaultAsync<Artista>(sql, new { Email = email });
        }

        public async Task AddAsync(Artista artista)
        {
            await using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            var sql = @"INSERT INTO ""Artistas"" (""IdUsuario"", ""NombreArtistico"", ""NombreReal"", ""Pais"", ""Estado"", ""FechaRegistro"") 
                        VALUES (@IdUsuario, @NombreArtistico, @NombreReal, @Pais, @Estado, NOW())";
            await conn.ExecuteAsync(sql, artista);
        }

        public async Task UpdateAsync(Artista artista)
        {
            await using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            var sql = @"UPDATE ""Artistas"" 
                        SET ""NombreArtistico"" = @NombreArtistico, 
                            ""NombreReal"" = @NombreReal, 
                            ""Pais"" = @Pais, 
                            ""Estado"" = @Estado 
                        WHERE ""IdArtista"" = @IdArtista";
            await conn.ExecuteAsync(sql, artista);
        }

        public async Task DeleteAsync(int id)
        {
            await using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            await conn.ExecuteAsync(@"DELETE FROM ""Artistas"" WHERE ""IdArtista"" = @Id", new { Id = id });
        }
    }
}