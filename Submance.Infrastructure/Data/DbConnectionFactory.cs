using Npgsql; // Driver de PostgreSQL para Supabase
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Submance.Infrastructure.Data
{
    public class DbConnectionFactory
    {
        private readonly IConfiguration _configuration;

        public DbConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection CreateConnection()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            // CORRECTO: NpgsqlConnection es el que gestiona la conexión física a Supabase
            return new NpgsqlConnection(connectionString);
        }
    }
}