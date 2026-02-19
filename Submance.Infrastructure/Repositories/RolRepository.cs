#nullable enable
using Dapper;
using Submance.Application.Interfaces.Repositories;
using Submance.Domain.Entities;
using Submance.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Submance.Infrastructure.Repositories
{
    public class RolRepository : IRolRepository
    {
        private readonly DbConnectionFactory _db;
        public RolRepository(DbConnectionFactory db) => _db = db;

        public async Task<IEnumerable<Rol>> GetAllAsync()
        {
            await using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            return await conn.QueryAsync<Rol>(@"SELECT ""IdRol"", ""NombreRol"" FROM ""Roles""");
        }

        public async Task<Rol?> GetByIdAsync(int id)
        {
            await using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            return await conn.QueryFirstOrDefaultAsync<Rol>(
                @"SELECT ""IdRol"", ""NombreRol"" FROM ""Roles"" WHERE ""IdRol"" = @Id", new { Id = id });
        }
    }
}