#nullable enable
using Npgsql;
using Submance.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Submance.Infrastructure.Repositories
{
    public class RolRepository
    {
        private readonly DbConnectionFactory _dbConnectionFactory;
        public RolRepository(DbConnectionFactory dbFactory) => _dbConnectionFactory = dbFactory;

        public async Task<IEnumerable<string>> GetRolesAsync()
        {
            var roles = new List<string>();
            // Usar NpgsqlConnection para la conexión física
            using (var con = _dbConnectionFactory.CreateConnection() as NpgsqlConnection)
            {
                if (con == null) return roles;
                await con.OpenAsync();

                string sql = @"SELECT ""Nombre"" FROM ""Roles""";

                // AQUÍ ESTABA EL ERROR: Debe ser NpgsqlCommand, no NpgsqlParameter
                using (var cmd = new NpgsqlCommand(sql, con))
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        roles.Add(reader["Nombre"].ToString() ?? "");
                    }
                }
            }
            return roles;
        }
    }
}