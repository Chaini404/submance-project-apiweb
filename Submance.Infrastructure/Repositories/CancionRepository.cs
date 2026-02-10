using Microsoft.Data.SqlClient;
using Submance.Application.Interfaces.Repositories;
using Submance.Domain.Entities;
// using Submance.Domain.Enums; // YA NO LO NECESITAMOS AQUÍ
using Submance.Infrastructure.Data;
using System.Data;

namespace Submance.Infrastructure.Repositories
{
    public class CancionRepository : ICancionRepository
    {
        private readonly DbConnectionFactory _dbConnectionFactory;

        public CancionRepository(DbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<IEnumerable<Cancion>> GetAllAsync()
        {
            var lista = new List<Cancion>();

            using var connection = _dbConnectionFactory.CreateConnection() as SqlConnection
                ?? throw new InvalidOperationException("No se pudo crear SqlConnection");

            using var command = new SqlCommand("sp_Cancion_GetAll", connection);
            command.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                lista.Add(Map(reader));
            }

            return lista;
        }

        public async Task<Cancion> GetByIdAsync(int id)
        {
            Cancion cancion = null;

            using var connection = _dbConnectionFactory.CreateConnection() as SqlConnection
                ?? throw new InvalidOperationException("No se pudo crear SqlConnection");

            using var command = new SqlCommand("sp_Cancion_GetById", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@idCancion", id);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
                cancion = Map(reader);

            return cancion;
        }

        public async Task<IEnumerable<Cancion>> GetByArtistaAsync(int idArtista)
        {
            var lista = new List<Cancion>();

            using var connection = _dbConnectionFactory.CreateConnection() as SqlConnection
                ?? throw new InvalidOperationException("No se pudo crear SqlConnection");

            using var command = new SqlCommand("sp_Cancion_GetByArtista", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@idArtista", idArtista);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                lista.Add(Map(reader));
            }

            return lista;
        }

        public async Task<IEnumerable<Cancion>> GetPendientesRevisionAsync()
        {
            var lista = new List<Cancion>();

            using var connection = _dbConnectionFactory.CreateConnection() as SqlConnection
                ?? throw new InvalidOperationException("No se pudo crear SqlConnection");

            using var command = new SqlCommand("sp_Cancion_GetPendientesRevision", connection);
            command.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                lista.Add(Map(reader));
            }

            return lista;
        }

        public async Task AddAsync(Cancion cancion)
        {
            using var connection = _dbConnectionFactory.CreateConnection() as SqlConnection
                ?? throw new InvalidOperationException("No se pudo crear SqlConnection");

            using var command = new SqlCommand("sp_Cancion_Add", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@titulo", cancion.Titulo);
            // Manejo de nulos seguro para TimeSpam
            command.Parameters.AddWithValue("@duracion", cancion.Duracion ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@archivo", cancion.Archivo ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@idAlbum", cancion.IdAlbum ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@idGenero", cancion.IdGenero);
            command.Parameters.AddWithValue("@idArtista", cancion.IdArtista);

            // Si tu SP espera estado, descomenta esto:
            // command.Parameters.AddWithValue("@estado", cancion.Estado ?? "Pendiente");

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(Cancion cancion)
        {
            using var connection = _dbConnectionFactory.CreateConnection() as SqlConnection
                ?? throw new InvalidOperationException("No se pudo crear SqlConnection");

            using var command = new SqlCommand("sp_Cancion_Update", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@idCancion", cancion.IdCancion);
            command.Parameters.AddWithValue("@titulo", cancion.Titulo);
            command.Parameters.AddWithValue("@duracion", cancion.Duracion ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@archivo", cancion.Archivo ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@idAlbum", cancion.IdAlbum ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@idGenero", cancion.IdGenero);
            command.Parameters.AddWithValue("@idArtista", cancion.IdArtista);

            // AQUÍ PASAMOS EL STRING DIRECTAMENTE
            command.Parameters.AddWithValue("@estado", cancion.Estado);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _dbConnectionFactory.CreateConnection() as SqlConnection
                ?? throw new InvalidOperationException("No se pudo crear SqlConnection");

            using var command = new SqlCommand("sp_Cancion_Delete", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@idCancion", id);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        // ==========================================
        // CORRECCIÓN DEL MAPPER (Adiós Enum)
        // ==========================================
        private Cancion Map(SqlDataReader reader)
        {
            return new Cancion
            {
                IdCancion = (int)reader["idCancion"],
                Titulo = reader["titulo"]?.ToString(),
                Duracion = reader["duracion"] == DBNull.Value
                            ? null
                            : (TimeSpan?)reader["duracion"],
                Archivo = reader["archivo"]?.ToString(),
                IdAlbum = reader["idAlbum"] == DBNull.Value
                            ? null
                            : (int?)reader["idAlbum"],
                IdGenero = (int)reader["idGenero"],
                IdArtista = (int)reader["idArtista"],

                // CORREGIDO: Asignación directa de String
                // Si viene nulo, ponemos "Pendiente" por defecto
                Estado = reader["estado"]?.ToString() ?? "Pendiente"
            };
        }
    }
}
