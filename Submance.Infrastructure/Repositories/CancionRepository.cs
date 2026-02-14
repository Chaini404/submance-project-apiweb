#nullable enable
using Npgsql;
using NpgsqlTypes;
using Submance.Application.Interfaces.Repository;
using Submance.Domain.Entities;
using Submance.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Submance.Infrastructure.Repositories
{
    public class CancionRepository : ICancionRepository
    {
        private readonly DbConnectionFactory _dbConnectionFactory;
        public CancionRepository(DbConnectionFactory dbFactory) => _dbConnectionFactory = dbFactory;

        public async Task<IEnumerable<Cancion>> GetByArtistaAsync(int idArtista)
        {
            var lista = new List<Cancion>();
            using (var con = _dbConnectionFactory.CreateConnection() as NpgsqlConnection)
            {
                if (con == null) return lista;
                await con.OpenAsync();

                string sql = @"SELECT ""IdDemo"" as ""IdCancion"", ""Titulo"", ""UrlAudio"", ""Estado"", ""IdArtista"", ""IdGenero""
                               FROM ""Demos"" WHERE ""IdArtista"" = @id";

                using (var cmd = new NpgsqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", idArtista);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync()) lista.Add(Map(reader));
                    }
                }
            }
            return lista;
        }

        public async Task AddAsync(Cancion cancion)
        {
            using (var con = _dbConnectionFactory.CreateConnection() as NpgsqlConnection)
            {
                if (con == null) return;
                await con.OpenAsync();

                string sql = @"INSERT INTO ""Demos"" (""Titulo"", ""UrlAudio"", ""IdArtista"", ""IdGenero"", ""Estado"", ""FechaEnvio"")
                               VALUES (@titulo, @url, @idArt, @idGen, 'Pendiente', NOW())";

                using (var cmd = new NpgsqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@titulo", cancion.Titulo);
                    cmd.Parameters.AddWithValue("@url", cancion.Archivo ?? (object)System.DBNull.Value);
                    cmd.Parameters.AddWithValue("@idArt", cancion.IdArtista);
                    cmd.Parameters.AddWithValue("@idGen", cancion.IdGenero);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<IEnumerable<Cancion>> GetPendientesRevisionAsync()
        {
            var lista = new List<Cancion>();
            using (var con = _dbConnectionFactory.CreateConnection() as NpgsqlConnection)
            {
                if (con == null) return lista;
                await con.OpenAsync();

                string sql = @"SELECT ""IdDemo"" as ""IdCancion"", ""Titulo"", ""UrlAudio"", ""Estado"", ""IdArtista"", ""IdGenero""
                                FROM ""Demos"" WHERE ""Estado"" = 'Pendiente'";

                using (var cmd = new NpgsqlCommand(sql, con))
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync()) lista.Add(Map(reader));
                }
            }
            return lista;
        }

        public async Task UpdateAsync(Cancion cancion)
        {
            using (var con = _dbConnectionFactory.CreateConnection() as NpgsqlConnection)
            {
                if (con == null) return;
                await con.OpenAsync();
                string sql = @"UPDATE ""Demos"" SET ""Estado"" = @estado WHERE ""IdDemo"" = @id";
                using (var cmd = new NpgsqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@estado", cancion.Estado);
                    cmd.Parameters.AddWithValue("@id", cancion.IdCancion);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<Cancion?> GetByIdAsync(int id)
        {
            using (var con = _dbConnectionFactory.CreateConnection() as NpgsqlConnection)
            {
                if (con == null) return null;
                await con.OpenAsync();
                string sql = @"SELECT ""IdDemo"" as ""IdCancion"", ""Titulo"", ""UrlAudio"", ""Estado"", ""IdArtista"", ""IdGenero""
                                FROM ""Demos"" WHERE ""IdDemo"" = @id";
                using (var cmd = new NpgsqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        return await reader.ReadAsync() ? Map(reader) : null;
                    }
                }
            }
        }

        public async Task<IEnumerable<Cancion>> GetAllAsync()
        {
            var lista = new List<Cancion>();
            using (var con = _dbConnectionFactory.CreateConnection() as NpgsqlConnection)
            {
                if (con == null) return lista;
                await con.OpenAsync();
                string sql = @"SELECT ""IdDemo"" as ""IdCancion"", ""Titulo"", ""UrlAudio"", ""Estado"", ""IdArtista"", ""IdGenero"" FROM ""Demos""";
                using (var cmd = new NpgsqlCommand(sql, con))
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync()) lista.Add(Map(reader));
                }
            }
            return lista;
        }

        public async Task DeleteAsync(int id) => await Task.CompletedTask;

        private Cancion Map(NpgsqlDataReader reader)
        {
            return new Cancion
            {
                IdCancion = reader.GetInt32(reader.GetOrdinal("IdCancion")),
                Titulo = reader["Titulo"].ToString() ?? "",
                Archivo = reader["UrlAudio"] is System.DBNull ? null : reader["UrlAudio"].ToString(),
                Estado = reader["Estado"].ToString() ?? "Pendiente",
                IdArtista = reader.GetInt32(reader.GetOrdinal("IdArtista")),
                IdGenero = reader["IdGenero"] is System.DBNull ? 0 : reader.GetInt32(reader.GetOrdinal("IdGenero"))
            };
        }
    }
}