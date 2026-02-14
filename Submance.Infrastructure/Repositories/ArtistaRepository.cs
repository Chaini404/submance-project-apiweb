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
    public class ArtistaRepository : IArtistaRepository
    {
        private readonly DbConnectionFactory _dbConnectionFactory;
        public ArtistaRepository(DbConnectionFactory dbFactory) => _dbConnectionFactory = dbFactory;

        public async Task<IEnumerable<Artista>> GetAllAsync()
        {
            var artistas = new List<Artista>();
            using (var con = _dbConnectionFactory.CreateConnection() as NpgsqlConnection)
            {
                if (con == null) return artistas;
                await con.OpenAsync();

                string sql = @"SELECT * FROM ""Artistas""";
                using (var cmd = new NpgsqlCommand(sql, con))
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync()) artistas.Add(Map(reader));
                }
            }
            return artistas;
        }

        public async Task<Artista?> GetByEmailAsync(string email)
        {
            Artista? artista = null;
            using (var con = _dbConnectionFactory.CreateConnection() as NpgsqlConnection)
            {
                if (con == null) return null;
                await con.OpenAsync();

                string sql = @"SELECT * FROM ""Artistas"" WHERE ""Correo"" = @correo";
                using (var cmd = new NpgsqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@correo", email);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync()) artista = Map(reader);
                    }
                }
            }
            return artista;
        }

        public async Task AddAsync(Artista artista)
        {
            using (var con = _dbConnectionFactory.CreateConnection() as NpgsqlConnection)
            {
                if (con == null) return;
                await con.OpenAsync();

                string sql = @"INSERT INTO ""Artistas"" (""NombreArtistico"", ""NombreReal"", ""Correo"", ""Pais"", ""Estado"", ""FechaRegistro"")
                               VALUES (@nombre, @real, @correo, @pais, TRUE, NOW())";

                using (var cmd = new NpgsqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@nombre", artista.NombreArtistico);
                    cmd.Parameters.AddWithValue("@real", artista.NombreReal ?? (object)System.DBNull.Value);
                    cmd.Parameters.AddWithValue("@correo", artista.Correo);
                    cmd.Parameters.AddWithValue("@pais", artista.Pais ?? (object)System.DBNull.Value);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<Artista?> GetByIdAsync(int id)
        {
            Artista? artista = null;
            using (var con = _dbConnectionFactory.CreateConnection() as NpgsqlConnection)
            {
                if (con == null) return null;
                await con.OpenAsync();
                string sql = @"SELECT * FROM ""Artistas"" WHERE ""IdArtista"" = @id";
                using (var cmd = new NpgsqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync()) artista = Map(reader);
                    }
                }
            }
            return artista;
        }

        public async Task UpdateAsync(Artista artista) => await Task.CompletedTask;
        public async Task DeleteAsync(int id) => await Task.CompletedTask;

        private Artista Map(NpgsqlDataReader reader)
        {
            return new Artista
            {
                IdArtista = reader.GetInt32(reader.GetOrdinal("IdArtista")),
                NombreArtistico = reader["NombreArtistico"].ToString() ?? "",
                Correo = reader["Correo"].ToString() ?? "",
                NombreReal = reader["NombreReal"]?.ToString(),
                Pais = reader["Pais"]?.ToString()
            };
        }
    }
}