#nullable enable
using Dapper;
using Submance.Application.Interfaces.Repositories;
using Submance.Domain.Entities;
using Submance.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Submance.Infrastructure.Repositories
{
    public class RevisionRepository : IRevisionRepository
    {
        private readonly DbConnectionFactory _db;
        public RevisionRepository(DbConnectionFactory db) => _db = db;

        public async Task<IEnumerable<Revision>> GetAllAsync()
        {
            await using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            return await conn.QueryAsync<Revision>(@"SELECT * FROM ""Revisiones""");
        }

        public async Task<Revision?> GetByIdAsync(int id)
        {
            await using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            return await conn.QueryFirstOrDefaultAsync<Revision>(
                @"SELECT * FROM ""Revisiones"" WHERE ""IdRevision"" = @Id", new { Id = id });
        }

        public async Task<IEnumerable<Revision>> GetByDemoAsync(int idDemo)
        {
            await using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            return await conn.QueryAsync<Revision>(
                @"SELECT * FROM ""Revisiones"" WHERE ""IdDemo"" = @IdDemo", new { IdDemo = idDemo });
        }

        public async Task AddAsync(Revision revision)
        {
            await using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            var sql = @"INSERT INTO ""Revisiones"" (""IdDemo"", ""Observacion"", ""Resultado"", ""FechaRevision"", ""IdRevisor"") 
                        VALUES (@IdDemo, @Observacion, @Resultado, NOW(), @IdRevisor)";
            await conn.ExecuteAsync(sql, revision);
        }
    }
}