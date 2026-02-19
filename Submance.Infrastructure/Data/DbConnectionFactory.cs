using Npgsql;
using Microsoft.Extensions.Configuration;
using System.Data.Common; // <--- CAMBIO: Usar System.Data.Common

namespace Submance.Infrastructure.Data
{
    public class DbConnectionFactory
    {
        private readonly IConfiguration _configuration;

        public DbConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // CAMBIO: Retornar DbConnection para habilitar OpenAsync y BeginTransactionAsync
        public DbConnection CreateConnection()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            return new NpgsqlConnection(connectionString); // Solo crear, NO abrir aquí
        }
    }
}