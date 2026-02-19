using Dapper;
using Submance.Application.Interfaces.Repositories;
using Submance.Domain.Entities;
using Submance.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Submance.Infrastructure.Repositories
{
    public class GeneroRepository : IGeneroRepository
    {
        private readonly DbConnectionFactory _db;
        public GeneroRepository(DbConnectionFactory db) => _db = db;

        public async Task<IEnumerable<Genero>> GetAllAsync()
        {
            await using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            return await conn.QueryAsync<Genero>(@"SELECT ""IdGenero"", ""NombreGenero"", ""Descripcion"" FROM ""Generos""");
        }

        public async Task<Genero?> GetByIdAsync(int id)
        {
            await using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            var sql = @"SELECT ""IdGenero"", ""NombreGenero"", ""Descripcion"" FROM ""Generos"" WHERE ""IdGenero"" = @Id";
            return await conn.QueryFirstOrDefaultAsync<Genero>(sql, new { Id = id });
        }

        public async Task AddAsync(Genero genero)
        {
            await using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            var sql = @"INSERT INTO ""Generos"" (""NombreGenero"", ""Descripcion"") VALUES (@NombreGenero, @Descripcion)";
            await conn.ExecuteAsync(sql, genero);
        }

        public async Task UpdateAsync(Genero genero)
        {
            await using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            var sql = @"UPDATE ""Generos"" SET ""NombreGenero"" = @NombreGenero, ""Descripcion"" = @Descripcion WHERE ""IdGenero"" = @IdGenero";
            await conn.ExecuteAsync(sql, genero);
        }

        public async Task DeleteAsync(int id)
        {
            await using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            await conn.ExecuteAsync(@"DELETE FROM ""Generos"" WHERE ""IdGenero"" = @Id", new { Id = id });
        }
    }
}