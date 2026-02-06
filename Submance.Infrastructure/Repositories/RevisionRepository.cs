using Microsoft.Data.SqlClient;
using Submance.Application.Interfaces.Repositories;
using Submance.Domain.Entities;
using Submance.Domain.Enums;
using Submance.Infrastructure.Data;
using System.Data;

namespace Submance.Infrastructure.Repositories
{
    public class RevisionRepository : IRevisionRepository
    {
        private readonly DbConnectionFactory _dbConnectionFactory;

        public RevisionRepository(DbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<IEnumerable<Revision>> GetAllAsync()
        {
            var lista = new List<Revision>();

            using var connection = _dbConnectionFactory.CreateConnection() as SqlConnection
                ?? throw new InvalidOperationException("No se pudo crear SqlConnection");

            using var command = new SqlCommand("sp_Revision_GetAll", connection);
            command.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                lista.Add(Map(reader));
            }

            return lista;
        }

        public async Task<Revision> GetByIdAsync(int id)
        {
            Revision revision = null;

            using var connection = _dbConnectionFactory.CreateConnection() as SqlConnection
                ?? throw new InvalidOperationException("No se pudo crear SqlConnection");

            using var command = new SqlCommand("sp_Revision_GetById", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@idRevision", id);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
                revision = Map(reader);

            return revision;
        }

        public async Task<IEnumerable<Revision>> GetByCancionAsync(int idCancion)
        {
            var lista = new List<Revision>();

            using var connection = _dbConnectionFactory.CreateConnection() as SqlConnection
                ?? throw new InvalidOperationException("No se pudo crear SqlConnection");

            using var command = new SqlCommand("sp_Revision_GetByCancion", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@idCancion", idCancion);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                lista.Add(Map(reader));
            }

            return lista;
        }

        public async Task Add(Revision revision)
        {
            using var connection = _dbConnectionFactory.CreateConnection() as SqlConnection
                ?? throw new InvalidOperationException("No se pudo crear SqlConnection");

            using var command = new SqlCommand("sp_Revision_Add", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@idCancion", revision.IdCancion);
            command.Parameters.AddWithValue("@idRevisor", revision.IdRevisor);
            command.Parameters.AddWithValue("@observacion", revision.Observacion);
            command.Parameters.AddWithValue("@resultado", revision.Resultado);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        // Mapper privado
        private Revision Map(SqlDataReader reader)
        {
            return new Revision
            {
                IdRevision = (int)reader["idRevision"],
                IdCancion = (int)reader["idCancion"],
                IdRevisor = (int)reader["idRevisor"],

                FechaRevision = reader["fechaRevision"] == DBNull.Value
                    ? DateTime.Now
                    : (DateTime)reader["fechaRevision"],

                Observacion = reader["observacion"]?.ToString(),

                Resultado = Enum.TryParse<ResultadoRevision>(
                                reader["resultado"]?.ToString(),
                                true,
                                out var resultado)
                            ? resultado
                            : ResultadoRevision.Pendiente
            };
        }

    }
}
