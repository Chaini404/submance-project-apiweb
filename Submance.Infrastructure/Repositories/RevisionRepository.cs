#nullable enable
using Npgsql;
using NpgsqlTypes; // Necesario para tipos específicos de Postgres
using Submance.Infrastructure.Data;
using System.Threading.Tasks;

namespace Submance.Infrastructure.Repositories
{
    public class RevisionRepository
    {
        private readonly DbConnectionFactory _dbConnectionFactory;

        public RevisionRepository(DbConnectionFactory dbFactory)
        {
            _dbConnectionFactory = dbFactory;
        }

        public async Task AddRevisionAsync(int idDemo, string comentario, string estado)
        {
            using (var con = _dbConnectionFactory.CreateConnection() as NpgsqlConnection)
            {
                if (con == null) return;
                await con.OpenAsync();

                string sql = @"INSERT INTO ""Revisiones"" (""IdDemo"", ""Comentario"", ""Estado"", ""FechaRevision"") 
                               VALUES (@id, @com, @est, NOW())";

                using (var cmd = new NpgsqlCommand(sql, con))
                {
                    // Evitamos el error de constructor: NpgsqlCommand usa (sql, connection)
                    cmd.Parameters.AddWithValue("@id", idDemo);

                    // Manejo de nulos para la base de datos
                    cmd.Parameters.AddWithValue("@com", comentario ?? (object)System.DBNull.Value);
                    cmd.Parameters.AddWithValue("@est", estado);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}