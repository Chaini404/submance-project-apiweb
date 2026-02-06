using Microsoft.Data.SqlClient;
using Submance.Application.Interfaces.Repositories;
using Submance.Domain.Entities;
using Submance.Infrastructure.Data;
using System.Data;

namespace Submance.Infrastructure.Repositories
{
    public class GeneroRepository : IGeneroRepository
    {
        private readonly DbConnectionFactory _dbConnectionFactory;

        public GeneroRepository(DbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<IEnumerable<Genero>> GetAllAsync()
        {
            var lista = new List<Genero>();

            using var connection = _dbConnectionFactory.CreateConnection() as SqlConnection
                ?? throw new InvalidOperationException("No se pudo crear SqlConnection");

            using var command = new SqlCommand("sp_Genero_GetAll", connection);
            command.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                lista.Add(Map(reader));
            }

            return lista;
        }

        public async Task<Genero> GetByIdAsync(int id)
        {
            Genero genero = null;

            using var connection = _dbConnectionFactory.CreateConnection() as SqlConnection
                ?? throw new InvalidOperationException("No se pudo crear SqlConnection");

            using var command = new SqlCommand("sp_Genero_GetById", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@idGenero", id);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
                genero = Map(reader);

            return genero;
        }

        public async Task AddAsync(Genero genero)
        {
            using var connection = _dbConnectionFactory.CreateConnection() as SqlConnection
                ?? throw new InvalidOperationException("No se pudo crear SqlConnection");

            using var command = new SqlCommand("sp_Genero_Add", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@nombreGenero", genero.NombreGenero);
            command.Parameters.AddWithValue("@descripcion", genero.Descripcion);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(Genero genero)
        {
            using var connection = _dbConnectionFactory.CreateConnection() as SqlConnection
                ?? throw new InvalidOperationException("No se pudo crear SqlConnection");

            using var command = new SqlCommand("sp_Genero_Update", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@idGenero", genero.IdGenero);
            command.Parameters.AddWithValue("@nombreGenero", genero.NombreGenero);
            command.Parameters.AddWithValue("@descripcion", genero.Descripcion);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _dbConnectionFactory.CreateConnection() as SqlConnection
                ?? throw new InvalidOperationException("No se pudo crear SqlConnection");

            using var command = new SqlCommand("sp_Genero_Delete", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@idGenero", id);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        // Mapper privado para evitar repetir código
        private Genero Map(SqlDataReader reader)
        {
            return new Genero
            {
                IdGenero = (int)reader["idGenero"],
                NombreGenero = reader["nombreGenero"].ToString(),
                Descripcion = reader["descripcion"].ToString()
            };
        }
    }
}
